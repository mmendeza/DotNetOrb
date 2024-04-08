// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DotNetty.Buffers;
using GIOP;
using System;
using Version = GIOP.Version;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPHeader
    {
        public static int HeaderSize = 12;

        public Version GIOPVersion { get; set; }

        public bool IsLittleEndian { get; set; }

        public bool HasMoreFragments { get; set; }

        public MsgType11 MessageType { get; set; }

        public uint MessageSize { get; set; }

        public bool IsCompressed { get; set; }

        public GIOPHeader(MsgType11 msgType, byte giopminor)
        {
            MessageType = msgType;
            GIOPVersion = new Version(1, giopminor);
        }

        public GIOPHeader(IByteBuffer input)
        {
            byte firstMagicChar = input.ReadByte();

            if (firstMagicChar != (byte)'G' && firstMagicChar != (byte)'Z'
                || input.ReadByte() != (byte)'I'
                || input.ReadByte() != (byte)'O'
                || input.ReadByte() != (byte)'P')
                throw new CORBA.Marshal("Bad GIOP Message header: Invalid header identifier.", 0, CompletionStatus.No);

            IsCompressed = firstMagicChar == 'Z';

            byte major = input.ReadByte();
            byte minor = input.ReadByte();

            GIOPVersion = new Version(major, minor);

            if (GIOPVersion == null)
                throw new CORBA.Marshal("Bad GIOP Message header: Invalid version number.", 0, CompletionStatus.No);

            byte flag = input.ReadByte();

            if (GIOPVersion.Minor == 0)
                IsLittleEndian = flag != 0;
            else
            {
                IsLittleEndian = (flag & 0x1) != 0;
                HasMoreFragments = (flag & 0x2) != 0;
            }

            var messageType = input.ReadByte();

            if (!Enum.IsDefined(typeof(MsgType11), (int)messageType))
                throw new CORBA.Marshal("Bad GIOP Message header: Invalid message type.", 0, CompletionStatus.No);

            MessageType = (MsgType11)messageType;

            if (IsLittleEndian)
            {
                MessageSize = input.ReadUnsignedIntLE();
            }
            else
            {
                MessageSize = input.ReadUnsignedInt();
            }
        }

        //public GIOPHeader(CDRInputStream input)
        //{
        //    byte firstMagicChar = input.ReadByte();

        //    if (((firstMagicChar != (byte)'G') && (firstMagicChar != (byte)'Z'))
        //        || (input.ReadByte() != (byte)'I')
        //        || (input.ReadByte() != (byte)'O')
        //        || (input.ReadByte() != (byte)'P'))
        //        throw new CORBA.Marshal("Bad GIOP Message header: Invalid header identifier.", 0, CompletionStatus.No);

        //    IsCompressed = (firstMagicChar == 'Z');

        //    byte major = input.ReadByte();
        //    byte minor = input.ReadByte();

        //    GIOPVersion = new Version(major, minor);

        //    if (GIOPVersion == null)
        //        throw new CORBA.Marshal("Bad GIOP Message header: Invalid version number.", 0, CompletionStatus.No);

        //    byte flag = input.ReadByte();

        //    if (GIOPVersion.Minor == 0)
        //        IsLittleEndian = (flag != 0);
        //    else
        //    {
        //        IsLittleEndian = ((flag & 0x1) != 0);
        //        HasMoreFragments = ((flag & 0x2) != 0);
        //    }

        //    var messageType = input.ReadByte();

        //    if (!Enum.IsDefined(typeof(MsgType11), (int)messageType))
        //        throw new CORBA.Marshal("Bad GIOP Message header: Invalid message type.", 0, CompletionStatus.No);

        //    MessageType = (MsgType11)messageType;

        //    input.IsLittleEndian = IsLittleEndian;

        //    MessageSize = input.ReadULong();
        //}

        public void Write(CDROutputStream output)
        {
            if (IsCompressed)
                output.WriteByte((byte)'Z');
            else
                output.WriteByte((byte)'G');
            output.WriteByte((byte)'I');
            output.WriteByte((byte)'O');
            output.WriteByte((byte)'P');

            output.WriteByte(GIOPVersion.Major);
            output.WriteByte(GIOPVersion.Minor);

            if (GIOPVersion.Minor == 0)
                output.WriteBoolean(IsLittleEndian);
            else
            {
                int flag = 0;
                if (IsLittleEndian)
                    flag |= 0x1;
                if (HasMoreFragments)
                    flag |= 0x2;
                output.WriteByte((byte)flag);
            }

            output.WriteByte((byte)MessageType);
            //Skip message size
            output.Skip(4);
        }
    }
}
