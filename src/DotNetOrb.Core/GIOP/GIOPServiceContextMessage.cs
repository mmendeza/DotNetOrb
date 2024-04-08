// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetty.Buffers;
using IOP;
using System.Collections.Generic;

namespace DotNetOrb.Core.GIOP
{
    public abstract class GIOPServiceContextMessage : GIOPFragmentedMessage
    {
        protected static ServiceContext paddingCtx = new ServiceContext((uint)ORBConstants.SERVICE_PADDING_CONTEXT, new byte[0]);

        protected int headerEnd = -1;

        protected int headerPadding = 0;

        private List<ServiceContext> serviceContextList = new List<ServiceContext>();
        public List<ServiceContext> ServiceContextList { get => serviceContextList; }

        public GIOPServiceContextMessage(ORB orb) : base(orb)
        {

        }

        /// <summary>
        /// GIOP 1.2 requires the message body to start on an 8 byteborder, 
        /// while 1.0/1.1 does not. Additionally, this padding shall only be performed, 
        /// if the body is not empty (which we don't know at this stage.
        /// /// </summary>
        protected void MarkHeaderEnd()
        {
            headerEnd = content.ReaderIndex;

            if (Header.GIOPVersion.Minor > 1)
            {
                var padding = 8 - content.ReaderIndex % 8; //difference to next 8 byte border
                padding = padding == 8 ? 0 : padding;

                if (content.WriterIndex >= content.ReaderIndex + padding)
                {
                    headerPadding = padding;
                    content.SkipBytes(headerPadding);
                }
                else
                {
                    //message with no body (no padding applied)
                }
            }
        }

        public override void InsertMsgSize()
        {
            content.SetReaderIndex(0);
            if (headerPadding == 0)
            {
                content.SetInt(8, content.WriterIndex - GIOPHeader.HeaderSize);
            }
            else
            {
                if (content.WriterIndex > headerEnd + headerPadding)
                {
                    //has a body, so include padding by not removing it :-)
                    content.SetInt(8, content.WriterIndex - GIOPHeader.HeaderSize);
                }
                else
                {
                    //no body written, so remove padding
                    content.SetInt(8, content.WriterIndex - headerPadding - GIOPHeader.HeaderSize);
                    content.SetWriterIndex(content.WriterIndex - headerPadding);
                }
            }
        }

        public override void Write(IByteBuffer buf)
        {
            if (ServiceContextList == null || ServiceContextList.Count == 0)
            {
                //no additional service contexts present, so buffer can be sent as a whole
                InsertMsgSize();
                buf.WriteBytes(content, 0, content.WriterIndex);
            }
            else
            {
                CDROutputStream ctxOut = null;

                switch (Header.GIOPVersion.Minor)
                {
                    case 0:
                    case 1:
                        {
                            //For GIOP 1.1, we have to add a padding context
                            ServiceContextList.Add(paddingCtx);
                            ctxOut = CreateContextStream();

                            //pad to next 8 byte border
                            int padding = 8 - (GIOPHeader.HeaderSize + ctxOut.Pos) % 8;
                            padding = padding == 8 ? 0 : padding;

                            if (padding > 0)
                            {
                                //The last padding context has a 0 length data part. Therefore, the last data is a ulong
                                //with value 0 (the length of the array). To increase the data part, we have to increase
                                //the size and add the actual data.

                                //"unwrite" the last ulong
                                ctxOut.ReduceSize(4);

                                //write new length
                                ctxOut.WriteULong((uint)padding);

                                //add "new" data (by just increasing the size of the stream and not actually writing anything).
                                ctxOut.IncreaseSize(padding);
                            }

                            //Then, we have to update the message size in the GIOP message header.
                            //The new size is the size of the "original" message minus the length ulong (4 bytes) of
                            //the original empty ServiceContext array plus the length of the new service context array
                            var size = content.WriterIndex - GIOPHeader.HeaderSize - 4 + ctxOut.Pos;
                            content.SetInt(8, size);

                            //The ServiceContexts are the first attribute in the RequestHeader struct. Therefore firstly, we
                            //have to write the GIOP message header...
                            buf.WriteBytes(content, 0, GIOPHeader.HeaderSize);

                            //... then add the contexts ...
                            buf.WriteBytes(ctxOut.Buffer);

                            //... and finally the rest of the message (omitting the empty original context array).
                            buf.WriteBytes(content, GIOPHeader.HeaderSize + 4, content.WriterIndex - (GIOPHeader.HeaderSize + 4));

                        }
                        break;
                    case 2:
                        {
                            //For GIOP 1.2, the header is padded per spec, so no additional context is needed
                            ctxOut = CreateContextStream();

                            //the new header end is the old header end minus the length ulong of the context array plus the
                            //length of the context array (wich contains its own length ulong)
                            int newHeaderEnd = headerEnd - 4 + ctxOut.Pos;

                            // pad to next 8 byte border
                            int padding = 8 - newHeaderEnd % 8;
                            padding = padding == 8 ? 0 : padding;

                            if (padding > 0 && content.WriterIndex > headerEnd + headerPadding)
                            {
                                //add padding bytes (by just increasing the size of the stream and not actually writing anything).
                                //If no body is present, no padding has to be inserted
                                ctxOut.IncreaseSize(padding);
                            }

                            //Then, we have to update the message size in the GIOP message header.
                            //The new size is the size of the "original" message minus the length ulong (4 bytes)
                            //of the original empty ServiceContext array minus the "original" header padding plus
                            //the length of the new service context array (containing the new padding)
                            var size = content.WriterIndex - GIOPHeader.HeaderSize - 4 - headerPadding + ctxOut.Pos;
                            content.SetInt(8, size);

                            // The GIOP message and request header(up until the ServiceContexts) stay unmanipulated.
                            // We also have to remove the length ulong of the "original" empty service context array, because
                            // the new one has its own length attribute
                            buf.WriteBytes(content, 0, headerEnd - 4);

                            //... then add the contexts ...
                            buf.WriteBytes(ctxOut.Buffer);
                            //... and finally the rest of the message (omitting the empty original context array).
                            buf.WriteBytes(content, headerEnd + headerPadding, content.WriterIndex - (headerEnd + headerPadding));
                        }
                        break;
                    default:
                        return;
                }
                if (ctxOut != null)
                {
                    ctxOut.Close();
                }
            }
        }

        private CDROutputStream CreateContextStream()
        {
            var outputStream = new CDROutputStream(ORB);
            //write the length of the service context array.
            outputStream.WriteULong((uint)ServiceContextList.Count);
            foreach (var ctx in ServiceContextList)
            {
                ServiceContextHelper.Write(outputStream, ctx);
            }
            return outputStream;
        }
    }
}
