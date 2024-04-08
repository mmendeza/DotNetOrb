// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.ETF;
using DotNetOrb.Core.IIOP;
using DotNetOrb.Core.Util;
using System;
using System.Collections.Generic;
using Protocol = DotNetOrb.Core.ETF.ListenEndpoint.Protocol;

namespace DotNetOrb.Core.GIOP
{
    public class TransportManager : IConfigurable
    {
        private IConfiguration configuration = null;
        private ILogger logger = null;
        private List<string> factoryClassNames = null;
        private IProfileSelector profileSelector = null;
        public IProfileSelector ProfileSelector => profileSelector;

        private object syncObject = new object();

        /// <summary>
        /// Maps ETF Profile tags (integer) to GIOP connection factories.
        /// </summary>
        private Dictionary<uint, IConnectionFactory> factoriesMap = null;

        /// <summary>
        /// List of all installed GIOPConnection Factories. This list contains an instance 
        /// of each Factory class, ordered in the same way as they were specified 
        /// in the DotNetOrb.Transport.Factories property.
        /// </summary>
        private List<IConnectionFactory> factoriesList = null;

        /// <summary>
        /// List of all IIOP/SLIOP endpoint address-pairs
        /// </summary>
        private Dictionary<Protocol, List<ListenEndpoint>> listenEndpointList = null;

        public TransportManager()
        {

        }

        public void Configure(IConfiguration config)
        {
            configuration = config;
            logger = configuration.GetLogger(GetType().FullName);

            // get factory class names
            factoryClassNames = configuration.GetAsList("DotNetOrb.Transport.Factories");

            if (factoryClassNames.Count == 0)
            {
                factoryClassNames.Add("DotNetOrb.Core.IIOP.IIOPConnectionFactory");
            }

            // pickup listen endpoints specified by arguments -ORBListenEndpoints
            // and populate the array list listenEndpointList
            UpdateListenEndpointAddresses();

            // pickup the default OAAdress/OASSLAddress from prop file
            // and add them to the end of the array list listenEndpointList as well.
            UpdateDefaultEndpointAddresses();

            // get profile selector info
            profileSelector = (IProfileSelector)configuration.GetAsObject("DotNetOrb.Transport.Client.Selector");

            if (profileSelector == null)
            {
                profileSelector = new DefaultProfileSelector();
            }
        }

        internal IConnectionFactory GetFactory(uint tag)
        {
            lock (syncObject)
            {
                if (factoriesMap == null)
                {
                    LoadFactories();
                }
                return factoriesMap[tag];
            }
        }

        /// <summary>
        ///  Returns a list of Factories for all configured transport plugins, 
        ///  in the same order as they were specified in DotNetOrb.Transport.Factories
        /// </summary>        
        internal ICollection<IConnectionFactory> GetFactoriesList()
        {
            lock (syncObject)
            {
                if (factoriesList == null)
                {
                    LoadFactories();
                }
                return factoriesList.ToArray();
            }
        }


        public List<ListenEndpoint> GetListenEndpoints(Protocol p)
        {
            List<ListenEndpoint> endpoints = listenEndpointList[p];

            if (endpoints == null)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Unable to find endpoints for " + p);
                }
                ListenEndpoint l = new ListenEndpoint();
                l.ProtocolId = p;
                endpoints = new List<ListenEndpoint>();
                endpoints.Add(l);
                listenEndpointList.Add(p, endpoints);
            }

            return endpoints;
        }

        private void LoadFactories()
        {
            if (configuration == null)
            {
                throw new BadInvOrder("TransportManager not configured!");
            }

            if (factoryClassNames == null)
            {
                throw new CORBA.Internal("factoryClassNames may not be null");
            }

            factoriesMap = new Dictionary<uint, IConnectionFactory>();
            factoriesList = new List<IConnectionFactory>();

            foreach (var className in factoryClassNames)
            {
                IConnectionFactory factory = InstantiateFactory(className);
                factoriesMap.Add(factory.ProfileTag, factory);
                factoriesList.Add(factory);
            }
        }

        private IConnectionFactory InstantiateFactory(string className)
        {
            try
            {
                var instance = ObjectUtil.GetInstance(className);

                if (instance is IConfigurable configurable)
                {
                    configurable.Configure(configuration);
                }

                logger.Debug("created GIOPConnection Factory: " + className);

                return (IConnectionFactory)instance;
            }
            catch (Exception e)
            {
                throw new BadParam("could not instantiate GIOPConnection factory class " + className + ", exception: " + e);
            }
        }

        private ProtocolAddressBase CreateProtocolAddress(string addressStr)
        {
            IIOPAddress address = new IIOPAddress();
            address.Configure(configuration);
            int protoDelim = addressStr.IndexOf(':');
            Protocol proto;
            try
            {
                proto = (Protocol) Enum.Parse(typeof(Protocol), addressStr.Substring(0, protoDelim).ToUpper());
            }
            catch (ArgumentException e)
            {
                throw new BadParam("Invalid protocol " + addressStr);
            }
            address.Protocol = proto;
            int addressStartOfs = protoDelim + 3;
            if (!address.FromString(addressStr.Substring(addressStartOfs)))
            {
                throw new CORBA.Internal("Invalid protocol address string: " + addressStr);
            }
            // set protocol string
            return address;
        }


        /// <summary>
        /// Pick default OAAdress/OASSLAddress pair from configuration
        /// and add them to the list of endpoint address list.
        /// </summary>
        private void UpdateDefaultEndpointAddresses()
        {
            IIOPAddress address = null;
            IIOPAddress sslAddress = null;

            // If we already have an endpoint list defined there is no point creating a default.
            if (listenEndpointList != null && listenEndpointList.Count > 0)
            {
                return;
            }

            ListenEndpoint defaultEndpoint = new ListenEndpoint();

            string addressStr = configuration.GetValue("OAAddress", null);
            if (addressStr != null)
            {
                // build an iiop/ssliop protocol address.
                // create_protocol_address will allow iiop and ssliop only
                ProtocolAddressBase addr = CreateProtocolAddress(addressStr);
                address = (IIOPAddress)addr;
                address.Configure(configuration);
            }
            else
            {
                int oaPort = configuration.GetAsInteger("OAPort", 0);
                string oaHost = configuration.GetValue("OAIAddr", "");
                address = new IIOPAddress(oaHost, oaPort);
                address.Configure(configuration);
            }

            string sslAddressStr = configuration.GetValue("OASSLAddress", null);
            if (sslAddressStr != null)
            {
                // build a protocol address
                ProtocolAddressBase sslAddr = CreateProtocolAddress(sslAddressStr);
                sslAddress = (IIOPAddress)sslAddr;
                sslAddress.Configure(configuration);

            }
            else
            {
                int sslOAPort = configuration.GetAsInteger("OASSLPort", 0);
                string sslOAHost = configuration.GetValue("OAIAddr", "");
                sslAddress = new IIOPAddress(sslOAHost, sslOAPort);
                sslAddress.Configure(configuration);
            }

            if (address.Protocol == null)
            {
                address.Protocol = Protocol.IIOP;
            }
            if (sslAddress.Protocol == null || sslAddress.Protocol == Protocol.SSLIOP)
            {
                sslAddress.Protocol = Protocol.IIOP;
            }
            defaultEndpoint.Address = address;
            defaultEndpoint.SSLAddress = sslAddress;
            defaultEndpoint.ProtocolId = address.Protocol;

            var s = new List<ListenEndpoint>();
            s.Add(defaultEndpoint);
            listenEndpointList.Add(address.Protocol, s);
        }

        /// <summary>
        /// Pickup endpoint addresses from command-line arguments -ORBListenEndPoints
        /// </summary>
        private void UpdateListenEndpointAddresses()
        {
            listenEndpointList = new Dictionary<Protocol, List<ListenEndpoint>>();

            // get original argument list from ORB
            string[] args = System.Environment.GetCommandLineArgs();

            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == null)
                    {
                        continue;
                    }

                    if (!args[i].Equals("-ORBListenEndpoints", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (i + 1 >= args.Length || args[i + 1] == null)
                    {
                        throw new BadParam("Invalid ORBListenEndpoint <value> format: -ORBListenEndpoints argument without value");
                    }

                    string epArgs = args[i + 1];
                    string epArgsTrim = epArgs.Trim();

                    //check and remove single quotes if needed
                    if (epArgsTrim[0] == '\'' &&
                            epArgsTrim[epArgsTrim.Length - 1] == '\'')
                    {
                        epArgsTrim = epArgs.Trim().Substring(1, epArgs.Trim().Length - 2);
                    }

                    // split up argument into segments using the semi-clone as delimiters
                    string[] segAddrsList = epArgsTrim.Split(';');

                    for (int xx = 0; xx < segAddrsList.Length; xx++)
                    {
                        string segArgs = segAddrsList[xx].Trim();

                        if (segArgs.Equals(""))
                        {
                            continue;
                        }

                        // split up group of args into individual arg segments
                        // using the coma as delimiters
                        string[] indivList = segArgs.Trim().Split(',');
                        for (int xxx = 0; xxx < indivList.Length; xxx++)
                        {
                            string addressStr = null;
                            string sslPort = null;
                            string hostStr = "";
                            string[] optionsArgs = null;

                            string addrArg = indivList[xxx].Trim();
                            if (addrArg.Equals(""))
                            {
                                continue;
                            }

                            // locate the first colon delimiter and
                            // pickup the protocol identifier string
                            int delim = addrArg.IndexOf(":");
                            Protocol protocol;
                            try
                            {
                                protocol = (Protocol)Enum.Parse(typeof(Protocol), addrArg.Substring(0, delim).ToUpper());
                            }
                            catch (ArgumentException e)
                            {
                                throw new BadParam("Invalid ORBListenEndPoints protocol " + addrArg);
                            }

                            // locate the double slash delimiter
                            int dbSlash = addrArg.IndexOf("//", delim + 1);
                            if (dbSlash == -1)
                            {
                                throw new BadParam("Invalid ORBListenEndPoints <value;value;...> format: listen endpoint \'" + addrArg + "\' is malformed!");
                            }

                            // check if additional option delimiter is present
                            // and pick up the protocol address
                            string dbs = "/";
                            if (protocol == Protocol.UIOP)
                            {
                                dbs = "|";
                            }
                            int optSlash = addrArg.IndexOf(dbs, dbSlash + 2);
                            if (optSlash == -1)
                            {
                                addressStr = addrArg.Substring(0);

                            }
                            else
                            {
                                addressStr = addrArg.Substring(0, optSlash);
                            }

                            // pick up optional arguments if present
                            if (optSlash != -1)
                            {
                                optionsArgs = addrArg.Substring(optSlash + 1).Split('&');
                                for (int y = 0; y < optionsArgs.Length; y++)
                                {
                                    string optionsArgsTrim = optionsArgs[xxx].Trim();

                                    int optDelim = optionsArgsTrim.IndexOf('=');
                                    if (optDelim == -1)
                                    {
                                        throw new BadParam("error: listen endpoint options \'" + optionsArgs[y] + "\' is malformed!");
                                    }
                                    else
                                    {
                                        string optStr = optionsArgsTrim.Substring(0, optDelim);
                                        string optValue = optionsArgsTrim.Substring(optDelim + 1);

                                        if (optStr.Equals("ssl_port", StringComparison.OrdinalIgnoreCase))
                                        {
                                            sslPort = optValue;
                                        }
                                        else
                                        {
                                            throw new BadParam("error: listen endpoint options \'" + optionsArgs[y] + "\' is not supported!");
                                        }
                                    }

                                }
                            }

                            if (addressStr != null)
                            {
                                string addressTrim = addressStr.Trim();
                                // build an iiop/ssliop protocol address.
                                // create_protocol_address will allow iiop and ssliop only
                                IIOPAddress address = null;
                                IIOPAddress sslAddress = null;
                                if (protocol == Protocol.IIOP)
                                {
                                    ProtocolAddressBase addr1 = CreateProtocolAddress(addressTrim);
                                    if (addr1 is IIOPAddress)
                                    {
                                        address = (IIOPAddress)addr1;
                                        address.Configure(configuration);
                                    }
                                    if (sslPort != null)
                                    {
                                        int colonDelim = addressTrim.IndexOf(":");
                                        int portDelim = addressTrim.IndexOf(":", colonDelim + 2);
                                        if (portDelim > 0)
                                        {
                                            hostStr = addressTrim.Substring(colonDelim + 3, portDelim - (colonDelim + 3));
                                        }
                                        else
                                        {
                                            hostStr = "";
                                        }

                                        sslAddress = new IIOPAddress(hostStr, int.Parse(sslPort));
                                        sslAddress.Configure(configuration);
                                    }
                                }
                                else if (protocol == Protocol.SSLIOP)
                                {
                                    ProtocolAddressBase addr2 = CreateProtocolAddress(addressTrim);
                                    if (addr2 is IIOPAddress)
                                    {
                                        sslAddress = (IIOPAddress)addr2;
                                        sslAddress.Configure(configuration);
                                    }

                                    //  Set the protocol to IIOP for using IIOP Protocol Factory
                                    protocol = Protocol.IIOP;
                                }
                                else
                                {
                                    ProtocolAddressBase addr1 = CreateProtocolAddress(addressTrim);
                                    if (addr1 is IIOPAddress)
                                    {
                                        address = (IIOPAddress)addr1;
                                        address.Configure(configuration);
                                    }
                                }
                                if (configuration.GetAsBoolean("DotNetOrb.Security.SupportSSL", false) && sslAddress == null)
                                {
                                    throw new BadParam("Error: an SSL port (ssl_port) is required when property DotNetOrb.Security.SupportSSL is enabled");
                                }

                                ListenEndpoint listenEp = new ListenEndpoint();
                                listenEp.Address = address;
                                listenEp.SSLAddress = sslAddress;
                                listenEp.ProtocolId = protocol;

                                if (!listenEndpointList.ContainsKey(protocol))
                                {
                                    var s = new List<ListenEndpoint>();
                                    listenEndpointList.Add(protocol, s);
                                }
                                listenEndpointList[protocol].Add(listenEp);
                            }
                        }
                    }
                }
            }
        }
    }

}
