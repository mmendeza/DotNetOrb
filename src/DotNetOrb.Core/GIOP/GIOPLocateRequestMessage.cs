// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetty.Buffers;
using GIOP;
using IOP;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPLocateRequestMessage : GIOPFragmentedMessage
    {
        public TargetAddress Target { get; set; }

        public GIOPLocateRequestMessage(ORB orb, IByteBuffer content) : base(orb)
        {
            this.content.AddComponent(content);
            this.content.SetWriterIndex(this.content.WriterIndex + content.ReadableBytes);
            Header = new GIOPHeader(this.content);
            if (Header.MessageType != MsgType11.LocateRequest)
            {
                throw new CORBA.Marshal("Not a Locate request!");
            }
            CDRInputStream input = new CDRInputStream(ORB, content, Header.GIOPVersion.Minor, Header.IsLittleEndian);
            switch (Header.GIOPVersion.Minor)
            {
                case 0:
                case 1:
                    {
                        RequestId = input.ReadULong();
                        Target = new TargetAddress()
                        {
                            ObjectKey = ObjectKeyHelper.Read(input)
                        };
                    }
                    break;
                case 2:
                    {
                        RequestId = input.ReadULong();
                        Target = TargetAddressHelper.Read(input);
                    }
                    break;
                default:
                    throw new CORBA.Marshal("Unknown GIOP minor version: " + Header.GIOPVersion.Minor);
            }
        }
    }
}
