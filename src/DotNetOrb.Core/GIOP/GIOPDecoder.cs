// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using GIOP;
using System;
using System.Collections.Generic;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPDecoder : ByteToMessageDecoder, IConfigurable
    {
        private ORB orb;

        /// <summary>
        /// Messages that are waiting for more fragments, keyed by request id.
        /// A decoder belongs to a single channel and is only called on that channel's event loop,
        /// so no locking is needed.
        /// </summary>
        private Dictionary<uint, PendingMessage> fragments = new Dictionary<uint, PendingMessage>();

        /// <summary>
        /// GIOP 1.1 fragments have no request id, they always continue the last message
        /// that was sent with the 'more fragments' flag set.
        /// </summary>
        private uint? lastFragmentedRequestId = null;

        private class PendingMessage
        {
            public GIOPFragmentedMessage Message;
            public DateTime ExpiresUtc;
        }

        enum State
        {
            ReadHeader,
            ReadMessage
        }

        private GIOPHeader header;

        State currentState = State.ReadHeader;

        private uint fragmentTimeout_ms = 300000;

        ILogger logger;

        public GIOPDecoder(ORB orb)
        {
            Configure(orb.Configuration);
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            switch (currentState)
            {
                case State.ReadHeader:
                    {
                        if (input.ReadableBytes < GIOPHeader.HeaderSize)
                        {
                            return;
                        }
                        input.MarkReaderIndex();
                        header = new GIOPHeader(input);
                        input.ResetReaderIndex();
                        currentState = State.ReadMessage;
                        goto case State.ReadMessage;
                    }
                    break;
                case State.ReadMessage:
                    {
                        if (input.ReadableBytes < GIOPHeader.HeaderSize + header.MessageSize)
                        {
                            return;
                        }
                        RemoveExpiredFragments();
                        var content = input.ReadBytes(GIOPHeader.HeaderSize + (int)header.MessageSize);
                        switch (header.MessageType)
                        {
                            case MsgType11.Request:
                                {
                                    var message = new GIOPRequestMessage(orb, content);
                                    if (header.HasMoreFragments)
                                    {
                                        AddPendingMessage(message);
                                    }
                                    else
                                    {
                                        output.Add(message);
                                    }
                                }
                                break;
                            case MsgType11.LocateRequest:
                                {
                                    var message = new GIOPLocateRequestMessage(orb, content);
                                    if (header.HasMoreFragments)
                                    {
                                        AddPendingMessage(message);
                                    }
                                    else
                                    {
                                        output.Add(message);
                                    }
                                }
                                break;
                            case MsgType11.Reply:
                                {
                                    var message = new GIOPReplyMessage(orb, content);
                                    if (header.HasMoreFragments)
                                    {
                                        AddPendingMessage(message);
                                    }
                                    else
                                    {
                                        output.Add(message);
                                    }
                                }
                                break;
                            case MsgType11.LocateReply:
                                {
                                    var message = new GIOPLocateReplyMessage(orb, content);
                                    if (header.HasMoreFragments)
                                    {
                                        AddPendingMessage(message);
                                    }
                                    else
                                    {
                                        output.Add(message);
                                    }
                                }
                                break;
                            case MsgType11.Fragment:
                                {
                                    var message = new GIOPFragmentMessage(orb, content);
                                    uint? requestId = message.Header.GIOPVersion.Minor >= 2 ? message.RequestId : lastFragmentedRequestId;
                                    if (requestId.HasValue && fragments.TryGetValue(requestId.Value, out PendingMessage pending))
                                    {
                                        // the fragment's body (everything after its header) is appended to the original message
                                        pending.Message.AddFragment(message);
                                        if (header.HasMoreFragments)
                                        {
                                            pending.ExpiresUtc = GetExpiry();
                                        }
                                        else
                                        {
                                            // the message is complete, so hand on the whole message (not just the last fragment)
                                            fragments.Remove(requestId.Value);
                                            if (lastFragmentedRequestId == requestId)
                                            {
                                                lastFragmentedRequestId = null;
                                            }
                                            output.Add(pending.Message);
                                        }
                                    }
                                    else
                                    {
                                        if (logger.IsErrorEnabled)
                                            logger.Error("Fragment not found for request id: " + message.RequestId);
                                        message.Release();
                                    }
                                }
                                break;
                            case MsgType11.CancelRequest:
                                {
                                    var message = new GIOPCancelRequestMessage(orb, content);
                                    RemovePendingMessage(message.RequestId);
                                    output.Add(message);
                                }
                                break;
                            case MsgType11.CloseConnection:
                            case MsgType11.MessageError:
                                {
                                    var message = new GIOPMessage(orb, content);
                                    output.Add(message);
                                }
                                break;
                            default:
                                {
                                    if (logger.IsErrorEnabled)
                                        logger.Error("Received a unknow message Type: " + header.MessageType);
                                    content.Release();
                                }
                                break;
                        }
                        header = null;
                        currentState = State.ReadHeader;
                    }
                    break;
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            // release any messages that will never be completed
            foreach (var pending in fragments.Values)
            {
                pending.Message.Release();
            }
            fragments.Clear();
            lastFragmentedRequestId = null;

            base.ChannelInactive(context);
        }

        private void AddPendingMessage(GIOPFragmentedMessage message)
        {
            RemovePendingMessage(message.RequestId);
            fragments[message.RequestId] = new PendingMessage
            {
                Message = message,
                ExpiresUtc = GetExpiry()
            };
            lastFragmentedRequestId = message.RequestId;
        }

        private void RemovePendingMessage(uint requestId)
        {
            if (fragments.TryGetValue(requestId, out PendingMessage pending))
            {
                fragments.Remove(requestId);
                pending.Message.Release();
            }
            if (lastFragmentedRequestId == requestId)
            {
                lastFragmentedRequestId = null;
            }
        }

        private DateTime GetExpiry()
        {
            // a timeout of 0 means fragments are kept until the message is complete
            return fragmentTimeout_ms > 0 ? DateTime.UtcNow.AddMilliseconds(fragmentTimeout_ms) : DateTime.MaxValue;
        }

        private void RemoveExpiredFragments()
        {
            if (fragments.Count == 0)
            {
                return;
            }

            var now = DateTime.UtcNow;
            List<uint> expired = null;
            foreach (var kvp in fragments)
            {
                if (kvp.Value.ExpiresUtc <= now)
                {
                    if (expired == null)
                    {
                        expired = new List<uint>();
                    }
                    expired.Add(kvp.Key);
                }
            }

            if (expired != null)
            {
                foreach (var requestId in expired)
                {
                    if (logger.IsWarnEnabled)
                        logger.Warn("Discarding incomplete fragmented message for request id: " + requestId);
                    RemovePendingMessage(requestId);
                }
            }
        }

        public void Configure(IConfiguration config)
        {
            try
            {
                fragmentTimeout_ms = (uint)config.GetAsInteger("DotNetOrb.GIOP.FragmentTimeout", 300000);
                logger = config.GetLogger(GetType().FullName);
            }
            catch (ConfigException)
            {
                throw new CORBA.Internal("Unable to configure GIOPDecoder");
            }
        }


    }
}
