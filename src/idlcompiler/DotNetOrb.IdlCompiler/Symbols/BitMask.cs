// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class BitMask: IDLSymbol, ITypeSymbol
    {
        public byte Length { get; set; }        
        private List<BitValue> values = new List<BitValue>();
        public List<BitValue> Values { get => values; }

        public int TCKind
        {
            get
            {
                return 17;
            }
        }

        public string IDLType
        {
            get
            {
                return "bitmask";
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

        public BitMask(string name, List<Annotation> annotations = null) : base(name, annotations)
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
                    PrintEnum(currentDir, $"{indent}\t", stream);
                    if (!String.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine("}");
                    }
                }
                GenerateHelper(currentDir);
            }
            else
            {
                PrintEnum(currentDir, indent, stream);
                PrintHelper(currentDir, indent, stream);
            }
            
        }

        private void PrintEnum(DirectoryInfo currentDir, string indent, StreamWriter stream)
        {
            stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
            stream.WriteLine($"{indent}[RepositoryID(\"{RepositoryId}\")]");
            stream.WriteLine($"{indent}[Helper(typeof({HelperName}))]");
            stream.WriteLine($"{indent}[Flags]");
            stream.WriteLine($"{indent}public enum {MappedName}");
            stream.WriteLine($"{indent}{{");
            int index = 0;
            //Process values
            var sortedList = Values.OrderBy(o => o.Position).ToList();
            foreach (var e in sortedList)
            {
                stream.WriteLine($"{indent}\t[IdlName(\"{e.Name}\")]");
                stream.WriteLine($"{indent}\t{e.MappedName} = 1 << {index++},");
            }
            stream.WriteLine($"{indent}}}");
            stream.WriteLine();
        }

        private void PrintTypeCodeExp(string indent, StreamWriter stream)
        {
            var members = new List<string>();
            var sortedList = Values.OrderBy(o => o.Position).ToList();
            foreach (var e in sortedList)
            {
                members.Add(e.Name);
            }
            stream.WriteLine($"{indent}type = \"CORBA.ORB.Init().CreateEnumTc({FullHelperName}.Id(),\"{(Compiler.TypeCodeFullNamespace ? FullName : Name)}\", new String[] {{{String.Join(", ", members)}}});");
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
            stream.WriteLine($"{indent}\tpublic static void Insert(CORBA.Any any, {MappedType} value)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tany.Type = Type();");
            stream.WriteLine($"{indent}\t\tWrite(any.CreateOutputStream(), value);");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static {MappedType} Extract(CORBA.Any any)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tvar inputStream = any.CreateInputStream();");
            stream.WriteLine($"{indent}\t\ttry");
            stream.WriteLine($"{indent}\t\t{{");
            stream.WriteLine($"{indent}\t\treturn Read(inputStream);");
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t\tfinally");
            stream.WriteLine($"{indent}\t\t{{");
            stream.WriteLine($"{indent}\t\tinputStream.Close();");
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
            stream.WriteLine($"{indent}\t\treturn ({MappedType}) inputStream.ReadLong();");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static void Write(CORBA.IOutputStream outputStream, {MappedType} value)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\toutputStream.WriteLong((int) value);");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}}}");
        }
    }
}
