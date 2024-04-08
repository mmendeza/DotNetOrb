// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;

namespace DotNetOrb.Core
{
    public class DataOutputStream : IDataOutputStream
    {
        private IOutputStream stream;

        /**
         * Constructor
         */
        public DataOutputStream(IOutputStream stream)
        {
            this.stream = stream;
        }

        public void WriteAny(CORBA.Any value)
        {
            stream.WriteAny(value);
        }

        public void WriteBoolean(bool value)
        {
            stream.WriteBoolean(value);
        }

        public void WriteBooleanArray(bool[] value, int offset, int length)
        {
            stream.WriteBooleanArray(value, offset, length);
        }

        public void WriteChar(char value)
        {
            stream.WriteChar(value);
        }

        public void WriteCharArray(char[] value, int offset, int length)
        {
            stream.WriteCharArray(value, offset, length);
        }

        public void WriteDouble(double value)
        {
            stream.WriteDouble(value);
        }

        public void WriteDoubleArray(double[] value, int offset, int length)
        {
            stream.WriteDoubleArray(value, offset, length);
        }

        public void WriteFixed(decimal value, ushort digits, ushort scale)
        {
            stream.WriteFixed(value, digits, scale);
        }

        public void WriteFloat(float value)
        {
            stream.WriteFloat(value);
        }

        public void WriteFloatArray(float[] value, int offset, int length)
        {
            stream.WriteFloatArray(value, offset, length);
        }

        public void WriteLong(int value)
        {
            stream.WriteLong(value);
        }

        public void WriteLongArray(int[] value, int offset, int length)
        {
            stream.WriteLongArray(value, offset, length);
        }

        public void WriteLongLong(long value)
        {
            stream.WriteLongLong(value);
        }

        public void WriteLongLongArray(long[] value, int offset, int length)
        {
            stream.WriteLongLongArray(value, offset, length);
        }

        public void WriteObject(CORBA.Object value)
        {
            stream.WriteObject(value);
        }

        public void WriteObject(IObject value)
        {
            stream.WriteObject(value);
        }

        public void WriteOctet(byte value)
        {
            stream.WriteOctet(value);
        }

        public void WriteOctetArray(byte[] value, int offset, int length)
        {
            stream.WriteOctetArray(value, offset, length);
        }

        public void WriteShort(short value)
        {
            stream.WriteShort(value);
        }

        public void WriteShortArray(short[] value, int offset, int length)
        {
            stream.WriteShortArray(value, offset, length);
        }

        public void WriteString([WideChar(false)] string value)
        {
            stream.WriteString(value);
        }

        public void WriteTypeCode(CORBA.TypeCode value)
        {
            stream.WriteTypeCode(value);
        }

        public void WriteULong(uint value)
        {
            stream.WriteULong(value);
        }

        public void WriteULongArray(uint[] value, int offset, int length)
        {
            stream.WriteULongArray(value, offset, length);
        }

        public void WriteULongLong(ulong value)
        {
            stream.WriteULongLong(value);
        }

        public void WriteULongLongArray(ulong[] value, int offset, int length)
        {
            stream.WriteULongLongArray(value, offset, length);
        }

        public void WriteUShort(ushort value)
        {
            stream.WriteUShort(value);
        }

        public void WriteUShortArray(ushort[] value, int offset, int length)
        {
            stream.WriteUShortArray(value, offset, length);
        }

        public void WriteWChar(char value)
        {
            stream.WriteWChar(value);
        }

        public void WriteWCharArray(char[] value, int offset, int length)
        {
            stream.WriteWCharArray(value, offset, length);
        }

        public void WriteWString([WideChar(true)] string value)
        {
            stream.WriteWString(value);
        }
    }
}
