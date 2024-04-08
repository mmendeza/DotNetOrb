// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetty.Buffers;
using GIOP;

namespace DotNetOrb.Core.GIOP
{
    public class LocateRequestInputStream : CDRInputStream
    {
        public uint RequestId { get; set; }
        public TargetAddress Target { get; set; }

        public LocateRequestInputStream(ORB orb, uint requestId, int giopMinor, bool isLittleEndian, IByteBuffer buf, TargetAddress target) : base(orb, buf)
        {
            GiopMinor = giopMinor;
            IsLittleEndian = isLittleEndian;
            RequestId = requestId;
            Target = target;
        }
    }

}
