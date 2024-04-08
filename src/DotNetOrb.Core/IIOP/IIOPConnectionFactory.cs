// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using DotNetOrb.Core.ETF;
using DotNetOrb.Core.GIOP;
using ETF;
using IOP;

namespace DotNetOrb.Core.IIOP
{
    internal class IIOPConnectionFactory : IConnectionFactory
    {
        private ORB orb;
        private IConfiguration configuration;

        public uint ProfileTag
        {
            get => TAG_INTERNET_IOP.Value;
        }
        public void Configure(IConfiguration configuration)
        {
            orb = configuration.ORB;
            this.configuration = configuration;
        }

        public ClientConnection CreateConnection(IProfile profile)
        {
            return new ClientIIOPConnection(orb, profile);
        }

        public Listener CreateListener(ListenEndpoint listenEndpoint)
        {
            return new IIOPListener(orb, listenEndpoint);
        }

        /// <summary>
        /// Demarshall and return the correct type of profile
        /// </summary>
        public IProfile DemarshalProfile(ref TaggedProfile taggedProfile, out TaggedComponent[] components)
        {
            ProfileBase profile = new IIOPProfile();

            profile.Configure(configuration);

            components = new TaggedComponent[0];

            profile.Demarshal(ref taggedProfile, ref components);

            return profile;
        }

        public ProtocolAddressBase CreateProtocolAddress(string addr)
        {
            int addressStart = MatchTag(addr);
            ProtocolAddressBase address = new IIOPAddress();
            address.Configure(configuration);

            if (addressStart >= 0)
            {
                // general form is "prot://address"
                if (!address.FromString(addr.Substring(addressStart + 2)))
                {
                    throw new CORBA.Internal("Invalid protocol address string: " + address);
                }
            }
            return address;
        }

        public IProfile DecodeCorbaloc(string corbaloc)
        {
            int colon = corbaloc.IndexOf(':');
            string token = corbaloc.Substring(0, colon).ToLower();
            if (token.Length == 0 || "iiop".Equals(token) || "ssliop".Equals(token))
            {
                IIOPProfile result = new IIOPProfile(corbaloc);
                try
                {
                    result.Configure(configuration);
                }
                catch (ConfigException e)
                {
                    throw new CORBA.Internal("ConfigurationException: " + e);
                }
                return result;
            }
            return null;
        }

        private int MatchTag(string address)
        {
            if (address == null)
            {
                return -1;
            }
            int colon = address.IndexOf(':');
            string token = address.Substring(0, colon).ToLower();
            if ("iiop".Equals(token) || "ssliop".Equals(token))
            {
                return colon + 1;
            }
            return -1;
        }
    }
}
