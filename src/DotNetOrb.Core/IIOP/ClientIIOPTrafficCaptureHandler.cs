using System;
using System.IO;
using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DotNetOrb.Core.IIOP;

public class ClientIIOPTrafficCaptureHandler(DirectoryInfo outputPath, ISocketChannel channel) : ChannelHandlerAdapter
{
    // Pre-Check outputPath for access violations
    public static DirectoryInfo? ValidateDirectory(string outputPath)
    {
        if (string.IsNullOrEmpty(outputPath))
            return null;

        var directory = new DirectoryInfo(outputPath);
        
        if (!directory.Exists)
            directory.Create();
        directory.Refresh();

        return directory;
    }

    private readonly PcapWriter _writer = new(outputPath, channel);

    // (Server → Client)
    public override void ChannelRead(IChannelHandlerContext ctx, object msg)
    {
        if (msg is IByteBuffer buf)
        {
            var data = new byte[buf.ReadableBytes];
            buf.GetBytes(buf.ReaderIndex, data);
            _writer.WritePacket(data, fromClient: false);
        }

        ctx.FireChannelRead(msg);
    }

    // Client → Server
    public override Task WriteAsync(IChannelHandlerContext ctx, object msg)
    {
        if (msg is IByteBuffer buf)
        {
            var data = new byte[buf.ReadableBytes];
            buf.GetBytes(buf.ReaderIndex, data);
            _writer.WritePacket(data, fromClient: true);
        }

        return ctx.WriteAsync(msg);
    }

    public override void ChannelInactive(IChannelHandlerContext ctx)
    {
        _writer.Dispose();
        ctx.FireChannelInactive();
    }

    #region PcapWriter
    private class PcapWriter : IDisposable
    {
        private readonly ISocketChannel _channel;
        private readonly BinaryWriter _writer;
        private uint _seqClient = 1000;
        private uint _seqServer = 2000;

        private const uint Magic = 0xa1b2c3d4;
        private const ushort VersionMajor = 2;
        private const ushort VersionMinor = 4;
        private const uint SnapLen = 65535;
        private const uint LinkType = 0; // LINKTYPE_NULL (Loopback)

        private static int counter = 0;
        public PcapWriter(DirectoryInfo path, ISocketChannel channel)
        {
            _channel = channel;

            var filename = Path.Combine(path.FullName, $"DotNetOrb.{DateTime.Now.ToString("yyyyMMdd-HHmmss-fff")}.pcap");
            _writer = new BinaryWriter(File.Open(filename, FileMode.Create));

            WriteGlobalHeader();
        }

        private void WriteGlobalHeader()
        {
            _writer.Write(Magic);
            _writer.Write(VersionMajor);
            _writer.Write(VersionMinor);
            _writer.Write(0);       
            _writer.Write(0u);      
            _writer.Write(SnapLen);
            _writer.Write(LinkType);
        }

        public void WritePacket(byte[] payload, bool fromClient = true)
        {
            var now = DateTimeOffset.UtcNow;
            var tsSec = (uint)now.ToUnixTimeSeconds();
            var tsUsec = (uint)(now.Millisecond * 1000);

            var tcpPacket = BuildTcpPacket(payload, fromClient);

            var len = (uint)tcpPacket.Length;
            _writer.Write(tsSec);
            _writer.Write(tsUsec);
            _writer.Write(len);   
            _writer.Write(len);   
            _writer.Write(tcpPacket);
            _writer.Flush();
        }

        private bool isIPv6 = false;
        private IPEndPoint? localEndPoint = null;
        private IPEndPoint? remoteEndPoint = null;
        private void ResolveEndPoints()
        {
            if (localEndPoint == null)
            {
                var chanLocal = (IPEndPoint)_channel.LocalAddress;
                if (chanLocal.Address.AddressFamily == AddressFamily.InterNetwork)
                    localEndPoint = new IPEndPoint(chanLocal.Address, chanLocal.Port);
                else if (chanLocal.Address is { AddressFamily: AddressFamily.InterNetworkV6, IsIPv4MappedToIPv6: true })
                    localEndPoint = new IPEndPoint(chanLocal.Address.MapToIPv4(), chanLocal.Port);
                else if (chanLocal.Address is { AddressFamily: AddressFamily.InterNetworkV6 })
                {
                    localEndPoint = new IPEndPoint(chanLocal.Address, chanLocal.Port);
                    isIPv6 = true;
                }
                else
                    // Dummy
                    localEndPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }), chanLocal.Port);
            }
            if (remoteEndPoint == null)
            {
                var chanRemote = (IPEndPoint)_channel.RemoteAddress;
                if (chanRemote.Address.AddressFamily == AddressFamily.InterNetwork)
                    remoteEndPoint = new IPEndPoint(chanRemote.Address, chanRemote.Port);
                else if (chanRemote.Address is { AddressFamily: AddressFamily.InterNetworkV6, IsIPv4MappedToIPv6: true })
                    remoteEndPoint = new IPEndPoint(chanRemote.Address.MapToIPv4(), chanRemote.Port);
                else if (chanRemote.Address is { AddressFamily: AddressFamily.InterNetworkV6 })
                {
                    remoteEndPoint = new IPEndPoint(chanRemote.Address, chanRemote.Port);
                }
                else
                    // Dummy
                    remoteEndPoint = new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 2 }), chanRemote.Port);
            }
        }

        private byte[] BuildTcpPacket(byte[] payload, bool fromClient)
        {
            ResolveEndPoints();

            // Ports & Adressen
            var srcPort = fromClient ? (ushort)localEndPoint.Port : (ushort)remoteEndPoint.Port;
            var dstPort = fromClient ? (ushort)remoteEndPoint.Port : (ushort)localEndPoint.Port;
            var srcIp = fromClient ? localEndPoint.Address.GetAddressBytes() : remoteEndPoint.Address.GetAddressBytes();
            var dstIp = fromClient ? remoteEndPoint.Address.GetAddressBytes() : localEndPoint.Address.GetAddressBytes();

            ref var seq = ref fromClient ? ref _seqClient : ref _seqServer;
            var ack = fromClient ? _seqServer : _seqClient;

            var tcp = BuildTcp(srcPort, dstPort, seq, ack, payload);
            seq += (uint)payload.Length;

            var ip = isIPv6 
                ? BuildIpv6(srcIp, dstIp, tcp)
                : BuildIpv4(srcIp, dstIp, tcp);

            // LINKTYPE_NULL: 4-Byte-Header mit AF_INET = 2
            var frame = new byte[4 + ip.Length];
            frame[0] = 0x02; frame[1] = 0x00; frame[2] = 0x00; frame[3] = 0x00;
            Buffer.BlockCopy(ip, 0, frame, 4, ip.Length);
            return frame;
        }

        private byte[] BuildTcp(ushort src, ushort dst, uint seq, uint ack, byte[] payload)
        {
            var tcp = new byte[20 + payload.Length];
            tcp[0] = (byte)(src >> 8); tcp[1] = (byte)src;
            tcp[2] = (byte)(dst >> 8); tcp[3] = (byte)dst;
            // Sequence
            tcp[4] = (byte)(seq >> 24); tcp[5] = (byte)(seq >> 16);
            tcp[6] = (byte)(seq >> 8); tcp[7] = (byte)seq;
            // Ack
            tcp[8] = (byte)(ack >> 24); tcp[9] = (byte)(ack >> 16);
            tcp[10] = (byte)(ack >> 8); tcp[11] = (byte)ack;
            tcp[12] = 0x50;  // Data offset: 5*4 = 20 Byte
            tcp[13] = 0x18;  // Flags: PSH + ACK
            tcp[14] = 0xFF; tcp[15] = 0xFF; // Window
            Buffer.BlockCopy(payload, 0, tcp, 20, payload.Length);
            // Checksum weglassen (0x0000) – Wireshark ignoriert das bei Loopback
            return tcp;
        }

        private byte[] BuildIpv4(byte[] src, byte[] dst, byte[] payload)
        {
            var total = (ushort)(20 + payload.Length); // TCP-Header + Payload
            var ip = new byte[total];
            ip[0] = 0x45;             // Version + IHL
            ip[2] = (byte)(total >> 8); ip[3] = (byte)total;
            ip[8] = 64;               // TTL
            ip[9] = 6;                // TCP
            Buffer.BlockCopy(src, 0, ip, 12, 4);
            Buffer.BlockCopy(dst, 0, ip, 16, 4);
            Buffer.BlockCopy(payload, 0, ip, 20, payload.Length);
            return ip;
        }
        private byte[] BuildIpv6(byte[] src, byte[] dst, byte[] payload)
        {
            var total = (ushort)(payload.Length + 40); // TCP-Header + Payload
            var ip = new byte[total];

            // Version (4 Bit) + Traffic Class (8 Bit) + Flow Label (20 Bit)
            ip[0] = 0x60; // Version = 6
            ip[1] = 0x00;
            ip[2] = 0x00;
            ip[3] = 0x00;

            // Payload Length (TCP-Header + Daten)
            ip[4] = (byte)(total >> 8);
            ip[5] = (byte)(total);

            ip[6] = 6;    // Next Header = TCP
            ip[7] = 64;   // Hop Limit

            Buffer.BlockCopy(src, 0, ip, 8, 16);
            Buffer.BlockCopy(dst, 0, ip, 24, 16);
            Buffer.BlockCopy(payload, 0, ip, 40, payload.Length);
            return ip;
        }

        public void Dispose() => _writer.Dispose();
    }
    #endregion
}