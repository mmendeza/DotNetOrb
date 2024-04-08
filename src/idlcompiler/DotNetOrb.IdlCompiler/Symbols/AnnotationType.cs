// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class AnnotationType: IDLSymbol, IScopeSymbol, IFwdDeclSymbol
    {
        public Scope NamingScope { get; set; }
        public bool IsForwardDeclaration { get; set; }                
        public List<AnnotationType> Inherits { get; set; }


        public AnnotationType(string name, bool dotNetNaming, List<Annotation> annotations = null) : base(name, dotNetNaming, annotations)
        {
            Inherits = new List<AnnotationType>();
        }

        public void Define(IFwdDeclSymbol symbol)
        {
            if (symbol is AnnotationType s)
            {
                IsForwardDeclaration = false;
                Inherits = s.Inherits;
                Annotations = s.Annotations;
            }
            else
            {
                throw new ArgumentException("Symbol must be an annotation definition");
            }
        }

        public override void Include(DirectoryInfo currentDir, string indent = "", StreamWriter stream = null)
        {
            if (IsIncluded)
            {
                return;
            }
            if (IsForwardDeclaration)
            {
                //throw new IdlCompilerException("undefined interface " + Name);
                Console.WriteLine("Undefined symbol " + Name);
            }
            else
            {
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

        private void PrintClass(DirectoryInfo currentDir, string indent, StreamWriter stream)
        {
            stream.WriteLine("[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]");
            stream.WriteLine($"{indent}public class {MappedName}: System.Attribute");
            stream.WriteLine($"{indent}{{");
            //Process non members
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                symbol.Include(currentDir, indent, stream);
            }

            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is AnnotationMember m)
                {
                    foreach (var att in m.MappedAttributes)
                    {
                        stream.WriteLine($"{indent}\t{att}");
                    }
                    foreach (var att in m.DataType.MappedAttributes)
                    {
                        stream.WriteLine($"{indent}\t{att}");
                    }
                    stream.WriteLine($"{indent}\t{m.DataType.MappedType} {m.MappedName} {{ get; set; }}");
                }
            }

            //Default Constructor            
            stream.WriteLine($"{indent}\tpublic {MappedName}()");
            stream.WriteLine($"{indent}\t{{");
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is AnnotationMember m)
                {
                    if (m.DefaultValue != null)
                    {
                        stream.Write($"{m.MappedName} = {m.DefaultValue.ToString()};");
                    }
                }
            }
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            //Constructor
            stream.Write($"{indent}\tpublic {MappedName}(");
            int index = 0;
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is AnnotationMember m)
                {
                    if (index > 0)
                    {
                        stream.Write(", ");
                    }
                    stream.Write($"{m.DataType.MappedType} {Utils.ToCamelCase(m.MappedName)}");
                    index++;
                }
            }
            stream.WriteLine(")");
            stream.WriteLine($"{indent}\t{{");
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is AnnotationMember m)
                {
                    stream.WriteLine($"{indent}\t\t{m.MappedName} = {Utils.ToCamelCase(m.MappedName)};");
                }
            }
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine($"{indent}}}");
            stream.WriteLine();
        }
    }
}
