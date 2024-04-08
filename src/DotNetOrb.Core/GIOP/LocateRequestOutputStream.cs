// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using GIOP;
using IOP;

namespace DotNetOrb.Core.GIOP
{
    public class LocateRequestOutputStream : CDROutputStream
    {
        public uint RequestId { get; set; }
        public TargetAddress Target { get; set; }
        public GIOPChannelHandler ChannelHandler { get; private set; }

        public LocateRequestOutputStream(ORB orb, GIOPChannelHandler channelHandler, TargetAddress target, int giopMinor) : base(orb)
        {
            Target = target;
            ChannelHandler = channelHandler;
            GIOPMinor = giopMinor;
            RequestId = channelHandler.GetRequestId();
            InitBuffer();
        }

        public void InitBuffer()
        {
            var header = new GIOPHeader(MsgType11.LocateRequest, (byte)GIOPMinor);
            header.Write(this);
            switch (GIOPMinor)
            {
                case 0:
                case 1:
                    {
                        WriteULong(RequestId);
                        ObjectKeyHelper.Write(this, Target.ObjectKey);
                    }
                    break;
                case 2:
                    {
                        WriteULong(RequestId);
                        TargetAddressHelper.Write(this, Target);
                    }
                    break;
                default:
                    throw new Marshal("Unknown GIOP minor version: " + GIOPMinor);
            }
        }
    }
}
