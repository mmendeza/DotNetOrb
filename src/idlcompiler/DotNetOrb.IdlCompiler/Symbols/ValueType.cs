// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class ValueType: IDLSymbol, IScopeSymbol, ITypeSymbol, IFwdDeclSymbol
    {
        public Scope NamingScope { get; set; }
        public bool IsBoxed { get; set; }
        public bool IsCustom { get; set; }
        public bool IsAbstract { get; set; }
        public ValueType Truncatable { get; set; }
        public bool IsForwardDeclaration { get; set; }
        public List<ValueType> Inherits { get; set; }
        public List<Interface> Supports { get; set; }
        public ValueType Base { get; set; }
        public ITypeSymbol BoxedValue { get; set; }

        public int TCKind
        {
            get
            {               
                return 29;
            }
        }

        public string IDLType => "valuetype";

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

        public override string MappedName
        {
            get
            {
                if (IsAbstract)
                {
                    return GetMappedName("I");
                }
                return GetMappedName();
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

        public ValueType(string name, List<Annotation> annotations = null) : base(name, annotations)
        {
            Inherits = new List<ValueType>();
            Supports = new List<Interface>();            
        }

        public void Define(IFwdDeclSymbol symbol)
        {
            if (symbol is ValueType v)
            {
                IsForwardDeclaration = false;
                IsCustom= v.IsCustom;
                IsAbstract = v.IsAbstract;                
                IsBoxed = v.IsBoxed;
                Inherits = v.Inherits;
                Supports = v.Supports;
                Truncatable = v.Truncatable;
                Annotations = v.Annotations;
                Base = v.Base;
            }
            else
            {
                throw new ArgumentException("Symbol must be a value type definition");
            }
        }

        public List<ValueType> GetTruncatableValues()
        {
            var res = new List<ValueType>();

            if (Truncatable != null)
            {
                res.Add(Truncatable);
                res.AddRange(Truncatable.GetTruncatableValues());
            }
            return res;
        }

        public List<String> GetTruncatableIds()
        {
            var res = new List<String>();
            res.Add(RepositoryId);
            var tvs = GetTruncatableValues();
            foreach ( var t in tvs )
            {
                res.Add(t.RepositoryId);
            }            
            return res;
        }

        public List<Interface> GetInheritanceSupports()
        {
            var res = new List<Interface>();

            foreach (var vt in Inherits)
            {
                foreach (var i in vt.Supports)
                {
                    if (!i.IsAbstract)
                    {
                        res.Add(i);
                    }                    
                }
            }

            return res;
        }

        public void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {HelperName}.Read({streamName});");
        }

        public void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{HelperName}.Write({streamName}, {varName});");
        }

        public void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            sw.WriteLine($"{indent}{varName} = {HelperName}.Extract({anyName});");
        }

        public void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.Insert(({MappedType}) {varName});");
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
                GenerateInterface(currentDir);
                GenerateClass(currentDir);
                GenerateHelper(currentDir);
                GenerateFactory(currentDir);
            }
        }

        private void GenerateInterface(DirectoryInfo currentDir)
        {
            if (IsAbstract)
            {                
                var file = new FileInfo($"{currentDir.FullName}\\{MappedName}.cs");
                using (var stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    stream.WriteLine("");
                    var indent = "";
                    if (!String.IsNullOrEmpty(Namespace))
                    {
                        indent = "\t";
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }
                    stream.WriteLine($"{indent}[RepositoryID(\"{RepositoryId}\")]");
                    stream.WriteLine($"{indent}[Helper(typeof({HelperName}))]");
                    stream.Write($"{indent}public interface {MappedName} : CORBA.IValueBase");
                    foreach (var vt in Inherits)
                    {
                        stream.Write($", {vt.MappedType}");
                    }
                    stream.WriteLine();
                    stream.WriteLine($"{indent}{{");
                    PrintTypes(currentDir, $"{indent}\t", stream);
                    PrintOperations($"{indent}\t", stream, false, new HashSet<string>());
                    stream.WriteLine($"{indent}}}");
                    if (!String.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine("}");
                    }
                    stream.WriteLine();
                }
            }            
        }
                
        private void GenerateClass(DirectoryInfo currentDir)
        {
            if (IsBoxed)
            {
                //var symbol = NamingScope.Symbols.Values.OfType<IIDLSymbol>().FirstOrDefault();
                if (BoxedValue != null && BoxedValue is IBasicType type)
                {
                    var file = new FileInfo($"{currentDir.FullName}\\{MappedName}.cs");
                    using (var stream = file.CreateText())
                    {
                        type.PrintComment(stream);
                        type.PrintUsings(stream);
                        stream.WriteLine("");
                        var indent = "";
                        if (!String.IsNullOrEmpty(Namespace))
                        {
                            indent = "\t";
                            stream.WriteLine($"namespace {Namespace}");
                            stream.WriteLine("{");
                        }
                        stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
                        stream.WriteLine($"{indent}[RepositoryID(\"{RepositoryId}\")]");
                        stream.WriteLine($"{indent}[Helper(typeof({HelperName}))]");
                        stream.WriteLine($"{indent}public partial class {MappedName} : CORBA.IValueBase");
                        stream.WriteLine($"{indent}{{");
                        stream.WriteLine($"{indent}\tpublic {type.MappedType} Value {{ get; set; }}");
                        var truncatableIds = GetTruncatableIds();
                        stream.WriteLine($"{indent}\tpublic string[] _TruncatableIds {{ get => new[] {{{String.Join(", ", truncatableIds.Select(s => String.Format("\"{0}\"", s)))}}}; }}");
                        stream.WriteLine();
                        stream.WriteLine($"{indent}\tpublic {MappedName}({type.MappedType} value)");
                        stream.WriteLine($"{indent}\t{{");
                        stream.WriteLine($"{indent}\t\tValue = value;");
                        stream.WriteLine($"{indent}\t}}");
                        stream.WriteLine($"{indent}}}");
                        if (!String.IsNullOrEmpty(Namespace))
                        {
                            stream.WriteLine("}");
                        }
                        stream.WriteLine();
                    }
                }
            }
            else if (!IsAbstract)
            {
                var file = new FileInfo($"{currentDir.FullName}\\{MappedName}.cs");
                using (var stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    stream.WriteLine("");
                    var indent = "";
                    if (!String.IsNullOrEmpty(Namespace))
                    {
                        indent = "\t";
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }
                    var parents = new List<String>();
                    if (Base != null)
                    {
                        parents.Add(Base.MappedType);
                    }
                    else
                    {
                        parents.Add("CORBA.Object");
                    }
                    foreach (var vt in Inherits)
                    {                        
                        parents.Add(vt.MappedType);                        
                    }
                    foreach (var ifz in Supports)
                    {
                        parents.Add(ifz.MappedType);
                    }
                    if (IsCustom)
                    {
                        parents.Add("CORBA.ICustomValue");
                    }
                    else
                    {
                        parents.Add("CORBA.IStreamableValue");
                    }
                    stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
                    var truncatableValues = GetTruncatableValues();
                    foreach (var tv in truncatableValues)
                    {
                        stream.WriteLine($"{indent}[Truncatable(typeof({tv.MappedType}))]");
                    }
                    stream.WriteLine($"{indent}[RepositoryID(\"{RepositoryId}\")]");
                    stream.WriteLine($"{indent}[Helper(typeof({HelperName}))]");
                    stream.WriteLine($"{indent}public abstract class {MappedName} : {String.Join(", ", parents)}");
                    stream.WriteLine($"{indent}{{");
                    var truncatableIds = GetTruncatableIds();                    
                    stream.WriteLine($"{indent}\tpublic {(Base != null ? "override" : "virtual")} string[] _TruncatableIds {{ get => new[] {{{String.Join(", ", truncatableIds.Select(s => String.Format("\"{0}\"", s)))}}}; }} ");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic {(Base != null ? "override" : "virtual")} CORBA.TypeCode _Type {{ get => {FullHelperName}.Type(); }}");
                    stream.WriteLine();
                    PrintTypes(currentDir, $"{indent}\t", stream);
                    stream.WriteLine();
                    PrintMembers($"{indent}\t", stream);
                    stream.WriteLine();
                    PrintOperations($"{indent}\t", stream, true, new HashSet<string>(), "abstract");
                    stream.WriteLine();
                    if (!IsCustom)
                    {
                        stream.WriteLine($"{indent}\tpublic {(Base != null ? "override" : "virtual")} void _Write(CORBA.IOutputStream outputStream)");
                        stream.WriteLine($"{indent}\t{{");
                        if (Base != null)
                        {
                            stream.WriteLine($"{indent}\t\tbase._Write(outputStream);");
                        }
                        PrintWriteMembers($"{indent}\t\t", stream, "outputStream");
                        stream.WriteLine($"{indent}\t}}");
                        stream.WriteLine();
                        stream.WriteLine($"{indent}\tpublic {(Base != null ? "override" : "virtual")} void _Read(CORBA.IInputStream inputStream)");
                        stream.WriteLine($"{indent}\t{{");
                        if (Base != null)
                        {
                            stream.WriteLine($"{indent}\t\tbase._Read(inputStream);");
                        }
                        PrintReadMembers($"{indent}\t\t", stream, "inputStream");
                        stream.WriteLine($"{indent}\t}}");
                    }
                    stream.WriteLine($"{indent}}}");
                    if (!String.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine("}");
                    }
                    stream.WriteLine();
                }
            }            
        }

        private void GenerateFactory(DirectoryInfo currentDir)
        {
            if (!IsAbstract)
            {
                var factoryName = GetMappedName("I", "ValueFactory");
                var file = new FileInfo($"{currentDir.FullName}\\{factoryName}.cs");
                using (var stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    stream.WriteLine("");
                    var indent = "";
                    if (!String.IsNullOrEmpty(Namespace))
                    {
                        indent = "\t";
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }
                    stream.WriteLine($"{indent}public interface {factoryName} : CORBA.IValueFactory");
                    stream.WriteLine($"{indent}{{");                    
                    PrintInitializers($"{indent}\t", stream);
                    stream.WriteLine($"{indent}}}");
                    if (!String.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine("}");
                    }
                    stream.WriteLine();
                }
            }
        }

        public void PrintInitializers(string indent, StreamWriter stream)
        {            
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is Initializer ini)
                {
                    ini.PrintInitializer(indent, stream);                    
                }
            }
        }

        public void PrintTypes(DirectoryInfo currentDir, string indent, StreamWriter stream)
        {
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                symbol.Include(currentDir, indent, stream);
            }
        }

        public void PrintOperations(string indent, StreamWriter stream, bool printAncestors, HashSet<string> writtenOperations, string modifier = null)
        {
            if (printAncestors)
            {
                foreach (var ancestor in Inherits)
                {
                    ancestor.PrintOperations(indent, stream, printAncestors, writtenOperations, modifier);
                }
                foreach (var ancestor in Supports)
                {
                    ancestor.PrintOperations(indent, stream, printAncestors, writtenOperations, modifier);
                }
            }            
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is Operation op && !writtenOperations.Contains(symbol.Name))
                {
                    writtenOperations.Add(symbol.Name);
                    op.PrintOperation(indent, stream, modifier);                    
                }
                else if (symbol is AttributeType att && !writtenOperations.Contains(symbol.Name))
                {
                    writtenOperations.Add(symbol.Name);
                    att.PrintAttribute(indent, stream, modifier);
                }
            }
        }

        private void PrintMembers(string indent, TextWriter sw)
        {
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is StateMember m)
                {
                    sw.WriteLine($"{indent}[IdlName(\"{m.Name}\")]");
                    foreach (var att in m.MappedAttributes)
                    {
                        sw.WriteLine($"{indent}{att}");
                    }
                    foreach (var att in m.DataType.MappedAttributes)
                    {
                        sw.WriteLine($"{indent}{att}");
                    }
                    sw.WriteLine($"{indent}{(m.IsPublic ? "public" : "protected")} {m.DataType.MappedType} {m.MappedName} {{ get; set; }}");
                }
            }
        }

        private void PrintReadMembers(string indent, TextWriter sw, string streamName)
        {
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is StateMember m)
                {
                    m.DataType.PrintRead(indent, sw, streamName, m.GetMappedName(), 0);
                }
            }
        }

        private void PrintWriteMembers(string indent, TextWriter sw, string streamName)
        {            
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is StateMember m)
                {
                    m.DataType.PrintWrite(indent, sw, streamName, m.GetMappedName(), 0);
                }
            }
        }

        private void PrintTypeCodeExp(string indent, StreamWriter stream)
        {                        
            if (IsBoxed && BoxedValue != null)
            {
                //var boxedType = NamingScope.Symbols.Values.OfType<IIDLSymbol>().FirstOrDefault() as ITypeSymbol;
                stream.WriteLine($"{indent}type = CORBA.ORB.Init().CreateValueBoxTc(\"{RepositoryId}\", \"{(Compiler.TypeCodeFullNamespace ? FullName : Name)}\", {BoxedValue.TypeCodeExp});");
            }
            else
            {
                var typeModifier = 0;
                if (IsCustom)
                {
                    typeModifier = 1;
                }
                var concreteBase = "null";
                if (Base != null)
                {
                    concreteBase = $"{Base.FullHelperName}.Type()";
                }
                stream.Write($"{indent}type = CORBA.ORB.Init().CreateValueTc({FullHelperName}.Id(), \"{(Compiler.TypeCodeFullNamespace ? FullName : Name)}\", {typeModifier}, {concreteBase}, new CORBA.ValueMember[] {{");                
                foreach (IDLSymbol child in NamingScope.Symbols.Values)
                {
                    if (child is StateMember m)
                    {
                        stream.Write($"new CORBA.ValueMember(\"{m.Name}\", \"{m.RepositoryId}\", \"{(Compiler.TypeCodeFullNamespace ? FullName : Name)}\", \"1.0\", {m.DataType.TypeCodeExp}, null, {(m.IsPublic ? 1 : 0)}), ");
                    }
                }
                stream.WriteLine($"}});");
            }
        }

        public void GenerateHelper(DirectoryInfo currentDir)
        {
            var className = HelperName;            
            //Interface
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
                stream.WriteLine($"{indent}\t\tany.InsertValue(value);");
                stream.WriteLine($"{indent}\t}}");
                stream.WriteLine();
                stream.WriteLine($"{indent}\tpublic static {MappedType} Extract(CORBA.Any any)");
                stream.WriteLine($"{indent}\t{{");
                stream.WriteLine($"{indent}\t\ttry");
                stream.WriteLine($"{indent}\t\t{{");
                stream.WriteLine($"{indent}\t\t\treturn ({MappedType}) any.ExtractValue();");
                stream.WriteLine($"{indent}\t\t}}");
                stream.WriteLine($"{indent}\t\tcatch (InvalidCastException ex)");
                stream.WriteLine($"{indent}\t\t{{");
                stream.WriteLine($"{indent}\t\t\tthrow new CORBA.Marshal(ex.Message);");
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
                stream.WriteLine($"{indent}\t\treturn ({MappedType}) inputStream.ReadValue(\"{RepositoryId}\");");
                stream.WriteLine($"{indent}\t}}");
                stream.WriteLine();
                stream.WriteLine($"{indent}\tpublic static void Write(CORBA.IOutputStream outputStream, {MappedType} value)");
                stream.WriteLine($"{indent}\t{{");
                stream.WriteLine($"{indent}\t\toutputStream.WriteValue(value, \"{RepositoryId}\");");
                stream.WriteLine($"{indent}\t}}");
                stream.WriteLine();
                foreach (IDLSymbol s in NamingScope.Symbols.Values)
                {
                    if (s is Initializer ini)
                    {
                        ini.PrintHelper($"{indent}\t", stream);
                        stream.WriteLine();
                    }
                }
                stream.WriteLine($"{indent}}}");
                if (!String.IsNullOrEmpty(Namespace))
                {
                    stream.WriteLine($"}}");
                }
            }
        }
    }
}
