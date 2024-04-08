// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetty.Buffers;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPFragmentMessage : GIOPMessage
    {
        public uint RequestId { get; protected set; }

        public GIOPFragmentMessage(ORB orb, IByteBuffer content) : base(orb)
        {
            this.content.AddComponent(content);
            this.content.SetWriterIndex(this.content.WriterIndex + content.ReadableBytes);
            Header = new GIOPHeader(this.content);
            var input = new CDRInputStream(ORB, this.content, Header.GIOPVersion.Minor, Header.IsLittleEndian);
            switch (Header.GIOPVersion.Minor)
            {
                case 2:
                    {
                        RequestId = input.ReadULong();
                    }
                    break;
            }
        }
    }
}
