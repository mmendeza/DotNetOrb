// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class BitSet: IDLSymbol, ITypeSymbol, IScopeSymbol
    {
        public Scope NamingScope { get; set; }
        public BitSet Base { get; set; }       

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
                return "bitset";
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

        public BitSet(string name, bool dotNetNaming, List<Annotation> annotations = null) : base(name, dotNetNaming, annotations)        
        {            
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
                    PrintStruct(currentDir, $"{indent}\t", stream);
                    if (!String.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine("}");
                    }
                }
                GenerateHelper(currentDir);
            }
            else
            {
                PrintStruct(currentDir, indent, stream);
                PrintHelper(currentDir, indent, stream);
            }            
        }

        private void PrintStruct(DirectoryInfo currentDir, string indent, StreamWriter stream)
        {
            stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
            stream.WriteLine($"{indent}[RepositoryID(\"{RepositoryId}\")]");
            stream.WriteLine($"{indent}[Helper(typeof({HelperName}))]");
            stream.Write($"{indent}public struct {MappedName}: CORBA.IIDLEntity, IEquatable<{MappedType}>");
            var ancestorName = "";
            if (Base != null)
            {
                stream.Write(", ");
                stream.Write($"{Base.MappedType}");
            }
            stream.WriteLine();
            stream.WriteLine($"{indent}{{");
            //Process non members
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                symbol.Include(currentDir, indent, stream);
            }
            //Default Constructor            
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
                    if (symbol is BitField f)
                    {
                        if (i > 0)
                        {
                            stream.Write(", ");
                        }
                        stream.WriteLine($"other.{f.MappedName}");
                    }
                }
                stream.Write($")");
            }
            stream.WriteLine();
            stream.WriteLine($"{indent}\t{{");
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is BitField f)
                {
                    stream.WriteLine($"{indent}\\t{f.MappedName} = other.{MappedName};");
                }
            }
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            //Constructor
            stream.Write($"{indent}\tpublic {MappedName}(");
            if (Base != null)
            {
                stream.Write($"{ancestorName} parent, ");
            }
            int index = 0;
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is BitField f)
                {
                    if (index > 0)
                    {
                        stream.Write(", ");
                    }
                    stream.Write($"{f.DataType.MappedType} {Utils.ToCamelCase(f.MappedName)}");
                    index++;
                }
            }
            stream.Write(")");
            if (Base != null)
            {
                stream.Write(": base(parent)");
            }
            stream.WriteLine();
            stream.WriteLine($"{indent}\t{{");
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is BitField f)
                {
                    stream.WriteLine($"{indent}\\tthis.{f.MappedName} = {Utils.ToCamelCase(f.MappedName)};");
                }
            }
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            //Equals
            stream.WriteLine($"{indent}\tpublic bool Equals({MappedName} other)");
            stream.WriteLine($"{indent}\t{{");
            if (Base != null)
            {
                foreach (IDLSymbol symbol in Base.NamingScope.Symbols.Values)
                {
                    if (symbol is BitField f)
                    {
                        stream.WriteLine($"if ({f.MappedName} != other.{f.MappedName}) return false;");
                    }
                }
            }
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is BitField f)
                {
                    stream.WriteLine($"if ({f.MappedName} != other.{f.MappedName}) return false;");
                }
            }
            stream.WriteLine($"{indent}\t\treturn true;");
            stream.WriteLine($"{indent}\t}}");

            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is BitField f)
                {
                    stream.WriteLine($"{indent}\t[IdlName(\"{f.Name}\")]");
                    foreach (var att in f.MappedAttributes)
                    {
                        stream.WriteLine($"{indent}\t{att}");
                    }
                    foreach (var att in f.DataType.MappedAttributes)
                    {
                        stream.WriteLine($"{indent}\t{att}");
                    }
                    stream.WriteLine($"{indent}\t{f.DataType.MappedType} {f.MappedName} {{ get; set; }}");
                }
            }
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
            stream.WriteLine($"\tpublic static class {className}");
            stream.WriteLine($"\t{{");
            stream.WriteLine($"\t\tprivate static volatile CORBA.TypeCode type;");
            stream.WriteLine();
            stream.WriteLine($"\t\tpublic static CORBA.TypeCode Type()");
            stream.WriteLine($"\t\t{{");
            stream.WriteLine($"\t\t\tif (type == null)");
            stream.WriteLine($"\t\t\t{{");
            stream.WriteLine($"\t\t\t\tlock (typeof({className}))");
            stream.WriteLine($"\t\t\t\t{{");
            stream.WriteLine($"\t\t\t\t\tif (type == null)");
            stream.WriteLine($"\t\t\t\t\t{{");
            PrintTypeCodeExp("\"\\t\\t\\t\\t\\t\\t", stream);
            stream.WriteLine($"\t\t\t\t\t}}");
            stream.WriteLine($"\t\t\t\t}}");
            stream.WriteLine($"\t\t\t}}");
            stream.WriteLine($"\t\t\treturn type;");
            stream.WriteLine($"\t\t}}");
            stream.WriteLine();
            stream.WriteLine($"\t\tpublic static void Insert(CORBA.Any any, {MappedType} s)");
            stream.WriteLine($"\t\t{{");
            stream.WriteLine($"\t\t\tany.Type = Type();");
            stream.WriteLine($"\t\t\tWrite(any.CreateOutputStream(),s);");
            stream.WriteLine($"\t\t}}");
            stream.WriteLine();
            stream.WriteLine($"\t\tpublic static {MappedType} Extract(CORBA.Any any)");
            stream.WriteLine($"\t\t{{");
            stream.WriteLine($"\t\t\tvar inputStream = any.CreateInputStream();");
            stream.WriteLine($"\t\t\ttry");
            stream.WriteLine($"\t\t\t{{");
            stream.WriteLine($"\t\t\treturn Read(inputStream);");
            stream.WriteLine($"\t\t\t}}");
            stream.WriteLine($"\t\t\tfinally");
            stream.WriteLine($"\t\t\t{{");
            stream.WriteLine($"\t\t\tinputStream.Close();");
            stream.WriteLine($"\t\t\t}}");
            stream.WriteLine($"\t\t}}");
            stream.WriteLine();
            stream.WriteLine($"\t\tpublic static string Id()");
            stream.WriteLine($"\t\t{{");
            stream.WriteLine($"\t\t    return \"{RepositoryId}\";");
            stream.WriteLine($"\t\t}}");
            stream.WriteLine();
            stream.WriteLine($"\t\tpublic static {MappedType} Read(CORBA.IInputStream inputStream)");
            stream.WriteLine($"\t\t{{");
            stream.WriteLine($"\t\t\tvar result = new {MappedType}();");
            PrintReadMembers("\t\t\t", stream, "inputStream", "result");
            stream.WriteLine($"\t\t\treturn result;");
            stream.WriteLine($"\t\t}}");
            stream.WriteLine();
            stream.WriteLine($"\t\tpublic static void Write(CORBA.IOutputStream outputStream, {MappedType} s)");
            stream.WriteLine($"\t\t{{");
            PrintWriteMembers("\t\t\t", stream, "outputStream", "s");
            stream.WriteLine($"\t\t}}");
            stream.WriteLine();
            stream.WriteLine($"\t}}");
        }

        private void PrintReadMembers(string indent, TextWriter sw, string streamName, string varName)
        {
            var sbase = Base;
            while (sbase != null)
            {
                foreach (IDLSymbol child in sbase.NamingScope.Symbols.Values)
                {
                    if (child is BitField f)
                    {
                        f.DataType.PrintRead(indent, sw, streamName, varName + "." + f.MappedName, 0);
                        sw.WriteLine();
                    }
                }
                sbase = sbase.Base;
            }
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is BitField f)
                {
                    f.DataType.PrintRead(indent, sw, streamName, varName + "." + f.MappedName, 0);
                    sw.WriteLine();
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
                    if (child is BitField f)
                    {
                        f.DataType.PrintWrite(indent, sw, streamName, varName + "." + f.MappedName, 0);
                        sw.WriteLine();
                    }
                }
                sbase = sbase.Base;
            }
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is BitField f)
                {
                    f.DataType.PrintWrite(indent, sw, streamName, varName + "." + f.MappedName, 0);
                    sw.WriteLine();
                }
            }
        }

        private void PrintTypeCodeExp(string indent, StreamWriter stream)
        {
            stream.Write($"{indent}type = CORBA.ORB.Init().CreateStructTc({FullHelperName}.Id(), \"{Name}\", new CORBA.StructMember[] {{");
            var sbase = Base;
            while (sbase != null)
            {
                foreach (IDLSymbol child in sbase.NamingScope.Symbols.Values)
                {
                    if (child is BitField f)
                    {
                        stream.Write($"new CORBA.StructMember(\"{f.Name}\", {f.DataType.TypeCodeExp}, null), ");
                    }
                }
                sbase = sbase.Base;
            }
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is BitField f)
                {
                    stream.Write($"new CORBA.StructMember(\"{f.Name}\", {f.DataType.TypeCodeExp}, null), ");
                }
            }
            stream.WriteLine($"}});");
        }
    }
}
