// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using PortableServer;

namespace DotNetOrb.Core.POA
{
    public interface IAOMListener : IEventListener
    {
        void ObjectActivated(byte[] oid, Servant servant, int size);
        void ObjectDeactivated(byte[] oid, Servant servant, int size);
        void ServantEtherialized(byte[] oid, Servant servant);
        void ServantIncarnated(byte[] oid, Servant servant);
    }
}
