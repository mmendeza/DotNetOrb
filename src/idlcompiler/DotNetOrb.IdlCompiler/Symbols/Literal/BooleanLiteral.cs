// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public struct BooleanLiteral : Literal
    {

        private Boolean value;

        public BooleanLiteral(String val)
        {
            value = val.Equals("TRUE");
        }

        public BooleanLiteral(Boolean value)
        {
            this.value = value;
        }

        public Literal Or(Literal toOrWith)
        {
            throw new IdlCompilerException("Cannot use bitwise or on bool");
        }

        public Literal Xor(Literal toXorWith)
        {
            throw new IdlCompilerException("Cannot use bitwise xor on bool");
        }

        public Literal And(Literal toAndWith)
        {
            throw new IdlCompilerException("Cannot use bitwise and on bool");
        }

        public Literal ShiftRightBy(Literal shift)
        {
            throw new IdlCompilerException("Cannot use bitwise shift on bool");
        }

        public Literal ShiftLeftBy(Literal shift)
        {
            throw new IdlCompilerException("Cannot use bitwise shift on bool");
        }

        public Literal MultBy(Literal toMultWith)
        {
            throw new IdlCompilerException("Cannot use mult with bool");
        }

        public Literal DivBy(Literal toDivBy)
        {
            throw new IdlCompilerException("Cannot use div with bool");
        }

        public Literal ModBy(Literal toModBy)
        {
            throw new IdlCompilerException("Cannot use mod with bool");
        }

        public Literal Add(Literal toAddTo)
        {
            throw new IdlCompilerException("Cannot use add on bool");
        }

        public Literal Sub(Literal toSubTo)
        {
            throw new IdlCompilerException("Cannot use sub on bool");
        }

        public void Negate()
        {
            value = !value;
        }

        public void InvertSign()
        {
            throw new IdlCompilerException("unary operator - not allowed for boolean");
        }

        public object GetValue()
        {
            return value;
        }

        public Double GetFloatValue()
        {
            throw new IdlCompilerException("require a float operand, but found a boolean operand: " + value);
        }

        public Decimal GetDecimalValue()
        {
            throw new IdlCompilerException("require a decimal operand, but found a boolean operand: " + value);
        }

        public Int64 GetIntValue()
        {
            throw new IdlCompilerException("require an integer operand, but found a boolean operand: " + value);
        }

        public Char GetCharValue()
        {
            throw new IdlCompilerException("require a char operand, but found a boolean operand: " + value);
        }

        public String GetStringValue()
        {
            throw new IdlCompilerException("require a string operand, but found a boolean operand: " + value);
        }
        public Boolean GetBooleanValue()
        {
            return value;
        }

        public override String ToString()
        {
            return value.ToString();
        }
    }
}
