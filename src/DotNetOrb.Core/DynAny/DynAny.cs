// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DynamicAny;
using System;
using System.IO;
using static DynamicAny.DynAny;
using static DynamicAny.DynAnyFactory;

namespace DotNetOrb.Core.DynAny
{
    public class DynAny : _DynAnyLocalBase
    {
        protected CORBA.TypeCode typeCode;
        protected int pos = -1;
        protected uint limit = 0;
        protected IDynAnyFactory dynFactory;
        protected ORB orb;
        protected ILogger logger;

        //our representation of a primitive type any is the any itself
        protected CORBA.Any anyRepresentation;

        /// <summary>
        /// DynAny's internal any representation, overwritten in subclasses that represent constructed types 
        /// and need to traverse structures.
        /// </summary>
        public virtual CORBA.Any Representation { get { return anyRepresentation; } }

        internal DynAny(IDynAnyFactory factory, ORB orb, ILogger logger) : base()
        {
            this.orb = orb;
            dynFactory = factory;
            this.logger = logger;
        }

        public DynAny(IDynAnyFactory dynFactory, CORBA.TypeCode type, ORB orb, ILogger logger) : this(dynFactory, orb, logger)
        {
            typeCode = type.OriginalTypeCode;
            anyRepresentation = DefaultValue(typeCode);
        }

        protected void CheckDestroyed()
        {
            if (anyRepresentation == null && typeCode == null)
            {
                throw new ObjectNotExist();
            }
        }

        private CORBA.Any DefaultValue(CORBA.TypeCode type)
        {
            var any = orb.CreateAny();
            any.Type = type;
            switch (type.Kind)
            {
                case TCKind.TkBoolean:
                    any.InsertBoolean(false);
                    break;
                case TCKind.TkShort:
                    any.InsertShort(0);
                    break;
                case TCKind.TkUshort:
                    any.InsertUShort(0);
                    break;
                case TCKind.TkLong:
                    any.InsertLong(0);
                    break;
                case TCKind.TkDouble:
                    any.InsertDouble(0);
                    break;
                case TCKind.TkUlong:
                    any.InsertULong(0);
                    break;
                case TCKind.TkLonglong:
                    any.InsertLongLong(0);
                    break;
                case TCKind.TkUlonglong:
                    any.InsertULongLong(0);
                    break;
                case TCKind.TkFloat:
                    any.InsertFloat(0);
                    break;
                case TCKind.TkChar:
                    any.InsertChar((char)0);
                    break;
                case TCKind.TkWchar:
                    any.InsertWChar((char)0);
                    break;
                case TCKind.TkOctet:
                    any.InsertOctet(0);
                    break;
                case TCKind.TkString:
                    any.InsertString("");
                    break;
                case TCKind.TkWstring:
                    any.InsertWString("");
                    break;
                case TCKind.TkObjref:
                    any.InsertObject(null);
                    break;
                case TCKind.TkTypeCode:
                    any.InsertTypeCode(orb.GetPrimitiveTc(TCKind.TkNull));
                    break;
                case TCKind.TkAny:
                    var _any = orb.CreateAny();
                    _any.Type = orb.GetPrimitiveTc(TCKind.TkNull);
                    any.InsertAny(_any);
                    break;
                case TCKind.TkNull:
                case TCKind.TkVoid:
                    // legal TypeCodes that have no associated value
                    break;
                default:
                    throw new TypeMismatch();
            }
            return any;
        }

        public override void Assign(IDynAny dynAny)
        {
            CheckDestroyed();
            if (dynAny.Type().Equivalent(Type()))
            {
                try
                {
                    FromAny(dynAny.ToAny());
                }
                catch (InvalidValue iv)
                {
                    throw UnexpectedException(iv);
                }
            }
            else
            {
                throw new TypeMismatch();
            }
        }

        public override uint ComponentCount()
        {
            CheckDestroyed();
            return limit;
        }

        public override IDynAny Copy()
        {
            CheckDestroyed();
            try
            {
                return dynFactory.CreateDynAny(ToAny());
            }
            catch (InconsistentTypeCode tm)
            {
                throw UnexpectedException(tm);
            }
        }

        public override IDynAny CurrentComponent()
        {
            CheckDestroyed();
            throw new TypeMismatch();
        }

        public override void Destroy()
        {
            CheckDestroyed();
            anyRepresentation = null;
            typeCode = null;
        }

        public override bool Equal(IDynAny dynAny)
        {
            CheckDestroyed();

            if (Representation == null)
            {
                throw new BadInvOrder("DynAny not initialized");
            }

            return dynAny.ToAny().Equal(Representation);
        }

        public override void FromAny(CORBA.Any value)
        {
            FromAnyInternal(false, value);
        }

        internal virtual void FromAnyInternal(bool useCurrentRepresentation, CORBA.Any value)
        {
            CheckDestroyed();
            if (!value.Type.Equivalent(Type()))
            {
                throw new TypeMismatch();
            }
            typeCode = value.Type.OriginalTypeCode;

            if (useCurrentRepresentation)
            {
                anyRepresentation = value;
            }
            else
            {
                try
                {
                    anyRepresentation = orb.CreateAny();
                    anyRepresentation.ReadValue(value.CreateInputStream(), Type());
                }
                catch (Exception e)
                {
                    throw new InvalidValue(e.ToString());
                }
            }
        }

        public override CORBA.Any GetAny()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractAny();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override bool GetBoolean()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractBoolean();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override char GetChar()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractChar();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override double GetDouble()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractDouble();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override IDynAny GetDynAny()
        {
            CheckDestroyed();
            try
            {
                return dynFactory.CreateDynAny(GetAny());
            }
            catch (InconsistentTypeCode tm)
            {
                throw UnexpectedException(tm);
            }
        }

        public override float GetFloat()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractFloat();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override int GetLong()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractLong();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override long GetLonglong()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractLongLong();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override byte GetOctet()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractOctet();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override IObject GetReference()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractObject();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override short GetShort()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractShort();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        [return: WideChar(false)]
        public override string GetString()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractString();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override CORBA.TypeCode GetTypecode()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractTypeCode();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override uint GetUlong()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractULong();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override ulong GetUlonglong()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractULongLong();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override ushort GetUshort()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractUShort();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override char GetWchar()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractWChar();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        [return: WideChar(true)]
        public override string GetWstring()
        {
            CheckDestroyed();
            var any = Representation;
            try
            {
                return any.ExtractWString();
            }
            catch (BadOperation b)
            {
                throw new TypeMismatch();
            }
        }

        public override void InsertAny(CORBA.Any value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkAny)
            {
                throw new TypeMismatch();
            }
            any.InsertAny(value);
        }

        public override void InsertBoolean(bool value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkBoolean)
            {
                throw new TypeMismatch();
            }
            any.InsertBoolean(value);
        }

        public override void InsertChar(char value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkChar)
            {
                throw new TypeMismatch();
            }
            any.InsertChar(value);
        }

        public override void InsertDouble(double value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkDouble)
            {
                throw new TypeMismatch();
            }
            any.InsertDouble(value);
        }

        public override void InsertDynAny(IDynAny value)
        {
            CheckDestroyed();
            InsertAny(value.ToAny());
        }

        public override void InsertFloat(float value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkFloat)
            {
                throw new TypeMismatch();
            }
            any.InsertFloat(value);
        }

        public override void InsertLong(int value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkLong)
            {
                throw new TypeMismatch();
            }
            any.InsertLong(value);
        }

        public override void InsertLonglong(long value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkLonglong)
            {
                throw new TypeMismatch();
            }
            any.InsertLongLong(value);
        }

        public override void InsertOctet(byte value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkOctet)
            {
                throw new TypeMismatch();
            }
            any.InsertOctet(value);
        }

        public override void InsertReference(IObject value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkObjref)
            {
                throw new TypeMismatch();
            }
            any.InsertObject(value);
        }

        public override void InsertShort(short value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkShort)
            {
                throw new TypeMismatch();
            }
            any.InsertShort(value);
        }

        public override void InsertString([WideChar(false)] string value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkString)
            {
                throw new TypeMismatch();
            }
            any.InsertString(value);
        }

        public override void InsertTypecode(CORBA.TypeCode value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkTypeCode)
            {
                throw new TypeMismatch();
            }
            any.InsertTypeCode(value);
        }

        public override void InsertUlong(uint value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkUlong)
            {
                throw new TypeMismatch();
            }
            any.InsertULong(value);
        }

        public override void InsertUlonglong(ulong value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkUlonglong)
            {
                throw new TypeMismatch();
            }
            any.InsertULongLong(value);
        }

        public override void InsertUshort(ushort value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkUshort)
            {
                throw new TypeMismatch();
            }
            any.InsertUShort(value);
        }

        public override void InsertWchar(char value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkWchar)
            {
                throw new TypeMismatch();
            }
            any.InsertWChar(value);
        }

        public override void InsertWstring([WideChar(true)] string value)
        {
            CheckDestroyed();
            var any = Representation;
            if (any.Type.Kind != TCKind.TkWstring)
            {
                throw new TypeMismatch();
            }
            any.InsertWString(value);
        }

        public override bool Next()
        {
            CheckDestroyed();
            if (pos < limit - 1)
            {
                pos++;
                return true;
            }
            pos = -1;
            return false;
        }

        public override void Rewind()
        {
            CheckDestroyed();
            Seek(0);
        }

        public override bool Seek(int index)
        {
            CheckDestroyed();
            if (index < 0)
            {
                pos = -1;
                return false;
            }
            if (index < limit)
            {
                pos = index;
                return true;
            }
            pos = -1;

            return false;
        }

        public override CORBA.Any ToAny()
        {
            CheckDestroyed();
            var outAny = orb.CreateAny();
            outAny.Type = Type();
            var inputStream = Representation.CreateInputStream();
            try
            {
                outAny.ReadValue(inputStream, Type());
                return outAny;
            }
            finally
            {
                try
                {
                    inputStream.Close();
                }
                catch (IOException e)
                {
                    logger.Error("unable to close stream", e);
                }
            }
        }

        public override CORBA.TypeCode Type()
        {
            CheckDestroyed();
            return typeCode;
        }

        protected CORBA.Internal UnexpectedException(Exception cause)
        {
            logger.Debug("An unexpected error occured", cause);
            return new CORBA.Internal(cause.ToString());
        }

        public override bool[] GetBooleanSeq()
        {
            throw new NoImplement();
        }
        public override char[] GetCharSeq()
        {
            throw new NoImplement();
        }
        public override double[] GetDoubleSeq()
        {
            throw new NoImplement();
        }
        public override float[] GetFloatSeq()
        {
            throw new NoImplement();
        }
        public override decimal GetLongdouble()
        {
            throw new NoImplement();
        }
        public override decimal[] GetLongdoubleSeq()
        {
            throw new NoImplement();
        }
        public override long[] GetLonglongSeq()
        {
            throw new NoImplement();
        }
        public override int[] GetLongSeq()
        {
            throw new NoImplement();
        }
        public override byte[] GetOctetSeq()
        {
            throw new NoImplement();
        }
        public override short[] GetShortSeq()
        {
            throw new NoImplement();
        }
        public override ulong[] GetUlonglongSeq()
        {
            throw new NoImplement();
        }
        public override uint[] GetUlongSeq()
        {
            throw new NoImplement();
        }
        public override ushort[] GetUshortSeq()
        {
            throw new NoImplement();
        }
        public override object GetVal()
        {
            throw new NoImplement();
        }
        public override char[] GetWcharSeq()
        {
            throw new NoImplement();
        }

        public override void InsertBooleanSeq(bool[] value)
        {
            throw new NoImplement();
        }
        public override void InsertCharSeq(char[] value)
        {
            throw new NoImplement();
        }
        public override void InsertDoubleSeq(double[] value)
        {
            throw new NoImplement();
        }
        public override void InsertLongdouble(decimal value)
        {
            throw new NoImplement();
        }
        public override void InsertFloatSeq(float[] value)
        {
            throw new NoImplement();
        }
        public override void InsertLongdoubleSeq(decimal[] value)
        {
            throw new NoImplement();
        }
        public override void InsertLonglongSeq(long[] value)
        {
            throw new NoImplement();
        }
        public override void InsertLongSeq(int[] value)
        {
            throw new NoImplement();
        }
        public override void InsertOctetSeq(byte[] value)
        {
            throw new NoImplement();
        }
        public override void InsertShortSeq(short[] value)
        {
            throw new NoImplement();
        }
        public override void InsertUlonglongSeq(ulong[] value)
        {
            throw new NoImplement();
        }
        public override void InsertUlongSeq(uint[] value)
        {
            throw new NoImplement();
        }
        public override void InsertUshortSeq(ushort[] value)
        {
            throw new NoImplement();
        }
        public override void InsertVal(object value)
        {
            throw new NoImplement();
        }
        public override void InsertWcharSeq(char[] value)
        {
            throw new NoImplement();
        }
    }
}
