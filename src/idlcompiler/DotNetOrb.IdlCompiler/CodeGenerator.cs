// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.IdlCompiler.Symbols;
using System.IO;

namespace DotNetOrb.IdlCompiler
{
    internal class CodeGenerator
    {
        public static readonly string[] cSharpKeywords = { "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "unit", "ulong", "unchecked", "unsafe", "ushort", "using", "using", "static", "virtual", "void", "volatile", "while" };

        public CodeGenerator()
        {
            
        }

        public void Run(Scope scope, DirectoryInfo outputDir)
        {
            if (!outputDir.Exists)
            {
                outputDir.Create();
            }
            foreach (IDLSymbol symbol in scope.Symbols.Values)
            {
                symbol.Include(outputDir);
            }
        }
    }
}
