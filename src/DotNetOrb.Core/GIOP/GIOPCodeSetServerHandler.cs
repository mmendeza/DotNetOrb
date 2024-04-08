// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CONV_FRAME;
using CORBA;
using DotNetOrb.Core.CDR;
using DotNetty.Transport.Channels;
using GIOP;
using IOP;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPCodeSetServerHandler : ChannelHandlerAdapter
    {
        private ORB orb;
        private ServerConnection connection;

        public GIOPCodeSetServerHandler(ORB orb, ServerConnection connection)
        {
            this.orb = orb;
            this.connection = connection;
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is GIOPMessage msg)
            {
                if (msg.Header.GIOPVersion.Minor > 0)
                {
                    switch (msg.Header.MessageType)
                    {
                        case MsgType11.Request:
                            {
                                var request = (GIOPRequestMessage)msg;
                                var tcs = context.GetAttribute(GIOPConnection.TCSAttrKey).Get();
                                //var tcsw = context.GetAttribute(GIOPConnection.TCSWAttrKey).Get();
                                if (tcs == null)
                                {
                                    foreach (var serviceCtx in request.ServiceContextList)
                                    {
                                        if (serviceCtx.ContextId == TAG_CODE_SETS.Value)
                                        {
                                            using (var inputStream = new CDRInputStream(serviceCtx.ContextData))
                                            {
                                                inputStream.OpenEncapsulatedArray();
                                                var ctx = CodeSetContextHelper.Read(inputStream);
                                                if (ctx != null)
                                                {
                                                    try
                                                    {
                                                        var charCodeSet = CodeSet.GetCodeSet(ctx.CharData);
                                                        var wcharCodeSet = CodeSet.GetCodeSet(ctx.WcharData);
                                                        context.GetAttribute(GIOPConnection.TCSAttrKey).Set(charCodeSet);
                                                        context.GetAttribute(GIOPConnection.TCSWAttrKey).Set(wcharCodeSet);
                                                    }
                                                    catch (CodeSetIncompatible ex)
                                                    {
                                                        var os = new CDROutputStream(orb);
                                                        var header = new GIOPHeader(MsgType11.Reply, request.Header.GIOPVersion.Minor);
                                                        header.Write(os);
                                                        SystemExceptionHelper.Write(os, ex);
                                                        var reply = new GIOPReplyMessage(orb, os.Buffer);
                                                        context.WriteAndFlushAsync(reply);
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            base.ChannelRead(context, message);
        }
    }
}
