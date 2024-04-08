// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using GIOP;
using IOP;
using System.Collections.Generic;

namespace DotNetOrb.Core.GIOP
{
    public class RequestOutputStream : CDROutputStream
    {
        public uint RequestId { get; set; }
        public ICollection<ServiceContext> ServiceContextList { get; set; }
        public CORBA.Object Object { get; private set; }
        public TargetAddress Target { get; private set; }
        public SyncScope SyncScope { get; private set; }

        public bool ResponseExpected
        {
            get
            {
                switch (SyncScope)
                {
                    case SyncScope.SERVER:
                    case SyncScope.TARGET:
                        return true;
                    default:
                        return false;
                }
            }
            set
            {
                if (value)
                {
                    SyncScope = SyncScope.TARGET;
                }
                else
                {
                    SyncScope = SyncScope.NONE;
                }
            }
        }

        public string Operation { get; private set; }
        public GIOPChannelHandler ChannelHandler { get; private set; }

        public RequestOutputStream(ORB orb, GIOPChannelHandler channelHandler, string operation, SyncScope syncScope, TargetAddress target, CORBA.Object obj, int giopMinor) : base(orb)
        {
            SyncScope = syncScope;
            Object = obj;
            Target = target;
            Operation = operation;
            ChannelHandler = channelHandler;
            GIOPMinor = giopMinor;
            ServiceContextList = new List<ServiceContext>();
            RequestId = channelHandler.GetRequestId();
            InitBuffer();
        }

        public RequestOutputStream(ORB orb, GIOPChannelHandler channelHandler, string operation, bool responseExpected, TargetAddress target, CORBA.Object obj, int giopMinor) : base(orb)
        {
            ResponseExpected = responseExpected;
            Object = obj;
            Target = target;
            Operation = operation;
            ChannelHandler = channelHandler;
            GIOPMinor = giopMinor;
            ServiceContextList = new List<ServiceContext>();
            RequestId = channelHandler.GetRequestId();
            InitBuffer();
        }

        public void InitBuffer()
        {
            var header = new GIOPHeader(MsgType11.Request, (byte)GIOPMinor);
            header.Write(this);
            /**
             * The serviceContext array is to align the data following this array on an 8 byte boundary.
             * This allows for adding service contexts to the header without having to remarshal everything that follows.
             * There are already 12 bytes on the stream (the GIOP message header), so the next possible 8 byte boundary is 16
             * This is reached by adding an array of length 0, because that will only write a ulong for the length (0), 
             * which consumes 4 bytes.
             *
             *  This is only necessary for GIOP 1.0/1.1, because in 1.2, the service context array is the last attribute 
             *  of the [Request|Reply]Header, and the body is per spec aligned to an 8 byte boundary.
             */
            var serviceContext = new ServiceContext[0];
            switch (GIOPMinor)
            {
                case 0:
                case 1:
                    {
                        ServiceContextListHelper.Write(this, serviceContext);
                        WriteULong(RequestId);
                        switch (SyncScope)
                        {
                            case SyncScope.TARGET:
                                WriteBoolean(true);
                                break;
                            default:
                                WriteBoolean(false);
                                break;
                        }
                        //Reserved                        
                        Buffer.SetZero(Pos, 3);
                        Skip(3);
                        ObjectKeyHelper.Write(this, Target.ObjectKey);
                        WriteString(Operation);
                        PrincipalHelper.Write(this, new byte[0]);
                    }
                    break;
                case 2:
                    {
                        WriteULong(RequestId);
                        switch (SyncScope)
                        {
                            case SyncScope.SERVER:
                                WriteByte(0x01);
                                break;
                            case SyncScope.TARGET:
                                WriteByte(0x03);
                                break;
                            default:
                                WriteByte(0x00);
                                break;
                        }
                        //Reserved                        
                        Buffer.SetZero(Pos, 3);
                        Skip(3);
                        TargetAddressHelper.Write(this, Target);
                        WriteString(Operation);
                        ServiceContextListHelper.Write(this, serviceContext);
                        Skip(GetHeaderPadding(Pos));
                    }
                    break;
                default:
                    return;
            }
        }

        protected static int GetHeaderPadding(int pos)
        {
            int headerPadding = 8 - pos % 8; //difference to next 8 byte border
            headerPadding = headerPadding == 8 ? 0 : headerPadding;
            return headerPadding;
        }
    }
}
