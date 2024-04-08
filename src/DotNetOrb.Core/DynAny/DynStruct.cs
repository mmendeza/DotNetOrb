// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DynamicAny;
using static DynamicAny.DynAny;
using static DynamicAny.DynAnyFactory;

namespace DotNetOrb.Core.DynAny
{
    public class DynStruct : DynAny, IDynStruct
    {
        private NameValuePair[] members;

        // only set if this represents an exception
        private string exceptionMsg;

        public override CORBA.Any Representation
        {
            get
            {
                return members[pos].Value;
            }
        }
        public DynStruct(IDynAnyFactory dynFactory, CORBA.TypeCode type, ORB orb, ILogger logger) : base(dynFactory, orb, logger)
        {
            var _type = type.OriginalTypeCode;
            if (_type.Kind != TCKind.TkExcept && _type.Kind != TCKind.TkStruct)
            {
                throw new TypeMismatch();
            }
            typeCode = _type;
            try
            {
                /* initialize position for all except empty exceptions */
                if (!IsEmptyEx())
                {
                    pos = 0;
                }
                if (_type.Kind == TCKind.TkExcept)
                {
                    exceptionMsg = typeCode.Id;
                }

                limit = (uint)typeCode.MemberCount;
                members = new NameValuePair[limit];
                for (int i = 0; i < limit; i++)
                {
                    var _tc = typeCode.MemberType(i).OriginalTypeCode;
                    members[i] = new NameValuePair(typeCode.MemberName(i), dynFactory.CreateDynAnyFromTypeCode(_tc).ToAny());

                }
            }
            catch (InconsistentTypeCode e)
            {
                throw UnexpectedException(e);
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
            catch (Bounds e)
            {
                throw UnexpectedException(e);
            }
        }

        internal override void FromAnyInternal(bool useCurrentRepresentation, CORBA.Any value)
        {
            CheckDestroyed();

            if (!value.Type.Equivalent(Type()))
            {
                throw new TypeMismatch();
            }

            try
            {
                limit = (uint)Type().MemberCount;
                members = new NameValuePair[limit];
                var inputStream = value.CreateInputStream();

                if (typeCode.Kind == TCKind.TkExcept)
                {
                    exceptionMsg = inputStream.ReadString();
                }

                for (int i = 0; i < limit; i++)
                {
                    try
                    {
                        var any = orb.CreateAny();
                        any.ReadValue(inputStream, typeCode.MemberType(i).OriginalTypeCode);

                        members[i] = new NameValuePair(Type().MemberName(i), any);
                    }
                    catch (Bounds e)
                    {
                        throw UnexpectedException(e);
                    }
                }
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
            base.FromAnyInternal(useCurrentRepresentation, value);
        }

        public override CORBA.Any ToAny()
        {
            CheckDestroyed();
            var outAny = orb.CreateAny();
            outAny.Type = Type();
            var outputStream = new CDROutputStream(orb);
            try
            {
                if (Type().Kind == TCKind.TkExcept)
                {
                    outputStream.WriteString(exceptionMsg);
                }

                for (int i = 0; i < members.Length; i++)
                {
                    members[i].Value.Type.RemarshalValue(members[i].Value.CreateInputStream(), outputStream);
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

            var other = DynStructHelper.Narrow(dynAny);

            NameValuePair[] elements = GetMembers();
            NameValuePair[] other_elements = other.GetMembers();

            for (int i = 0; i < elements.Length; i++)
            {
                if (!elements[i].Value.Equal(other_elements[i].Value))
                {
                    return false;
                }
            }
            return true;
        }

        public TCKind CurrentMemberKind()
        {
            CheckDestroyed();

            if (IsEmptyEx())
            {
                throw new TypeMismatch();
            }
            if (pos == -1)
            {
                throw new InvalidValue();
            }
            return members[pos].Value.Type.Kind;
        }

        [return: WideChar(false)]
        public string CurrentMemberName()
        {
            CheckDestroyed();

            if (IsEmptyEx())
            {
                throw new TypeMismatch();
            }
            if (pos == -1)
            {
                throw new InvalidValue();
            }
            return members[pos].Id;
        }

        public NameValuePair[] GetMembers()
        {
            CheckDestroyed();
            return members;
        }

        public NameDynAnyPair[] GetMembersAsDynAny()
        {
            CheckDestroyed();
            NameDynAnyPair[] result = new NameDynAnyPair[limit];
            try
            {
                for (int i = 0; i < limit; i++)
                {
                    result[i] = new NameDynAnyPair(members[i].Id, dynFactory.CreateDynAny(members[i].Value));
                }

                return result;
            }
            catch (InconsistentTypeCode e)
            {
                throw UnexpectedException(e);
            }
        }

        public void SetMembers(NameValuePair[] nvp)
        {
            CheckDestroyed();
            if (nvp.Length != limit)
            {
                throw new InvalidValue();
            }
            for (int i = 0; i < limit; i++)
            {
                if (!nvp[i].Value.Type.Equivalent(members[i].Value.Type))
                {
                    throw new TypeMismatch();
                }
                if (!(nvp[i].Id.Equals("") || nvp[i].Id.Equals(members[i].Id)))
                {
                    throw new TypeMismatch();
                }
            }
            members = nvp;
        }

        public void SetMembersAsDynAny(NameDynAnyPair[] nvp)
        {
            CheckDestroyed();
            if (nvp.Length != limit)
            {
                throw new InvalidValue();
            }

            for (int i = 0; i < limit; i++)
            {
                if (!nvp[i].Value.Type().Equivalent(members[i].Value.Type))
                {
                    throw new TypeMismatch();
                }

                if (!(nvp[i].Id.Equals("") || nvp[i].Id.Equals(members[i].Id)))
                {
                    throw new TypeMismatch();
                }

            }
            members = new NameValuePair[nvp.Length];
            for (int i = 0; i < limit; i++)
            {
                members[i] = new NameValuePair(nvp[i].Id, nvp[i].Value.ToAny());
            }
        }

        public override void Destroy()
        {
            base.Destroy();
            members = null;
        }

        public override IDynAny CurrentComponent()
        {
            CheckDestroyed();
            try
            {
                /*  special case for empty exceptions */
                if (IsEmptyEx())
                {
                    throw new TypeMismatch();
                }

                if (pos == -1)
                {
                    return null;
                }

                var result = dynFactory.CreateDynAnyFromTypeCode(members[pos].Value.Type);
                try
                {
                    ((DynAny)result).FromAnyInternal(true, members[pos].Value);
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
                return result;
            }
            catch (InconsistentTypeCode e)
            {
                logger.Error("unable to create DynAny", e);
                throw UnexpectedException(e);
            }
        }

        private bool IsEmptyEx()
        {
            try
            {
                return typeCode.Kind == TCKind.TkExcept && typeCode.MemberCount == 0;
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
        }
    }
}
