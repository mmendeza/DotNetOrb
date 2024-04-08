// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.Core.ETF
{
    /// <summary>
    /// The purpose of the ListenEndpoint class is to hold a set of two endpoint
    /// address and sslAddress, and other informations that are relevant to the endpoint.
    /// Upon being initiated, the TransportManager will create a list of default ListenEndpoint 
    /// objects for the endpoints that are specified by the network properties OAAddress, OASSLAddress, 
    /// OASSLPort, OAPort and OAIAddr, and it will create an overriding list of endpoints that
    /// are specified by the command-line -ORBListendEndpints argument. The TransportManager 
    /// will then store the ListenEndpoint objects which will be used by BasicAdapter to update 
    /// the listeners for the assigned ListenEndpoint object.
    /// </summary>
    public class ListenEndpoint
    {
        public enum Protocol
        {
            IIOP, MIOP, UIOP, NIOP, SSLIOP, DIOP
        }

        public static Protocol MapProfileTag(uint tag)
        {
            if (tag == IOP.TAG_UIPMC.Value)
            {
                return Protocol.MIOP;
            }
            else
            {
                return Protocol.IIOP;
            }
        }

        private ProtocolAddressBase address;

        public ProtocolAddressBase Address
        {
            get
            {
                return address?.Copy();
            }
            set
            {
                address = value;
            }

        }

        private ProtocolAddressBase sslAddress;

        public ProtocolAddressBase SSLAddress
        {
            get
            {
                return sslAddress?.Copy();
            }
            set
            {
                sslAddress = value;
            }

        }
        public Protocol ProtocolId { get; set; }

    }

}
