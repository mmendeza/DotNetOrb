// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public interface IOutputStream
    {
        int Pos { get; set; }

        bool IsIndirectionEnabled { get; }

        ORB ORB { get; }

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
        void WriteLongDouble(decimal value);
        void WriteString(string value);
        void WriteWString(string value);
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
        void WriteLongDoubleArray(decimal[] value, int offset, int length);
        void WriteObject(IObject value);
        void WriteValue(object value);
        void WriteValue(object value, string repId);
        void WriteValue(object value, IBoxedValueHelper helper);
        void WriteAbstractInterface(object obj);
        void WriteTypeCode(TypeCode value);
        void WriteTypeCode(TypeCode value, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap);
        void WriteAny(Any value);
        void WriteContext(IContext value);
        void WriteFixed(decimal value, ushort digits, ushort scale);

        void BeginEncapsulation();
        void EndEncapsulation();

        void Close();

        IInputStream CreateInputStream();
    }
}
