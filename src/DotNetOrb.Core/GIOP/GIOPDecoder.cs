// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using GIOP;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPDecoder : ByteToMessageDecoder, IConfigurable
    {
        private ORB orb;

        private MemoryCache fragments = new MemoryCache("GIOPFragments");
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
                        var content = input.ReadBytes(GIOPHeader.HeaderSize + (int)header.MessageSize);
                        switch (header.MessageType)
                        {
                            case MsgType11.Request:
                                {
                                    var message = new GIOPRequestMessage(orb, content);
                                    if (header.HasMoreFragments)
                                    {
                                        if (fragmentTimeout_ms > 0)
                                        {
                                            fragments.Set(message.RequestId.ToString(), message, DateTimeOffset.FromUnixTimeMilliseconds(fragmentTimeout_ms));
                                        }
                                        else
                                        {
                                            fragments.Set(message.RequestId.ToString(), message, ObjectCache.InfiniteAbsoluteExpiration);
                                        }
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
                                        if (fragmentTimeout_ms > 0)
                                        {
                                            fragments.Set(message.RequestId.ToString(), message, DateTimeOffset.FromUnixTimeMilliseconds(fragmentTimeout_ms));
                                        }
                                        else
                                        {
                                            fragments.Set(message.RequestId.ToString(), message, ObjectCache.InfiniteAbsoluteExpiration);
                                        }
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
                                        if (fragmentTimeout_ms > 0)
                                        {
                                            fragments.Set(message.RequestId.ToString(), message, DateTimeOffset.FromUnixTimeMilliseconds(fragmentTimeout_ms));
                                        }
                                        else
                                        {
                                            fragments.Set(message.RequestId.ToString(), message, ObjectCache.InfiniteAbsoluteExpiration);
                                        }
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
                                        if (fragmentTimeout_ms > 0)
                                        {
                                            fragments.Set(message.RequestId.ToString(), message, DateTimeOffset.FromUnixTimeMilliseconds(fragmentTimeout_ms));
                                        }
                                        else
                                        {
                                            fragments.Set(message.RequestId.ToString(), message, ObjectCache.InfiniteAbsoluteExpiration);
                                        }
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
                                    if (fragments.Contains(message.RequestId.ToString()))
                                    {
                                        var fragmentedMessage = (GIOPFragmentedMessage)fragments.Get(message.RequestId.ToString());
                                        fragmentedMessage.AddFragment(message);
                                        if (header.HasMoreFragments)
                                        {
                                            if (fragmentTimeout_ms > 0)
                                            {
                                                fragments.Set(fragmentedMessage.RequestId.ToString(), fragmentedMessage, DateTimeOffset.FromUnixTimeMilliseconds(fragmentTimeout_ms));
                                            }
                                            else
                                            {
                                                fragments.Set(fragmentedMessage.RequestId.ToString(), fragmentedMessage, ObjectCache.InfiniteAbsoluteExpiration);
                                            }
                                        }
                                        else
                                        {
                                            fragments.Remove(message.RequestId.ToString());
                                            output.Add(message);
                                        }
                                    }
                                    else
                                    {
                                        logger.Error("Fragment not found for request id: " + message.RequestId);
                                    }
                                }
                                break;
                            case MsgType11.CancelRequest:
                                {
                                    var message = new GIOPCancelRequestMessage(orb, content);
                                    if (fragments.Contains(message.RequestId.ToString()))
                                    {
                                        fragments.Remove(message.RequestId.ToString());
                                    }
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
                                    logger.Error("Received a unknow message Type: " + header.MessageType);
                                }
                                break;
                        }
                        header = null;
                        currentState = State.ReadHeader;
                    }
                    break;
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
