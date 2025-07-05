// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Interface : IDLSymbol, IScopeSymbol, ITypeSymbol, IFwdDeclSymbol
    {
        public Scope NamingScope { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsLocal { get; set; }
        public bool IsForwardDeclaration { get; set; }
        public List<Interface> Inherits { get; set; }

        public Interface(string name, List<Annotation> annotations = null) : base(name, annotations)
        {
            Inherits = new List<Interface>();
        }

        public void Define(IFwdDeclSymbol symbol)
        {
            if (symbol is Interface ifz)
            {
                IsForwardDeclaration = false;
                IsAbstract = ifz.IsAbstract;
                IsLocal = ifz.IsLocal;
                Inherits = ifz.Inherits;
                Annotations = ifz.Annotations;
            }
            else
            {
                throw new ArgumentException("Symbol must be an interface definition");
            }
        }

        public override string MappedName
        {
            get
            {
                return GetMappedName("I");
            }
        }

        public bool DerivesFrom(Interface ifz)
        {
            foreach (var ancestorIfz in Inherits)
            {
                if (ancestorIfz == ifz)
                {
                    return true;
                }
                if (ancestorIfz.DerivesFrom(ifz))
                {
                    return true;
                }
            }
            return false;
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
            sw.WriteLine($"{indent}{anyName}.Insert(({MappedType}) {varName});");
        }

        public int TCKind
        {
            get
            {
                if (IsAbstract)
                {
                    return 32;
                }
                if (IsLocal)
                {
                    return 33;
                }
                return 14;
            }
        }

        public string IDLType => "interface";

        public string MappedType
        {
            get
            {
                if (string.IsNullOrEmpty(Namespace))
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
                GenerateInterfaceOperations(currentDir);
                GenerateClass(currentDir);
                GenerateHelper(currentDir);
                GenerateStub(currentDir);
                GeneratePOA(currentDir);
                GeneratePOATie(currentDir);
                GenerateLocalTie(currentDir);
            }            
        }

        private void GenerateInterface(DirectoryInfo currentDir)
        {
            var ifzOperationsName = GetMappedName("I", "Operations");
            var stubName = IsLocal ? GetMappedName("_", "LocalBase") : GetMappedName("_", "Stub");            
            //Interface
            var file = new FileInfo($"{currentDir.FullName}\\{MappedName}.cs");
            using (var stream = file.CreateText())
            {
                PrintComment(stream);
                PrintUsings(stream);
                stream.WriteLine("");
                var indent = "";
                if (!string.IsNullOrEmpty(Namespace))
                {
                    indent = "\t";
                    stream.WriteLine($"namespace {Namespace}");
                    stream.WriteLine("{");
                }
                stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
                stream.WriteLine($"{indent}[RepositoryID(\"{RepositoryId}\")]");
                stream.WriteLine($"{indent}[Helper(typeof({HelperName}))]");
                stream.WriteLine($"{indent}[Stub(typeof({stubName}))]");
                foreach (var att in MappedAttributes)
                {
                    stream.WriteLine($"{indent}\t{att}");
                }
                stream.Write($"{indent}public interface {MappedName} : CORBA.IIDLEntity, {(IsLocal ? "CORBA.ILocalInterface" : "CORBA.IObject")}");
                if (!IsAbstract)
                {
                    stream.Write($", {ifzOperationsName}");
                }
                foreach (var ancestor in Inherits)
                {
                    stream.Write(", ");
                    stream.Write($"{ancestor.MappedType}");
                }
                stream.WriteLine();
                stream.WriteLine($"{indent}{{");
                if (IsAbstract)
                {
                    PrintOperations($"{indent}\t", stream, false, new HashSet<string>());
                }
                stream.WriteLine($"{indent}}}");
                if (!string.IsNullOrEmpty(Namespace))
                {
                    stream.WriteLine("}");
                }
                stream.WriteLine();
            }
        }

        private void GenerateClass(DirectoryInfo currentDir)
        {
            bool hasImpl = false;
            foreach (IDLSymbol s in NamingScope.Symbols.Values)
            {
                if (s is Constant || s is ITypeSymbol)
                {
                    hasImpl = true;
                    break;
                }
            }
            if (hasImpl)
            {
                var className = GetMappedName();
                var file = new FileInfo($"{currentDir.FullName}\\{className}.cs");
                using (var stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    stream.WriteLine("");
                    var indent = "";
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        indent = "\t";
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }
                    stream.WriteLine($"{indent}public abstract partial class {className} : CORBA.Object, {MappedType}");
                    stream.WriteLine($"{indent}{{");
                    PrintTypes(currentDir, $"{indent}\t", stream);
                    PrintOperations($"{indent}\t", stream, true, new HashSet<string>(), "abstract");
                    stream.WriteLine($"{indent}}}");
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine("}");
                    }
                    stream.WriteLine();
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

        public void PrintPoaOperations(string indent, StreamWriter stream, bool printAncestors, HashSet<string> writtenOperations, string modifier = null)
        {
            if (printAncestors)
            {
                foreach (var ancestor in Inherits)
                {
                    ancestor.PrintPoaOperations(indent, stream, printAncestors, writtenOperations, modifier);
                }
            }
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is Operation op && !writtenOperations.Contains(symbol.Name))
                {
                    writtenOperations.Add(symbol.Name);
                    op.PrintPOAOperation(indent, stream, modifier);
                }
                else if (symbol is AttributeType att && !writtenOperations.Contains(symbol.Name))
                {
                    writtenOperations.Add(symbol.Name);
                    att.PrintAttribute(indent, stream, modifier);
                }
            }
        }

        public void GenerateInterfaceOperations(DirectoryInfo currentDir)
        {
            if (!IsAbstract)
            {
                var ifzOperationsName = GetMappedName("I", "Operations");
                var file = new FileInfo($"{currentDir.FullName}\\{ifzOperationsName}.cs");
                using (var stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    stream.WriteLine();
                    var indent = "";
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        indent = "\t";
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }
                    stream.Write($"{indent}public interface {ifzOperationsName}");
                    if (Inherits.Count > 0)
                    {
                        stream.Write(" : ");
                        int index = 0;
                        foreach (var ancestor in Inherits)
                        {
                            if (index > 0)
                            {
                                stream.Write(", ");
                            }
                            if (ancestor.IsAbstract)
                            {
                                stream.Write($"{ancestor.MappedType}");
                            }
                            else
                            {
                                var opsName = ancestor.GetMappedName("I", "Operations");
                                if (!string.IsNullOrEmpty(ancestor.Namespace))
                                {
                                    opsName = ancestor.Namespace + "." + opsName;
                                }                                
                                stream.Write($"{opsName}");
                            }
                            index++;
                        }
                    }
                    stream.WriteLine();
                    stream.WriteLine($"{indent}{{");
                    PrintOperations($"{indent}\t", stream, false, new HashSet<string>());
                    stream.WriteLine($"{indent}}}");
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine("}");
                    }
                    stream.WriteLine();
                }
            }
        }

        private void PrintTypeCodeExp(string indent, StreamWriter stream)
        {
            if (IsAbstract)
            {
                stream.WriteLine($"{indent}type = CORBA.ORB.Init().CreateAbstractInterfaceTc(\"{RepositoryId}\", \"{(Compiler.TypeCodeFullNamespace ? FullName : Name)}\");");
            }
            else if (IsLocal)
            {
                stream.WriteLine($"{indent}type = CORBA.ORB.Init().CreateLocalInterfaceTc(\"{RepositoryId}\", \"{(Compiler.TypeCodeFullNamespace ? FullName : Name)}\");");
            }
            else
            {
                stream.WriteLine($"{indent}type = CORBA.ORB.Init().CreateInterfaceTc(\"{RepositoryId}\", \"{(Compiler.TypeCodeFullNamespace ? FullName : Name)}\");");
            }
        }

        public void GenerateHelper(DirectoryInfo currentDir)
        {
            var className = HelperName;
            var stubName = GetMappedName("_", "Stub");
            //Interface
            var file = new FileInfo($"{currentDir.FullName}\\{className}.cs");
            using (StreamWriter stream = file.CreateText())
            {
                PrintComment(stream);
                PrintUsings(stream);
                stream.WriteLine();
                var indent = "";
                if (!string.IsNullOrEmpty(Namespace))
                {
                    indent = "\t";
                    stream.WriteLine($"namespace {Namespace}");
                    stream.WriteLine("{");
                }
                stream.WriteLine($"{indent}\tpublic static class {className}");
                stream.WriteLine($"{indent}\t{{");
                stream.WriteLine($"{indent}\t\tprivate static volatile CORBA.TypeCode type;");
                stream.WriteLine();
                stream.WriteLine($"{indent}\t\tpublic static CORBA.TypeCode Type()");
                stream.WriteLine($"{indent}\t\t{{");
                stream.WriteLine($"{indent}\t\t\tif (type == null)");
                stream.WriteLine($"{indent}\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\tlock (typeof({className}))");
                stream.WriteLine($"{indent}\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\tif (type == null)");
                stream.WriteLine($"{indent}\t\t\t\t\t{{");
                PrintTypeCodeExp($"{indent}\t\t\t\t\t\t", stream);
                stream.WriteLine($"{indent}\t\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\treturn type;");
                stream.WriteLine($"{indent}\t\t}}");
                stream.WriteLine();
                stream.WriteLine($"{indent}\t\tpublic static void Insert(CORBA.Any any, {MappedType} ifz)");
                stream.WriteLine($"{indent}\t\t{{");
                if (IsAbstract)
                {
                    stream.WriteLine($"{indent}\t\t\tif (ifz is CORBA.IObject obj)");
                    stream.WriteLine($"{indent}\t\t\t{{");
                    stream.WriteLine($"{indent}\t\t\t\tany.InsertObject(ifz);");
                    stream.WriteLine($"{indent}\t\t\t}}");
                    stream.WriteLine($"{indent}\t\t\telse");
                    stream.WriteLine($"{indent}\t\t\t{{");
                    stream.WriteLine($"{indent}\t\t\t\tany.InsertValue(ifz);");
                    stream.WriteLine($"{indent}\t\t\t}}");
                }
                else
                {
                    stream.WriteLine($"{indent}\t\t\tany.InsertObject(ifz);");
                }
                stream.WriteLine($"{indent}\t\t}}");
                stream.WriteLine();
                stream.WriteLine($"{indent}\t\tpublic static {MappedType} Extract(CORBA.Any any)");
                stream.WriteLine($"{indent}\t\t{{");
                if (IsAbstract)
                {

                    stream.WriteLine($"{indent}\t\t\ttry");
                    stream.WriteLine($"{indent}\t\t\t{{");
                    stream.WriteLine($"{indent}\t\t\t\treturn Narrow(any.ExtractObject());");
                    stream.WriteLine($"{indent}\t\t\t}}");
                    stream.WriteLine($"{indent}\t\t\tcatch (CORBA.BadOperation ex)");
                    stream.WriteLine($"{indent}\t\t\t{{");
                    stream.WriteLine($"{indent}\t\t\t\ttry");
                    stream.WriteLine($"{indent}\t\t\t\t{{");
                    stream.WriteLine($"{indent}\t\t\t\t\treturn ({MappedType}) any.ExtractValue();");
                    stream.WriteLine($"{indent}\t\t\t\t}}");
                    stream.WriteLine($"{indent}\t\t\t\tcatch (InvalidCastException ex)");
                    stream.WriteLine($"{indent}\t\t\t\t{{");
                    stream.WriteLine($"{indent}\t\t\t\t\tthrow new CORBA.Marshal(ex.Message);");
                    stream.WriteLine($"{indent}\t\t\t\t}}");
                    stream.WriteLine($"{indent}\t\t\t}}");
                }
                else
                {
                    stream.WriteLine($"{indent}\t\t\treturn Narrow(any.ExtractObject());");
                }
                stream.WriteLine($"{indent}\t\t}}");
                stream.WriteLine();
                stream.WriteLine($"{indent}\t\tpublic static string Id()");
                stream.WriteLine($"{indent}\t\t{{");
                stream.WriteLine($"{indent}\t\t\treturn \"{RepositoryId}\";");
                stream.WriteLine($"{indent}\t\t}}");
                stream.WriteLine();
                stream.WriteLine($"{indent}\t\tpublic static {MappedType} Read(CORBA.IInputStream inputStream)");
                stream.WriteLine($"{indent}\t\t{{");
                if (IsLocal)
                {
                    stream.WriteLine($"{indent}\t\t\tthrow new CORBA.Marshal(\"Local interface\");");
                }
                else if (IsAbstract)
                {
                    stream.WriteLine($"{indent}\t\t\treturn Narrow(inputStream.ReadAbstractInterface());");
                }
                else
                {
                    stream.WriteLine($"{indent}\t\t\treturn Narrow(inputStream.ReadObject(typeof({stubName})));");
                }
                stream.WriteLine($"{indent}\t\t}}");
                stream.WriteLine();
                stream.WriteLine($"{indent}\t\tpublic static void Write(CORBA.IOutputStream outputStream, {MappedType} ifz)");
                stream.WriteLine($"{indent}\t\t{{");
                if (IsLocal)
                {
                    stream.WriteLine($"{indent}\t\t\tthrow new CORBA.Marshal(\"Local interface\");");
                }
                else if (IsAbstract)
                {
                    stream.WriteLine($"{indent}\t\t\toutputStream.WriteAbstractInterface(ifz);");
                }
                else
                {
                    stream.WriteLine($"{indent}\t\t\toutputStream.WriteObject(ifz);");
                }
                stream.WriteLine($"{indent}\t\t}}");
                stream.WriteLine();
                stream.WriteLine($"{indent}\t\tpublic static {MappedType} Narrow({(IsAbstract ? "object" : "CORBA.IObject")} obj)");
                stream.WriteLine($"{indent}\t\t{{");
                stream.WriteLine($"{indent}\t\t\tif (obj == null)");
                stream.WriteLine($"{indent}\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\treturn null;");
                stream.WriteLine($"{indent}\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\telse if (obj is {MappedType})");
                stream.WriteLine($"{indent}\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\treturn ({MappedType})obj;");
                stream.WriteLine($"{indent}\t\t\t}}");
                if (!IsLocal)
                {
                    if (IsAbstract)
                    {
                        stream.WriteLine($"{indent}\t\t\telse if (obj is CORBA.IObject o && o._IsA(\"{RepositoryId}\"))");
                    }
                    else
                    {
                        stream.WriteLine($"{indent}\t\t\telse if (obj._IsA(\"{RepositoryId}\"))");
                    }
                    stream.WriteLine($"{indent}\t\t\t{{");
                    stream.WriteLine($"{indent}\t\t\t\t{stubName} stub = new {stubName}();");
                    stream.WriteLine($"{indent}\t\t\t\tstub._Delegate = ((CORBA.Object)obj)._Delegate;");
                    stream.WriteLine($"{indent}\t\t\t\treturn stub;");
                    stream.WriteLine($"{indent}\t\t\t}}");
                }
                stream.WriteLine($"{indent}\t\t\telse");
                stream.WriteLine($"{indent}\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\tthrow new CORBA.BadParam(\"Narrow failed\");");
                stream.WriteLine($"{indent}\t\t\t}}");
                stream.WriteLine($"{indent}\t\t}}");
                stream.WriteLine();
                stream.WriteLine($"{indent}\t\tpublic static {MappedType} UncheckedNarrow({(IsAbstract ? "object" : "CORBA.IObject")} obj)");
                stream.WriteLine($"{indent}\t\t{{");
                stream.WriteLine($"{indent}\t\t\tif (obj == null)");
                stream.WriteLine($"{indent}\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\treturn null;");
                stream.WriteLine($"{indent}\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\telse if (obj is {MappedType})");
                stream.WriteLine($"{indent}\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\treturn ({MappedType})obj;");
                stream.WriteLine($"{indent}\t\t\t}}");
                if (IsLocal)
                {
                    stream.WriteLine($"{indent}\t\t\telse");
                    stream.WriteLine($"{indent}\t\t\t{{");
                    stream.WriteLine($"{indent}\t\t\t\tthrow new CORBA.BadParam(\"Narrow failed\");");
                    stream.WriteLine($"{indent}\t\t\t}}");
                }
                else
                {
                    if (IsAbstract)
                    {
                        stream.WriteLine($"{indent}\t\t\telse if (obj is CORBA.IObject)");
                    }
                    else
                    {
                        stream.WriteLine($"{indent}\t\t\telse");
                    }
                    stream.WriteLine($"{indent}\t\t\t{{");
                    stream.WriteLine($"{indent}\t\t\t\t{stubName} stub = new {stubName}();");
                    stream.WriteLine($"{indent}\t\t\t\tstub._Delegate = ((CORBA.Object)obj)._Delegate;");
                    stream.WriteLine($"{indent}\t\t\t\treturn stub;");
                    stream.WriteLine($"{indent}\t\t\t}}");
                    if (IsAbstract)
                    {
                        stream.WriteLine($"{indent}\t\t\telse");
                        stream.WriteLine($"{indent}\t\t\t{{");
                        stream.WriteLine($"{indent}\t\t\t\tthrow new CORBA.BadParam(\"Narrow failed\");");
                        stream.WriteLine($"{indent}\t\t\t}}");
                    }
                }
                stream.WriteLine($"{indent}\t\t}}");
                stream.WriteLine($"{indent}\t}}");                                
                if (!string.IsNullOrEmpty(Namespace))
                {
                    stream.WriteLine($"}}");
                }
            }
        }

        public void GenerateStub(DirectoryInfo currentDir)
        {
            if (IsLocal)
            {
                var stubName = GetMappedName("_", "LocalBase");
                var repIds = new List<string>();
                repIds.Add($"\"{RepositoryId}\"");
                foreach (var ancestor in Inherits)
                {
                    repIds.Add($"\"{ancestor.RepositoryId}\"");
                }
                var file = new FileInfo($"{currentDir.FullName}\\{stubName}.cs");
                using (StreamWriter stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    stream.WriteLine();
                    var indent = "";
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        indent = "\t";
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }                                        
                    stream.WriteLine($"{indent}public abstract class {stubName}: CORBA.LocalObject, {MappedName}");
                    stream.WriteLine($"{indent}{{");
                    stream.WriteLine($"{indent}\tprivate string[] _ids = {{{string.Join(",", repIds)}}};");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic override string[] _Ids()");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\treturn _ids;");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();
                    PrintOperations($"{indent}\t", stream, true, new HashSet<string>(), "abstract");
                    stream.WriteLine($"{indent}}}");
                    stream.WriteLine();                    
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine($"}}");
                    }                    
                }
            }
            else
            {
                var stubName = GetMappedName("_", "Stub");
                var operationsName = GetMappedName("I", "Operations");
                var repIds = new List<string>();
                repIds.Add($"\"{RepositoryId}\"");
                foreach (var ancestor in Inherits)
                {
                    repIds.Add($"\"{ancestor.RepositoryId}\"");
                }
                var file = new FileInfo($"{currentDir.FullName}\\{stubName}.cs");
                using (StreamWriter stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    stream.WriteLine();
                    var indent = "";
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        indent = "\t";
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }
                    stream.WriteLine($"{indent}public class {stubName}: CORBA.Object, {MappedName}");
                    stream.WriteLine($"{indent}{{");
                    stream.WriteLine($"{indent}\tprivate new string[] _ids = {{{string.Join(",", repIds)}}};");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic override string[] _Ids()");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\treturn _ids;");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic static Type _opsType = typeof({operationsName});");
                    stream.WriteLine();
                    var writtenOperations = new HashSet<string>();
                    PrintStubAttributes($"{indent}\t", stream, writtenOperations);
                    PrintStubOperations($"{indent}\t", stream, writtenOperations);
                    stream.WriteLine();
                    stream.WriteLine($"\t}}");
                    stream.WriteLine();
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine($"}}");
                    }
                }
            }
        }

        public void PrintStubAttributes(string indent, TextWriter stream, HashSet<string> writtenOperations)
        {
            foreach (var ancestor in Inherits)
            {
                ancestor.PrintStubAttributes(indent, stream, writtenOperations);
            }            
            foreach (IIDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is AttributeType att && !writtenOperations.Contains(symbol.Name))
                {
                    writtenOperations.Add(symbol.Name);
                    att.PrintStub(indent, stream);
                }
            }
        }

        public void PrintStubOperations(string indent, TextWriter stream, HashSet<string> writtenOperations)
        {
            foreach (var ancestor in Inherits)
            {
                ancestor.PrintStubOperations(indent, stream, writtenOperations);
            }
            foreach (IIDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is Operation op && !writtenOperations.Contains(symbol.Name))
                {
                    writtenOperations.Add(symbol.Name);
                    op.PrintStub(indent, stream);
                }
            }
        }        

        public void GeneratePOA(DirectoryInfo currentDir)
        {
            if (!IsAbstract && !IsLocal)
            {
                var poaName = GetMappedName("", "POA");
                var operationsName = GetMappedName("I", "Operations");
                var repIds = new List<string>();
                repIds.Add($"\"{RepositoryId}\"");
                foreach (var ancestor in Inherits)
                {
                    repIds.Add($"\"{ancestor.RepositoryId}\"");
                }
                var file = new FileInfo($"{currentDir.FullName}\\{poaName}.cs");
                using (StreamWriter stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    stream.WriteLine("");
                    var indent = "";
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        indent = "\t";
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }
                    stream.WriteLine();
                    stream.WriteLine($"{indent}public abstract class {poaName}: PortableServer.Servant, CORBA.IInvokeHandler, {operationsName}");
                    stream.WriteLine($"{indent}{{");
                    stream.WriteLine($"{indent}\tstatic private Dictionary<string,int> _opsDict = new Dictionary<string,int>();");
                    stream.WriteLine($"{indent}\tstatic {poaName}()");
                    stream.WriteLine($"{indent}\t{{");
                    var index = 0;                    
                    PrintPoaOperationsDict(indent, stream, new HashSet<string>(), ref index);
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine($"{indent}\tprivate string[] _ids = {{{string.Join(",", repIds)}}};");
                    stream.WriteLine();
                    PrintPoaOperations($"{indent}\t", stream, true, new HashSet<string>(), "abstract");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic override string[] _AllInterfaces(PortableServer.IPOA poa, byte[] objId)");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\treturn _ids;");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic virtual {MappedType} _This()");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\treturn {FullHelperName}.Narrow(_ThisObject());");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic virtual {MappedType} _This(CORBA.ORB orb)");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\treturn {FullHelperName}.Narrow(_ThisObject(orb));");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic CORBA.IOutputStream _Invoke(string method, CORBA.IInputStream inputStream, CORBA.IResponseHandler handler)");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\tCORBA.IOutputStream outputStream = null;");
                    stream.WriteLine($"{indent}\t\tint opIndex;");
                    stream.WriteLine($"{indent}\t\tif (_opsDict.TryGetValue(method, out opIndex))");
                    stream.WriteLine($"{indent}\t\t{{");
                    stream.WriteLine($"{indent}\t\t\tswitch (opIndex)");
                    stream.WriteLine($"{indent}\t\t\t{{");
                    index = 0;
                    PrintPoaInvokeOperations(indent, stream, new HashSet<string>(), ref index);
                    stream.WriteLine($"{indent}\t\t\t}}");
                    stream.WriteLine($"{indent}\t\t\treturn outputStream;");
                    stream.WriteLine($"{indent}\t\t}}");
                    stream.WriteLine($"{indent}\t\telse");
                    stream.WriteLine($"{indent}\t\t{{");
                    stream.WriteLine($"{indent}\t\t\tthrow new CORBA.BadOperation(method + \" not found\");");
                    stream.WriteLine($"{indent}\t\t}}");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine($"{indent}}}");
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine($"}}");
                    }
                }
            }
        }

        public void PrintPoaOperationsDict(string indent, TextWriter stream, HashSet<string> writtenOperations, ref int index)
        {
            foreach (var ancestor in Inherits)
            {
                ancestor.PrintPoaOperationsDict(indent, stream, writtenOperations, ref index);
            }
            foreach (IIDLSymbol s in NamingScope.Symbols.Values)
            {
                if (writtenOperations.Contains(s.Name))
                {
                    continue;
                }
                switch (s)
                {
                    case AttributeType att:
                        if (!att.IsReadOnly)
                        {
                            stream.WriteLine($"{indent}\t\t_opsDict.Add(\"_set_{att.Name}\", {index++});");
                        }
                        stream.WriteLine($"{indent}\t\t_opsDict.Add(\"_get_{att.Name}\", {index++});");
                        writtenOperations.Add(s.Name);
                        break;
                    case Operation op:
                        stream.WriteLine($"{indent}\t\t_opsDict.Add(\"{op.Name}\", {index++});");
                        writtenOperations.Add(s.Name);
                        break;

                }
            }
        }

        public void PrintPoaInvokeOperations(string indent, StreamWriter stream, HashSet<string> writtenOperations, ref int index)
        {
            foreach (var ancestor in Inherits)
            {
                ancestor.PrintPoaInvokeOperations(indent, stream, writtenOperations, ref index);
            }
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (writtenOperations.Contains(symbol.Name))
                {
                    continue;
                }
                switch (symbol)
                {
                    case AttributeType att:
                        if (!att.IsReadOnly)
                        {
                            stream.WriteLine($"{indent}\t\t\t\tcase {index++}:");
                            stream.WriteLine($"{indent}\t\t\t\t{{");
                            att.PrintPOASet($"{indent}\t\t\t\t\t", stream);
                            stream.WriteLine($"{indent}\t\t\t\t}}");
                            stream.WriteLine($"{indent}\t\t\t\tbreak;");
                        }
                        stream.WriteLine($"{indent}\t\t\t\tcase {index++}:");
                        stream.WriteLine($"{indent}\t\t\t\t{{");
                        att.PrintPOAGet($"{indent}\t\t\t\t\t", stream);
                        stream.WriteLine($"{indent}\t\t\t\t}}");
                        stream.WriteLine($"{indent}\t\t\t\tbreak;");
                        writtenOperations.Add(symbol.Name);
                        break;
                    case Operation op:
                        stream.WriteLine($"{indent}\t\t\t\tcase {index++}:");
                        stream.WriteLine($"{indent}\t\t\t\t{{");
                        op.PrintPOAInvoke($"{indent}\t\t\t\t\t", stream);
                        stream.WriteLine($"{indent}\t\t\t\t}}");
                        stream.WriteLine($"{indent}\t\t\t\tbreak;");
                        writtenOperations.Add(symbol.Name);
                        break;
                }
            }
        }

        public void GeneratePOATie(DirectoryInfo currentDir)
        {
            if (!IsAbstract && !IsLocal)
            {
                var poaName = GetMappedName("", "POA");
                var poaTieName = GetMappedName("", "POATie");
                var operationsName = GetMappedName("I", "Operations");                
                var file = new FileInfo($"{currentDir.FullName}\\{poaTieName}.cs");
                using (StreamWriter stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    stream.WriteLine("");
                    var indent = "";
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        indent = "\t";
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }
                    stream.WriteLine();
                    stream.WriteLine($"{indent}public class {poaTieName}: {poaName}");
                    stream.WriteLine($"{indent}{{");
                    stream.WriteLine($"{indent}\tpublic {operationsName} _OperationsDelegate {{ get; set; }}");                    
                    stream.WriteLine($"{indent}\tprivate PortableServer.IPOA _poa;");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic {poaTieName}({operationsName} d)");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\t_OperationsDelegate = d;");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic {poaTieName}({operationsName} d, PortableServer.POA poa)");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\t_OperationsDelegate = d;");
                    stream.WriteLine($"{indent}\t\t_poa = poa;");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic override PortableServer.IPOA _DefaultPOA()");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\tif (_poa != null)");
                    stream.WriteLine($"{indent}\t\t{{");
                    stream.WriteLine($"{indent}\t\t\treturn _poa;");
                    stream.WriteLine($"{indent}\t\t}}");
                    stream.WriteLine($"{indent}\t\treturn base._DefaultPOA();");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();                    
                    stream.WriteLine($"{indent}\tpublic override {MappedType} _This()");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\treturn {FullHelperName}.Narrow(_ThisObject());");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic override {MappedType} _This(CORBA.ORB orb)");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\treturn {FullHelperName}.Narrow(_ThisObject(orb));");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();
                    var writtenOperations = new HashSet<string>();
                    PrintTieMembers($"{indent}\t", stream, writtenOperations);
                    stream.WriteLine($"{indent}}}");
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine($"}}");
                    }
                }
            }
        }

        public void GenerateLocalTie(DirectoryInfo currentDir)
        {
            if (IsLocal)
            {
                var stubName = GetMappedName("_", "LocalBase");                 
                var localTieName = GetMappedName("", "LocalTie");
                var operationsName = GetMappedName("I", "Operations");
                var file = new FileInfo($"{currentDir.FullName}\\{localTieName}.cs");
                using (StreamWriter stream = file.CreateText())
                {
                    PrintComment(stream);
                    PrintUsings(stream);
                    stream.WriteLine("");
                    var indent = "";
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        indent = "\t";
                        stream.WriteLine($"namespace {Namespace}");
                        stream.WriteLine("{");
                    }
                    stream.WriteLine();
                    stream.WriteLine($"{indent}public class {localTieName}: {stubName}");
                    stream.WriteLine($"{indent}{{");
                    stream.WriteLine($"{indent}\tpublic {operationsName} _OperationsDelegate {{ get; set; }}");                    
                    stream.WriteLine();
                    stream.WriteLine($"{indent}\tpublic {localTieName}({operationsName} d)");
                    stream.WriteLine($"{indent}\t{{");
                    stream.WriteLine($"{indent}\t\t_OperationsDelegate = d;");
                    stream.WriteLine($"{indent}\t}}");
                    stream.WriteLine();
                    var writtenOperations = new HashSet<string>();
                    PrintTieMembers($"{indent}\t", stream, writtenOperations);
                    stream.WriteLine($"{indent}}}");
                    if (!string.IsNullOrEmpty(Namespace))
                    {
                        stream.WriteLine($"}}");
                    }
                }
            }
        }

        public void PrintTieMembers(string indent, StreamWriter stream, HashSet<string> writtenOperations)
        {
            foreach (var ancestor in Inherits)
            {
                ancestor.PrintTieMembers(indent, stream, writtenOperations);
            }
            foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
            {
                if (symbol is Operation op && !writtenOperations.Contains(symbol.Name))
                {
                    writtenOperations.Add(symbol.Name);
                    op.PrintTie(indent, stream);
                }
                else if (symbol is AttributeType att && !writtenOperations.Contains(symbol.Name))
                {
                    writtenOperations.Add(symbol.Name);
                    att.PrintTie(indent, stream);
                }
            }
        }
    }
}
