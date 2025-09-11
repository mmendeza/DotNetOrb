// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.IIOP;
using DotNetty.Transport.Channels;
using GIOP;
using IIOP;
using IOP;
using System.Collections.Generic;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPBiDirServerHandler : ChannelHandlerAdapter
    {
        private ORB orb;
        private ILogger logger;

        public GIOPBiDirServerHandler(ORB orb)
        {
            this.orb = orb;
            logger = this.orb.Configuration.GetLogger(GetType());
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (orb.IsBidir && message is GIOPMessage msg)
            {
                if (msg.Header.GIOPVersion.Minor > 0)
                {
                    switch (msg.Header.MessageType)
                    {
                        case MsgType11.Request:
                            {
                                var request = (GIOPRequestMessage)msg;
                                foreach (var serviceCtx in request.ServiceContextList)
                                {
                                    if (serviceCtx.ContextId == BI_DIR_IIOP.Value)
                                    {
                                        using (var inputStream = new CDRInputStream(serviceCtx.ContextData))
                                        {
                                            inputStream.OpenEncapsulatedArray();
                                            var bidirCtx = BiDirIIOPServiceContextHelper.Read(inputStream);
                                            if (bidirCtx != null)
                                            {
                                                IIOPProfile prof = null;
                                                int numLP = bidirCtx.ListenPoints.Length;
                                                var alternates = numLP == 1 ? null : new List<IIOPAddress>(numLP - 1);
                                                for (int i = 0; i < numLP; i++)
                                                {
                                                    var listenPoint = bidirCtx.ListenPoints[i];
                                                    var addr = new IIOPAddress(listenPoint.Host, listenPoint.Port);
                                                    try
                                                    {
                                                        addr.Configure(orb.Configuration);
                                                    }
                                                    catch (ConfigException ce)
                                                    {
                                                        logger.Warn("Configuration Exception", ce);
                                                    }
                                                    if (i == 0)
                                                    {
                                                        if (logger.IsDebugEnabled)
                                                        {
                                                            logger.Debug("BiDirIIOPContext: Client conn. added to target " + addr);
                                                        }
                                                        prof = new IIOPProfile(addr, null, orb.GiopMinorVersion);
                                                    }
                                                    else
                                                    {
                                                        if (logger.IsDebugEnabled)
                                                        {
                                                            logger.Debug("BiDirIIOPContext: target alternate " + addr);
                                                        }
                                                        alternates.Add(addr);
                                                    }
                                                }
                                                if (numLP > 1)
                                                {
                                                    prof.SetAlternateAddresses(alternates);
                                                }
                                                var channelHandler = context.Channel.Pipeline.Get<GIOPChannelHandler>();
                                                if (channelHandler != null)
                                                {
                                                    orb.ConnectionManager.AddBidirConnection(channelHandler, prof);
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
