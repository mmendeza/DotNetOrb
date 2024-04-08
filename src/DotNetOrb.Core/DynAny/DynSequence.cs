// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DynamicAny;
using System.Collections.Generic;
using static DynamicAny.DynAny;
using static DynamicAny.DynAnyFactory;

namespace DotNetOrb.Core.DynAny
{
    public class DynSequence : DynAny, IDynSequence
    {
        private List<CORBA.Any> members = new List<CORBA.Any>();
        private uint length;
        private CORBA.TypeCode elementType;

        public override CORBA.Any Representation
        {
            get
            {
                return members[pos];
            }
        }
        public DynSequence(IDynAnyFactory dynFactory, CORBA.TypeCode type, ORB orb, ILogger logger) : base(dynFactory, orb, logger)
        {
            var _type = type.OriginalTypeCode;
            if (_type.Kind != TCKind.TkSequence)
            {
                throw new TypeMismatch();
            }
            try
            {
                typeCode = _type;
                elementType = Type().ContentType.OriginalTypeCode;
                limit = (uint)typeCode.Length;
                length = 0;
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }

            if (elementType == null)
            {
                throw new CORBA.Internal("DynSequence.set_length, elementType null");
            }
        }

        internal override void FromAnyInternal(bool useCurrentRepresentation, CORBA.Any value)
        {
            CheckDestroyed();
            if (!Type().Equivalent(value.Type))
            {
                throw new TypeMismatch();
            }
            try
            {
                base.FromAnyInternal(useCurrentRepresentation, value);

                limit = (uint)Type().Length;

                var inputStream = value.CreateInputStream();
                length = inputStream.ReadULong();

                if (length > 0)
                {
                    pos = 0;
                }

                if (limit != 0 && length > limit)
                {
                    throw new InvalidValue();
                }

                members.Clear();
                elementType = Type().ContentType.OriginalTypeCode;

                for (int i = 0; i < length; i++)
                {
                    var any = orb.CreateAny();
                    any.ReadValue(inputStream, elementType);
                    members.Add(any);
                }
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
            if (elementType == null)
            {
                throw new CORBA.Internal("DynSequence.set_length, elementType null");
            }
        }

        public override CORBA.Any ToAny()
        {
            CheckDestroyed();
            var outAny = orb.CreateAny();
            outAny.Type = Type();

            var outputStream = new CDROutputStream(orb);
            try
            {
                outputStream.WriteULong(length);

                for (int i = 0; i < length; i++)
                {
                    elementType.RemarshalValue(members[i].CreateInputStream(), outputStream);
                }

                var inputStream = new CDRInputStream(orb, outputStream.GetBufferCopy());
                try
                {
                    outAny.ReadValue(inputStream, Type());
                    return outAny;
                }
                finally
                {
                    inputStream.Close();
                }
            }
            finally
            {
                outputStream.Close();
            }
        }

        public override bool Equal(IDynAny dynAny)
        {
            CheckDestroyed();
            if (!Type().Equal(dynAny.Type()))
            {
                return false;
            }
            var other = DynSequenceHelper.Narrow(dynAny);
            if (other.GetLength() != GetLength())
            {
                return false;
            }
            CORBA.Any[] elements = GetElements();
            CORBA.Any[] other_elements = other.GetElements();
            for (int i = 0; i < elements.Length; i++)
            {
                if (!elements[i].Equal(other_elements[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public CORBA.Any[] GetElements()
        {
            CheckDestroyed();
            Any[] result = new Any[members.Count];
            for (int i = members.Count; i-- > 0;)
            {
                result[i] = (Any)members[i];
            }
            return result;
        }

        public IDynAny[] GetElementsAsDynAny()
        {
            CheckDestroyed();
            IDynAny[] result = new IDynAny[members.Count];
            try
            {
                for (int i = members.Count; i-- > 0;)
                {
                    result[i] = dynFactory.CreateDynAny(members[i]);
                }
                return result;
            }
            catch (InconsistentTypeCode e)
            {
                throw UnexpectedException(e);
            }
        }

        public uint GetLength()
        {
            CheckDestroyed();
            return length;
        }

        public void SetElements(CORBA.Any[] value)
        {
            CheckDestroyed();
            if (limit > 0 && value.Length > limit)
            {
                throw new InvalidValue();
            }

            for (int i = value.Length; i-- > 0;)
            {
                CORBA.TypeCode tc = value[i].Type.OriginalTypeCode;

                if (tc.Kind != elementType.Kind)
                {
                    throw new TypeMismatch();
                }
            }
            length = (uint)value.Length;
            members.Clear();
            for (int i = 0; i < length; i++)
            {
                members.Add(value[i]);
            }
            if (length > 0)
            {
                pos = 0;
            }
            else
            {
                pos = -1;
            }
        }

        public void SetElementsAsDynAny(IDynAny[] value)
        {
            CheckDestroyed();
            CORBA.Any[] anySeq = new CORBA.Any[value.Length];
            for (int i = value.Length; i-- > 0;)
            {
                anySeq[i] = value[i].ToAny();
            }

            SetElements(anySeq);
        }

        public void SetLength(uint len)
        {
            CheckDestroyed();
            if (limit > 0 && len > limit)
            {
                throw new InvalidValue();
            }
            if (elementType == null)
            {
                throw new CORBA.Internal("DynSequence.set_length, elementType null");
            }
            if (len == 0)
            {
                members.Clear();
                pos = -1;
            }
            else if (len > length)
            {
                try
                {
                    for (uint i = length; i < len; i++)
                    {
                        // create correctly initialized anys
                        members.Add(dynFactory.CreateDynAnyFromTypeCode(elementType).ToAny());
                    }
                }
                catch (InconsistentTypeCode e)
                {
                    throw UnexpectedException(e);
                }

                if (pos == -1)
                {
                    pos = (int)(len - length - 1);
                }
            }
            else if (len < length)
            {
                uint toremove = length - len;
                for (uint x = 0; x < toremove; ++x)
                {
                    uint index = length - 1 - x;
                    members.RemoveAt((int)index);
                }
                if (pos > len)
                {
                    pos = -1;
                }
            }
            length = len;
        }

        public override void Destroy()
        {
            base.Destroy();
            members.Clear();
            elementType = null;
        }

        public override bool Next()
        {
            CheckDestroyed();
            if (pos < length - 1)
            {
                pos++;
                return true;
            }
            pos = -1;
            return false;
        }

        public override bool Seek(int index)
        {
            CheckDestroyed();
            if (index < 0)
            {
                pos = -1;
                return false;
            }
            if (index < length)
            {
                pos = index;
                return true;
            }
            pos = -1;
            return false;
        }

        public override IDynAny CurrentComponent()
        {
            CheckDestroyed();
            if (pos == -1)
            {
                return null;
            }
            try
            {
                var result = (DynAny)dynFactory.CreateDynAnyFromTypeCode(members[pos].Type);
                result.FromAnyInternal(true, members[pos]);
                return result;
            }
            catch (InvalidValue iv)
            {
                logger.Error("unable to create DynAny", iv);
                throw UnexpectedException(iv);
            }
            catch (TypeMismatch itc)
            {
                logger.Error("unable to create DynAny", itc);
                throw UnexpectedException(itc);
            }
            catch (InconsistentTypeCode e)
            {
                logger.Error("unable to create DynAny", e);
                throw UnexpectedException(e);
            }
        }

        public override uint ComponentCount()
        {
            CheckDestroyed();
            return GetLength();
        }
    }
}
