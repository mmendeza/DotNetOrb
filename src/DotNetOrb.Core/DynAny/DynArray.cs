// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DynamicAny;
using static DynamicAny.DynAny;
using static DynamicAny.DynAnyFactory;

namespace DotNetOrb.Core.DynAny
{
    public class DynArray : DynAny, IDynArray
    {
        private CORBA.TypeCode elementType;
        private CORBA.Any[] members;

        public override CORBA.Any Representation
        {
            get
            {
                return members[pos];
            }
        }

        public DynArray(IDynAnyFactory dynFactory, CORBA.TypeCode type, ORB orb, ILogger logger) : base(dynFactory, orb, logger)
        {
            var _type = type.OriginalTypeCode;

            if (_type.Kind != TCKind.TkArray)
            {
                throw new TypeMismatch();
            }

            try
            {
                typeCode = _type;
                elementType = typeCode.ContentType.OriginalTypeCode;
                limit = (uint)typeCode.Length;
                members = new Any[limit];
                try
                {
                    for (uint i = limit; i-- > 0;)
                    {
                        members[i] = dynFactory.CreateDynAnyFromTypeCode(elementType).ToAny();
                    }
                }
                catch (InconsistentTypeCode e)
                {
                    throw UnexpectedException(e);
                }
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
        }

        internal override void FromAnyInternal(bool useCurrentRepresentation, CORBA.Any value)
        {
            CheckDestroyed();
            if (!typeCode.Equivalent(value.Type))
            {
                throw new TypeMismatch();
            }
            base.FromAnyInternal(useCurrentRepresentation, value);
            try
            {
                limit = (uint)Type().Length;
                elementType = typeCode.ContentType.OriginalTypeCode;

                if (limit > 0)
                {
                    pos = 0;
                }

                var inputStream = value.CreateInputStream();
                members = new CORBA.Any[limit];

                for (int i = 0; i < limit; i++)
                {
                    members[i] = orb.CreateAny();
                    members[i].ReadValue(inputStream, elementType);
                }
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
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
                for (int i = 0; i < limit; i++)
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
            var other = DynArrayHelper.Narrow(dynAny);
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
            return members;
        }

        public IDynAny[] GetElementsAsDynAny()
        {
            CheckDestroyed();
            var result = new IDynAny[members.Length];
            try
            {
                for (int i = members.Length; i-- > 0;)
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

        public void SetElements(CORBA.Any[] value)
        {
            CheckDestroyed();
            if (value.Length != limit)
            {
                throw new InvalidValue();
            }
            for (int i = value.Length; i-- > 0;)
            {
                var tc = value[i].Type.OriginalTypeCode;
                if (tc.Kind != elementType.Kind)
                {
                    throw new TypeMismatch();
                }
            }
            members = value;
        }

        public void SetElementsAsDynAny(IDynAny[] value)
        {
            CheckDestroyed();
            var anySeq = new CORBA.Any[value.Length];
            for (int i = value.Length; i-- > 0;)
            {
                anySeq[i] = value[i].ToAny();
            }

            SetElements(anySeq);
        }

        public override void Destroy()
        {
            base.Destroy();
            members = null;
            elementType = null;
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
                var result = dynFactory.CreateDynAnyFromTypeCode(members[pos].Type);
                ((DynAny)result).FromAnyInternal(true, members[pos]);
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
    }
}
