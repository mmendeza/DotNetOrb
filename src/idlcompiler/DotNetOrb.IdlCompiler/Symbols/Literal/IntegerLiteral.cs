// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public struct IntegerLiteral : Literal
    {

        private Decimal value;

        public IntegerLiteral(String val)
        {
            var temp = val;
            if (val.EndsWith("l") || val.EndsWith("L"))
            {
                temp = val.Substring(0, val.Length - 1);
            }
            if (temp.StartsWith("0x") || temp.StartsWith("0X"))
            {
                value = new Decimal(Convert.ToInt64(temp.Substring(2), 16));
            }
            else if (temp.StartsWith("0"))
            {
                value = new Decimal(Convert.ToInt64(temp, 8));
            }
            else
            {

                value = Decimal.Parse(temp, System.Globalization.NumberStyles.Integer);
            }
        }

        public IntegerLiteral(Decimal val)
        {
            value = val;
        }

        public Literal Or(Literal toOrWith)
        {
            return new IntegerLiteral(GetIntValue() | toOrWith.GetIntValue());
        }

        public Literal Xor(Literal toXorWith)
        {
            return new IntegerLiteral(GetIntValue() ^ toXorWith.GetIntValue());
        }

        public Literal And(Literal toAndWith)
        {
            return new IntegerLiteral(GetIntValue() & toAndWith.GetIntValue());
        }

        public Literal ShiftRightBy(Literal shift)
        {
            if ((shift.GetIntValue() >= 64) || (shift.GetIntValue() < 0))
            {
                throw new IdlCompilerException("Invalid shift by argument " +
                    shift.GetIntValue() + "; shift is only possible with values in the range 0 <= shift < 63.");
            }
            return new IntegerLiteral(GetIntValue() >> (int)shift.GetIntValue());
        }

        public Literal ShiftLeftBy(Literal shift)
        {
            if ((shift.GetIntValue() >= 64) || (shift.GetIntValue() < 0))
            {
                throw new IdlCompilerException("Invalid shift by argument " +
                    shift.GetIntValue() + "; shift is only possible with values in the range 0 <= shift < 63.");
            }
            return new IntegerLiteral(GetIntValue() << (int)shift.GetIntValue());
        }

        public Literal MultBy(Literal toMultWith)
        {
            return new IntegerLiteral(value * toMultWith.GetIntValue());
        }

        public Literal DivBy(Literal toDivBy)
        {
            return new IntegerLiteral(value / toDivBy.GetIntValue());
        }

        public Literal ModBy(Literal toModBy)
        {
            return new IntegerLiteral(value % toModBy.GetIntValue());
        }

        public Literal Add(Literal toAddTo)
        {
            return new IntegerLiteral(value + toAddTo.GetIntValue());
        }

        public Literal Sub(Literal toSubTo)
        {
            return new IntegerLiteral(value - toSubTo.GetIntValue());
        }

        public void Negate()
        {
            value = ~GetIntValue();
        }

        public void InvertSign()
        {
            value = value * (-1);
        }

        public object GetValue()
        {
            return value;
        }

        public Double GetFloatValue()
        {
            throw new IdlCompilerException("require an float operand, but found a integer operand: " + value);
        }

        public Int64 GetIntValue()
        {
            if (value >= Int64.MinValue && value <= Int64.MaxValue)
            {
                return Convert.ToInt64(value);
            }
            else if (value >= UInt64.MinValue && value <= UInt64.MaxValue)
            {
                UInt64 asUint64 = Convert.ToUInt64(value);
                return unchecked((Int64)asUint64); // do an unchecked cast, overflow no issue here
            }
            else
            {
                throw new IdlCompilerException("integer literal need more than 64bit, not convertible to int64 value");
            }
        }

        public Char GetCharValue()
        {
            throw new IdlCompilerException("require a char operand, but found an integer operand: " + value);
        }

        public String GetStringValue()
        {
            throw new IdlCompilerException("require a string operand, but found an integer operand: " + value);
        }

        public Boolean GetBooleanValue()
        {
            throw new IdlCompilerException("require a boolean operand, but found an integer operand: " + value);
        }

        public override String ToString()
        {
            return value.ToString();
        }
    }
}
