// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.GIOP;
using System.Threading.Tasks;

namespace DotNetOrb.Core.IIOP
{
    public class ClientIIOPChannelHandler : GIOPChannelHandler
    {

        public override bool IsSharable => true;

        public ClientIIOPChannelHandler(ORB orb, ClientIIOPConnection connection) : base(orb, connection)
        {
            requestIdCount = 0;
        }

        public override Task<IInputStream> SendRequestAsync(RequestOutputStream stream)
        {
            connection.Connect();
            return base.SendRequestAsync(stream);
        }

        public override Task SendReplyAsync(ReplyOutputStream stream)
        {
            connection.Connect();
            return base.SendReplyAsync(stream);
        }

        public override Task<IInputStream> SendLocateRequestAsync(LocateRequestOutputStream stream)
        {
            connection.Connect();
            return base.SendLocateRequestAsync(stream);
        }

        public override Task SendLocateReplyAsync(LocateReplyOutputStream stream)
        {
            connection.Connect();
            return base.SendLocateReplyAsync(stream);
        }
    }
}
