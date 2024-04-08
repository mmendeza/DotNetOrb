// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DynamicAny;
using System;
using System.Text;
using static DynamicAny.DynAny;

namespace DotNetOrb.Core.DynAny
{
    public class DynFixed : DynAny, IDynFixed
    {
        public DynFixed(DynAnyFactory dynFactory, CORBA.TypeCode type, ORB orb, ILogger logger) : base(dynFactory, orb, logger)
        {
            var _type = type.OriginalTypeCode;
            if (_type.Kind != TCKind.TkFixed)
            {
                throw new TypeMismatch();
            }

            typeCode = _type;
            pos = -1;
            anyRepresentation = orb.CreateAny();
            anyRepresentation.InsertFixed(new decimal(0), type);
        }

        public override void FromAny(CORBA.Any value)
        {
            CheckDestroyed();
            if (!value.Type.Equivalent(Type()))
            {
                throw new TypeMismatch();
            }
            typeCode = value.Type.OriginalTypeCode;
            anyRepresentation = orb.CreateAny();
            anyRepresentation.ReadValue(value.CreateInputStream(), Type());
        }

        [return: WideChar(false)]
        public string GetValue()
        {
            return anyRepresentation.ExtractFixed().ToString();
        }

        public bool SetValue([WideChar(false)] string value)
        {
            if (value == null)
            {
                throw new TypeMismatch();
            }
            string val = value.Trim();

            if (val.EndsWith("D") || val.EndsWith("d"))
            {
                val = val.Substring(0, val.Length - 1);
            }

            decimal fixedValue;
            try
            {
                fixedValue = decimal.Parse(val);
            }
            catch (FormatException ex)
            {
                throw new TypeMismatch();
            }

            bool truncate = false;
            try
            {
                int[] parts = decimal.GetBits(fixedValue);
                
                byte decimalScale = (byte)((parts[3] >> 16) & 0x7F);      

                int extra = decimalScale - Type().FixedScale;
                if (extra > 0)
                {
                    // truncate the value to fit the scale of the typecode
                    val = val.Substring(0, val.Length - extra);
                    truncate = true;
                }
                else if (extra < 0)
                {
                    var buffer = new StringBuilder(val);

                    // add the decimal point if necessary
                    if (val.IndexOf('.') == -1)
                    {
                        buffer.Append('.');
                    }

                    // pad the value with zeros to fit the scale of the typecode
                    for (int i = extra; i < 0; i++)
                    {
                        buffer.Append('0');
                    }
                    val = buffer.ToString();
                }
                fixedValue = decimal.Parse(val);

                var type = GetTypeCode(fixedValue);

                if (type.FixedDigits > Type().FixedDigits)
                {
                    throw new InvalidValue();
                }
                anyRepresentation.InsertFixed(fixedValue, type);
            }
            catch (BadKind e)
            {
                throw UnexpectedException(e);
            }
            return !truncate;
        }

        private CORBA.TypeCode GetTypeCode(decimal value)
        {
            string s = value.ToString();
            ushort digits = 0;
            ushort scale = 0;
            if (s.StartsWith("-"))
            {
                s = s.Substring(1);
            }
            if (!s.StartsWith("0."))
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '.')
                    {
                        break;
                    }
                    digits++;
                }
            }
            int point = s.IndexOf('.');
            if (point != -1)
            {
                s = s.Substring(point + 1);
                for (int i = 0; i < s.Length; i++)
                {
                    digits++;
                    scale++;
                }
            }
            return CORBA.ORB.Init().CreateFixedTc(digits, scale);
        }
    }
}
