// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace CORBA
{
    public interface IInputStream
    {
        int Available { get; }
        int Pos { get; set; }
        ORB Orb { get; }
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
        decimal ReadLongDouble();
        string ReadString();
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
        void ReadLongDoubleArray(ref decimal[] value, int offset, int length);
        CORBA.IObject ReadObject();
        CORBA.IObject ReadObject(Type type);
        object ReadValue();
        object ReadValue(string repId);
        object ReadValue(Type type);
        object ReadValue(IBoxedValueHelper factory);
        object ReadValue(object value);
        TypeCode ReadTypeCode();
        TypeCode ReadTypeCode(IDictionary<int, string> recursiveTCMap, IDictionary<int, TypeCode> repeatedTCMap);
        Any ReadAny();
        //IContext ReadContext();
        decimal ReadFixed(ushort digits, ushort scale);
        void Skip(int distance);
        int OpenEncapsulation();
        void CloseEncapsulation();
        object ReadAbstractInterface();
        TypeCode? ReadTypeCodeCache(string repositoryID, int startPosition);
        void UpdateTypeCodeCache(string repositoryID, int startPosition, int size);
        void Close();
    }
}
