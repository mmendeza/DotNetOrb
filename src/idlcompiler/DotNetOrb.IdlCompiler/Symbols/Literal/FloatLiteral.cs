// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.IdlCompiler.Symbols
{

    public struct FloatLiteral : Literal
    {

        private double value;

        public FloatLiteral(String val)
        {
            var temp = val;
            if (val.EndsWith("f") || val.EndsWith("F") || val.EndsWith("d") || val.EndsWith("D"))
            {
                temp = val.Substring(0, val.Length - 1);
            }
            value = Double.Parse(temp, System.Globalization.NumberStyles.Float);
        }

        public FloatLiteral(Double val)
        {
            value = val;
        }

        public Literal Or(Literal toOrWith)
        {
            throw new IdlCompilerException("Cannot use bitwise or on float");
        }

        public Literal Xor(Literal toXorWith)
        {
            throw new IdlCompilerException("Cannot use bitwise xor on float");
        }

        public Literal And(Literal toAndWith)
        {
            throw new IdlCompilerException("Cannot use bitwise and on float");
        }

        public Literal ShiftRightBy(Literal shift)
        {
            throw new IdlCompilerException("Cannot use bitwise shift on float");
        }

        public Literal ShiftLeftBy(Literal shift)
        {
            throw new IdlCompilerException("Cannot use bitwise shift on float");
        }

        public Literal MultBy(Literal toMultWith)
        {
            return new FloatLiteral(value * toMultWith.GetFloatValue());
        }

        public Literal DivBy(Literal toDivBy)
        {
            return new FloatLiteral(value / toDivBy.GetFloatValue());
        }

        public Literal ModBy(Literal toModBy)
        {
            return new FloatLiteral(value % toModBy.GetFloatValue());
        }

        public Literal Add(Literal toAddTo)
        {
            return new FloatLiteral(value + toAddTo.GetFloatValue());
        }

        public Literal Sub(Literal toSubTo)
        {
            return new FloatLiteral(value - toSubTo.GetFloatValue());
        }

        public void Negate()
        {
            throw new IdlCompilerException("Cannot negate a float");
        }

        public void InvertSign()
        {
            value = value * (-1.0);
        }

        public object GetValue()
        {
            return value;
        }

        public Double GetFloatValue()
        {
            return value;
        }

        public Int64 GetIntValue()
        {
            throw new IdlCompilerException("require an integer operand, but found a float operand: " + value);
        }

        public Char GetCharValue()
        {
            throw new IdlCompilerException("require a char operand, but found a float operand: " + value);
        }

        public String GetStringValue()
        {
            throw new IdlCompilerException("require a string operand, but found a float operand: " + value);
        }
        public Boolean GetBooleanValue()
        {
            throw new IdlCompilerException("require a boolean operand, but found a float operand: " + value);
        }

        public override String ToString()
        {
            return value.ToString();
        }
    }
}
