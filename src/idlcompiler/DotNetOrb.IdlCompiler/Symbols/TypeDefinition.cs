// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class TypeDefinition: IDLSymbol, ITypeSymbol
    {

        public ITypeSymbol DataType { get; set; }

        public int TCKind
        {
            get
            {
                return 21;
            }            
        }

        public string IDLType
        {
            get
            {
                return Name;
            }
        }

        public string MappedType
        {
            get
            {
                return ResolveDataType().MappedType;
            }
        }

        public string TypeCodeExp
        {
            get
            {                
                return $"CORBA.ORB.Init().CreateAliasTc({FullHelperName}.Id(),\"{Name}\", {DataType.TypeCodeExp})";
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
        
        public TypeDefinition(string name, bool dotNetNaming, List<Annotation> annotations = null) : base(name, dotNetNaming, annotations)
        {
        }

        public override List<string> MappedAttributes
        {
            get
            {
                var ret = base.MappedAttributes;
                ret.AddRange(ResolveDataType().MappedAttributes);
                return ret;
            }
        }

        /// <summary>
        /// Gets the undelying data type
        /// </summary>
        /// <returns>IDL type without alias</returns>
        public ITypeSymbol ResolveDataType()
        {
            var typeSymbol = DataType;
            if (typeSymbol is TypeDefinition typeDef)
            {                
                typeSymbol = typeDef.ResolveDataType();
            }
            return typeSymbol;
        }


        public void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            if (String.IsNullOrEmpty(varName))
            {
                sw.Write($"{FullHelperName}.Read({streamName})");
            }
            else
            {
                sw.WriteLine($"{indent}{varName} = {FullHelperName}.Read({streamName});");
            }

        }

        public void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{FullHelperName}.Write({streamName}, {varName});");
        }

        public void PrintExtract(string indent, TextWriter sw, string anyName, string varName = null, string type = null)
        {
            if (String.IsNullOrEmpty(varName))
            {
                sw.Write($"{FullHelperName}.Extract({anyName})");
            }
            else
            {
                sw.WriteLine($"{indent}{varName} = {FullHelperName}.Extract({anyName});");
            }

        }

        public void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{FullHelperName}.Insert({anyName}, {varName});");
        }

        public override void Include(DirectoryInfo currentDir, string indent = "", StreamWriter stream = null)
        {
            if (!IsIncluded)
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
                    GenerateHelper(currentDir);
                }
                else
                {                 
                    PrintHelper(currentDir, indent, stream);
                }
            }            
        }

        private void PrintTypeCodeExp(string indent, StreamWriter stream)
        {
            stream.WriteLine($"{indent}type = CORBA.ORB.Init().CreateAliasTc({FullHelperName}.Id(), \"{Name}\", {DataType.TypeCodeExp});");
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
            stream.WriteLine($"{indent}\tpublic static void Insert(CORBA.Any any, {DataType.MappedType} value)");
            stream.WriteLine($"{indent}\t{{");
            //DataType.PrintInsert($"{indent}\t\t", stream, "any", "value");
            stream.WriteLine($"{indent}\t\tany.Type = Type();");
            stream.WriteLine($"{indent}\t\tWrite(any.CreateOutputStream(), value);");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static {DataType.MappedType} Extract(CORBA.Any any)");
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
            stream.WriteLine($"{indent}\tpublic static {DataType.MappedType} Read(CORBA.IInputStream inputStream)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\t{DataType.MappedType} result;");
            DataType.PrintRead($"{indent}\t\t", stream, "inputStream", "result", 0);
            stream.WriteLine($"{indent}\t\treturn result;");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static void Write(CORBA.IOutputStream outputStream, {DataType.MappedType} value)");
            stream.WriteLine($"{indent}\t{{");
            DataType.PrintWrite($"{indent}\t\t", stream, "outputStream", "value", 0);
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}}}");
        }
    }
}
