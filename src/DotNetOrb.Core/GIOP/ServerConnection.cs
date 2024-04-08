// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetty.Transport.Bootstrapping;
using ETF;

namespace DotNetOrb.Core.GIOP
{
    public abstract class ServerConnection : GIOPConnection
    {
        public new IProfile Profile
        {
            get
            {
                return profile;
            }
            set
            {
                profile = value;
            }
        }

        protected ServerBootstrap bootstrap = new ServerBootstrap();

        protected ServerConnection(ORB orb) : base(orb)
        {
        }
    }

}
