// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using GIOP;

namespace DotNetOrb.Core.GIOP
{
    public class LocateReplyOutputStream : CDROutputStream
    {
        public uint RequestId { get; set; }
        public LocateStatusType12 LocateStatus { get; set; }        

        public LocateReplyOutputStream(ORB orb, uint requestId, LocateStatusType12 status, int giopMinor) : base(orb)
        {
            LocateStatus = status;
            RequestId = requestId;            
            GIOPMinor = giopMinor;
            InitBuffer();
        }

        public void InitBuffer()
        {
            var header = new GIOPHeader(MsgType11.LocateReply, (byte)GIOPMinor);
            header.Write(this);
            switch (GIOPMinor)
            {
                case 0:
                case 1:
                case 2:
                    {
                        WriteULong(RequestId);
                        LocateStatusType12Helper.Write(this, LocateStatus);
                    }
                    break;
                default:
                    throw new Marshal("Unknown GIOP minor version: " + GIOPMinor);
            }
        }

    }
}
