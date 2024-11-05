// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Constant: IDLSymbol
    {
        public ITypeSymbol DataType { get; set; }
        public Literal Value { get; set; }

        public Constant(string name, List<Annotation> annotations = null) : base(name, annotations)
        {
        }

        public void PrintClass(DirectoryInfo currentDir, string indent, StreamWriter stream)
        {
            stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
            stream.WriteLine($"{indent}public sealed class {MappedName}");
            stream.WriteLine($"{indent}{{");
            switch (Value)
            {
                case EnumValLiteral e:
                    var enumerator = e.GetValue() as Enumerator;
                    stream.WriteLine($"{indent}\tpublic const {DataType.MappedType} Value = {enumerator.Enumeration.MappedType}.{enumerator.MappedName};");
                    break;
                case StringLiteral:
                    stream.WriteLine($"{indent}\tpublic const {DataType.MappedType} Value = \"{Value.GetValue()}\";");
                    break;
                case CharLiteral:
                    stream.WriteLine($"{indent}\tpublic const {DataType.MappedType} Value = '{Value.GetValue()}';");
                    break;
                default:
                    stream.WriteLine($"{indent}\tpublic const {DataType.MappedType} Value = {Value.GetValue()};");
                    break;
            }
            stream.WriteLine($"{indent}}}");
        }

        public override void Include(DirectoryInfo currentDir, string indent = "", StreamWriter stream = null)
        {
            if (IsIncluded)
            {
                return;
            }
            IsIncluded = true;
            //if (ParentScope != null && ParentScope.Symbol != null)
            //{
            //    var dir = new DirectoryInfo($"{currentDir.FullName}\\{ParentScope.Symbol.GetMappedName()}");
            //    if (!dir.Exists)
            //    {
            //        dir.Create();
            //    }
            //    currentDir = dir;
            //}
            if (stream == null)
            {
                var file = new FileInfo($"{currentDir.FullName}\\{MappedName}.cs");
                using (stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    if (!String.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }
                    PrintClass(currentDir, $"{indent}\t", stream);
                    if (!String.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine("}");
                    }
                }
            }
            else
            {
                PrintClass(currentDir, indent, stream);
            }
        }
    }
}
