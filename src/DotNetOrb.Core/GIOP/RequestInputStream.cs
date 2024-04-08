// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetty.Buffers;
using GIOP;
using IOP;
using System.Collections.Generic;

namespace DotNetOrb.Core.GIOP
{
    public class RequestInputStream : CDRInputStream
    {
        public uint RequestId { get; set; }
        public ICollection<ServiceContext> ServiceContextList { get; set; }
        public TargetAddress Target { get; private set; }
        public SyncScope SyncScope { get; private set; }
        public string Operation { get; private set; }
        public bool ResponseExpected
        {
            get
            {
                switch (SyncScope)
                {
                    case SyncScope.SERVER:
                    case SyncScope.TARGET:
                        return true;
                    default:
                        return false;
                }
            }
            private set
            {
                if (value)
                {
                    SyncScope = SyncScope.TARGET;
                }
                else
                {
                    SyncScope = SyncScope.NONE;
                }
            }
        }
        public RequestInputStream(ORB orb, uint requestId, int giopMinor, bool isLittleEndian, string operation, IByteBuffer buf, SyncScope syncScope, TargetAddress target, ICollection<ServiceContext> serviceContext) : base(orb, buf)
        {
            GiopMinor = giopMinor;
            IsLittleEndian = isLittleEndian;
            RequestId = requestId;
            SyncScope = syncScope;
            Target = target;
            Operation = operation;
            ServiceContextList = serviceContext;
        }

        public RequestInputStream(ORB orb, uint requestId, int giopMinor, bool isLittleEndian, string operation, IByteBuffer buf, bool responseExpected, TargetAddress target, ICollection<ServiceContext> serviceContext) : base(orb, buf)
        {
            GiopMinor = giopMinor;
            IsLittleEndian = isLittleEndian;
            RequestId = requestId;
            ResponseExpected = responseExpected;
            Target = target;
            Operation = operation;
            ServiceContextList = serviceContext;
        }
    }

}
