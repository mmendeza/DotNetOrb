// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using GIOP;
using IOP;
using System.Collections.Generic;

namespace DotNetOrb.Core.GIOP
{
    public class ReplyOutputStream : CDROutputStream
    {
        public uint RequestId { get; set; }
        public ReplyStatusType12 ReplyStatus { get; set; }
        public ICollection<ServiceContext> ServiceContextList { get; set; }

        public ReplyOutputStream(ORB orb, uint requestId, ReplyStatusType12 replyStatus, ICollection<ServiceContext> serviceContextList, int giopMinor) : base(orb)
        {
            RequestId = requestId;            
            ReplyStatus = replyStatus;
            GIOPMinor = giopMinor;
            ServiceContextList = serviceContextList;
            InitBuffer();
        }

        public void InitBuffer()
        {
            var header = new GIOPHeader(MsgType11.Reply, (byte)GIOPMinor);
            header.Write(this);
            var serviceContext = new ServiceContext[0];
            switch (GIOPMinor)
            {
                case 0:
                case 1:
                    {
                        ServiceContextListHelper.Write(this, serviceContext);
                        WriteULong(RequestId);
                        ReplyStatusType12Helper.Write(this, ReplyStatus);
                    }
                    break;
                case 2:
                    {
                        WriteULong(RequestId);
                        ReplyStatusType12Helper.Write(this, ReplyStatus);
                        ServiceContextListHelper.Write(this, serviceContext);
                        Skip(GetHeaderPadding(Pos));
                    }
                    break;
                default:
                    throw new Marshal("Unknown GIOP minor version: " + GIOPMinor);
            }
        }

        protected static int GetHeaderPadding(int pos)
        {
            int headerPadding = 8 - pos % 8; //difference to next 8 byte border
            headerPadding = headerPadding == 8 ? 0 : headerPadding;
            return headerPadding;
        }
    }
}
