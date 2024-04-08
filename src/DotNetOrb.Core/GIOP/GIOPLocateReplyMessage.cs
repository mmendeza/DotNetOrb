// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetty.Buffers;
using GIOP;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPLocateReplyMessage : GIOPFragmentedMessage
    {
        public LocateStatusType12 LocateStatus { get; set; }

        public GIOPLocateReplyMessage(ORB orb, IByteBuffer content) : base(orb)
        {
            this.content.AddComponent(content);
            this.content.SetWriterIndex(this.content.WriterIndex + content.ReadableBytes);
            Header = new GIOPHeader(this.content);
            if (Header.MessageType != MsgType11.LocateReply)
            {
                throw new CORBA.Marshal("Not a Locate reply!");
            }
            CDRInputStream input = new CDRInputStream(ORB, this.content, Header.GIOPVersion.Minor, Header.IsLittleEndian);
            switch (Header.GIOPVersion.Minor)
            {
                case 0:
                case 1:
                case 2:
                    {
                        RequestId = input.ReadULong();
                        LocateStatus = LocateStatusType12Helper.Read(input);
                    }
                    break;
                default:
                    throw new CORBA.Marshal("Unknown GIOP minor version: " + Header.GIOPVersion.Minor);
            }
        }

    }
}
