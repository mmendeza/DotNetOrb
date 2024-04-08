// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public interface IDataOutputStream
    {
        void WriteBoolean(bool value);
        void WriteChar(char value);
        void WriteWChar(char value);
        void WriteOctet(byte value);
        void WriteShort(short value);
        void WriteUShort(ushort value);
        void WriteLong(int value);
        void WriteULong(uint value);
        void WriteLongLong(long value);
        void WriteULongLong(ulong value);
        void WriteFloat(float value);
        void WriteDouble(double value);
        void WriteString([WideChar(false)] string value);
        void WriteWString([WideChar(true)] string value);
        void WriteBooleanArray(bool[] value, int offset, int length);
        void WriteCharArray(char[] value, int offset, int length);
        void WriteWCharArray(char[] value, int offset, int length);
        void WriteOctetArray(byte[] value, int offset, int length);
        void WriteShortArray(short[] value, int offset, int length);
        void WriteUShortArray(ushort[] value, int offset, int length);
        void WriteLongArray(int[] value, int offset, int length);
        void WriteULongArray(uint[] value, int offset, int length);
        void WriteLongLongArray(long[] value, int offset, int length);
        void WriteULongLongArray(ulong[] value, int offset, int length);
        void WriteFloatArray(float[] value, int offset, int length);
        void WriteDoubleArray(double[] value, int offset, int length);
        void WriteObject(IObject value);
        void WriteTypeCode(TypeCode value);
        void WriteAny(Any value);
        [ThrowsIdlException(typeof(CORBA.BadFixedValue))]
        void WriteFixed(decimal value, ushort digits, ushort scale);
        //void WriteFixed(Any value);
        //[ThrowsIdlException(typeof(CORBA.BadFixedValue))]
        //void WriteFixedArray(Any[] value, int offset, int length);
    }
}
