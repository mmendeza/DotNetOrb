// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Enumeration : IDLSymbol, ITypeSymbol
    {
        public byte Length { get; set; }
        public string Type
        {
            get
            {
                if (Length > 0)
                {
                    if (Length <= 8)
                    {
                        return "sbyte";
                    }
                    else if (Length <= 16)
                    {
                        return "short";
                    }
                    else if (Length <= 32)
                    {
                        return "int";
                    }
                    else if (Length <= 64)
                    {
                        return "long";
                    }
                }
                return null;
            }
        }

        private List<Enumerator> enumerators = new List<Enumerator>();
        public List<Enumerator> Enumerators { get => enumerators; }

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
                return "enum";
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

        public Enumeration(string name, bool dotNetNaming, List<Annotation> annotations = null) : base(name, dotNetNaming, annotations)
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
                    stream.WriteLine();
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
            stream.Write($"{indent}public enum {MappedName}");
            if (Length > 0)
            {
                stream.Write($" : {Type}");
            }
            stream.WriteLine();
            stream.WriteLine($"{indent}{{");
            int index = 0;
            //Process enumerators
            foreach (var e in Enumerators)
            {
                stream.WriteLine($"{indent}\t[IdlName(\"{e.Name}\")]");
                stream.WriteLine($"{indent}\t{e.MappedName} = {(e.HasValue ? e.Value : index++)},");
            }
            stream.WriteLine($"{indent}}}");
            stream.WriteLine();
        }

        private void PrintTypeCodeExp(string indent, StreamWriter stream)
        {
            var members = new List<string>();
            foreach (var e in Enumerators)
            {
                members.Add('"' + e.Name + '"');
            }
            stream.WriteLine($"{indent}type = CORBA.ORB.Init().CreateEnumTc({FullHelperName}.Id(),\"{Name}\", new String[] {{{String.Join(", ", members)}}});");
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
            stream.WriteLine($"{indent}\t\t\tvar inputStream = any.CreateInputStream();");
            stream.WriteLine($"{indent}\t\ttry");
            stream.WriteLine($"{indent}\t\t{{");
            stream.WriteLine($"{indent}\t\treturn Read(inputStream);");
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
