// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.ETF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using static DotNetOrb.Core.ETF.ListenEndpoint;

namespace DotNetOrb.Core.IIOP
{
    public class IIOPAddress : ProtocolAddressBase
    {
        public static string[] NetworkVirtualInterfaces = { };

        private IPHostEntry host = null;
        public IPHostEntry Host
        {
            get => host;
        }

        private string hostName = null;
        public string HostName
        {
            get
            {
                if (hostName != null)
                {
                    return hostName;
                }
                else if (host != null)
                {
                    return host.HostName;
                }
                else if (address != null)
                {
                    return address.ToString();
                }
                else
                {
                    return null;
                }
            }
            set
            {
                //try to parse address
                try
                {
                    IPAddress ipAddress = IPAddress.Parse(value);
                    if (ipAddress.Equals(IPAddress.Any) || ipAddress.Equals(IPAddress.IPv6Any))
                    {
                        IsWildcard = true;
                    }
                    else
                    {
                        address = ipAddress;
                        hostName = value;
                    }
                    try
                    {
                        host = Dns.GetHostEntry(value);
                    }
                    catch (SocketException)
                    {
                        //throw new CORBA.Internal("Unable to resolve host " + value);
                    }
                }
                catch (FormatException)
                {
                    //try to resolve host
                    if (!string.IsNullOrEmpty(value))
                    {
                        hostName = value;
                    }
                    IsWildcard = true;
                    try
                    {
                        host = Dns.GetHostEntry(value);
                    }
                    catch (SocketException)
                    {
                        throw new CORBA.Internal("Unable to resolve host " + value);
                    }
                }
            }
        }

        private IPAddress address = null;
        public IPAddress Address
        {
            get
            {
                if (address != null)
                {
                    return address;
                }
                else if (host != null)
                {
                    foreach (var ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork ||
                            ip.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            address = ip;
                            return address;
                        }
                    }
                }
                return null;
            }
            set
            {
                address = value;
            }
        }

        private int port = -1; // 0 .. 65536
        public int Port
        {
            get { return port; }
            set
            {
                if (value < 0)
                {
                    port = value + 65536;
                }
                else
                {
                    port = value;
                }
            }
        }

        public Protocol Protocol { get; set; }
        public bool IsWildcard { get; private set; }

        /// <summary>
        /// Creates a new IIOPAddress that will be initialized later by a string
        /// </summary>
        public IIOPAddress() : base()
        {
        }

        /// <summary>
        /// Creates a new IIOPAddress for host and port.        
        /// </summary>
        /// <param name="hostStr">Either a DNS name, or a textual representation of a numeric IP address (dotted decimal)</param>
        /// <param name="port">
        /// The port number represented as an integer, in the range 0..65535. As a special convenience, a negative number is converted 
        /// by adding 65536 to it; this helps using values that were previously stored in a short
        /// </param>
        public IIOPAddress(string hostStr, int port) : this()
        {
            HostName = hostStr;
            Port = port;
        }

        public override void Configure(IConfiguration config)
        {
            configuration = config;
        }

        // This is called by TaggedComponentList.GetComponents from IIOPProfile.
        public static IIOPAddress Read(IInputStream input)
        {
            string host = input.ReadString();
            ushort port = input.ReadUShort();

            return new IIOPAddress(host, port);
        }


        /**
         * Returns the host as supplied to the constructor. This replaces
         * IIOPListener.getConfiguredHost().
         * Used by the IIOPListener to retrieve the host address for a wildcard listener after
         * the server socket has been instantiated.
         */
        public IPAddress GetConfiguredHost()
        {
            if (hostName == null || hostName.Length == 0)
            {
                return null;
            }
            return Address;
        }


        public override bool Equals(object? other)
        {
            if (other is IIOPAddress)
            {
                return ToString().Equals(other.ToString());
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return HostName + ":" + port;
        }

        public override bool FromString(string s)
        {
            if (s[0] == '[')
            {
                return FromStringIPv6(s);
            }
            return FromStringIPv4(s);
        }

        // NOTE: IPv6 format is "[address]:port" since address will include colons.
        private bool FromStringIPv6(string s)
        {
            int endBracket = s.IndexOf(']');
            if (endBracket < 0)
            {
                return false;
            }

            int routeDelim = s.LastIndexOf('%', endBracket);

            if (routeDelim < 0)
            {
                HostName = s.Substring(1, endBracket - 1);
            }
            else
            {
                HostName = s.Substring(1, routeDelim - 1);
            }

            int colon = s.IndexOf(':', endBracket);
            if (colon < 0)
            {
                return false;
            }
            try
            {
                Port = int.Parse(s.Substring(colon + 1));
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool FromStringIPv4(string s)
        {
            int colon = s.IndexOf(':');
            if (colon == -1)
            {
                return false;
            }

            if (colon > 0)
            {
                HostName = s.Substring(0, colon);
            }
            else
            {
                HostName = "";
            }
            if (colon < s.Length - 1)
            {
                try
                {
                    Port = int.Parse(s.Substring(colon + 1));
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public override void Write(CDROutputStream output)
        {
            // If host name contains a zone ID, we need to remove it.
            // This would be used to write the address on an IOR or other
            // things that could be used off-host. Writing a link-local zone
            // ID would break the client. Site-local zone IDs are still used,
            // but deprecated. For now, we will ignore site-local zone IDs.
            output.WriteString(HostName);
            output.WriteUShort((ushort)Port);
        }

        /**
         * Package level method used by IIOPProfile to cause selective replacement of either the
         * hostname or the port or both
         */
        public void ReplaceFrom(IIOPAddress other)
        {
            if (other.HostName != null)
            {
                HostName = other.HostName;
            }
            if (other.port != -1)
            {
                Port = other.Port;
            }
        }

        /**
         * Returns a string representation of the localhost address.
         */
        public static string GetLocalHostAddress()
        {
            var addr = GetLocalHost();
            if (addr != null)
            {
                return addr.ToString();
            }
            else
            {
                return "127.0.0.1";
            }
        }


        /// <summary>
        /// Returns an address for the localhost that is reasonable to use in the IORs we produce.
        /// </summary>
        public static IPAddress GetLocalHost()
        {
            return GetNetworkIPAddresses().FirstOrDefault();
        }


        /// <summary>
        /// Returns an ordered list of InetAddresses. Order is:
        /// IPv4/IPv6 routable address Point-to-point address Fallback to link-local and finally loopback.
        /// </summary>
        public static List<IPAddress> GetNetworkIPAddresses()
        {
            List<IPAddress> result = new List<IPAddress>();
            List<IPAddress> virtuals = new List<IPAddress>();
            List<IPAddress> p2plinklocal = new List<IPAddress>();
            List<IPAddress> loopback = new List<IPAddress>();

            try
            {
                var ifzs = NetworkInterface.GetAllNetworkInterfaces();
                foreach (var ni in ifzs)
                {
                    //if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        bool isVirtual = false;
                        foreach (var nvi in NetworkVirtualInterfaces)
                        {
                            string displayName = ni.Description;
                            if (displayName != null && displayName.Contains(nvi))
                            {
                                isVirtual = true;
                                break;
                            }
                        }
                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Ppp)
                        {
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork ||
                                    ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                                {
                                    p2plinklocal.Add(ip.Address);
                                }
                            }
                        }
                        else if (isVirtual)
                        {
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork ||
                                    ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                                {
                                    if (!IPAddress.IsLoopback(ip.Address) && !ip.Address.IsLinkLocal())
                                    {
                                        virtuals.Add(ip.Address);
                                    }
                                    else if (ip.Address.IsIPv6LinkLocal)
                                    {
                                        p2plinklocal.Add(ip.Address);
                                    }
                                    else
                                    {
                                        loopback.Add(ip.Address);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork ||
                                    ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                                {
                                    if (!IPAddress.IsLoopback(ip.Address) && !ip.Address.IsLinkLocal())
                                    {
                                        if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                                        {
                                            result.Add(ip.Address);
                                        }
                                        else
                                        {
                                            result.Insert(0, ip.Address);
                                        }
                                    }
                                    else if (ip.Address.IsLinkLocal())
                                    {
                                        p2plinklocal.Add(ip.Address);
                                    }
                                    else
                                    {
                                        loopback.Add(ip.Address);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception se)
            {
                throw new CORBA.Internal("Unable to determine network interfaces: " + se);
            }

            result.AddRange(virtuals);
            result.AddRange(p2plinklocal);
            result.AddRange(loopback);
            return result;
        }

        public override ProtocolAddressBase Copy()
        {
            IIOPAddress result = new IIOPAddress(HostName, Port);
            return result;
        }
    }
}
