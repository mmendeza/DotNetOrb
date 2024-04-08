// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using System.Collections.Generic;

namespace DotNetOrb.Core
{
    public class DataInputStream : IDataInputStream
    {
        private IInputStream stream;

        /**
         * Constructor
         */
        public DataInputStream(IInputStream stream)
        {
            this.stream = stream;
        }

        public CORBA.Any ReadAny()
        {
            return stream.ReadAny();
        }

        public bool ReadBoolean()
        {
            return stream.ReadBoolean();
        }

        public void ReadBooleanArray(ref bool[] value, int offset, int length)
        {
            stream.ReadBooleanArray(ref value, offset, length);
        }

        public char ReadChar()
        {
            return stream.ReadChar();
        }

        public void ReadCharArray(ref char[] value, int offset, int length)
        {
            stream.ReadCharArray(ref value, offset, length);
        }

        public double ReadDouble()
        {
            return stream.ReadDouble();
        }

        public void ReadDoubleArray(ref double[] value, int offset, int length)
        {
            stream.ReadDoubleArray(ref value, offset, length);
        }

        public decimal ReadFixed(ushort digits, ushort scale)
        {
            return stream.ReadFixed(digits, scale);
        }

        public float ReadFloat()
        {
            return stream.ReadFloat();
        }

        public void ReadFloatArray(ref float[] value, int offset, int length)
        {
            stream.ReadFloatArray(ref value, offset, length);
        }

        public int ReadLong()
        {
            return stream.ReadLong();
        }

        public void ReadLongArray(ref int[] value, int offset, int length)
        {
            stream.ReadLongArray(ref value, offset, length);
        }

        public long ReadLongLong()
        {
            return stream.ReadLongLong();
        }

        public void ReadLongLongArray(ref long[] value, int offset, int length)
        {
            stream.ReadLongLongArray(ref value, offset, length);
        }

        public IObject ReadObject()
        {
            return stream.ReadObject();
        }

        public byte ReadOctet()
        {
            return stream.ReadOctet();
        }

        public void ReadOctetArray(ref byte[] value, int offset, int length)
        {
            stream.ReadOctetArray(ref value, offset, length);
        }

        public short ReadShort()
        {
            return stream.ReadShort();
        }

        public void ReadShortArray(ref short[] value, int offset, int length)
        {
            stream.ReadShortArray(ref value, offset, length);
        }

        [return: WideChar(false)]
        public string ReadString()
        {
            return stream.ReadString();
        }

        public CORBA.TypeCode ReadTypeCode()
        {
            return stream.ReadTypeCode();
        }

        public CORBA.TypeCode ReadTypeCode(IDictionary<int, string> recursiveTCMap, IDictionary<int, CORBA.TypeCode> repeatedTCMap)
        {
            return stream.ReadTypeCode(recursiveTCMap, repeatedTCMap);
        }

        public uint ReadULong()
        {
            return stream.ReadULong();
        }

        public void ReadULongArray(ref uint[] value, int offset, int length)
        {
            stream.ReadULongArray(ref value, offset, length);
        }

        public ulong ReadULongLong()
        {
            return stream.ReadULongLong();
        }

        public void ReadULongLongArray(ref ulong[] value, int offset, int length)
        {
            stream.ReadULongLongArray(ref value, offset, length);
        }

        public ushort ReadUShort()
        {
            return stream.ReadUShort();
        }

        public void ReadUShortArray(ref ushort[] value, int offset, int length)
        {
            stream.ReadUShortArray(ref value, offset, length);
        }

        public char ReadWChar()
        {
            return stream.ReadWChar();
        }

        public void ReadWCharArray(ref char[] value, int offset, int length)
        {
            stream.ReadWCharArray(ref value, offset, length);
        }

        [return: WideChar(true)]
        public string ReadWString()
        {
            return stream.ReadWString();
        }

    }
}
