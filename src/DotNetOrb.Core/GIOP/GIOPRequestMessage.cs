// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DotNetty.Buffers;
using GIOP;
using IOP;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPRequestMessage : GIOPServiceContextMessage
    {
        public SyncScope SyncScope { get; set; }

        public TargetAddress Target { get; set; }

        public string Operation { get; set; }

        public GIOPRequestMessage(ORB orb, IByteBuffer content) : base(orb)
        {
            this.content.AddComponent(content);
            this.content.SetWriterIndex(this.content.WriterIndex + content.ReadableBytes);
            Header = new GIOPHeader(this.content);
            CDRInputStream input = new CDRInputStream(ORB, this.content, Header.GIOPVersion.Minor, Header.IsLittleEndian);
            switch (Header.GIOPVersion.Minor)
            {
                case 0:
                case 1:
                    {
                        ServiceContextList.AddRange(ServiceContextListHelper.Read(input));
                        RequestId = input.ReadULong();
                        var withResponse = input.ReadBoolean();
                        if (withResponse)
                        {
                            SyncScope = SyncScope.TARGET;
                        }
                        else
                        {
                            SyncScope = SyncScope.NONE;
                        }
                        //Reserved
                        RequestReservedHelper.Read(input);
                        Target = new TargetAddress()
                        {
                            ObjectKey = ObjectKeyHelper.Read(input)
                        };
                        Operation = input.ReadString();
                        //ignored
                        PrincipalHelper.Read(input);
                    }
                    break;
                case 2:
                    {
                        RequestId = input.ReadULong();
                        var responseFlags = input.ReadOctet();
                        switch (responseFlags)
                        {
                            case 0x00:
                                SyncScope = SyncScope.NONE;
                                break;
                            case 0x01:
                                SyncScope = SyncScope.SERVER;
                                break;
                            case 0x03:
                                SyncScope = SyncScope.TARGET;
                                break;
                        }
                        RequestReservedHelper.Read(input);
                        Target = TargetAddressHelper.Read(input);
                        Operation = input.ReadString();
                        ServiceContextList.AddRange(ServiceContextListHelper.Read(input));
                    }
                    break;
                default:
                    throw new Marshal("Unknown GIOP minor version: " + Header.GIOPVersion.Minor);
            }
            MarkHeaderEnd();
        }

    }
}
