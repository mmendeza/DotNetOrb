// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Struct: IDLSymbol, IScopeSymbol, ITypeSymbol, IFwdDeclSymbol
    {
        public Scope NamingScope { get; set; }
        public bool IsForwardDeclaration { get; set; }                
        public Struct Base { get; set; }        

        public int TCKind
        {
            get
            {
                return 15;
            }
        }

        public string IDLType
        {
            get
            {
                return "struct";
            }
        }

        public string MappedType
        {
            get
            {
                if (String.IsNullOrEmpty(Namespace))
                {
                    return MappedName;
                }
                return Namespace + "." + MappedName;
            }
        }

        public string HelperName
        {
            get
            {
                return GetMappedName("", "Helper");
            }
        }

        public string FullHelperName
        {
            get
            {
                if (string.IsNullOrEmpty(Namespace))
                {
                    return GetMappedName("", "Helper");
                }
                return Namespace + "." + GetMappedName("", "Helper");
            }
        }

        public string TypeCodeExp
        {
            get
            {
                return $"{FullHelperName}.Type()";
            }
        }

        public Struct(string name, List<Annotation> annotations = null) : base(name, annotations)
        {         
        }

        public void Define(IFwdDeclSymbol symbol)
        {
            if (symbol is Struct s)
            {
                IsForwardDeclaration = false;                
                Annotations = s.Annotations;
            }
            else
            {
                throw new ArgumentException("Symbol must be a struct definition");
            }
        }

        public void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {FullHelperName}.Read({streamName});");
        }

        public void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{FullHelperName}.Write({streamName}, {varName});");
        }

        public void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            sw.WriteLine($"{indent}{varName} = {FullHelperName}.Extract({anyName});");
        }

        public void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{FullHelperName}.Insert({anyName}, {varName});");
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
                /*if (ParentScope != null && ParentScope.Symbol != null)
                {
                    var dir = new DirectoryInfo($"{currentDir.FullName}\\{ParentScope.Symbol.GetMappedName()}");
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    currentDir = dir;
                }*/
                if (stream == null)
                {
                    var file = new FileInfo($"{currentDir.FullName}\\{MappedName}.cs");
                    using (stream = file.CreateText())
                    {
                        PrintComment(stream);
                        PrintUsings(stream);
                        if (!String.IsNullOrEmpty(Namespace))
                        {
                            indent = "\t";
                            stream.WriteLine($"namespace {Namespace}");
                            stream.WriteLine("{");
                        }
                        PrintClass(currentDir, $"{indent}", stream);
                        if (!String.IsNullOrEmpty(Namespace))
                        {
                            stream.WriteLine("}");
                        }
                    }
                    GenerateHelper(currentDir);
                }
                else
                {
                    PrintClass(currentDir, indent, stream);
                    PrintHelper(currentDir, indent, stream);
                }
            }
        }

        private void PrintClass(DirectoryInfo currentDir, string indent, StreamWriter stream)
        {
            stream.WriteLine($"{indent}[RepositoryID(\"{RepositoryId}\")]");
            stream.WriteLine($"{indent}[Helper(typeof({HelperName}))]");
            foreach (var att in MappedAttributes)
            {
                stream.WriteLine($"{indent}{att}");
            }
            stream.Write($"{indent}public partial class {MappedName}: ");
            if (Base != null)
            {
                stream.Write($"{Base.MappedType}");
                stream.Write(", ");
            }
            stream.WriteLine($"CORBA.IIDLEntity, IEquatable<{MappedType}>");
            stream.WriteLine($"{indent}{{");
            //Process non members
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                symbol.Include(currentDir, $"{indent}\t", stream);
            }

            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is Member m)
                {
                    stream.WriteLine($"{indent}\t[IdlName(\"{m.Name}\")]");
                    foreach (var att in m.MappedAttributes)
                    {
                        stream.WriteLine($"{indent}\t{att}");
                    }
                    foreach (var att in m.DataType.MappedAttributes)
                    {
                        stream.WriteLine($"{indent}\t{att}");
                    }
                    stream.WriteLine($"{indent}\tpublic {m.DataType.MappedType} {m.MappedName} {{ get; set; }}");
                }
            }

            //Default Constructor            
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic {MappedName}()");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            //Copy Constructor
            stream.Write($"{indent}\tpublic {MappedName}({MappedName} other)");
            if (Base != null)
            {
                stream.Write($": base(");
                int i = 0;
                foreach (IDLSymbol symbol in Base.NamingScope.Symbols.Values)
                {
                    if (symbol is Member m)
                    {
                        if (i > 0)
                        {
                            stream.Write(", ");
                        }
                        stream.WriteLine($"other.{m.MappedName}");
                    }
                }
                stream.Write($")");
            }
            stream.WriteLine();
            stream.WriteLine($"{indent}\t{{");
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is Member m)
                {
                    stream.WriteLine($"{indent}\t\t{m.MappedName} = other.{m.MappedName};");
                }
            }
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            //Constructor            
            int index = 0;
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is Member m)
                {
                    if (index == 0)
                    {
                        stream.Write($"{indent}\tpublic {MappedName}(");
                        if (Base != null)
                        {
                            stream.Write($"{Base.MappedType} parent, ");
                        }
                    }
                    else
                    {
                        stream.Write(", ");
                    }
                    stream.Write($"{m.DataType.MappedType} {Utils.ToCamelCase(m.MappedName)}");
                    index++;
                }
            }
            if (index > 0)
            {
                stream.Write(")");
                if (Base != null)
                {
                    stream.Write(": base(parent)");
                }
                stream.WriteLine();
                stream.WriteLine($"{indent}\t{{");
                foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
                {
                    if (symbol is Member m)
                    {
                        stream.WriteLine($"{indent}\t\tthis.{m.MappedName} = {Utils.ToCamelCase(m.MappedName)};");
                    }
                }
                stream.WriteLine($"{indent}\t}}");
                stream.WriteLine();
            }            
            //Equals
            stream.WriteLine($"{indent}\tpublic bool Equals({MappedType}? other)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tif (other == null) return false;");
            if (Base != null)
            {
                foreach (IDLSymbol symbol in Base.NamingScope.Symbols.Values)
                {
                    if (symbol is Member m)
                    {
                        var dataType = m.DataType;
                        if (dataType is RecursiveType rec)
                        {
                            dataType = rec.ActualType;
                        }
                        if (dataType is TypeDefinition typeDef)
                        {
                            dataType = typeDef.ResolveDataType();
                        }
                        if (dataType is Sequence || dataType is ArrayType)
                        {
                            stream.WriteLine($"{indent}\t\tif (!{m.MappedName}.SequenceEqual(other.{m.MappedName})) return false;");
                        }
                        else if (dataType is Map)
                        {
                            stream.WriteLine($"{indent}\t\tif (!{m.MappedName}.OrderBy(kv => kv.Key).SequenceEqual(other.{m.MappedName}.OrderBy(kv => kv.Key))) return false;");
                        }
                        else
                        {
                            stream.WriteLine($"{indent}\t\tif (!{m.MappedName}.Equals(other.{m.MappedName})) return false;");
                        }
                    }
                }
            }
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is Member m)
                {
                    var dataType = m.DataType;
                    if (dataType is RecursiveType rec)
                    {
                        dataType = rec.ActualType;
                    }
                    if (dataType is TypeDefinition typeDef)
                    {
                        dataType = typeDef.ResolveDataType();
                    }
                    if (dataType is Sequence || dataType is ArrayType)
                    {
                        stream.WriteLine($"{indent}\t\tif (!{m.MappedName}.SequenceEqual(other.{m.MappedName})) return false;");
                    }
                    else if (dataType is Map)
                    {
                        stream.WriteLine($"{indent}\t\tif (!{m.MappedName}.OrderBy(kv => kv.Key).SequenceEqual(other.{m.MappedName}.OrderBy(kv => kv.Key))) return false;");
                    }
                    else
                    {
                        stream.WriteLine($"{indent}\t\tif (!{m.MappedName}.Equals(other.{m.MappedName})) return false;");
                    }                    
                }
            }
            stream.WriteLine($"{indent}\t\treturn true;");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine($"{indent}}}");
            stream.WriteLine();
        }

        private void GenerateHelper(DirectoryInfo currentDir)
        {
            var className = HelperName;            
            var file = new FileInfo($"{currentDir.FullName}\\{className}.cs");
            using (StreamWriter stream = file.CreateText())
            {
                PrintComment(stream);
                PrintUsings(stream);
                stream.WriteLine();
                var indent = "";
                if (!String.IsNullOrEmpty(Namespace))
                {
                    indent = "\t";
                    stream.WriteLine($"namespace {Namespace}");
                    stream.WriteLine("{");
                }
                PrintHelper(currentDir, indent, stream);
                if (!String.IsNullOrEmpty(Namespace))
                {
                    stream.WriteLine($"}}");
                }
            }
        }

        private void PrintHelper(DirectoryInfo currentDir, string indent, StreamWriter stream)
        {
            var className = HelperName;
            stream.WriteLine($"{indent}public static class {className}");
            stream.WriteLine($"{indent}{{");
            stream.WriteLine($"{indent}\tprivate static volatile CORBA.TypeCode type;");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static CORBA.TypeCode Type()");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tif (type == null)");
            stream.WriteLine($"{indent}\t\t{{");
            stream.WriteLine($"{indent}\t\t\tlock (typeof({className}))");
            stream.WriteLine($"{indent}\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\tif (type == null)");
            stream.WriteLine($"{indent}\t\t\t\t{{");
            PrintTypeCodeExp($"{indent}\t\t\t\t\t", stream);
            stream.WriteLine($"{indent}\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t}}");
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t\treturn type;");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static void Insert(CORBA.Any any, {MappedType} s)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tany.Type = Type();");
            stream.WriteLine($"{indent}\t\tWrite(any.CreateOutputStream(), s);");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static {MappedType} Extract(CORBA.Any any)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tvar inputStream = any.CreateInputStream();");
            stream.WriteLine($"{indent}\t\ttry");
            stream.WriteLine($"{indent}\t\t{{");
            stream.WriteLine($"{indent}\t\t\treturn Read(inputStream);");
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t\tfinally");
            stream.WriteLine($"{indent}\t\t{{");
            stream.WriteLine($"{indent}\t\t\tinputStream.Close();");
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static string Id()");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\treturn \"{RepositoryId}\";");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static {MappedType} Read(CORBA.IInputStream inputStream)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tvar result = new {MappedType}();");
            PrintReadMembers($"{indent}\t\t", stream, "inputStream", "result");
            stream.WriteLine($"{indent}\t\treturn result;");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static void Write(CORBA.IOutputStream outputStream, {MappedType} s)");
            stream.WriteLine($"{indent}\t{{");
            PrintWriteMembers($"{indent}\t\t", stream, "outputStream", "s");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}}}");
        }

        private void PrintReadMembers(string indent, TextWriter sw, string streamName, string varName)
        {
            var sbase = Base;
            while (sbase != null)
            {
                foreach (IDLSymbol child in sbase.NamingScope.Symbols.Values)
                {
                    if (child is Member m)
                    {
                        m.DataType.PrintRead(indent, sw, streamName, varName + "." + m.GetMappedName(), 0);
                    }
                }
                sbase = sbase.Base;
            }
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is Member m)
                {
                    m.DataType.PrintRead(indent, sw, streamName, varName + "." + m.GetMappedName(), 0);
                }
            }
        }

        private void PrintWriteMembers(string indent, TextWriter sw, string streamName, string varName)
        {
            var sbase = Base;
            while (sbase != null)
            {
                foreach (IDLSymbol child in sbase.NamingScope.Symbols.Values)
                {
                    if (child is Member m)
                    {
                        m.DataType.PrintWrite(indent, sw, streamName, varName + "." + m.GetMappedName(), 0);
                    }
                }
                sbase = sbase.Base;
            }
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is Member m)
                {
                    m.DataType.PrintWrite(indent, sw, streamName, varName + "." + m.GetMappedName(), 0);
                }
            }
        }

        private void PrintTypeCodeExp(string indent, StreamWriter stream)
        {
            stream.Write($"{indent}type = CORBA.ORB.Init().CreateStructTc({FullHelperName}.Id(), \"{(Compiler.TypeCodeFullNamespace ? FullName : Name)}\", new CORBA.StructMember[] {{");
            var sbase = Base;
            while (sbase != null)
            {
                foreach (IDLSymbol child in sbase.NamingScope.Symbols.Values)
                {
                    if (child is Member m)
                    {
                        stream.Write($"new CORBA.StructMember(\"{m.Name}\", {m.DataType.TypeCodeExp}, null), ");
                    }
                }
                sbase = sbase.Base;
            }
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is Member m)
                {
                    stream.Write($"new CORBA.StructMember(\"{m.Name}\", {m.DataType.TypeCodeExp}, null), ");
                }
            }
            stream.WriteLine($"}});");
        }        
    }
}
