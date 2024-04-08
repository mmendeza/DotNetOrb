// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using CSI;
using DotNetOrb.Core;
using DotNetOrb.Core.DSI;
using DotNetOrb.Core.GIOP;
using DotNetOrb.Core.POA.Exceptions;
using DotNetOrb.Core.POA.Util;
using DotNetOrb.Core.Util;
using DotNetty.Transport.Channels;
using GIOP;
using IOP;
using PortableServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetOrb.ImplementationRepository
{
    public class ImRChannelHandler : ChannelHandlerAdapter
    {
        protected Core.ORB orb;

        protected Core.ILogger logger;

        protected IChannel? channel;

        public IChannel? Channel { get { return channel; } }

        protected GIOPConnection connection;

        public GIOPConnection Connection => connection;

        private ImplementationRepository imr;

        public event EventHandler<EventArgs> ConnectionClosed;

        private object syncObject = new object();

        public bool IsConnected
        {
            get
            {
                if (channel != null)
                {
                    return channel.Active;
                }
                return false;
            }
        }

        public ImRChannelHandler(Core.ORB orb, ImplementationRepository imr, GIOPConnection connection) : base()
        {
            this.orb = orb;
            this.imr = imr;
            this.connection = connection;
            logger = orb.Configuration.GetLogger(GetType());
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            base.ChannelActive(context);
            ConnectionClosed?.Invoke(this, EventArgs.Empty);
        }

        public override void HandlerAdded(IChannelHandlerContext context)
        {
            channel = context.Channel;
            base.HandlerAdded(context);
        }

        public override Task WriteAsync(IChannelHandlerContext context, object message)
        {
            if (message is ReplyOutputStream replyStream)
            {
                var giopMessage = new GIOPReplyMessage(orb, replyStream.Buffer);
                return context.WriteAsync(giopMessage);
            }
            else if (message is LocateReplyOutputStream locateReplyStream)
            {
                var giopMessage = new GIOPLocateReplyMessage(orb, locateReplyStream.Buffer);
                return context.WriteAsync(giopMessage);
            }
            else
            {
                return context.WriteAsync(message);
            }
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            if (message is GIOPMessage giopMessage)
            {
                switch (giopMessage.Header.MessageType)
                {
                    case MsgType11.Request:
                        {
                            var request = (GIOPRequestMessage)message;
                            request.Retain();
                            var inputStream = new RequestInputStream(orb, request.RequestId, request.Header.GIOPVersion.Minor, request.Header.IsLittleEndian, request.Operation, request.Content, request.SyncScope, request.Target, request.ServiceContextList);
                            try
                            {
                                imr.ReplyNewLocation(orb.MapObjectKey(ParsedIOR.ExtractObjectKey(inputStream.Target, orb)),
                                  inputStream.RequestId,
                                  inputStream.GiopMinor,
                                  this,
                                  false);
                            }
                            finally
                            {
                                inputStream.Close();
                            }
                        }
                        break;
                    case MsgType11.LocateRequest:
                        {
                            var locateRequest = (GIOPLocateRequestMessage)message;
                            locateRequest.Retain();
                            var inputStream = new LocateRequestInputStream(orb, locateRequest.RequestId, locateRequest.Header.GIOPVersion.Minor, locateRequest.Header.IsLittleEndian, locateRequest.Content, locateRequest.Target);
                            try
                            {
                                imr.ReplyNewLocation(orb.MapObjectKey(ParsedIOR.ExtractObjectKey(inputStream.Target, orb)),
                              inputStream.RequestId,
                              inputStream.GiopMinor,
                              this,
                              true);
                            }
                            finally
                            {
                                inputStream.Close();
                            }

                        }
                        break;
                }
            }
            else
            {
                context.FireChannelRead(message);
            }
        }

        public virtual Task SendReplyAsync(ReplyOutputStream stream)
        {
            if (channel != null && IsConnected)
            {
                var invocationPolicies = new InvocationPolicies(stream.ServiceContextList);
                var tcs = new TaskCompletionSource<object>();
                if (Time.HasPassed(invocationPolicies.ReplyEndTime))
                {
                    tcs.SetException(new CORBA.Timeout("Reply End Time exceeded", 3, CompletionStatus.No));
                    return tcs.Task;
                }
                if (invocationPolicies.ReplyEndTime != null)
                {
                    var timeout = Time.MillisTo(invocationPolicies.ReplyEndTime);
                    if (timeout > 0)
                    {
                        var ct = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeout));
                        ct.Token.Register(() =>
                        {
                            tcs.TrySetException(new CORBA.Timeout("Reply end time exceeded", 3, CompletionStatus.No));
                        }, useSynchronizationContext: false);
                    }
                    else
                    {
                        tcs.TrySetException(new CORBA.Timeout("Reply end time exceeded", 3, CompletionStatus.No));
                    }
                }
                var future = channel.WriteAndFlushAsync(stream);
                future.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        tcs.TrySetException(t.Exception.InnerException);
                    }
                    else
                    {
                        tcs.TrySetResult(null);
                    }
                });
                return tcs.Task;
            }
            else
            {
                throw new Transient(0, CompletionStatus.Maybe);
            }
        }

        public virtual Task SendLocateReplyAsync(LocateReplyOutputStream stream)
        {
            if (channel != null && IsConnected)
            {
                return channel.WriteAndFlushAsync(stream);
            }
            else
            {
                throw new Transient(0, CompletionStatus.Maybe);
            }
        }

        public void Close()
        {
            if (channel != null && channel.Active)
            {
                var task = channel.CloseAsync().ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        logger.Error("Unable to close channel" + channel.RemoteAddress.ToString(), t.Exception?.InnerException);
                    }
                });
                task.Wait();
            }
        }
    }
}
