// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetOrb.Core.ETF;
using DotNetOrb.Core.IIOP;
using DotNetty.Transport.Channels;
using GIOP;
using IIOP;
using IOP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPBiDirClientHandler : ChannelHandlerAdapter
    {
        private ORB orb;
        private ClientConnection connection;
        private ServiceContext bidirCtx = null;

        public GIOPBiDirClientHandler(ORB orb, ClientConnection connection)
        {
            this.orb = orb;
            this.connection = connection;
            InitContext();
        }

        private void InitContext()
        {
            BasicAdapter ba = orb.GetBasicAdapter();
            var endpointProfiles = ba.GetEndpointProfiles();
            var listenPoints = new List<ListenPoint>();
            foreach (var profile in endpointProfiles)
            {
                if (profile is ProfileBase)
                {
                    if (profile is IIOPProfile && ((IIOPProfile)profile).GetSSL() != null)
                    {
                        IIOPProfile sslprof = ((IIOPProfile)profile).ToNonSSL();
                        listenPoints.AddRange(sslprof.AsListenPoints());
                    }
                    else
                    {
                        listenPoints.AddRange(((ProfileBase)profile).AsListenPoints());
                    }
                }
            }
            var listenPointsArray = listenPoints.ToArray();
            BiDirIIOPServiceContext context = new BiDirIIOPServiceContext(listenPointsArray);
            var any = orb.CreateAny();
            BiDirIIOPServiceContextHelper.Insert(any, context);
            CDROutputStream cdrOut = new CDROutputStream(orb);
            try
            {
                cdrOut.BeginEncapsulatedArray();
                BiDirIIOPServiceContextHelper.Write(cdrOut, context);

                bidirCtx = new ServiceContext(BI_DIR_IIOP.Value, cdrOut.GetBufferCopy());
            }
            finally
            {
                cdrOut.Close();
            }
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
                                var bidirContext = context.GetAttribute(GIOPConnection.BiDirAttrKey).Get();
                                if (bidirContext != null)
                                {
                                    var request = (GIOPRequestMessage)msg;
                                    request.ServiceContextList.Add(bidirCtx);
                                    context.GetAttribute(GIOPConnection.BiDirAttrKey).Set(bidirCtx);
                                }
                            }
                            break;
                    }
                }
            }
            return base.WriteAsync(context, message);
        }
    }
}
