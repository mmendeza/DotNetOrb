// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using CSIIOP;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.ETF;
using SSLIOP;
using System.Net;

namespace DotNetOrb.Core.IIOP
{
    internal class IIOPListener : Listener
    {
        /// <summary>
        /// The maximum set of security options supported by the SSL mechanism 
        /// </summary>
        private static ushort MAX_SSL_OPTIONS = Integrity.Value |
                                                   Confidentiality.Value |
                                                   DetectReplay.Value |
                                                   DetectMisordering.Value |
                                                   EstablishTrustInTarget.Value |
                                                   EstablishTrustInClient.Value;

        /// <summary>
        /// The minimum set of security options supported by the SSL mechanism which cannot be turned off, so they are always supported
        /// </summary>
        private static ushort MIN_SSL_OPTIONS = Integrity.Value | DetectReplay.Value | DetectMisordering.Value;
        private bool supportSSL;
        private ushort targetSupports = 0;
        private ushort targetRequires = 0;
        private bool requireSSL;
        private ListenEndpoint listenEndpoint;
        private IIOPAddress address;
        private IIOPAddress sslAddress;
        private bool alwaysOpenUnsecuredAddress;
        private ServerIIOPConnection serverIIOPConnection;
        private ServerIIOPConnection serverSSLIOPConnection;

        public IIOPListener(ORB orb, ListenEndpoint listenEndpoint)
        {
            this.orb = orb;
            this.listenEndpoint = listenEndpoint;
            configuration = orb.Configuration;
            Configure(configuration);
        }

        public override void Configure(IConfiguration config)
        {
            logger = config.GetLogger(GetType());

            if (listenEndpoint != null)
            {
                if (listenEndpoint.SSLAddress != null)
                {
                    sslAddress = (IIOPAddress)listenEndpoint.SSLAddress;
                }
                if (listenEndpoint.Address != null)
                {
                    address = (IIOPAddress)listenEndpoint.Address;
                }
                if (address != null)
                {
                    address.Configure(config);
                }
                if (sslAddress != null)
                {
                    sslAddress.Configure(config);
                }
            }
            else
            {
                throw new Initialize("listen endpoint may not be null");
            }
            supportSSL = config.GetAsBoolean("DotNetOrb.Security.SSL.IsSupported", false);

            if (supportSSL)
            {
                targetRequires = (ushort)config.GetAsInteger("DotNetOrb.Security.SSL.Server.RequiredOptions", 0x20, 16);
                targetSupports = (ushort)config.GetAsInteger("DotNetOrb.Security.SSL.Server.SupportedOptions", 0, 16);
                // make sure that the minimum options are always in the set of supported options
                targetSupports |= MIN_SSL_OPTIONS;
                //if we require EstablishTrustInTarget or EstablishTrustInClient, SSL must be used.
                requireSSL = (targetRequires & MAX_SSL_OPTIONS) != 0;
            }

            alwaysOpenUnsecuredAddress = config.GetAsBoolean("DotNetOrb.Security.SSL.AlwaysOpenUnsecuredAddress", false);

            if (address != null && (!requireSSL || alwaysOpenUnsecuredAddress))
            {
                serverIIOPConnection = new ServerIIOPConnection(orb, address, false);
                //serverIIOPConnection.Connect();
            }

            if (sslAddress != null && supportSSL)
            {
                serverSSLIOPConnection = new ServerIIOPConnection(orb, sslAddress, true);
                //serverSSLIOPConnection.Connect();
            }
        }

        public override void Destroy()
        {
            if (serverIIOPConnection != null)
            {
                serverIIOPConnection.Close();
            }
            if (serverSSLIOPConnection != null)
            {
                serverSSLIOPConnection.Close();
            }
        }

        public override void Listen()
        {
            if (serverIIOPConnection != null)
            {
                serverIIOPConnection.Connect();
            }
            if (serverSSLIOPConnection != null)
            {
                serverSSLIOPConnection.Connect();
            }
            profile = CreateAddressProfile();
            if (serverIIOPConnection != null)
            {
                serverIIOPConnection.Profile = profile;
            }
            if (serverSSLIOPConnection != null)
            {
                serverSSLIOPConnection.Profile = profile;
            }
        }

        private IIOPProfile CreateAddressProfile()
        {
            IIOPProfile result = null;

            if (serverIIOPConnection != null)
            {
                if (address.Port == 0 && serverIIOPConnection.Channel != null)
                {
                    address.Port = ((IPEndPoint)serverIIOPConnection.Channel.LocalAddress).Port;
                }
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("IIOPAddress using port " + address.Port);
                }

                if (address.HostName == null && serverIIOPConnection.Channel != null)
                {
                    address.HostName = ((IPEndPoint)serverIIOPConnection.Channel.LocalAddress).Address.ToString();
                }

                result = new IIOPProfile(address, null, orb.GiopMinorVersion);
                result.Configure(configuration);

                // Add all wildcard addresses to the list of alternative addresses
                if (address.IsWildcard)
                {
                    result.AddAllWildcardAddresses(configuration);
                }
            }
            else if (serverSSLIOPConnection == null)
            {
                throw new Initialize("no listeners found, cannot create address profile");
            }

            if (serverSSLIOPConnection != null && serverSSLIOPConnection.Channel != null)
            {
                if (sslAddress.Port == 0)
                {
                    sslAddress.Port = ((IPEndPoint)serverSSLIOPConnection.Channel.LocalAddress).Port;
                }

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("IIOPAddress using ssl port " + sslAddress.Port);
                }

                if (sslAddress.GetConfiguredHost() == null && serverSSLIOPConnection.Channel != null)
                {
                    address.HostName = ((IPEndPoint)serverSSLIOPConnection.Channel.LocalAddress).Address.ToString();
                }

                // This means a non-ssl profile hasn't been created, so create it here.
                if (result == null)
                {
                    IIOPAddress tempAddress = new IIOPAddress(sslAddress.HostName, 0);
                    if (address != null && address.Port != 0)
                    {
                        tempAddress.Port = address.Port;
                    }
                    result = new IIOPProfile(tempAddress, null, orb.GiopMinorVersion);
                    result.Configure(configuration);

                    // Add all wildcard addresses to the list of alternative addresses
                    if (tempAddress.IsWildcard)
                    {
                        result.AddAllWildcardAddresses(configuration);
                    }
                }

                result.AddComponent(TAG_SSL_SEC_TRANS.Value, new SSLIOP.SSL(targetSupports, targetRequires, (ushort)sslAddress.Port), typeof(SSLHelper));
            }

            return result;
        }
    }



}
