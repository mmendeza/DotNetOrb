// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.GIOP;

namespace DotNetOrb.Core.IIOP
{
    public class ServerIIOPChannelHandler : GIOPChannelHandler
    {
        public ServerIIOPChannelHandler(ORB orb, ServerIIOPConnection connection) : base(orb, connection)
        {
            requestIdCount = 1;
        }
    }
}
