// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetty.Buffers;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPCancelRequestMessage : GIOPMessage
    {
        public uint RequestId { get; set; }

        public GIOPCancelRequestMessage(ORB orb, IByteBuffer content) : base(orb)
        {
            this.content.AddComponent(content);
            this.content.SetWriterIndex(this.content.WriterIndex + content.ReadableBytes);
            Header = new GIOPHeader(this.content);
            CDRInputStream input = new CDRInputStream(ORB, content, Header.GIOPVersion.Minor, Header.IsLittleEndian);
            RequestId = input.ReadULong();
        }

    }
}
