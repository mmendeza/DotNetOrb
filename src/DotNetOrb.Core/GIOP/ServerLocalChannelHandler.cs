// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.Core.GIOP
{
    public class ServerLocalChannelHandler : GIOPChannelHandler
    {
        public ServerLocalChannelHandler(ORB orb, ServerLocalConnection connection) : base(orb, connection)
        {
            requestIdCount = 1;
        }
    }
}
