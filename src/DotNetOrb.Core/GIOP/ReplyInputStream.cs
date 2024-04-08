// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetty.Buffers;
using GIOP;
using IOP;
using System.Collections.Generic;

namespace DotNetOrb.Core.GIOP
{
    public class ReplyInputStream : CDRInputStream
    {
        public uint RequestId { get; set; }
        public ReplyStatusType12 ReplyStatus { get; set; }
        public ICollection<ServiceContext> ServiceContextList { get; set; }

        public ReplyInputStream(ORB orb, uint requestId, int giopMinor, bool isLittleEndian, IByteBuffer buf, ReplyStatusType12 status, ICollection<ServiceContext> contextList) : base(orb, buf)
        {
            GiopMinor = giopMinor;
            IsLittleEndian = isLittleEndian;
            RequestId = requestId;
            ReplyStatus = status;
            ServiceContextList = contextList;
        }
    }

}
