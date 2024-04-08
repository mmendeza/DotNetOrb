// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using PortableServer;

namespace DotNetOrb.Core.POA
{
    public class LocalInvocationContext : IInvocationContext
    {
        public byte[] ObjectId { get; set; }
        public ORB ORB { get; set; }
        public POA POA { get; set; }
        public Servant Servant { get; set; }

        public LocalInvocationContext(byte[] objectId, ORB orb, POA poa, Servant servant)
        {
            ObjectId = objectId;
            ORB = orb;
            POA = poa;
            Servant = servant;
        }
    }
}
