// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Union: IDLSymbol, IScopeSymbol, ITypeSymbol, IFwdDeclSymbol
    { 
        public Scope NamingScope { get; set; }
        public bool IsForwardDeclaration { get; set; }
        public ITypeSymbol Discriminator { get; set; }        

        public int TCKind
        {
            get
            {
                return 16;
            }
        }

        public string IDLType
        {
            get
            {
                return "union";
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
        
        public Union(string name, bool dotNetNaming, List<Annotation> annotations = null) : base(name, dotNetNaming, annotations)
        {
        }


        public void Define(IFwdDeclSymbol symbol)
        {
            if (symbol is Union u)
            {
                IsForwardDeclaration = false;
                Discriminator = u.Discriminator;
                Annotations = u.Annotations;
            }
            else
            {
                throw new ArgumentException("Symbol must be an union definition");
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

        private void PrintReadCaseStatements(string indent, TextWriter sw, string streamName, string varName)
        {            
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is CaseStatement c)
                {
                    foreach (var lbl in c.CaseLabels)
                    {
                        if (lbl is Literal literal)
                        {
                            sw.WriteLine($"{indent}case {literal.ToString()}:");
                        }
                        else if (lbl is Enumerator enumerator)
                        {
                            sw.WriteLine($"{indent}case {enumerator.MappedType}:");
                        }
                        
                    }
                    if (c.IsDefault)
                    {
                        sw.WriteLine($"{indent}default:");
                    }
                    sw.WriteLine($"{indent}{{");
                    c.DataType.PrintRead($"{indent}\t", sw, streamName, $"{varName}.{c.MappedName}", 0);
                    sw.WriteLine($"{indent}\tbreak;");
                    sw.WriteLine($"{indent}}}");
                }               
            }
        }

        private void PrintWriteCaseStatements(string indent, TextWriter sw, string streamName, string varName)
        {
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is CaseStatement c)
                {
                    foreach (var lbl in c.CaseLabels)
                    {
                        if (lbl is Literal literal)
                        {
                            sw.WriteLine($"{indent}case {literal.ToString()}:");
                        }
                        else if (lbl is Enumerator enumerator)
                        {
                            sw.WriteLine($"{indent}case {enumerator.MappedType}:");
                        }
                    }
                    if (c.IsDefault)
                    {
                        sw.WriteLine($"{indent}default:");
                    }
                    sw.WriteLine($"{indent}{{");
                    c.DataType.PrintWrite($"{indent}\t", sw, streamName, $"{varName}.{c.MappedName}", 0);
                    sw.WriteLine($"{indent}\tbreak;");
                    sw.WriteLine($"{indent}}}");
                }
            }
        }

        private void PrintTypeCodeExp(string indent, StreamWriter stream)
        {            
            stream.WriteLine($"{indent}var members = new List<CORBA.UnionMember>();");
            stream.WriteLine($"{indent}CORBA.Any anyLabel;");
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is CaseStatement c)
                {                    
                    foreach (var lbl in c.CaseLabels)
                    {
                        stream.WriteLine($"{indent}anyLabel = CORBA.ORB.Init().CreateAny();");
                        if (lbl is Literal literal)
                        {
                            Discriminator.PrintInsert($"{indent}", stream, "anyLabel", literal.GetValue().ToString());
                        }
                        else if (lbl is Enumerator enumerator)
                        {
                            Discriminator.PrintInsert($"{indent}", stream, "anyLabel", enumerator.MappedType);
                        }
                        
                        stream.WriteLine($"{indent}members.Add(new CORBA.UnionMember(\"{c.Name}\", anyLabel, {c.DataType.TypeCodeExp}, null));");
                        stream.WriteLine($"{indent}type = CORBA.ORB.Init().CreateUnionTc({FullHelperName}.Id(), \"{Name}\", {Discriminator.TypeCodeExp}, members.ToArray());");
                    }
                }
            }
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
            stream.WriteLine($"{indent}\t    return \"{RepositoryId}\";");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static {MappedType} Read(CORBA.IInputStream inputStream)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tvar result = new {MappedType}();");
            stream.WriteLine($"{indent}\t\t{Discriminator.MappedType} disc;");
            Discriminator.PrintRead($"{indent}\t\t", stream, "inputStream", "disc", 0);
            stream.WriteLine($"{indent}\t\tswitch (disc)");
            stream.WriteLine($"{indent}\t\t{{");
            PrintReadCaseStatements($"{indent}\t\t\t", stream, "inputStream", "result");
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t\treturn result;");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic static void Write(CORBA.IOutputStream outputStream, {MappedType} value)");
            stream.WriteLine($"{indent}\t{{");
            Discriminator.PrintWrite($"{indent}\t\t", stream, "outputStream", "value._Discriminator", 0);
            stream.WriteLine($"{indent}\t\tswitch (value._Discriminator)");
            stream.WriteLine($"{indent}\t\t{{");
            PrintWriteCaseStatements($"{indent}\t\t\t", stream, "outputStream", "value");
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}}}");
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
            stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
            stream.WriteLine($"{indent}[RepositoryID(\"{RepositoryId}\")]");
            stream.WriteLine($"{indent}[Helper(typeof({HelperName}))]");
            stream.WriteLine($"{indent}public partial class {MappedName}: CORBA.IIDLEntity, IEquatable<{MappedName}>");
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
            stream.WriteLine($"{indent}\tpublic {MappedName}({MappedName} other)");            
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\t_Discriminator = other._Discriminator;");
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is CaseStatement cs)
                {
                    stream.WriteLine($"{indent}\t\t{cs.MappedName} = other.{cs.MappedName};");
                }
            }
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            //Equals
            stream.WriteLine($"{indent}\tpublic bool Equals({MappedName}? other)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tif (other == null) return false;");
            stream.WriteLine($"{indent}\t\tif (_Discriminator != other._Discriminator) return false;");
            stream.WriteLine($"{indent}\t\tswitch (_Discriminator)");
            stream.WriteLine($"{indent}\t\t{{");
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {                
                if (symbol is CaseStatement cs)
                {
                    foreach (var lbl in cs.CaseLabels)
                    {
                        if (lbl is Literal literal)
                        {
                            stream.WriteLine($"{indent}\t\t\tcase {literal.ToString()}:");
                        }
                        else if (lbl is Enumerator enumerator)
                        {
                            stream.WriteLine($"{indent}\t\t\tcase {enumerator.MappedType}:");
                        }                        
                    }
                    if (cs.IsDefault)
                    {
                        stream.WriteLine($"{indent}\t\t\tdefault:");
                    }
                    var dataType = cs.DataType;
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
                        stream.WriteLine($"{indent}\t\t\t\tif (!{cs.MappedName}.SequenceEqual(other.{cs.MappedName})) return false;");
                    }
                    else if (dataType is Map)
                    {
                        stream.WriteLine($"{indent}\t\t\t\tif (!{cs.MappedName}.OrderBy(kv => kv.Key).SequenceEqual(other.{cs.MappedName}.OrderBy(kv => kv.Key))) return false;");
                    }
                    else
                    {
                        stream.WriteLine($"{indent}\t\t\t\tif (!{cs.MappedName}.Equals(other.{cs.MappedName})) return false;");
                    }
                    stream.WriteLine($"{indent}\t\t\t\tbreak;");
                }
            }
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t\treturn true;");
            stream.WriteLine($"{indent}\t}}");
            stream.WriteLine();
            stream.WriteLine($"{indent}\tpublic {Discriminator.MappedType} _Discriminator {{ get; private set; }}");
            stream.WriteLine();            
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is CaseStatement cs)
                {
                    var fieldName = "_" + Utils.ToCamelCase(cs.MappedName);
                    stream.WriteLine($"{indent}\tprivate {cs.DataType.MappedType} {fieldName};");
                    stream.WriteLine($"{indent}\tpublic {cs.DataType.MappedType} {cs.MappedName}");
                    stream.WriteLine($"{indent}\t{{");
                    //get
                    stream.WriteLine($"{indent}\t\tget");
                    stream.WriteLine($"{indent}\t\t{{");
                    stream.Write($"{indent}\t\t\tif (");
                    for (int i = 0; i < cs.CaseLabels.Count; i++)
                    {
                        if (i > 0)
                        {
                            stream.Write($" || ");
                        }
                        var caseLabel = cs.CaseLabels[i];
                        if (caseLabel is Literal literal)
                        {
                            stream.Write($"_Discriminator != {literal.ToString()}");
                        }
                        else if (caseLabel is Enumerator enumerator)
                        {
                            stream.Write($"_Discriminator != {enumerator.MappedType}");
                        }
                    }
                    if (cs.IsDefault)
                    {
                        if (cs.CaseLabels.Count > 0)
                        {
                            stream.Write($" || ");
                        }
                        stream.Write($"_Discriminator != ({Discriminator.MappedType}) 0");
                    }
                    stream.WriteLine($") throw new System.InvalidOperationException();");
                    stream.WriteLine($"{indent}\t\t\treturn {fieldName};");
                    stream.WriteLine($"{indent}\t\t}}");
                    //set
                    stream.WriteLine($"{indent}\t\tset");
                    stream.WriteLine($"{indent}\t\t{{");               
                    if (cs.CaseLabels.Count > 0)
                    {
                        var first = cs.CaseLabels.First();
                        if (first is Literal literal)
                        {
                            stream.WriteLine($"{indent}\t\t\t_Discriminator = {literal.ToString()};");
                        }
                        else if (first is Enumerator enumerator)
                        {
                            stream.WriteLine($"{indent}\t\t\t_Discriminator = {enumerator.MappedType};");
                        }                        
                    }
                    else if (cs.IsDefault)
                    {
                        stream.WriteLine($"{indent}\t\t\t_Discriminator = ({Discriminator.MappedType}) 0;");
                    }
                    stream.WriteLine($"{indent}\t\t\t{fieldName} = value;");                    
                    stream.WriteLine($"{indent}\t\t}}");
                    stream.WriteLine($"{indent}\t}}");
                }
            }
            stream.WriteLine($"{indent}}}");
            stream.WriteLine();
        }
    }
}
