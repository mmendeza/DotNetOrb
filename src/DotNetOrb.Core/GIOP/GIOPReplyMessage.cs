// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DotNetty.Buffers;
using GIOP;
using IOP;
using System;
using System.Collections;
using System.IO;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPReplyMessage : GIOPServiceContextMessage
    {
        public ReplyStatusType12 ReplyStatus { get; set; }

        public GIOPReplyMessage(ORB orb, IByteBuffer content) : base(orb)
        {
            //int length = content.ReadableBytes;
            //byte[] traceBuffer = new byte[length];
            //content.GetBytes(content.ReaderIndex, traceBuffer);
            this.content.AddComponent(content);
            this.content.SetWriterIndex(this.content.WriterIndex + content.ReadableBytes);
            Header = new GIOPHeader(this.content);
            if (Header.MessageType != MsgType11.Reply)
            {
                throw new Marshal("Not a Reply!");
            }
            CDRInputStream input = new CDRInputStream(ORB, this.content, Header.GIOPVersion.Minor, Header.IsLittleEndian);
            switch (Header.GIOPVersion.Minor)
            {
                case 0:
                case 1:
                    {
                        ServiceContextList.AddRange(ServiceContextListHelper.Read(input));
                        RequestId = input.ReadULong();
                        ReplyStatus = ReplyStatusType12Helper.Read(input);
                    }
                    break;
                case 2:
                    {
                        RequestId = input.ReadULong();
                        ReplyStatus = ReplyStatusType12Helper.Read(input);
                        ServiceContextList.AddRange(ServiceContextListHelper.Read(input));
                    }
                    break;
                default:
                    throw new Marshal("Unknown GIOP minor version: " + Header.GIOPVersion.Minor);
            }
            MarkHeaderEnd();
            //var path = @"L:\logs\reply_" + RequestId + "_" + DateTime.Now.ToFileTime();
            //File.WriteAllBytes(path, traceBuffer);
        }
    }

}
