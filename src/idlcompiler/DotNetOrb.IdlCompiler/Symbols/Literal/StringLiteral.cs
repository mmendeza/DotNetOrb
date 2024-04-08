// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public struct StringLiteral : Literal
    {

        private String value;

        /// <summary>constructed from a wide string literal?</summary>
        private bool isWide;


        public StringLiteral(String value, bool isWide)
        {
            this.value = value;
            this.isWide = isWide;
        }


        public Literal Or(Literal toOrWith)
        {
            throw new IdlCompilerException("Cannot use bitwise or on string");
        }

        public Literal Xor(Literal toXorWith)
        {
            throw new IdlCompilerException("Cannot use bitwise xor on string");
        }

        public Literal And(Literal toAndWith)
        {
            throw new IdlCompilerException("Cannot use bitwise and on string");
        }

        public Literal ShiftRightBy(Literal shift)
        {
            throw new IdlCompilerException("Cannot use bitwise shift on string");
        }

        public Literal ShiftLeftBy(Literal shift)
        {
            throw new IdlCompilerException("Cannot use bitwise shift on string");
        }

        public Literal MultBy(Literal toMultWith)
        {
            throw new IdlCompilerException("Cannot use mult with string");
        }

        public Literal DivBy(Literal toDivBy)
        {
            throw new IdlCompilerException("Cannot use div with string");
        }

        public Literal ModBy(Literal toModBy)
        {
            throw new IdlCompilerException("Cannot use mod with string");
        }

        public Literal Add(Literal toAddTo)
        {
            throw new IdlCompilerException("Cannot use add on string");
        }

        public Literal Sub(Literal toSubTo)
        {
            throw new IdlCompilerException("Cannot use sub on string");
        }

        public void Negate()
        {
            throw new IdlCompilerException("Cannot negate a string");
        }

        public void InvertSign()
        {
            throw new IdlCompilerException("unary operator - not allowed for strings");
        }

        public object GetValue()
        {
            return value;
        }

        public Double GetFloatValue()
        {
            throw new IdlCompilerException("require a float operand, but found a string operand: " + value);
        }

        public Int64 GetIntValue()
        {
            throw new IdlCompilerException("require an integer operand, but found a string operand: " + value);
        }

        public Char GetCharValue()
        {
            throw new IdlCompilerException("require a char operand, but found a string operand: " + value);
        }

        public String GetStringValue()
        {
            return value;
        }

        public Boolean GetBooleanValue()
        {
            throw new IdlCompilerException("require a boolean operand, but found a string operand: " + value);
        }

        public override String ToString()
        {
            return "\"" + value.ToString() + "\"";
        }
    }
}
