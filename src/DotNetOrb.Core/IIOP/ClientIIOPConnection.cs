// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using CSIIOP;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.ETF;
using DotNetOrb.Core.GIOP;
using DotNetty.Handlers.Timeout;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using ETF;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DotNetOrb.Core.IIOP
{
    public class ClientIIOPConnection : ClientConnection
    {
        private bool isSSL;
        private bool supportSSL;
        private int SSLPort = -1;
        private ushort clientRequired = 0;
        private ushort clientSupported = 0;
        private bool keepAlive;
        private int idleTimeout;
        protected int noOfRetries = 5;
        protected int retryInterval = 0;
        protected X509Certificate2 tlsCertificate = null;

        private IIOPAddress? address;
        private IPEndPoint? endpoint;

        protected MultithreadEventLoopGroup group = new MultithreadEventLoopGroup(1);

        private IChannel channel;

        public IChannel Channel => channel;

        public bool IsConnected
        {
            get
            {
                if (channel != null)
                {
                    return channel.Active;
                }
                return false;
            }
        }

        private ClientIIOPChannelHandler channelHandler;
        public override GIOPChannelHandler ChannelHandler => channelHandler;

        public ClientIIOPConnection(ORB orb, IProfile profile) : base(orb, profile)
        {
            channelHandler = new ClientIIOPChannelHandler(orb, this);
            Configure(orb.Configuration);
        }

        public override void Configure(IConfiguration config)
        {
            base.Configure(config);

            keepAlive = config.GetAsBoolean("DotNetOrb.Connection.Client.KeepAlive", true);

            idleTimeout = config.GetAsInteger("DotNetOrb.Connection.Client.IdleTimeout", 0);

            noOfRetries = config.GetAsInteger("DotNetOrb.Retries", 5);
            retryInterval = config.GetAsInteger("DotNetOrb.RetryInterval", 500);
            supportSSL = config.GetAsBoolean("DotNetOrb.Security.SSL.IsSupported", false);

            clientRequired = (ushort)config.GetAsInteger("DotNetOrb.Security.SSL.Client.RequiredOptions", 0x10, 16);

            clientSupported = (ushort)config.GetAsInteger("DotNetOrb.Security.SSL.Client.SupportedOptions", 0x10, 16);

            if (supportSSL)
            {
                SSLPort = ((IIOPProfile)profile).GetSslPortIfSupported(clientRequired, clientSupported);
                isSSL = SSLPort != -1;
            }

            if (isSSL)
            {
                //TODO Retrieve certificate from X509Store ??
                var certPath = config.GetValue("DotNetOrb.IIOP.SSL.Certificate");
                var pwd = config.GetValue("DotNetOrb.IIOP.SSL.CertificatePassword");
                tlsCertificate = new X509Certificate2(certPath, pwd);
                //targetHost = tlsCertificate.GetNameInfo(X509NameType.DnsName, false);
            }


            logger = config.GetLogger(GetType().FullName);

            bootstrap
                   .Group(group)
                   .Channel<TcpSocketChannel>()
                   .Option(ChannelOption.TcpNodelay, true)
                   .Option(ChannelOption.SoKeepalive, keepAlive)
                   .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                   {
                       IChannelPipeline pipeline = channel.Pipeline;
                       ClientTlsSettings settings;
                       if ((clientSupported & EstablishTrustInClient.Value) != 0 && tlsCertificate != null)
                       {
                           settings = new ClientTlsSettings(address?.HostName, new List<X509Certificate>() { tlsCertificate });
                       }
                       else
                       {
                           settings = new ClientTlsSettings(address?.HostName);
                       }
                       if (tlsCertificate != null)
                       {
                           var tls = channel.Pipeline.Get<TlsHandler>();
                           if (tls != null)
                           {
                               channel.Pipeline.Remove(tls);
                           }
                           channel.Pipeline.AddFirst(new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), settings));
                       }
                       if (idleTimeout > 0)
                       {
                           pipeline.AddLast(new IdleStateHandler(0, 0, idleTimeout));
                       }
                       pipeline.AddLast(new GIOPCodec(orb), new GIOPCodeSetClientHandler(orb, this));
                       if (orb.IsBidir)
                       {
                           pipeline.AddLast(new GIOPBiDirClientHandler(orb, this));
                       }
                       pipeline.AddLast(channelHandler);
                   }));
        }

        //public override void Connect()
        //{
        //    channelHandler.Connect();   
        //}

        public override void Connect()
        {
            if (channel == null)
            {
                var addressList = new List<ProtocolAddressBase>();
                addressList.Add(((IIOPProfile)profile).GetAddress());
                addressList.AddRange(((IIOPProfile)profile).GetAlternateAddresses());
                var retries = 0;
                while (!IsConnected && retries < noOfRetries)
                {
                    retries++;
                    foreach (IIOPAddress address in addressList)
                    {
                        this.address = address;
                        var port = isSSL ? SSLPort : address.Port;
                        endpoint = new IPEndPoint(address.Address, port);
                        try
                        {
                            var task = bootstrap.ConnectAsync(endpoint);
                            channel = task.Result;
                            break;
                        }
                        catch (AggregateException ae)
                        {
                            logger.Error("Unable to open client-side TCP/IP transport: " + endpoint, ae.InnerException);
                        }
                    }
                    if (!IsConnected)
                    {
                        Task.Delay(retryInterval).Wait();
                    }
                }
                if (!IsConnected)
                {
                    throw new Transient("Unable to open client-side TCP/IP transport: " + endpoint);
                }
            }
            else if (!channel.Active)
            {
                try
                {
                    var task = channel.ConnectAsync(endpoint);
                    task.Wait();
                }
                catch (AggregateException ae)
                {
                    logger.Error("Unable to open client-side TCP/IP transport: " + endpoint, ae.InnerException);
                }
            }
        }

        public override void Close()
        {
            try
            {
                var task = channel.CloseAsync().ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        logger.Error("Unable to close channel" + channel.RemoteAddress.ToString(), t.Exception?.InnerException);
                    }
                });
                task.Wait();
            }
            catch (Exception ex)
            {
                logger.Error("Unable to close client-side TCP/IP transport", ex);
            }
            finally
            {
                group.ShutdownGracefullyAsync().Wait(1000);
            }

        }
    }

}
