// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public abstract class Any: IIDLEntity
    {
        abstract public TypeCode Type { get; set; }
        abstract public object Value { get; }
        abstract public IInputStream CreateInputStream();
        abstract public IOutputStream CreateOutputStream();
        abstract public bool Equal(Any other);        
        abstract public Any ExtractAny();
        abstract public bool ExtractBoolean();
        abstract public char ExtractChar();
        abstract public double ExtractDouble();
        abstract public decimal ExtractFixed();
        abstract public float ExtractFloat();
        abstract public int ExtractLong();
        abstract public long ExtractLongLong();
        abstract public IObject ExtractObject();
        abstract public byte ExtractOctet();
        abstract public short ExtractShort();
        abstract public IStreamable ExtractStreamable();
        abstract public string ExtractString();
        abstract public TypeCode ExtractTypeCode();
        abstract public uint ExtractULong();
        abstract public ulong ExtractULongLong();
        abstract public ushort ExtractUShort();
        abstract public object ExtractValue();
        abstract public char ExtractWChar();
        abstract public string ExtractWString();
        abstract public void Insert(TypeCode typeCode, object value);
        abstract public void InsertAny(Any s);
        abstract public void InsertBoolean(bool s);
        abstract public void InsertChar(char s);
        abstract public void InsertDouble(double s);
        abstract public void InsertFixed(decimal value, TypeCode type);
        abstract public void InsertFloat(float s);
        abstract public void InsertLong(int s);
        abstract public void InsertLongLong(long s);
        abstract public void InsertObject(IObject obj);
        abstract public void InsertObject(IObject obj, TypeCode type);
        abstract public void InsertOctet(byte s);
        abstract public void InsertShort(short s);
        abstract public void InsertStreamable(IStreamable s);
        abstract public void InsertString(string s);
        abstract public void InsertTypeCode(TypeCode s);
        abstract public void InsertULong(uint s);
        abstract public void InsertULongLong(ulong s);
        abstract public void InsertUShort(ushort s);
        abstract public void InsertValue(object value);
        abstract public void InsertValue(object value, TypeCode type);
        abstract public void InsertVoid();
        abstract public void InsertWChar(char s);
        abstract public void InsertWString(string s);
        abstract public void ReadValue(IInputStream input, TypeCode type);
        abstract public void WriteValue(IOutputStream output);
    }
}