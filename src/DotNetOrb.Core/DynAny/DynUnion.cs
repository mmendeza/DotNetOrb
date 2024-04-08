// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DynamicAny;
using System;
using static DynamicAny.DynAny;
using static DynamicAny.DynAnyFactory;

namespace DotNetOrb.Core.DynAny
{
    public class DynUnion : DynAny, IDynUnion
    {

        private CORBA.Any discriminator;
        private IDynAny member;
        private string memberName;
        private int memberIndex;

        public DynUnion(IDynAnyFactory dynFactory, CORBA.TypeCode tc, ORB orb, ILogger logger) : base(dynFactory, orb, logger)
        {
            CORBA.TypeCode _type = tc.OriginalTypeCode;
            if (_type.Kind != TCKind.TkUnion)
            {
                throw new TypeMismatch();
            }
            typeCode = _type;
            pos = 0;
            limit = 2;

            try
            {
                for (int i = 0; i < typeCode.MemberCount; i++)
                {
                    discriminator = typeCode.MemberLabel(i);

                    if (discriminator.Type.Kind != TCKind.TkOctet)
                    {
                        break;
                    }
                }

                // rare case when the union only has a default case label
                if (discriminator.Type.Kind == TCKind.TkOctet)
                {
                    try
                    {
                        SetToUnlistedLabel();
                    }
                    catch (TypeMismatch e)
                    {
                        throw UnexpectedException(e);
                    }
                }
                SelectMember();
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

        public override void FromAny(CORBA.Any value)
        {
            CheckDestroyed();
            if (!Type().Equivalent(value.Type))
            {
                throw new TypeMismatch();
            }
            try
            {
                typeCode = value.Type.OriginalTypeCode;
                base.FromAny(value);
                limit = 2;
                var inputStream = value.CreateInputStream();
                discriminator = orb.CreateAny();
                discriminator.Type = Type().DiscriminatorType;
                discriminator.ReadValue(inputStream, Type().DiscriminatorType);

                CORBA.Any memberAny = null;
                for (int i = 0; i < Type().MemberCount; i++)
                {
                    if (Type().MemberLabel(i).Equal(discriminator))
                    {
                        memberAny = orb.CreateAny();
                        memberAny.ReadValue(inputStream, Type().MemberType(i));
                        memberName = Type().MemberName(i);
                        memberIndex = i;
                        break;
                    }
                }
                if (memberAny == null)
                {
                    int defIdx = Type().DefaultIndex;
                    if (defIdx != -1)
                    {
                        memberAny = orb.CreateAny();
                        memberAny.ReadValue(inputStream, Type().MemberType(defIdx));
                        memberName = Type().MemberName(defIdx);
                        memberIndex = defIdx;
                    }
                }
                if (memberAny != null)
                {
                    try
                    {
                        member = dynFactory.CreateDynAny(memberAny);
                    }
                    catch (InconsistentTypeCode e)
                    {
                        throw UnexpectedException(e);
                    }
                }
            }
            catch (Bounds e)
            {
                throw UnexpectedException(e);
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
        }

        public override CORBA.Any ToAny()
        {
            CheckDestroyed();
            var outputStream = new CDROutputStream(orb);

            try
            {
                discriminator.Type.RemarshalValue(discriminator.CreateInputStream(), outputStream);
                if (member != null)
                {
                    member.Type().RemarshalValue(member.ToAny().CreateInputStream(), outputStream);
                }

                var outAny = orb.CreateAny();

                outAny.Type = Type();
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

        public override uint ComponentCount()
        {
            if (HasNoActiveMember())
            {
                return 1;
            }
            return limit;
        }

        public override bool Next()
        {
            CheckDestroyed();
            if (pos < ComponentCount() - 1)
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
            if (index < ComponentCount())
            {
                pos = index;
                return true;
            }
            pos = -1;
            return false;
        }

        public override bool Equal(IDynAny dynAny)
        {
            CheckDestroyed();
            if (!Type().Equal(dynAny.Type()))
            {
                return false;
            }

            var other = DynUnionHelper.Narrow(dynAny);

            if (!GetDiscriminator().Equal(other.GetDiscriminator()))
            {
                return false;
            }

            if (!HasNoActiveMember())
            {
                // other must have the same here because we know the
                // discriminators are equal
                try
                {
                    if (!member.Equal(other.Member()))
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    throw UnexpectedException(e);
                }
            }
            return true;
        }


        public TCKind DiscriminatorKind()
        {
            CheckDestroyed();
            return discriminator.Type.Kind;
        }

        public IDynAny GetDiscriminator()
        {
            CheckDestroyed();
            try
            {
                return dynFactory.CreateDynAny(discriminator);
            }
            catch (InconsistentTypeCode e)
            {
                throw UnexpectedException(e);
            }
        }

        public bool HasNoActiveMember()
        {
            CheckDestroyed();
            try
            {
                if (Type().DefaultIndex != -1)
                {
                    return false;
                }

                for (int i = 0; i < typeCode.MemberCount; i++)
                {
                    if (discriminator.Equal(typeCode.MemberLabel(i)))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Bounds e)
            {
                throw UnexpectedException(e);
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
        }

        public IDynAny Member()
        {
            CheckDestroyed();
            if (HasNoActiveMember())
            {
                throw new InvalidValue();
            }

            return member;
        }

        public TCKind MemberKind()
        {
            CheckDestroyed();
            if (HasNoActiveMember())
            {
                throw new InvalidValue();
            }

            return member.Type().Kind;
        }

        [return: WideChar(false)]
        public string MemberName()
        {
            CheckDestroyed();
            if (HasNoActiveMember())
            {
                throw new InvalidValue();
            }
            return memberName;
        }

        public void SetDiscriminator(IDynAny dynAny)
        {
            CheckDestroyed();
            if (!dynAny.Type().Equivalent(discriminator.Type))
            {
                throw new TypeMismatch();
            }
            discriminator = dynAny.ToAny();
            pos = 1;

            //check if the new discriminator is consistent with the currently active member. If not, select a new one
            try
            {
                if (memberIndex == typeCode.DefaultIndex)
                {
                    // default member, only change the member if a non-default discriminator value is specified
                    for (int i = 0; i < typeCode.MemberCount; i++)
                    {
                        if (Type().MemberLabel(i).Equal(discriminator))
                        {
                            SelectMember();
                            break;
                        }
                    }
                }
                else
                {
                    // non-default member, check if the member has changed
                    if (!Type().MemberLabel(memberIndex).Equal(discriminator))
                    {
                        SelectMember();
                    }
                }
            }
            catch (Bounds e)
            {
                throw UnexpectedException(e);
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
            // set the current position to zero if there is no active member
            if (HasNoActiveMember())
            {
                pos = 0;
            }
        }

        /// <summary>
        /// Updates the private instance variables member, memberName and memberIndex according to the current discriminator value
        /// </summary>
        private void SelectMember()
        {
            member = null;
            try
            {
                // search through all members and compare their label with the discriminator
                for (int i = 0; i < Type().MemberCount; i++)
                {
                    if (Type().MemberLabel(i).Equal(discriminator))
                    {
                        try
                        {
                            member = dynFactory.CreateDynAnyFromTypeCode(Type().MemberType(i));
                        }
                        catch (InconsistentTypeCode e)
                        {
                            throw UnexpectedException(e);
                        }
                        memberName = Type().MemberName(i);
                        memberIndex = i;
                        break;
                    }
                }

                // none found, use default, if there is one                   
                if (member == null)
                {
                    int defIdx = Type().DefaultIndex;
                    if (defIdx != -1)
                    {
                        try
                        {
                            member = dynFactory.CreateDynAnyFromTypeCode(Type().MemberType(defIdx));
                        }
                        catch (InconsistentTypeCode e)
                        {
                            throw UnexpectedException(e);
                        }
                        memberName = Type().MemberName(defIdx);
                        memberIndex = defIdx;
                    }
                }
            }
            catch (Bounds e)
            {
                throw UnexpectedException(e);
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
        }

        /// <summary>
        /// Sets the discriminator to  a value that is consistent with the value of the default case of a union;
        /// it sets  the current position to zero and causes componentCount to return  2.
        /// </summary>
        /// <exception cref="TypeMismatch">if the union  does not have an explicit default case.</exception>
        public void SetToDefaultMember()
        {
            CheckDestroyed();
            try
            {
                int defIdx = Type().DefaultIndex;
                if (defIdx == -1)
                {
                    throw new TypeMismatch();
                }
                pos = 0;

                // do nothing if the discriminator is already set to a default value
                if (memberIndex != defIdx)
                {
                    try
                    {
                        SetToUnlistedLabel();
                    }
                    catch (TypeMismatch e)
                    {
                        throw UnexpectedException(e);
                    }
                    SelectMember();
                }
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
        }

        /// <summary>
        /// Sets the discriminator to a value that does not correspond to any of the union's case labels;
        /// it sets the current position to zero and causes componentCount to return 1.
        /// </summary>
        /// <exception cref="TypeMismatch">if the union  has an explicit default case or uses the entire 
        /// range of discriminator values for explicit case labels.</exception>
        public void SetToNoActiveMember()
        {
            CheckDestroyed();
            try
            {
                // if there is a default index, we do have active members
                if (Type().DefaultIndex != -1)
                {
                    throw new TypeMismatch();
                }
                pos = 0;

                // do nothing if discriminator is already set to no active member
                if (!HasNoActiveMember())
                {
                    SetToUnlistedLabel();
                }
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
        }

        private void SetToUnlistedLabel()
        {
            try
            {
                switch (Type().DiscriminatorType.Kind)
                {
                    case TCKind.TkBoolean:
                        {
                            bool found;
                            bool cur_bool;
                            CORBA.Any check_val = null;

                            for (int i = 0; i < 2; i++)
                            {
                                if (i == 0)
                                {
                                    cur_bool = true;
                                }
                                else
                                {
                                    cur_bool = false;
                                }
                                check_val = orb.CreateAny();
                                check_val.InsertBoolean(cur_bool);

                                found = false; // is the value used as a label?
                                for (int j = 0; j < Type().MemberCount && !found; j++)
                                {
                                    if (check_val.Equal(Type().MemberLabel(j)))
                                    {
                                        found = true;
                                    }
                                }

                                if (!found)
                                {
                                    // the value is not found among the union's label
                                    discriminator = check_val;
                                    return;
                                }
                            }
                            // no unused value found
                            throw new TypeMismatch();
                        }
                    case TCKind.TkChar:
                        {
                            // assume there is a printable char not used as a label!
                            bool found;
                            CORBA.Any check_val = null;

                            // 33 to 126 defines a reasonable set of printable chars
                            for (int i = 0; i < 127; i++)
                            {
                                check_val = orb.CreateAny();
                                check_val.InsertChar((char)i);

                                found = false; // is the value used as a label?
                                for (int j = 0; j < Type().MemberCount && !found; j++)
                                {
                                    if (check_val.Equal(Type().MemberLabel(j)))
                                    {
                                        found = true;
                                    }
                                }

                                if (!found)
                                {
                                    // the value is not found among the union's label
                                    discriminator = check_val;
                                    return;
                                }
                            }
                            // no unused value found, should not happen
                            throw new TypeMismatch();
                        }
                    case TCKind.TkShort:
                        {
                            // assume there is an unsigned short not used as a label!
                            bool found;
                            CORBA.Any check_val = null;

                            short max_short = 32767;
                            for (short i = 0; i < max_short; i++)
                            {
                                check_val = orb.CreateAny();
                                check_val.InsertShort(i);

                                found = false; // is the value used as a label?
                                for (int j = 0; j < Type().MemberCount && !found; j++)
                                {
                                    if (check_val.Equal(Type().MemberLabel(j)))
                                    {
                                        found = true;
                                    }
                                }

                                if (!found)
                                {
                                    // the value is not found among the union's label
                                    discriminator = check_val;
                                    return;
                                }
                            }
                            // no unused value found, should not happen
                            throw new TypeMismatch();
                        }
                    case TCKind.TkLong:
                        {
                            // assume there is an unsigned int not used as a label!
                            bool found;
                            CORBA.Any check_val = null;

                            int max_int = 2147483647;
                            for (int i = 0; i < max_int; i++)
                            {
                                check_val = orb.CreateAny();
                                check_val.InsertLong(i);

                                found = false; // is the value used as a label?
                                for (int j = 0; j < Type().MemberCount && !found; j++)
                                {
                                    if (check_val.Equal(Type().MemberLabel(j)))
                                    {
                                        found = true;
                                    }
                                }

                                if (!found)
                                {
                                    // the value is not found among the union's label
                                    discriminator = check_val;
                                    return;
                                }
                            }
                            // no unused value found, should not happen
                            throw new TypeMismatch();
                        }
                    case TCKind.TkLonglong:
                        {
                            // assume there is an unsigned long not used as a label!
                            bool found;
                            CORBA.Any check_val = null;

                            long max_long = 2147483647; // this should be sufficient!
                            for (long i = 0; i < max_long; i++)
                            {
                                check_val = orb.CreateAny();
                                check_val.InsertLongLong(i);

                                found = false; // is the value used as a label?
                                for (int j = 0; j < Type().MemberCount && !found; j++)
                                {
                                    if (check_val.Equal(Type().MemberLabel(j)))
                                    {
                                        found = true;
                                    }
                                }

                                if (!found)
                                {
                                    // the value is not found among the union's label
                                    discriminator = check_val;
                                    return;
                                }
                            }
                            // no unused value found, should not happen
                            throw new TypeMismatch();
                        }
                    case TCKind.TkEnum:
                        {
                            IDynEnum dynEnum = null;
                            try
                            {
                                dynEnum = (IDynEnum)dynFactory.CreateDynAnyFromTypeCode(discriminator.Type);
                            }
                            catch (InconsistentTypeCode e)
                            {
                                throw UnexpectedException(e);
                            }

                            bool found;
                            for (int i = 0; i < discriminator.Type.MemberCount; i++)
                            {
                                try
                                {
                                    dynEnum.SetAsString(discriminator.Type.MemberName(i));
                                }
                                catch (InvalidValue e)
                                {
                                    throw UnexpectedException(e);
                                }

                                found = false; // is the value used as a label?
                                for (int j = 0; j < Type().MemberCount && !found; j++)
                                {
                                    if (dynEnum.ToAny().Equal(Type().MemberLabel(j)))
                                    {
                                        found = true;
                                    }
                                }

                                if (!found)
                                {
                                    // the enum value is not found among the union's label
                                    discriminator = dynEnum.ToAny();
                                    return;
                                }
                            }
                            // no unused value found
                            throw new TypeMismatch();
                        }
                    default:
                        throw new TypeMismatch();
                }
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

        public override void Destroy()
        {
            base.Destroy();
            discriminator = null;
            member = null;
            memberName = null;
            memberIndex = -1;
        }

        public override IDynAny CurrentComponent()
        {
            CheckDestroyed();
            if (pos == -1)
            {
                return null;
            }

            if (pos == 0)
            {
                return GetDiscriminator();
            }

            try
            {
                return Member();
            }
            catch (InvalidValue e)
            {
                throw UnexpectedException(e);
            }
        }
    }
}
