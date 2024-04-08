// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using System;
using System.Linq;
using TypeCode = CORBA.TypeCode;

namespace DotNetOrb.Core
{
    public class Any : CORBA.Any
    {
        private TypeCode typeCode;
        private object? value;
        public override object? Value => value;
        private ORB orb;

        public override TypeCode Type
        {
            get { return typeCode; }
            set
            {
                typeCode = value;
                this.value = null;
            }
        }

        public TypeCode OriginalType
        {
            get
            {
                return typeCode.OriginalTypeCode;
            }
        }

        public TCKind Kind
        {
            get
            {
                return typeCode.Kind;
            }
        }


        public Any(ORB orb)
        {
            this.orb = orb;
            typeCode = orb.GetPrimitiveTc(TCKind.TkNull);
        }

        public override bool Equal(CORBA.Any other)
        {
            if (other == null)
            {
                throw new BadParam("Null passed to Any equal operation");
            }

            if (!typeCode.Equivalent(other.Type))
            {
                return false;
            }

            switch (Kind)
            {
                case TCKind.TkNull:       // 0
                                          // fallthrough
                case TCKind.TkVoid:       // 1
                    {
                        return true;
                    }
                case TCKind.TkShort:      // 2
                    {
                        return ExtractShort() == other.ExtractShort();
                    }
                case TCKind.TkLong:       // 3
                    {
                        return ExtractLong() == other.ExtractLong();
                    }
                case TCKind.TkUshort:     // 4
                    {
                        return ExtractUShort() == other.ExtractUShort();
                    }
                case TCKind.TkUlong:      // 5
                    {
                        return ExtractULong() == other.ExtractULong();
                    }
                case TCKind.TkFloat:      // 6
                    {
                        return ExtractFloat() == other.ExtractFloat();
                    }
                case TCKind.TkDouble:     // 7
                    {
                        return ExtractDouble() == other.ExtractDouble();
                    }
                case TCKind.TkBoolean:    // 8
                    {
                        return ExtractBoolean() == other.ExtractBoolean();
                    }
                case TCKind.TkChar:       // 9
                    {
                        return ExtractChar() == other.ExtractChar();
                    }
                case TCKind.TkOctet:      // 10
                    {
                        return ExtractOctet() == other.ExtractOctet();
                    }
                case TCKind.TkAny:        // 11
                    {
                        return ExtractAny().Equal(other.ExtractAny());
                    }
                case TCKind.TkTypeCode:   // 12
                    {
                        return ExtractTypeCode().Equal(other.ExtractTypeCode());
                    }
                case TCKind.TkPrincipal:  // 13
                    {
                        throw new NoImplement("Principal deprecated");
                    }
                case TCKind.TkObjref:     // 14
                    {
                        object myValue = ExtractObject();
                        object otherValue = other.ExtractObject();
                        if (myValue == null && otherValue == null)
                        {
                            return true;
                        }
                        else if (myValue != null)
                        {
                            return myValue.Equals(otherValue);
                        }
                        else //if (otherValue != null)
                        {
                            // For this case otherValue must be non-null. Can there
                            // be a case where an actual object instance represents
                            // a null object reference? Ignore the FindBugs complaint
                            // here.
                            return otherValue.Equals(myValue);
                        }
                    }
                case TCKind.TkStruct:     // 15
                                          // fallthrough
                case TCKind.TkUnion:      // 16
                                          // falltrough
                case TCKind.TkEnum:       // 17
                    {
                        return CompareComplexValue((Any)other);
                    }
                case TCKind.TkString:     // 18
                    {
                        return ExtractString().Equals(other.ExtractString());
                    }
                case TCKind.TkSequence:   // 19
                                          // fallthrough
                case TCKind.TkArray:      // 20
                                          // fallthrough
                case TCKind.TkAlias:      // 21
                                          // fallthrough
                case TCKind.TkExcept:     // 22
                    {
                        return CompareComplexValue((Any)other);
                    }
                case TCKind.TkLonglong:   // 23
                    {
                        return ExtractLongLong() == other.ExtractLongLong();
                    }
                case TCKind.TkUlonglong:  // 24
                    {
                        return ExtractULongLong() == other.ExtractULongLong();
                    }
                case TCKind.TkLongdouble: // 25
                    {
                        throw new BadTypeCode("type longdouble not supported");
                    }
                case TCKind.TkWchar:      // 26
                    {
                        return ExtractWChar() == other.ExtractWChar();
                    }
                case TCKind.TkWstring:    // 27
                    {
                        return ExtractWString().Equals(other.ExtractWString());
                    }
                case TCKind.TkFixed:      // 28
                    {
                        return ExtractFixed().Equals(other.ExtractFixed());
                    }
                case TCKind.TkValue:      // 29
                                          // fallthrough
                case TCKind.TkValueBox:  // 30
                    {
                        return CompareComplexValue((Any)other);
                    }
                // These typecodes are not currently supported.
                //case TCKind.Tk_tk_native:               // 31
                //case TCKind.Tk_tk_abstract_interface:   // 32
                //case TCKind.Tk_tk_local_interface:      // 33
                default:
                    {
                        throw new BadTypeCode("Cannot compare anys with TypeCode kind " + Kind);
                    }
            }
        }

        public override string ToString()
        {
            if (value != null)
            {
                return value.ToString();
            }
            return "null";
        }

        public override IOutputStream CreateOutputStream()
        {
            value = new CDROutputStream(orb);

            return (IOutputStream)value;
        }

        public override IInputStream CreateInputStream()
        {
            if (value is CDROutputStream)
            {
                return new CDRInputStream(orb, ((CDROutputStream)value).GetBufferCopy()) as IInputStream;
            }

            var outputStream = new CDROutputStream(orb);

            try
            {
                WriteValue(outputStream);
                return new CDRInputStream(orb, outputStream.GetBufferCopy()) as IInputStream;
            }
            finally
            {
                outputStream.Close();
            }
        }

        public override void ReadValue(IInputStream input, TypeCode type)
        {
            if (type == null)
            {
                throw new BadParam("TypeCode is null");
            }
            typeCode = type;

            var kind = type.Kind;

            if (value is IStreamable &&
                kind != TCKind.TkValue &&
                kind != TCKind.TkValueBox &&
                kind != TCKind.TkAbstractInterface)
            {
                ((IStreamable)value)._Read(input);
            }
            else
            {
                switch (kind)
                {
                    case TCKind.TkNull:       // 0
                    case TCKind.TkVoid:       // 1
                        {
                            value = null;
                            break;
                        }
                    case TCKind.TkShort:      // 2
                        {
                            InsertShort(input.ReadShort());
                            break;
                        }
                    case TCKind.TkLong:       // 3
                        {
                            InsertLong(input.ReadLong());
                            break;
                        }
                    case TCKind.TkUshort:     // 4
                        {
                            InsertUShort(input.ReadUShort());
                            break;
                        }
                    case TCKind.TkUlong:      // 5
                        {
                            InsertULong(input.ReadULong());
                            break;
                        }
                    case TCKind.TkFloat:      // 6
                        {
                            InsertFloat(input.ReadFloat());
                            break;
                        }
                    case TCKind.TkDouble:     // 7
                        {
                            InsertDouble(input.ReadDouble());
                            break;
                        }
                    case TCKind.TkBoolean:    // 8
                        {
                            InsertBoolean(input.ReadBoolean());
                            break;
                        }
                    case TCKind.TkChar:       // 9
                        {
                            InsertChar(input.ReadChar());
                            break;
                        }
                    case TCKind.TkOctet:      // 10
                        {
                            InsertOctet(input.ReadOctet());
                            break;
                        }
                    case TCKind.TkAny:        // 11
                        {
                            InsertAny(input.ReadAny());
                            break;
                        }
                    case TCKind.TkTypeCode:   // 12
                        {
                            InsertTypeCode(input.ReadTypeCode());
                            break;
                        }
                    case TCKind.TkPrincipal:  // 13
                        {
                            throw new NoImplement("Principal deprecated");
                        }
                    case TCKind.TkObjref:     // 14
                        {
                            InsertObject(input.ReadObject(), type);
                            break;
                        }
                    case TCKind.TkStruct:     // 15
                                              // fallthrough
                    case TCKind.TkUnion:      // 16
                                              // fallthrough
                    case TCKind.TkEnum:       // 17
                        {
                            CDROutputStream outputStream = new CDROutputStream(orb);
                            type.RemarshalValue(input, outputStream);
                            value = outputStream;
                            break;
                        }
                    case TCKind.TkString:     // 18
                        {
                            InsertString(input.ReadString());
                            break;
                        }
                    case TCKind.TkSequence:   // 19
                                              // fallthrough
                    case TCKind.TkArray:      // 20
                                              // fallthrough
                    case TCKind.TkAlias:      // 21
                                              // fallthrough
                    case TCKind.TkExcept:     // 22
                        {
                            CDROutputStream outputStream = new CDROutputStream(orb);
                            type.RemarshalValue(input, outputStream);
                            value = outputStream;
                            break;
                        }
                    case TCKind.TkLonglong:   // 23
                        {
                            InsertLongLong(input.ReadLongLong());
                            break;
                        }
                    case TCKind.TkUlonglong:  // 24
                        {
                            InsertULongLong(input.ReadULongLong());
                            break;
                        }
                    case TCKind.TkLongdouble: // 25
                        {
                            throw new BadTypeCode("type longdouble not supported");
                        }
                    case TCKind.TkWchar:      // 26
                        {
                            InsertWChar(input.ReadWChar());
                            break;
                        }
                    case TCKind.TkWstring:    // 27
                        {
                            InsertWString(input.ReadWString());
                            break;
                        }
                    case TCKind.TkFixed:      // 28
                        {
                            try
                            {
                                // move the decimal based on the scale
                                decimal d = ((CDRInputStream)input).ReadFixed(type.FixedDigits, type.FixedScale);
                                InsertFixed(d, type);
                            }
                            catch (BadKind bk)
                            {
                                throw new CORBA.Internal("should never happen");
                            }
                            break;
                        }
                    case TCKind.TkValue:      // 29
                    case TCKind.TkValueBox:  // 30
                        {
                            InsertValue(input.ReadValue(), type);
                            break;
                        }
                    case TCKind.TkNative:     //31
                        {
                            throw new BadTypeCode("Cannot handle TypeCode with kind " + kind);
                        }
                    case TCKind.TkAbstractInterface: // 32
                        {
                            object obj = input.ReadAbstractInterface();
                            if (obj is CORBA.Object)
                            {
                                Insert(type, obj);
                            }
                            else
                            {
                                InsertValue(obj, type);
                            }
                            break;
                        }
                    default:
                        {
                            throw new BadTypeCode("Cannot handle TypeCode with kind " + kind);
                        }
                }
            }
        }

        public override void WriteValue(IOutputStream output)
        {
            var kind = typeCode.Kind;

            if (value is IStreamable &&
                kind != TCKind.TkValue &&
                kind != TCKind.TkValueBox &&
                kind != TCKind.TkAbstractInterface)
            {
                ((IStreamable)value)._Write(output);
            }
            else
            {
                switch (kind)
                {
                    case TCKind.TkNull:       // 0
                    case TCKind.TkVoid:       // 1
                        {
                            break;
                        }
                    case TCKind.TkShort:      // 2
                        {
                            output.WriteShort(ExtractShort());
                            break;
                        }
                    case TCKind.TkLong:       // 3
                        {
                            output.WriteLong(ExtractLong());
                            break;
                        }
                    case TCKind.TkUshort:     // 4
                        {
                            output.WriteUShort(ExtractUShort());
                            break;
                        }
                    case TCKind.TkUlong:      // 5
                        {
                            output.WriteULong(ExtractULong());
                            break;
                        }
                    case TCKind.TkFloat:      // 6
                        {
                            output.WriteFloat(ExtractFloat());
                            break;
                        }
                    case TCKind.TkDouble:     // 7
                        {
                            output.WriteDouble(ExtractDouble());
                            break;
                        }
                    case TCKind.TkBoolean:    // 8
                        {
                            output.WriteBoolean(ExtractBoolean());
                            break;
                        }
                    case TCKind.TkChar:       // 9
                        {
                            output.WriteChar(ExtractChar());
                            break;
                        }
                    case TCKind.TkOctet:      // 10
                        {
                            output.WriteOctet(ExtractOctet());
                            break;
                        }
                    case TCKind.TkAny:        // 11
                        {
                            output.WriteAny(ExtractAny());
                            break;
                        }
                    case TCKind.TkTypeCode:   // 12
                        {
                            output.WriteTypeCode(ExtractTypeCode());
                            break;
                        }
                    case TCKind.TkPrincipal:  // 13
                        {
                            throw new NoImplement("Principal deprecated");
                        }
                    case TCKind.TkObjref:     // 14
                        {
                            output.WriteObject(ExtractObject());
                            break;
                        }
                    case TCKind.TkStruct:     // 15
                    case TCKind.TkUnion:      // 16
                    case TCKind.TkEnum:       // 17
                        {
                            WriteComplexValue(output);
                            break;
                        }
                    case TCKind.TkString:     // 18
                        {
                            output.WriteString(ExtractString());
                            break;
                        }
                    case TCKind.TkSequence:   // 19
                    case TCKind.TkArray:      // 20
                    case TCKind.TkAlias:      // 21
                    case TCKind.TkExcept:     // 22
                        {
                            WriteComplexValue(output);
                            break;
                        }
                    case TCKind.TkLonglong:   // 23
                        {
                            output.WriteLongLong(ExtractLongLong());
                            break;
                        }
                    case TCKind.TkUlonglong:  // 24
                        {
                            output.WriteULongLong(ExtractULongLong());
                            break;
                        }
                    case TCKind.TkLongdouble: // 25
                        {
                            throw new BadTypeCode("type longdouble not supported");
                        }
                    case TCKind.TkWchar:      // 26
                        {
                            output.WriteWChar(ExtractWChar());
                            break;
                        }
                    case TCKind.TkWstring:    // 27
                        {
                            output.WriteWString(ExtractWString());
                            break;
                        }
                    case TCKind.TkFixed:     // 28
                        {
                            output.WriteFixed(ExtractFixed(), typeCode.FixedDigits, typeCode.FixedScale);
                            break;
                        }
                    case TCKind.TkValue:      // 29
                    case TCKind.TkValueBox:  // 30
                        {
                            output.WriteValue(value);
                            break;
                        }
                    case TCKind.TkNative:     //31
                        {
                            throw new BadTypeCode("Cannot handle TypeCode with kind " + kind);
                        }
                    case TCKind.TkAbstractInterface:  //32
                        {
                            output.WriteAbstractInterface(value);
                            break;
                        }
                    default:
                        {
                            throw new BadTypeCode("Cannot handle TypeCode with kind " + kind);
                        }
                }
            }
        }

        private void WriteComplexValue(IOutputStream output)
        {
            if (value is IStreamable streamable)
            {
                streamable._Write(output);
            }
            else if (value is IOutputStream)
            {
                CDROutputStream outputStream = (CDROutputStream)value;
                CDRInputStream inputStream = new CDRInputStream(orb, outputStream.GetBufferCopy());
                typeCode.RemarshalValue(inputStream, output);
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type for any value: " + value.GetType());
            }
        }

        private bool CompareComplexValue(Any other)
        {
            CDROutputStream thisStream;
            if (value is CDROutputStream)
            {
                thisStream = (CDROutputStream)value;
            }
            else
            {
                thisStream = new CDROutputStream(orb);
                WriteValue(thisStream);
            }

            CDROutputStream otherStream;
            if (other is Any && other.value is CDROutputStream)
            {
                otherStream = (CDROutputStream)other.value;
            }
            else
            {
                otherStream = new CDROutputStream(orb);
                other.WriteValue(otherStream);
            }

            return thisStream.GetBufferCopy().SequenceEqual(otherStream.GetBufferCopy());
        }


        public override void InsertVoid()
        {
            typeCode = orb.GetPrimitiveTc(TCKind.TkVoid);
            value = null;
        }

        public override void Insert(TypeCode typeCode, object value)
        {
            this.typeCode = typeCode;
            this.value = value;
        }

        public override void InsertShort(short s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkShort);
        }

        public override short ExtractShort()
        {
            if (OriginalType.Kind != TCKind.TkShort)
            {
                throw new BadOperation("Cannot extract short");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is short)
            {
                return (short)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadShort();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertUShort(ushort s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkUshort);
        }

        public override ushort ExtractUShort()
        {
            if (OriginalType.Kind != TCKind.TkUshort)
            {
                throw new BadOperation("Cannot extract ushort");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is ushort)
            {
                return (ushort)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadUShort();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertLong(int s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkLong);
        }

        public override int ExtractLong()
        {
            if (OriginalType.Kind != TCKind.TkLong)
            {
                throw new BadOperation("Cannot extract long");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is int)
            {
                return (int)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadLong();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertULong(uint s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkUlong);
        }

        public override uint ExtractULong()
        {
            if (OriginalType.Kind != TCKind.TkUlong)
            {
                throw new BadOperation("Cannot extract ulong");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is uint)
            {
                return (uint)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadULong();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertLongLong(long s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkLonglong);
        }

        public override long ExtractLongLong()
        {
            if (OriginalType.Kind != TCKind.TkLonglong)
            {
                throw new BadOperation("Cannot extract longlong");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is long)
            {
                return (long)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadLongLong();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertULongLong(ulong s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkUlonglong);
        }

        public override ulong ExtractULongLong()
        {
            if (OriginalType.Kind != TCKind.TkUlonglong)
            {
                throw new BadOperation("Cannot extract ulonglong");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is ulong)
            {
                return (ulong)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadULongLong();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertFloat(float s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkFloat);
        }

        public override float ExtractFloat()
        {
            if (OriginalType.Kind != TCKind.TkFloat)
            {
                throw new BadOperation("Cannot extract float");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is float)
            {
                return (float)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadFloat();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertDouble(double s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkDouble);
        }

        public override double ExtractDouble()
        {
            if (OriginalType.Kind != TCKind.TkDouble)
            {
                throw new BadOperation("Cannot extract double");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is double)
            {
                return (double)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadDouble();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertBoolean(bool s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkBoolean);
        }

        public override bool ExtractBoolean()
        {
            if (OriginalType.Kind != TCKind.TkBoolean)
            {
                throw new BadOperation("Cannot extract boolean");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is bool)
            {
                return (bool)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadBoolean();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertChar(char s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkChar);
        }

        public override char ExtractChar()
        {
            if (OriginalType.Kind != TCKind.TkChar)
            {
                throw new BadOperation("Cannot extract char");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is char)
            {
                return (char)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadChar();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertWChar(char s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkWchar);
        }

        public override char ExtractWChar()
        {
            if (OriginalType.Kind != TCKind.TkWchar)
            {
                throw new BadOperation("Cannot extract wchar");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is char)
            {
                return (char)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadWChar();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertOctet(byte s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkOctet);
        }

        public override byte ExtractOctet()
        {
            if (OriginalType.Kind != TCKind.TkOctet)
            {
                throw new BadOperation("Cannot extract octet");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is byte)
            {
                return (byte)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadOctet();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertAny(CORBA.Any s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkAny);
        }

        public override CORBA.Any ExtractAny()
        {
            if (OriginalType.Kind != TCKind.TkAny)
            {
                throw new BadOperation("Cannot extract any");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is CORBA.Any)
            {
                return (CORBA.Any)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadAny();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertTypeCode(TypeCode s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkTypeCode);
        }

        public override TypeCode ExtractTypeCode()
        {
            if (OriginalType.Kind != TCKind.TkTypeCode)
            {
                throw new BadOperation("Cannot extract typecode");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is TypeCode)
            {
                return (TypeCode)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadTypeCode();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertString(string s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkString);
        }

        public override string ExtractString()
        {
            if (OriginalType.Kind != TCKind.TkString)
            {
                throw new BadOperation("Cannot extract string");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is string)
            {
                return (string)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadString();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertWString(string s)
        {
            value = s;
            typeCode = orb.GetPrimitiveTc(TCKind.TkWstring);
        }

        public override string ExtractWString()
        {
            if (OriginalType.Kind != TCKind.TkWstring)
            {
                throw new BadOperation("Cannot extract wstring");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            if (value is string)
            {
                return (string)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadWString();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertFixed(decimal value, TypeCode type)
        {
            try
            {
                int[] parts = decimal.GetBits(value);

                byte decimalScale = (byte)(parts[3] >> 16 & 0x7F);

                int extra = decimalScale - type.FixedScale;

                if (extra > 0)
                {
                    // truncate the value to fit the scale of the typecode
                    value = decimal.Round(value, type.FixedScale);
                }

                this.value = value;
                typeCode = type;
            }
            catch (BadKind ex)
            {
                throw new BadTypeCode();
            }
        }

        public override decimal ExtractFixed()
        {
            if (OriginalType.Kind != TCKind.TkFixed)
            {
                throw new BadOperation("Cannot extract fixed");
            }
            if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }

            if (value is decimal)
            {
                return (decimal)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadFixed(typeCode.FixedDigits, typeCode.FixedScale);
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertObject(IObject obj)
        {
            string typeId = null;
            string name = "";

            if (obj == null)
            {
                typeId = "IDL:omg.org/CORBA/Object:1.0";
                name = "Object";
            }
            else
            {
                typeId = ((CORBA.Object)obj)._Ids()[0];

                // check if the repository Id is in IDL format
                if (typeId.StartsWith("IDL:"))
                {
                    // parse the interface name from a repository Id string
                    // like "IDL:some.prefix/Some/Module/TheInterfaceName"
                    name = typeId.Substring(4, typeId.LastIndexOf(':') - 4);
                    name = name.Substring(name.LastIndexOf('/') + 1);
                }
                else if (typeId.StartsWith("RMI:"))
                {
                    // parse the interface name from a repository Id string
                    // like "RMI:some.java.package.TheInterfaceName"
                    name = typeId.Substring(4, typeId.LastIndexOf(':') - 4);
                    name = name.Substring(name.LastIndexOf('.') + 1);
                }
                else
                {
                    throw new BadParam("Unknown repository id format");
                }
            }
            typeCode = orb.CreateInterfaceTc(typeId, name);
            value = obj;
        }

        public override void InsertObject(IObject obj, TypeCode type)
        {
            if (type.Kind != TCKind.TkObjref)
            {
                throw new BadOperation("Illegal, non-object TypeCode!");
            }

            value = obj;
            typeCode = type;
        }

        public override IObject ExtractObject()
        {
            if (OriginalType.Kind != TCKind.TkObjref)
            {
                throw new BadOperation("Cannot extract object");
            }
            if (value == null)
            {
                return null;
            }
            if (value is CORBA.Object)
            {
                return (CORBA.Object)value;
            }
            else if (value is CDROutputStream)
            {
                CDRInputStream inputStream = (CDRInputStream)CreateInputStream();
                try
                {
                    return inputStream.ReadObject();
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.Message);
                }
                finally
                {
                    inputStream.Close();
                }
            }
            else
            {
                throw new CORBA.Internal("Encountered unexpected type of value: " + value.GetType());
            }
        }

        public override void InsertStreamable(IStreamable s)
        {
            var kind = s._Type.Kind;
            if (kind == TCKind.TkValue ||
                kind == TCKind.TkValueBox ||
                kind == TCKind.TkAbstractInterface ||
                kind == TCKind.TkNull)
            {
                throw new NoImplement("No support for valuetypes through streamable interface");
            }
            value = s;
            typeCode = s._Type;
        }

        public override IStreamable ExtractStreamable()
        {
            if (value is IStreamable)
            {
                return (IStreamable)value;
            }
            else if (value == null)
            {
                throw new BadOperation("No value has previously been inserted");
            }
            else
            {
                throw new BadInvOrder("Any value is not a Streamable, but a " + value.GetType());
            }
        }

        public override void InsertValue(object value)
        {
            if (value != null)
            {
                this.value = value;
                typeCode = TypeCode.CreateTypeCode(value.GetType());
            }
            else
            {
                this.value = null;
                typeCode = TypeCode.GetPrimitiveTypeCode(TCKind.TkNull);
            }
        }

        public override void InsertValue(object value, TypeCode type)
        {
            this.value = value;
            typeCode = type;
        }

        public override object ExtractValue()
        {
            var kind = typeCode.Kind;
            if (kind != TCKind.TkValue &&
                kind != TCKind.TkValueBox &&
                kind != TCKind.TkAbstractInterface &&
                kind != TCKind.TkNull)
            {
                throw new BadOperation("Cannot extract value!");
            }
            if (value == null)
            {
                //return null directly, saves cast
                return null;
            }
            return value;
        }
    }
}
