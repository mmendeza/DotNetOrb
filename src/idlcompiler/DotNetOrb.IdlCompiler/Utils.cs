// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetOrb.IdlCompiler
{
    public static class Utils
    {
        public static readonly string[] CSharpKeywords = { "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "unit", "ulong", "unchecked", "unsafe", "ushort", "using", "using", "static", "virtual", "void", "volatile", "while" };

        public static string ToPascalCase(this string original)
        {
            //Ignore uppercase strings
            if (original == original.ToUpper()) return original;
            Regex invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            Regex whiteSpace = new Regex(@"(?<=\s)");
            Regex startsWithLowerCaseChar = new Regex("^[a-z]");
            Regex firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            Regex lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            Regex upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            // replace white spaces with undescore, then replace all invalid chars with empty string
            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(original, "_"), string.Empty)
                // split by underscores
                .Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                // set first letter to uppercase
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpper()))
                // replace second and all following upper case letters to lower if there is no next lower (ABC -> Abc)
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLower()))
                // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpper()));
                // lower second and next upper case letters except the last if it follows by any lower (ABcDEf -> AbcDef)
                //.Select(w => upperCaseInside.Replace(w, m => m.Value.ToLower()));

            return string.Concat(pascalCase);
        }

        public static string ToCamelCase(this string str)
        {
            if (str == str.ToUpper()) return str.ToLower();

            TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
            var tmp = str.ToPascalCase();
            return Char.ToLowerInvariant(tmp[0]) + tmp.Substring(1);
        }

        public static int GetScale(this decimal value)
        {
            if (value == 0)
                return 0;
            int[] bits = decimal.GetBits(value);
            return (int)((bits[3] >> 16) & 0x7F);
        }

        public static int GetPrecision(this decimal value)
        {
            if (value == 0)
                return 0;
            int[] bits = decimal.GetBits(value);
            //We will use false for the sign (false =  positive), because we don't care about it.
            //We will use 0 for the last argument instead of bits[3] to eliminate the fraction point.
            decimal d = new Decimal(bits[0], bits[1], bits[2], false, 0);
            return (int)Math.Floor(Math.Log10((double)d)) + 1;
        }
        public static string GetSourceText(this ParserRuleContext context)
        {
            IToken startToken = context.Start is ITerminalNode startNode ? startNode.Symbol : context.Start;
            IToken stopToken = context.Stop is ITerminalNode stopNode ? stopNode.Symbol : context.Stop;

            ICharStream cs = startToken.InputStream;
            int stopIndex = stopToken != null ? stopToken.StopIndex : -1;
            return cs.GetText(new Interval(startToken.StartIndex, stopIndex));
        }

    }
}
