// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using DotNetOrb.Core.ETF;
using ETF;
using IOP;

namespace DotNetOrb.Core.GIOP
{
    internal interface IConnectionFactory : IConfigurable
    {
        public uint ProfileTag { get; }
        public abstract IProfile DemarshalProfile(ref TaggedProfile taggedProfile, out TaggedComponent[] components);
        public abstract ClientConnection CreateConnection(IProfile profile);
        public abstract Listener CreateListener(ListenEndpoint listenEndpoint);
        public abstract ProtocolAddressBase CreateProtocolAddress(string addr);
        public abstract IProfile DecodeCorbaloc(string corbaloc);
    }
}
