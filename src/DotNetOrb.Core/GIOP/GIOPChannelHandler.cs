// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using CSI;
using DotNetOrb.Core.DSI;
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
using static PortableServer.POA;
using System.IO;

namespace DotNetOrb.Core.GIOP
{
    public abstract class GIOPChannelHandler : ChannelHandlerAdapter
    {
        protected uint requestIdCount = 0;

        protected ORB orb;

        protected readonly Dictionary<uint, TaskCompletionSource<IInputStream>> requests = new Dictionary<uint, TaskCompletionSource<IInputStream>>();
        protected readonly Dictionary<uint, TaskCompletionSource<IInputStream>> locateRequests = new Dictionary<uint, TaskCompletionSource<IInputStream>>();

        protected ILogger logger;

        protected IChannel? channel;

        public IChannel? Channel { get { return channel; } }

        protected GIOPConnection connection;

        public GIOPConnection Connection => connection;

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

        public GIOPChannelHandler(ORB orb, GIOPConnection connection) : base()
        {
            this.orb = orb;
            this.connection = connection;
            logger = orb.Configuration.GetLogger(GetType());
        }

        public uint GetRequestId()
        {
            lock (syncObject)
            {
                var currentIdx = requestIdCount;
                requestIdCount += 2;
                return currentIdx;
            }
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
            if (message is RequestOutputStream requestStream)
            {
                //int length = requestStream.Buffer.ReadableBytes;
                //byte[] traceBuffer = new byte[length];
                //requestStream.Buffer.GetBytes(requestStream.Buffer.ReaderIndex, traceBuffer);
                //var path = @"L:\logs\request_" + requestStream.RequestId + "_" + requestStream.Operation + "_" + DateTime.Now.ToFileTime();
                //File.WriteAllBytes(path, traceBuffer);                
                var giopMessage = new GIOPRequestMessage(orb, requestStream.Buffer);
                return context.WriteAsync(giopMessage);
            }
            else if (message is ReplyOutputStream replyStream)
            {
                var giopMessage = new GIOPReplyMessage(orb, replyStream.Buffer);
                return context.WriteAsync(giopMessage);
            }
            else if (message is LocateRequestOutputStream locateRequestStream)
            {
                var giopMessage = new GIOPLocateRequestMessage(orb, locateRequestStream.Buffer);
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
                            var codeSet = context.GetAttribute(GIOPConnection.TCSAttrKey).Get();
                            if (codeSet != null)
                            {
                                inputStream.CodeSetChar = codeSet;
                            }
                            var codeSetW = context.GetAttribute(GIOPConnection.TCSWAttrKey).Get();
                            if (codeSetW != null)
                            {
                                inputStream.CodeSetWChar = codeSetW;
                            }
                            ServerRequest serverRequest = null;

                            try
                            {
                                serverRequest = new ServerRequest(orb, inputStream, this);
                                DeliverRequest(serverRequest);
                            }
                            catch (POAInternalException pie)
                            {
                                logger.Warn("Received a request with a non DotNetOrb object key");

                                var os = new ReplyOutputStream(orb, inputStream.RequestId, ReplyStatusType12.SYSTEM_EXCEPTION, inputStream.ServiceContextList, inputStream.GiopMinor);

                                try
                                {
                                    SystemExceptionHelper.Write(os, new ObjectNotExist((int)(OMGVMCID.Value | 2), CompletionStatus.No));
                                    SendReplyAsync(os);
                                }
                                catch (Exception ex)
                                {
                                    logger.Warn("unexpected exception during RequestReceived", ex);
                                }
                                return;
                            }
                        }
                        break;
                    case MsgType11.Reply:
                        {
                            var reply = (GIOPReplyMessage)message;
                            reply.Retain();
                            var inputStream = new ReplyInputStream(orb, reply.RequestId, reply.Header.GIOPVersion.Minor, reply.Header.IsLittleEndian, reply.Content, reply.ReplyStatus, reply.ServiceContextList);
                            int length = inputStream.Buffer.ReadableBytes;
                            var codeSet = context.GetAttribute(GIOPConnection.TCSAttrKey).Get();
                            if (codeSet != null)
                            {
                                inputStream.CodeSetChar = codeSet;
                            }
                            var codeSetW = context.GetAttribute(GIOPConnection.TCSWAttrKey).Get();
                            if (codeSetW != null)
                            {
                                inputStream.CodeSetWChar = codeSetW;
                            }
                            if (requests.ContainsKey(inputStream.RequestId))
                            {
                                var tcs = requests[inputStream.RequestId];
                                var request = tcs.Task.AsyncState as RequestOutputStream;
                                var invocationPolicies = new InvocationPolicies(request.ServiceContextList);
                                var delay = invocationPolicies.ReplyStartTime != null ? Time.MillisTo(invocationPolicies.ReplyStartTime) : 0;
                                Action action = () =>
                                {
                                    switch (inputStream.ReplyStatus)
                                    {
                                        case ReplyStatusType12.NO_EXCEPTION:
                                            {
                                                tcs.TrySetResult(inputStream);
                                                requests.Remove(inputStream.RequestId);
                                            }
                                            break;
                                        case ReplyStatusType12.USER_EXCEPTION:
                                            {
                                                var aex = GetApplicationException(inputStream);
                                                tcs.TrySetException(aex);
                                                requests.Remove(inputStream.RequestId);
                                            }
                                            break;
                                        case ReplyStatusType12.SYSTEM_EXCEPTION:
                                            {
                                                var sex = SystemExceptionHelper.Read(inputStream);
                                                tcs.TrySetException(sex);
                                                requests.Remove(inputStream.RequestId);
                                            }
                                            break;
                                        case ReplyStatusType12.LOCATION_FORWARD:
                                        case ReplyStatusType12.LOCATION_FORWARD_PERM:
                                            {
                                                var forwardReference = inputStream.ReadObject();
                                                var fex = new ForwardRequest(forwardReference);
                                                tcs.TrySetException(fex);
                                                requests.Remove(inputStream.RequestId);
                                            }
                                            break;
                                        case ReplyStatusType12.NEEDS_ADDRESSING_MODE:
                                            {
                                                tcs.TrySetException(new NoImplement("Got reply status NEEDS_ADDRESSING_MODE"));
                                                requests.Remove(inputStream.RequestId);
                                            }
                                            break;
                                    }
                                };
                                if (delay > 0)
                                {
                                    context.Executor.ScheduleAsync(action, TimeSpan.FromMilliseconds(delay));
                                }
                                else
                                {
                                    context.Executor.Execute(action);
                                }
                            }
                        }
                        break;
                    case MsgType11.LocateRequest:
                        {
                            var locateRequest = (GIOPLocateRequestMessage)message;
                            locateRequest.Retain();
                            var inputStream = new LocateRequestInputStream(orb, locateRequest.RequestId, locateRequest.Header.GIOPVersion.Minor, locateRequest.Header.IsLittleEndian, locateRequest.Content, locateRequest.Target);
                            var codeSet = context.GetAttribute(GIOPConnection.TCSAttrKey).Get();
                            if (codeSet != null)
                            {
                                inputStream.CodeSetChar = codeSet;
                            }
                            var codeSetW = context.GetAttribute(GIOPConnection.TCSWAttrKey).Get();
                            if (codeSetW != null)
                            {
                                inputStream.CodeSetWChar = codeSetW;
                            }
                            ServerRequest serverRequest = null;
                            try
                            {
                                var requestStream = new RequestInputStream(orb, locateRequest.RequestId, locateRequest.Header.GIOPVersion.Minor, locateRequest.Header.IsLittleEndian, "_non_existent", inputStream.Buffer, true, inputStream.Target, new ServiceContext[0]);
                                serverRequest = new ServerRequest(orb, requestStream, this);

                                if (!serverRequest.ObjectKey.SequenceEqual(ParsedIOR.ExtractObjectKey(inputStream.Target, orb)))
                                {
                                    CORBA.Object fwd = orb.GetReference(orb.GetRootPOA(), serverRequest.ObjectKey, null, true);

                                    if (logger.IsDebugEnabled)
                                    {
                                        logger.Debug("Sending locate reply with object forward to " + orb.ObjectToString(fwd));
                                    }

                                    var lrOut = new LocateReplyOutputStream(orb, inputStream.RequestId, LocateStatusType12.OBJECT_FORWARD, inputStream.GiopMinor);

                                    lrOut.WriteObject(fwd);

                                    context.WriteAndFlushAsync(lrOut).ContinueWith(x =>
                                    {
                                        fwd._Release();
                                    });
                                    return;
                                }
                                DeliverRequest(serverRequest);
                            }
                            catch (POAInternalException pie)
                            {
                                logger.Warn("Received a request with a non DotNetOrb object key");

                                var lrOut = new LocateReplyOutputStream(orb, inputStream.RequestId, LocateStatusType12.UNKNOWN_OBJECT, inputStream.GiopMinor);
                                context.WriteAndFlushAsync(lrOut);
                            }
                        }
                        break;
                    case MsgType11.LocateReply:
                        {
                            var locateReply = (GIOPLocateReplyMessage)message;
                            locateReply.Retain();
                            var inputStream = new LocateReplyInputStream(orb, locateReply.RequestId, locateReply.Header.GIOPVersion.Minor, locateReply.Header.IsLittleEndian, locateReply.Content, locateReply.LocateStatus);
                            var codeSet = context.GetAttribute(GIOPConnection.TCSAttrKey).Get();
                            if (codeSet != null)
                            {
                                inputStream.CodeSetChar = codeSet;
                            }
                            var codeSetW = context.GetAttribute(GIOPConnection.TCSWAttrKey).Get();
                            if (codeSetW != null)
                            {
                                inputStream.CodeSetWChar = codeSetW;
                            }
                            if (locateRequests.ContainsKey(inputStream.RequestId))
                            {
                                var tcs = locateRequests[inputStream.RequestId];
                                tcs.TrySetResult(inputStream);
                                locateRequests.Remove(inputStream.RequestId);
                            }
                        }
                        break;
                    case MsgType11.CancelRequest:
                        {
                            //CancelRequestReceived?.Invoke(this, new GIOPMessageEventArgs(giopMessage));
                        }
                        break;
                    case MsgType11.CloseConnection:
                        {
                            //CloseConnectionReceived?.Invoke(this, new GIOPMessageEventArgs(giopMessage));
                        }
                        break;
                    case MsgType11.MessageError:
                        {
                            //ErrorReceived?.Invoke(this, new GIOPMessageEventArgs(giopMessage));
                        }
                        break;
                }
            }
            else
            {
                context.FireChannelRead(message);
            }
        }
        public virtual Task<IInputStream> SendRequestAsync(RequestOutputStream stream)
        {
            if (channel != null && IsConnected)
            {
                var tcs = new TaskCompletionSource<IInputStream>(stream);
                var invocationPolicies = new InvocationPolicies(stream.ServiceContextList);
                if (Time.HasPassed(invocationPolicies.RequestEndTime))
                {
                    tcs.SetException(new CORBA.Timeout("Request End Time exceeded prior to invocation", 2, CompletionStatus.No));
                    return tcs.Task;
                }
                if (Time.HasPassed(invocationPolicies.ReplyEndTime))
                {
                    tcs.SetException(new CORBA.Timeout("Reply End Time exceeded prior to invocation", 3, CompletionStatus.No));
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
                            if (stream.ResponseExpected)
                            {
                                requests.Remove(stream.RequestId);
                            }
                        }, useSynchronizationContext: false);
                    }
                    else
                    {
                        tcs.TrySetException(new CORBA.Timeout("Reply end time exceeded", 3, CompletionStatus.No));
                        return tcs.Task;
                    }
                }
                if (stream.ResponseExpected)
                {
                    requests.Add(stream.RequestId, tcs);
                }
                var delay = invocationPolicies.RequestStartTime != null ? Time.MillisTo(invocationPolicies.RequestStartTime) : 0;
                Action action = async () =>
                {
                    if (invocationPolicies.RequestEndTime != null)
                    {
                        var timeout = Time.MillisTo(invocationPolicies.RequestEndTime);
                        if (timeout > 0)
                        {
                            var ct = new CancellationTokenSource(TimeSpan.FromMilliseconds(timeout));
                            ct.Token.Register(() =>
                            {
                                tcs.TrySetException(new CORBA.Timeout("Request end time exceeded", 2, CompletionStatus.No));
                                requests.Remove(stream.RequestId);
                            }, useSynchronizationContext: false);
                        }
                        else
                        {
                            tcs.TrySetException(new CORBA.Timeout("Request end time exceeded", 2, CompletionStatus.No));
                            requests.Remove(stream.RequestId);
                            return;
                        }
                    }
                    try
                    {
                        await channel.WriteAndFlushAsync(stream);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Unable to send request", ex);
                        tcs.TrySetException(new CommFailure(0, CompletionStatus.Maybe));
                        if (stream.ResponseExpected)
                        {
                            requests.Remove(stream.RequestId);
                        }
                    }
                };
                if (delay > 0)
                {
                    channel.EventLoop.ScheduleAsync(action, TimeSpan.FromMilliseconds(delay));
                }
                else
                {
                    channel.EventLoop.Execute(action);
                }
                return tcs.Task;
            }
            else
            {
                throw new Transient(0, CompletionStatus.Maybe);
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

        public virtual Task<IInputStream> SendLocateRequestAsync(LocateRequestOutputStream stream)
        {
            if (channel != null && IsConnected)
            {
                var tcs = new TaskCompletionSource<IInputStream>(stream);
                locateRequests.Add(stream.RequestId, tcs);
                var future = channel.WriteAndFlushAsync(stream);
                future.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        logger.Debug("Unable to send request", t.Exception);
                        if (t.Exception.InnerException is CORBA.SystemException)
                        {
                            tcs.TrySetException(t.Exception.InnerException);
                        }
                        else
                        {
                            tcs.TrySetException(new CommFailure(0, CompletionStatus.Maybe));
                        }
                        locateRequests.Remove(stream.RequestId);
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

        protected void DeliverRequest(ServerRequest request)
        {
            POA.POA tmpPOA = orb.GetRootPOA();

            List<string> scopes;
            try
            {
                string refImplName = "";
                string orbImplName = orb.ImplName;
                string orbServerId = orb.ServerIdString;

                try
                {
                    refImplName = POAUtil.ExtractImplName(request.ObjectKey);
                }
                catch (POAInternalException pie)
                {
                    logger.Debug("DeliverRequest: Reference generated by foreign POA");
                }
                if (!orbImplName.Equals(refImplName) && !orbServerId.Equals(refImplName))
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug
                        (
                           "DeliverRequest: implName mismatch (refImplName: " +
                           refImplName +
                           " and orbServerId " +
                           orbServerId +
                           " and orbImplName " +
                           orbImplName
                        );
                    }
                    throw new WrongAdapter();
                }

                // Get cached scopes from ServerRequest
                scopes = request.Scopes;

                for (int i = 0; i < scopes.Count; i++)
                {
                    string res = scopes[i];

                    if (res.Length == 0)
                    {
                        break;
                    }

                    /* the following is a call to a method in the private interface between the ORB and the POA. It does the
                       necessary synchronization between incoming, potentially concurrent requests to activate a POA
                       using its adapter activator. This call will block until the correct POA is activated and ready to
                       service requests. Thus, concurrent calls originating from a single, multi-threaded client
                       will be serialized because the thread that accepts incoming requests from the client process is
                       blocked. Concurrent calls from other destinations are not serialized unless they involve activating
                       the same adapter.
                    */
                    try
                    {
                        tmpPOA = tmpPOA.GetChildPOA(res);
                    }
                    catch (ParentIsHoldingException p)
                    {
                        /* if one of the POAs is in holding state, we simply deliver the request to this
                           POA. It will forward the request to its child POAs if necessary when changing back
                           to active For the POA to be able to forward this request to its child POAa, we need to
                           supply the remaining part of the child's POA name */

                        string[] restOfName = new string[scopes.Count - i];
                        for (int j = 0; j < restOfName.Length; j++)
                        {
                            restOfName[j] = scopes[j + i];
                        }

                        request.RemainingPOAName = restOfName;

                        break;
                    }
                }

                if (tmpPOA == null)
                {
                    throw new CORBA.Internal("Request POA null!");
                }

                /* hand over to the POA */
                tmpPOA.Invoke(request);
            }
            catch (WrongAdapter e)
            {
                // unknown oid (not previously generated)
                request.SystemException = new ObjectNotExist("unknown oid", (int)(OMGVMCID.Value | 2), CompletionStatus.No);
                request.Reply();
            }
            catch (CORBA.SystemException e)
            {
                request.SystemException = e;
                request.Reply();
            }
            catch (RuntimeException e)
            {
                request.SystemException = new Unknown(e.ToString());
                request.Reply();
                logger.Warn("unexpected exception", e);
            }
        }

        private CORBA.ApplicationException GetApplicationException(ReplyInputStream reply)
        {
            reply.Mark(0);
            string id = reply.ReadString();

            try
            {
                reply.Reset();
            }
            catch (Exception ioe)
            {
                logger.Error("unexpected Exception in Reset()", ioe);
            }

            return new CORBA.ApplicationException(id, reply);
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
