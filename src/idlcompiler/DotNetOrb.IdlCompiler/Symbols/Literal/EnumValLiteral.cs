// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public struct EnumValLiteral : Literal
    {

        private Enumerator value;

        public EnumValLiteral(Enumerator value)
        {            
            this.value = value;
        }

        public Literal Or(Literal toOrWith)
        {
            throw new IdlCompilerException("Cannot use bitwise or on enum");
        }

        public Literal Xor(Literal toXorWith)
        {
            throw new IdlCompilerException("Cannot use bitwise xor on enum");
        }

        public Literal And(Literal toAndWith)
        {
            throw new IdlCompilerException("Cannot use bitwise and on enum");
        }

        public Literal ShiftRightBy(Literal shift)
        {
            throw new IdlCompilerException("Cannot use bitwise shift on enum");
        }

        public Literal ShiftLeftBy(Literal shift)
        {
            throw new IdlCompilerException("Cannot use bitwise shift on enum");
        }

        public Literal MultBy(Literal toMultWith)
        {
            throw new IdlCompilerException("Cannot use mult with enum");
        }

        public Literal DivBy(Literal toDivBy)
        {
            throw new IdlCompilerException("Cannot use div with enum");
        }

        public Literal ModBy(Literal toModBy)
        {
            throw new IdlCompilerException("Cannot use mod with enum");
        }

        public Literal Add(Literal toAddTo)
        {
            throw new IdlCompilerException("Cannot use add on enum");
        }

        public Literal Sub(Literal toSubTo)
        {
            throw new IdlCompilerException("Cannot use sub on enum");
        }

        public void Negate()
        {
            throw new IdlCompilerException("Cannot negate an enum");
        }

        public void InvertSign()
        {
            throw new IdlCompilerException("unary operator - not allowed for enum value");
        }

        public object GetValue()
        {
            return value;
        }

        public Double GetFloatValue()
        {
            throw new IdlCompilerException("require an double operand, but found an enum val operand: " + value);
        }

        public Int64 GetIntValue()
        {
            throw new IdlCompilerException("require an integer operand, but found an enum val operand: " + value);
        }

        public Char GetCharValue()
        {
            throw new IdlCompilerException("require a char operand, but found an enum operand: " + value);
        }

        public String GetStringValue()
        {
            throw new IdlCompilerException("require a string operand, but found an enum operand: " + value);
        }

        public Boolean GetBooleanValue()
        {
            throw new IdlCompilerException("require a boolean operand, but found an enum operand: " + value);
        }

        public override String ToString()
        {
            return value.Value.ToString();
        }
    }
}
