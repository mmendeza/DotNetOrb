// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public struct CharLiteral : Literal
    {

        private Char value;

        /// <summary>constructed from a wide char literal?</summary>
        private bool isWide;

        public CharLiteral(String value, bool isWide)
        {
            this.value = ParseCharLiteral(value);
            this.isWide = isWide;
        }

        public Literal Or(Literal toOrWith)
        {
            throw new IdlCompilerException("Cannot use bitwise or on char");
        }

        public Literal Xor(Literal toXorWith)
        {
            throw new IdlCompilerException("Cannot use bitwise xor on char");
        }

        public Literal And(Literal toAndWith)
        {
            throw new IdlCompilerException("Cannot use bitwise and on char");
        }

        public Literal ShiftRightBy(Literal shift)
        {
            throw new IdlCompilerException("Cannot use bitwise shift on char");
        }

        public Literal ShiftLeftBy(Literal shift)
        {
            throw new IdlCompilerException("Cannot use bitwise shift on char");
        }

        public Literal MultBy(Literal toMultWith)
        {
            throw new IdlCompilerException("Cannot use mult with char");
        }

        public Literal DivBy(Literal toDivBy)
        {
            throw new IdlCompilerException("Cannot use div with char");
        }

        public Literal ModBy(Literal toModBy)
        {
            throw new IdlCompilerException("Cannot use mod with char");
        }

        public Literal Add(Literal toAddTo)
        {
            throw new IdlCompilerException("Cannot use add on char");
        }

        public Literal Sub(Literal toSubTo)
        {
            throw new IdlCompilerException("Cannot use sub on char");
        }

        public void Negate()
        {
            throw new IdlCompilerException("Cannot negate a char");
        }

        public void InvertSign()
        {
            throw new IdlCompilerException("unary operator - not allowed for characters");
        }

        public object GetValue()
        {
            return value;
        }

        public Double GetFloatValue()
        {
            throw new IdlCompilerException("require a float operand, but found a char operand: " + value);
        }

        public Int64 GetIntValue()
        {
            throw new IdlCompilerException("require an integer operand, but found a char operand: " + value);
        }

        public Char GetCharValue()
        {
            return value;
        }

        public String GetStringValue()
        {
            throw new IdlCompilerException("require a string operand, but found a char operand: " + value);
        }
        public Boolean GetBooleanValue()
        {
            throw new IdlCompilerException("require a boolean operand, but found a char operand: " + value);
        }

        private static Char ParseCharLiteral(String charLiteral)
        {
            if (!charLiteral.StartsWith("\\"))
                return charLiteral[0];

            if (charLiteral == "\\n")
                return '\n';

            if (charLiteral == "\\t")
                return '\t';

            if (charLiteral == "\\v")
                return Convert.ToChar(11);

            if (charLiteral == "\\b")
                return '\b';

            if (charLiteral == "\\r")
                return '\r';

            if (charLiteral == "\\f")
                return '\f';

            if (charLiteral == "\\a")
                return Convert.ToChar(7);

            if (charLiteral == "\\\\")
                return '\\';

            if (charLiteral == "\\?")
                return '?';

            if (charLiteral == "\\'")
                return '\'';

            if (charLiteral == "\\\"")
                return '\"';

            if (charLiteral.StartsWith("\\x") && (charLiteral.Length > 2 && charLiteral.Length < 5))
            {
                Int32 charValHex = Int32.Parse(charLiteral.Substring(2), NumberStyles.AllowHexSpecifier);
                return Convert.ToChar(charValHex);
            }

            if (charLiteral.Equals("\\0"))                            // non-standard literal
                return Convert.ToChar(0);

            if (charLiteral.Length > 1 && charLiteral.Length < 5)
            {
                Int32 charValDec = Convert.ToInt32(charLiteral.Substring(1), 8);
                return Convert.ToChar(charValDec);
            }

            throw new NotSupportedException("unsupported escape in character literal: " + charLiteral);
        }

        public override String ToString()
        {
            return "'" + value.ToString() + "'";
        }
    }
}
