// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CONV_FRAME;
using DotNetOrb.Core.CDR;
using DotNetty.Transport.Channels;
using GIOP;
using IOP;
using System.Threading.Tasks;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPCodeSetClientHandler : ChannelHandlerAdapter
    {
        private ORB orb;
        private ClientConnection connection;

        public GIOPCodeSetClientHandler(ORB orb, ClientConnection connection)
        {
            this.orb = orb;
            this.connection = connection;
        }

        public override Task WriteAsync(IChannelHandlerContext context, object message)
        {
            if (message is GIOPMessage msg)
            {
                if (msg.Header.GIOPVersion.Minor > 0)
                {
                    switch (msg.Header.MessageType)
                    {
                        case MsgType11.Request:
                            {
                                var tcs = context.GetAttribute(GIOPConnection.TCSAttrKey).Get();
                                //var tcsw = context.GetAttribute(GIOPConnection.TCSWAttrKey).Get();
                                if (tcs == null)
                                {
                                    var request = (GIOPRequestMessage)msg;
                                    request.ServiceContextList.Add(CreateCodesetContext(connection.CodeSetChar, connection.CodeSetWChar));
                                    context.GetAttribute(GIOPConnection.TCSAttrKey).Set(connection.CodeSetChar);
                                    context.GetAttribute(GIOPConnection.TCSWAttrKey).Set(connection.CodeSetWChar);
                                }
                            }
                            break;
                    }
                }
            }
            return base.WriteAsync(context, message);
        }

        private ServiceContext CreateCodesetContext(CodeSet tcs, CodeSet tcsw)
        {
            using (CDROutputStream os = new CDROutputStream(orb))
            {
                os.BeginEncapsulatedArray();
                CodeSetContextHelper.Write(os, new CodeSetContext(tcs.Id, tcsw.Id));
                return new ServiceContext(TAG_CODE_SETS.Value, os.GetBufferCopy());
            }
        }
    }
}
