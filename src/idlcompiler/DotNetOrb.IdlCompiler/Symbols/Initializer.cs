// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Initializer: IDLSymbol, IScopeSymbol
    {
        public Scope NamingScope { get; set; }        
        public List<ExceptionSymbol> Raises { get; set; }

        public Initializer(string name, bool dotNetNaming, List<Annotation> annotations = null) : base(name, dotNetNaming, annotations)
        {
            Raises = new List<ExceptionSymbol>();
        }

        public void PrintInitializer(string indent, TextWriter stream)
        {
            stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
            foreach (var ex in Raises)
            {
                stream.WriteLine($"{indent}[ThrowsIdlException(typeof({ex.MappedType}))]");
            }            
            //stream.WriteLine($"{indent}[Factory]");
            stream.Write($"{indent}{ParentScope.Symbol.MappedName} {MappedName}(");
            var parameters = new List<string>();
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is OperationParameter op)
                {
                    var p = "";
                    var attributes = op.MappedAttributes;
                    attributes.AddRange(op.DataType.MappedAttributes);
                    if (attributes.Count > 0)
                    {
                        p += String.Join("", attributes) + " ";
                    }
                    switch (op.Direction)
                    {
                        case ParameterDirection.OUT:
                            p += "out ";
                            break;
                        case ParameterDirection.INOUT:
                            p += "ref ";
                            break;
                    }
                    p += $"{op.DataType.MappedType} {op.MappedName}";
                    parameters.Add(p);
                }
            }
            stream.Write(String.Join(", ", parameters));
            stream.WriteLine(");");
        }

        public void PrintHelper(string indent, TextWriter stream)
        {
            foreach (var ex in Raises)
            {
                stream.WriteLine($"{indent}[ThrowsIdlException(typeof({ex.MappedType}))]");
            }
            //stream.WriteLine($"{indent}[Factory]");
            stream.Write($"{indent}public static {ParentScope.Symbol.MappedName} {MappedName}(CORBA.ORB orb, ");
            var parameters = new List<string>();
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is OperationParameter op)
                {
                    var p = "";
                    var attributes = op.MappedAttributes;
                    attributes.AddRange(op.DataType.MappedAttributes);
                    if (attributes.Count > 0)
                    {
                        p += String.Join("", attributes) + " ";
                    }
                    switch (op.Direction)
                    {
                        case ParameterDirection.OUT:
                            p += "out ";
                            break;
                        case ParameterDirection.INOUT:
                            p += "ref ";
                            break;
                    }
                    p += $"{op.DataType.MappedType} {op.MappedName}";
                    parameters.Add(p);
                }
            }
            stream.Write(String.Join(", ", parameters));
            stream.WriteLine(")");
            stream.WriteLine($"{indent}{{");
            var factoryName = ParentScope.Symbol.GetMappedName("I", "ValueFactory");
            stream.WriteLine($"{indent}\tvar f = ({factoryName}) orb.LookupValueFactory(Id());");
            stream.WriteLine($"{indent}\tif (f != null)");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tthrow new CORBA.Marshal(1, CompletionStatus.No);");
            stream.WriteLine($"{indent}\t}}");
            parameters.Clear();
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is OperationParameter op)
                {
                    var p = "";                    
                    switch (op.Direction)
                    {
                        case ParameterDirection.OUT:
                            p += "out ";
                            break;
                        case ParameterDirection.INOUT:
                            p += "ref ";
                            break;
                    }
                    p += $"{op.MappedName}";
                    parameters.Add(p);
                }
            }
            stream.WriteLine($"{indent}\treturn f.{MappedName}({String.Join(", ", parameters)});");
            stream.WriteLine($"{indent}}}");
        }
    }
}
