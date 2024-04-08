// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DynamicAny;
using static DynamicAny.DynAny;

namespace DotNetOrb.Core.DynAny
{
    public class DynEnum : DynAny, IDynEnum
    {
        private uint enumValue;
        private int max;
        private string[] memberNames;

        public DynEnum(IDynAnyFactory dynFactory, CORBA.TypeCode type, ORB orb, ILogger logger) : base(dynFactory, orb, logger)
        {
            var _type = type.OriginalTypeCode;

            if (_type.Kind != TCKind.TkEnum)
            {
                throw new TypeMismatch();
            }

            typeCode = _type;
            pos = -1;
            enumValue = 0;

            try
            {
                memberNames = new string[Type().MemberCount];
                max = memberNames.Length;
                for (int i = 0; i < memberNames.Length; i++)
                {
                    memberNames[i] = type.MemberName(i);
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

        public override void FromAny(CORBA.Any value)
        {
            CheckDestroyed();
            if (!value.Type.Equivalent(Type()))
            {
                throw new TypeMismatch();
            }
            typeCode = value.Type;
            try
            {
                enumValue = value.CreateInputStream().ReadULong();
                memberNames = new string[Type().MemberCount];
                max = memberNames.Length;
                for (int i = 0; i < memberNames.Length; i++)
                {
                    memberNames[i] = Type().MemberName(i);
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
        public override bool Equal(IDynAny dynAny)
        {
            CheckDestroyed();
            if (!Type().Equal(dynAny.Type()))
            {
                return false;
            }
            return DynEnumHelper.Narrow(dynAny).GetAsUlong() == GetAsUlong();
        }

        public override CORBA.Any ToAny()
        {
            CheckDestroyed();
            var outputStream = new CDROutputStream(orb);
            try
            {
                outputStream.WriteULong(enumValue);
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

        [return: WideChar(false)]
        public string GetAsString()
        {
            CheckDestroyed();
            return memberNames[enumValue];
        }

        public uint GetAsUlong()
        {
            CheckDestroyed();
            return enumValue;
        }

        public void SetAsString([WideChar(false)] string value)
        {
            CheckDestroyed();
            uint i = 0;
            while (i < memberNames.Length && !value.Equals(memberNames[i]))
            {
                i++;
            }
            if (i < memberNames.Length)
            {
                SetAsUlong(i);
            }
            else
            {
                throw new InvalidValue();
            }
        }

        public void SetAsUlong(uint value)
        {
            CheckDestroyed();
            if (value < 0 || value > max)
            {
                throw new InvalidValue();
            }
            enumValue = value;
        }
    }
}
