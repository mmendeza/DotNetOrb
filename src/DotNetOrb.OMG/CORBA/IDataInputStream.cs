// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public interface IDataInputStream
    {        
        bool ReadBoolean();
        char ReadChar();
        char ReadWChar();
        byte ReadOctet();
        short ReadShort();
        ushort ReadUShort();
        int ReadLong();
        uint ReadULong();
        long ReadLongLong();
        ulong ReadULongLong();
        float ReadFloat();
        double ReadDouble();
        [return: WideChar(false)]
        string ReadString();
        [return: WideChar(true)]
        string ReadWString();
        void ReadBooleanArray(ref bool[] value, int offset, int length);
        void ReadCharArray(ref char[] value, int offset, int length);
        void ReadWCharArray(ref char[] value, int offset, int length);
        void ReadOctetArray(ref byte[] value, int offset, int length);
        void ReadShortArray(ref short[] value, int offset, int length);
        void ReadUShortArray(ref ushort[] value, int offset, int length);
        void ReadLongArray(ref int[] value, int offset, int length);
        void ReadULongArray(ref uint[] value, int offset, int length);
        void ReadLongLongArray(ref long[] value, int offset, int length);
        void ReadULongLongArray(ref ulong[] value, int offset, int length);
        void ReadFloatArray(ref float[] value, int offset, int length);
        void ReadDoubleArray(ref double[] value, int offset, int length);
        CORBA.IObject ReadObject();
        TypeCode ReadTypeCode();
        TypeCode ReadTypeCode(IDictionary<int, string> recursiveTCMap, IDictionary<int, TypeCode> repeatedTCMap);
        Any ReadAny();
        //Any ReadAnyArray(ref Any[] value, int offset, int length);
        //[ThrowsIdlException(typeof(CORBA.BadFixedValue))]
        decimal ReadFixed(ushort digits, ushort scale);
        //[ThrowsIdlException(typeof(CORBA.BadFixedValue))]
        //void ReadFixedArray(ref decimal[] value, int offset, int length, ushort digits, ushort scale);
    }
}
