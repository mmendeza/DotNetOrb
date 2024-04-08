// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using System.Configuration;
using System.Net;
using SSLIOP;
using CSIIOP;
using ETF;
using IIOP;
using Version = GIOP.Version;
using VersionHelper = GIOP.VersionHelper;
using IOP;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.CDR;
using DotNetOrb.Core.ETF;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DotNetOrb.Core.IIOP
{
    public class IIOPProfile : ProfileBase, ICloneable
    {
        private IIOPAddress primaryAddress = null;
        private SSLIOP.SSL ssl = null;
        private bool isSSLSet;

        /// <summary>
        /// the following is used as a bit mask to check if any of these options are set
        /// </summary>
        private static AssociationOptions MinimumOptions =
            AssociationOptions.Integrity |
            AssociationOptions.Confidentiality |
            AssociationOptions.DetectReplay |
            AssociationOptions.DetectMisordering |
            AssociationOptions.EstablishTrustInTarget |
            AssociationOptions.EstablishTrustInClient;


        /// <summary>
        /// Flag to specify if we should check components for alternate addresses.
        /// <see cref="GetAlternateAddresses"/>
        /// </summary>
        private bool checkAlternateAddresses;

        private IIOPProfile(bool checkAlternateAddresses) : base()
        {
            this.checkAlternateAddresses = checkAlternateAddresses;
        }

        public IIOPProfile() : this(true)
        {
        }

        public IIOPProfile(byte[] data) : this(true)
        {
            InitFromProfileData(data);
        }

        public IIOPProfile(IIOPAddress address, byte[] objectKey, int minor) : this(false)
        {
            version = new Version(1, (byte)minor);
            primaryAddress = address;
            this.objectKey = objectKey;
            components = new TaggedComponentList();
        }

        /// <summary>
        /// Constructs an IIOPProfile from a corbaloc URL.  Only to be used from the corbaloc parser.
        /// </summary>
        public IIOPProfile(string corbaloc) : this(false)
        {
            version = null;
            primaryAddress = null;
            objectKey = null;
            components = null;
            corbalocStr = corbaloc;
        }


        public new void Configure(IConfiguration config)
        {
            base.Configure(config);

            logger = configuration.GetLogger(GetType().FullName);

            if (primaryAddress != null)
            {
                primaryAddress.Configure(configuration);
            }
            if (corbalocStr != null)
            {
                DecodeCorbaloc(corbalocStr);
            }
            AddAlternateAddresses(configuration);
        }

        /// <summary>
        /// An IPv6 corbaloc URL is of the format corbaloc:iiop:[fe80:5443::3333%3]:2809/my_object 
        /// where the zone ID seperator is / or % depending on what the underlying OS supports. 
        /// This preserves compatilibility with TAO, and falls in line with RFC 2732 and discussion on OMG news groups.
        /// </summary>
        private void DecodeCorbaloc(string address)
        {
            string addr = address;
            string host = "127.0.0.1"; //default to localhost
            short port = 2809; // default IIOP port
            int major = 1; // should be 1 by default. see 13.6.10.3
            int minor = 0; // should be 0 by default. see 13.6.10.3
            string errorstr = "Illegal IIOP protocol format in object address format: " + addr;
            int sep = addr.IndexOf(':');
            string protocolIdentifier = "";
            if (sep != 0)
            {
                protocolIdentifier = addr.Substring(0, sep);
            }
            if (sep + 1 == addr.Length)
            {
                throw new ArgumentException(errorstr);
            }
            addr = addr.Substring(sep + 1);
            // decode optional version number
            sep = addr.IndexOf('@');
            if (sep > -1)
            {
                string ver_str = addr.Substring(0, sep);
                addr = addr.Substring(sep + 1);
                sep = ver_str.IndexOf('.');
                if (sep != -1)
                {
                    try
                    {
                        major = int.Parse(ver_str.Substring(0, sep));
                        minor = int.Parse(ver_str.Substring(sep + 1));
                    }
                    catch (FormatException)
                    {
                        throw new ArgumentException(errorstr);
                    }
                }
            }
            version = new Version((byte)major, (byte)minor);
            int ipv6SeparatorStart = -1;
            int ipv6SeperatorEnd = -1;
            ipv6SeparatorStart = addr.IndexOf('[');
            if (ipv6SeparatorStart != -1)
            {
                ipv6SeperatorEnd = addr.IndexOf(']');
                if (ipv6SeperatorEnd == -1)
                {
                    throw new ArgumentException(errorstr);
                }
            }

            sep = addr.IndexOf(':');
            if (sep != -1)
            {
                if (ipv6SeparatorStart != -1) //IPv6
                {
                    host = addr.Substring(ipv6SeparatorStart + 1, ipv6SeperatorEnd - (ipv6SeparatorStart + 1));
                    if (addr[ipv6SeperatorEnd + 1] == ':')
                    {
                        try
                        {
                            port = short.Parse(addr.Substring(ipv6SeperatorEnd + 2));
                        }
                        catch (FormatException)
                        {
                            throw new ArgumentException(errorstr);
                        }
                    }
                    else
                    {
                        throw new ArgumentException(errorstr);
                    }
                }
                else //IPv4 or hostname
                {
                    try
                    {
                        port = short.Parse(addr.Substring(sep + 1));
                        host = addr.Substring(0, sep);
                    }
                    catch (FormatException)
                    {
                        throw new ArgumentException(errorstr);
                    }
                }
            }
            primaryAddress = new IIOPAddress(host, port);
            primaryAddress.Configure(configuration);

            DecodeExtensions(protocolIdentifier.ToLower());
        }

        private void DecodeExtensions(string ident)
        {
            components = new TaggedComponentList();
            if (ident.Equals("ssliop"))
            {
                ssl = new SSLIOP.SSL();
                ssl.Port = (ushort)primaryAddress.Port;
                ssl.TargetSupports = GetSSLOptions("DotNetOrb.Security.SSL.corbalocSSLIOP.SupportedOptions");
                ssl.TargetRequires = GetSSLOptions("DotNetOrb.Security.SSL.corbalocSSLIOP.RequiredOptions");
                isSSLSet = true;
                //create the tagged component containing the ssl struct
                CDROutputStream outputStream = new CDROutputStream();
                try
                {
                    outputStream.BeginEncapsulatedArray();
                    SSLHelper.Write(outputStream, ssl);

                    // TAG_SSL_SEC_TRANS must be disambiguated in case OpenORB-generated
                    // OMG classes are in the classpath.
                    components.AddComponent(new TaggedComponent(SSLIOP.TAG_SSL_SEC_TRANS.Value, outputStream.GetBufferCopy()));
                }
                finally
                {
                    outputStream.Close();
                }
            }
        }

        private ushort GetSSLOptions(string propname)
        {
            //For the time being, we only use EstablishTrustInTarget,
            //because we don't handle any of the other options anyway.
            // So this makes a reasonable default.

            // 16 is the base as we take the string value as hex
            var value = (ushort)configuration.GetAsInteger(propname, EstablishTrustInTarget.Value, 16);
            return value;
        }

        public void SetAlternateAddresses(List<IIOPAddress> alternates)
        {
            if (components == null)
            {
                components = new TaggedComponentList();
            }
            foreach (IIOPAddress address in alternates)
            {
                checkAlternateAddresses = true;
                components.AddComponent(TAG_ALTERNATE_IIOP_ADDRESS.Value, address.ToCDR());
            }
        }

        /// <summary>
        /// Adds further addresses to this profile as TAG_ALTERNATE_IIOP_ADDRESS,
        /// if this has been configured in the Configuration.
        /// </summary>
        private void AddAlternateAddresses(IConfiguration config)
        {
            string value = config.GetValue("DotNetOrb.IIOP.AlternateAddresses", null);
            if (value == null)
            {
                return;
            }
            else if (value.Trim().Equals("auto"))
            {
                AddNetworkAddresses();
            }
            else
            {
                List<string> addresses = value.Split(',').ToList();
                if (addresses.Count > 0 && components == null)
                {
                    components = new TaggedComponentList();
                }
                foreach (var addr in addresses)
                {
                    IIOPAddress iaddr = new IIOPAddress();
                    iaddr.Configure(config);
                    if (!iaddr.FromString(addr))
                    {
                        logger.Warn("could not decode " + addr + " from DotNetOrb.IIOP.AlternateAddresses");
                        continue;
                    }
                    else
                    {
                        checkAlternateAddresses = true;
                        components.AddComponent(TAG_ALTERNATE_IIOP_ADDRESS.Value, iaddr.ToCDR());
                    }
                }
            }
        }

        /// <summary>
        /// Method for use by IIOPListener to add all possible network addresses that are listened on by a wildcard listener.
        /// </summary>
        public void AddAllWildcardAddresses(IConfiguration config)
        {
            if (primaryAddress == null)
            {
                return;
            }

            if (primaryAddress.IsWildcard)
            {
                AddNetworkAddresses();
            }
        }

        /// <summary>
        /// Adds all the network addresses of this machine to the profile as TAG_ALTERNATE_IIOP_ADDRESS.
        /// This excludes loopback addresses, and the address that is already used as the primary address.
        /// Also excluded are non-IPv4 addresses for the moment.
        /// </summary>
        private void AddNetworkAddresses()
        {
            if (primaryAddress == null) return;
            if (components == null) components = new TaggedComponentList();

            string primaryIP = primaryAddress.Address.ToString();
            if (logger.IsDebugEnabled)
            {
                logger.Debug("primaryIP is <" + primaryIP + ">");
            }
            foreach (var addr in IIOPAddress.GetNetworkIPAddresses())
            {
                if (!IPAddress.IsLoopback(addr) && !addr.IsLinkLocal())
                {
                    IIOPAddress iaddr = new IIOPAddress();
                    iaddr.Configure(configuration);
                    string ipaddr = addr.ToString();
                    if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        iaddr.FromString(ipaddr + ":" + primaryAddress.Port);
                    }
                    else if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        iaddr.FromString("[" + ipaddr + "]:" + primaryAddress.Port);
                    }
                    if (!iaddr.Address.ToString().Equals(primaryIP))
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("components.addComponent: adding addrHostAddress <" + iaddr.HostName + "> as TAG_ALTERNATE_IIOP_ADDRESS");
                        }
                        checkAlternateAddresses = true;
                        components.AddComponent(TAG_ALTERNATE_IIOP_ADDRESS.Value, iaddr.ToCDR());
                    }
                }
            }
        }

        ///<summary>
        /// Writes the bytes that would make up the ETF::AddressProfile bytes (new spec) to a stream.
        /// Writes GIOP version, host string, and port.
        ///</summary>
        public override void WriteAddressProfile(CDROutputStream addressProfileStream)
        {
            VersionHelper.Write(addressProfileStream, version);
            primaryAddress.Write(addressProfileStream);
        }

        /// <summary>
        /// Reads the bytes that make up the ETF::AddressProfile bytes (new spec) from a stream.
        /// Reads GIOP version, host string, and port.
        /// </summary>
        public override void ReadAddressProfile(CDRInputStream addressProfileStream)
        {
            version = VersionHelper.Read(addressProfileStream);

            string hostname = addressProfileStream.ReadString();
            ushort port = addressProfileStream.ReadUShort();

            primaryAddress = new IIOPAddress(hostname, port);

            if (configuration != null)
            {
                try
                {
                    primaryAddress.Configure(configuration);
                }
                catch (ConfigurationException ex)
                {
                    logger.Error("Error configuring address", ex);
                    throw new CORBA.Internal("Error configuring address" + ex);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IIOPProfile))
                return false;
            else
                return IsMatch(obj as IProfile);
        }

        public override uint Hash()
        {
            return (uint)primaryAddress.GetHashCode();
        }

        public override int GetHashCode()
        {
            return primaryAddress.GetHashCode();
        }

        public override string ToString()
        {
            return primaryAddress.ToString();
        }

        public override object Clone()
        {
            IIOPProfile result = new IIOPProfile();
            result.primaryAddress = new IIOPAddress(primaryAddress.HostName, primaryAddress.Port);
            result.isSSLSet = isSSLSet;
            result.ssl = ssl;

            if (configuration != null)
            {
                try
                {
                    result.primaryAddress.Configure(configuration);
                }
                catch (ConfigurationException ex)
                {
                    logger.Error("Error configuring address", ex);
                    throw new CORBA.Internal("Error configuring address" + ex);
                }
            }

            result.version = new Version(version.Major, version.Minor);

            if (objectKey != null)
            {
                var tmp = new byte[objectKey.Length];
                Array.Copy(objectKey.ToArray(), 0, tmp, 0, objectKey.Length);
                result.objectKey = tmp;
            }
            if (components != null)
            {
                result.components = (TaggedComponentList)components.Clone();
            }

            return result;
        }



        /// <summary>
        /// This function shall determine if the passed profile, prof, is a match
        /// to this profile.The specifics of the match are left to the details
        /// of the underlying transport, however profiles shall be considered a
        /// match, if they would create connections that share the same attributes
        /// relevant to the transport setup.  Among others, this could include
        /// address information (eg.host address) and transport layer
        /// characteristics (eg.encryption levels). If a match is found, it
        /// shall return true, or false otherwise.
        /// </summary>
        public override bool IsMatch(IProfile prof)
        {
            if (prof == null)
            {
                return false;
            }

            if (prof is IIOPProfile)
            {
                IIOPProfile other = (IIOPProfile)prof;
                return

                    GetSSLPort() == other.GetSSLPort() &&
                    primaryAddress.Equals(other.primaryAddress) &&
                    GetAlternateAddresses().Equals(other.GetAlternateAddresses())
                ;
            }
            return false;
        }

        public override int Tag
        {
            get
            {
                return (int)TAG_INTERNET_IOP.Value;
            }
        }

        public ProtocolAddressBase GetAddress()
        {
            return primaryAddress;
        }

        /// <summary>
        /// Replaces the host in this profile's primary address with newHost 
        /// (if it is not null), and the port with newPort (if it is not -1).
        /// </summary>
        /// <param name="replacement"></param>
        public override void PatchPrimaryAddress(ProtocolAddressBase replacement)
        {
            if (replacement is IIOPAddress)
            {
                primaryAddress.ReplaceFrom((IIOPAddress)replacement);
            }
        }

        public List<IIOPAddress> GetAlternateAddresses()
        {
            if (checkAlternateAddresses)
            {
                var comp = components.GetComponents(configuration, TAG_ALTERNATE_IIOP_ADDRESS.Value, typeof(IIOPAddress));
                var tmp = new List<IIOPAddress>();
                foreach (var component in comp)
                {
                    if (component is IIOPAddress addr)
                    {
                        tmp.Add(addr);
                    }
                }
                return tmp;
            }
            else
            {
                return new List<IIOPAddress>();
            }
        }

        public SSLIOP.SSL GetSSL()
        {
            if (!isSSLSet)
            {
                // TAG_SSL_SEC_TRANS must be disambiguated in case OpenORB-generated
                // OMG classes are in the classpath.
                ssl = (SSLIOP.SSL)components.GetComponent(IOP.TAG_SSL_SEC_TRANS.Value, typeof(SSLHelper));
                isSSLSet = true;
            }
            return ssl;
        }


        ///<summary>
        /// Returns the port on which SSL is available according to this profile, or -1 if SSL is not supported.
        ///</summary>
        public int GetSSLPort()
        {
            TLS_SEC_TRANS tls = GetTlsSpecFromCSIComponent();
            if (tls != null && tls.Addresses.Length > 0)
            {
                return tls.Addresses[0].Port;
            }
            else
            {
                GetSSL();

                if (ssl != null)
                {
                    return ssl.Port;
                }
                else
                {
                    return -1;
                }
            }
        }

        ///<summary>
        /// Returns a copy of this profile that is compatible with GIOP 1.0.
        ///</summary>
        public IIOPProfile ToGIOP10()
        {
            IIOPProfile result = new IIOPProfile(primaryAddress, objectKey, 0);
            return result;
        }

        ///<summary>
        /// Returns the SSL port which should be used to communicate with an IOR containing this profile.
        /// Returns -1 if SSL should not be used.
        ///
        /// SSL is enabled only if both client and target support it, and at least one requires it.Target options
        /// are determined by reading either the SSL_SEC_TRANS component or the CSI_SEC_MECH_LIST component, if it specifies
        /// transport-level security.If both are present, the latter is used.
        ///
        /// <paramref name="clientRequired">the CSIv2 features required by the client</paramref>
        /// <paramref name="clientSupported">the CSIv2 features supported by the client</paramref>
        /// <returns>an ssl port number or -1, if none</returns>        
        ///</summary>
        internal int GetSslPortIfSupported(ushort clientRequired, ushort clientSupported)
        {
            TLS_SEC_TRANS tls = GetTlsSpecFromCSIComponent();
            var ssl = (SSLIOP.SSL)GetComponent(IOP.TAG_SSL_SEC_TRANS.Value, typeof(SSLHelper));

            if (tls != null && UseSsl(clientSupported, clientRequired, tls.TargetSupports, tls.TargetRequires))
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Selecting TLS for connection");
                }
                return tls.Addresses[0].Port;
            }
            else if (ssl != null && UseSsl(clientSupported, clientRequired, ssl.TargetSupports, ssl.TargetRequires))
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Selecting SSL for connection");
                }
                return ssl.Port;
            }
            else if (((AssociationOptions)clientRequired & MinimumOptions) != 0)
            {
                throw new NoPermission("Client-side policy requires SSL/TLS, but server doesn't support it");
            }
            return -1;
        }


        private static int AdjustedPortNum(short port)
        {
            return port < 0 ? port + 65536 : port;
        }


        private bool UseSsl(ushort clientSupports, ushort clientRequires, ushort targetSupports, ushort targetRequires)
        {
            return ((AssociationOptions)targetSupports & MinimumOptions) != 0 && // target supports ssl
                   ((AssociationOptions)clientSupports & MinimumOptions) != 0 && // client supports ssl
                   (((AssociationOptions)targetRequires & MinimumOptions) != 0 || ((AssociationOptions)clientRequires & MinimumOptions) != 0); // either side requires ssl
        }


        private TLS_SEC_TRANS GetTlsSpecFromCSIComponent()
        {
            CompoundSecMechList sas = null;
            try
            {
                sas = (CompoundSecMechList)GetComponent(TAG_CSI_SEC_MECH_LIST.Value, typeof(CompoundSecMechListHelper));
            }
            catch (Exception ex)
            {
                logger.Info("Not able to process security mech. component");
            }

            TLS_SEC_TRANS tls = null;
            if (sas != null && sas.MechanismList[0].TransportMech.Tag == IOP.TAG_TLS_SEC_TRANS.Value)
            {
                try
                {
                    var tagData = sas.MechanismList[0].TransportMech.ComponentData;
                    CDRInputStream inputStream = new CDRInputStream(tagData.ToArray());
                    try
                    {
                        inputStream.OpenEncapsulatedArray();
                        tls = TLS_SEC_TRANSHelper.Read(inputStream);
                    }
                    finally
                    {
                        inputStream.Close();
                    }
                }
                catch (Exception e)
                {
                    logger.Warn("Error parsing TLS_SEC_TRANS: " + e);
                }
            }
            return tls;
        }


        public new List<ListenPoint> AsListenPoints()
        {
            var result = new List<ListenPoint>();

            if (GetSSL() == null)
            {
                result.Add(new ListenPoint(primaryAddress.HostName, (ushort)primaryAddress.Port));
            }
            else
            {
                if (GetSSLPort() == 0)
                {
                    result.Add(new ListenPoint(primaryAddress.HostName, (ushort)primaryAddress.Port));
                }
                else
                {
                    result.Add(new ListenPoint(primaryAddress.HostName, (ushort)GetSSLPort()));
                }
            }
            foreach (var addr in GetAlternateAddresses())
            {
                result.Add(new ListenPoint(addr.HostName, (ushort)addr.Port));
            }
            return result;
        }


        ///<summary>
        ///Copy this (ssl) IIOPProfile. thereby replace the port of the primaryAddress with
        ///the SSL port from the tagged components.
        ///</summary>
        public IIOPProfile ToNonSSL()
        {
            if (GetSSL() != null)
            {
                IIOPAddress address = null;
                IIOPProfile result = null;

                try
                {
                    address = new IIOPAddress(primaryAddress.HostName, GetSSLPort());
                    address.Configure(configuration);

                    result = new IIOPProfile(address, objectKey, configuration.ORB.GiopMinorVersion);
                    result.Configure(configuration);
                }
                catch (ConfigurationException ex)
                {
                    logger.Error("Error configuring address", ex);
                    throw new CORBA.Internal("Error configuring address" + ex);
                }

                TaggedComponent[] taggedComponents = components.AsArray();
                for (int i = 0; i < taggedComponents.Length; i++)
                {
                    if (taggedComponents[i].Tag == SSLIOP.TAG_SSL_SEC_TRANS.Value)
                    {
                        continue;
                    }

                    result.components.AddComponent(taggedComponents[i]);
                }
                return result;
            }
            return this;
        }
    }

}
