// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Antlr4.Runtime.Atn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Operation: IDLSymbol, IScopeSymbol
    {
        public Scope NamingScope { get; set; }
        public bool IsOneWay { get; set; }
        public bool IsAsync { get; set; }
        public ITypeSymbol ReturnType { get; set; }        
        public List<ExceptionSymbol> Raises { get; set; }
        public List<string> Context { get; set; }

        public Operation(string name, bool isAsync, List<Annotation> annotations = null ) : base(name, annotations)
        {
            Raises = new List<ExceptionSymbol>();
            Context = new List<string>();
            this.IsAsync = isAsync;
            var amiAnnotation = GetAnnotation("ami");
            if (amiAnnotation != null )
            {
                this.IsAsync = true;
                Annotations.Remove(amiAnnotation);
            }
        }

        public void PrintOperation(string indent, TextWriter stream, string modifier = null)
        {
            InternalPrintOperation(indent, stream, modifier);
            stream.WriteLine(";");
            if (IsAsync)
            {                
                InternalPrintAsyncOperation(indent, stream, modifier);
                stream.WriteLine(";");
            }
        }

        public void PrintPOAOperation(string indent, TextWriter stream, string modifier = null)
        {
            InternalPrintOperation(indent, stream, modifier);
            stream.WriteLine(";");
            if (IsAsync)
            {
                PrintPOAAsync(indent, stream);
            }
        }

        private void PrintPOAAsync(string indent, TextWriter stream)
        {
            var returnParameters = InternalPrintAsyncOperation(indent, stream, "virtual");
            stream.WriteLine();
            stream.WriteLine($"{indent}{{");
            if (returnParameters.Count > 0)
            {
                foreach (var p in returnParameters)
                {
                    switch (p.dir)
                    {
                        case ParameterDirection.OUT:
                            stream.WriteLine($"{indent}\t{p.type} {p.name};");
                            break;
                    }
                    
                }
            }
            if (ReturnType != null)
            {
                stream.Write($"{indent}\t_result = {MappedName}(");
            }
            else
            {
                stream.Write($"{indent}\t{MappedName}(");
            }
            var parameters = new List<string>();
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
                    p += op.MappedName;
                    parameters.Add(p);
                }
            }
            stream.Write(String.Join(", ", parameters));
            stream.WriteLine(");");
            if (returnParameters.Count > 1)
            {
                stream.WriteLine($"{indent}\treturn Task.FromResult(({String.Join(", ", returnParameters.Select(p => $"{p.name}"))}));");
            }
            else if (returnParameters.Count == 1)
            {
                stream.WriteLine($"{indent}\treturn Task.FromResult({returnParameters[0].name});");
            }
            else
            {
                stream.WriteLine($"{indent}\treturn Task.CompletedTask;");
            }
            stream.WriteLine($"{indent}}}");
        }
    

        private void InternalPrintOperation(string indent, TextWriter stream, string modifier)
        {
            stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
            foreach (var att in MappedAttributes)
            {
                stream.WriteLine($"{indent}{att}");
            }
            foreach (var ex in Raises)
            {
                stream.WriteLine($"{indent}[ThrowsIdlException(typeof({ex.MappedType}))]");
            }
            var returnType = "void";
            IList<string> returnAttributes = new List<string>();
            if (ReturnType != null) 
            {
                returnType = ReturnType.MappedType;
                returnAttributes = ReturnType.MappedAttributes;
            }
            foreach (var att in returnAttributes)
            {
                stream.WriteLine($"{indent}{att.Replace("[","[return: ")}");
            }
            stream.Write($"{indent}public {(String.IsNullOrEmpty(modifier) ? "" : modifier + " ")}{returnType} {MappedName}(");
            var parameters = new List<string>();
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is OperationParameter op)
                {
                    var p = "";
                    var attributes = op.DataType.MappedAttributes;
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
            stream.Write(")");
        }

        private List<(string type, string name, ParameterDirection dir)> InternalPrintAsyncOperation(string indent, TextWriter stream, string modifier)
        {
            stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
            foreach (var att in MappedAttributes)
            {
                stream.WriteLine($"{indent}{att}");
            }
            foreach (var ex in Raises)
            {
                stream.WriteLine($"{indent}[ThrowsIdlException(typeof({ex.MappedType}))]");
            }
            var returnParameters = new List<(string type, string name, ParameterDirection dir)>();
            if (ReturnType != null)
            {
                returnParameters.Add((ReturnType.MappedType, "_result", ParameterDirection.OUT));
            }
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is OperationParameter op)
                {                    
                    switch (op.Direction)
                    {
                        case ParameterDirection.OUT:
                        case ParameterDirection.INOUT:
                            returnParameters.Add((op.DataType.MappedType, op.MappedName, op.Direction));
                            break;
                    }
                }
            }
            var returnType = "Task";

            if (returnParameters.Count == 1)
            {
                returnType = $"Task<{returnParameters[0].type}>";                
            }
            else if (returnParameters.Count > 1)
            {                
                returnType = $"Task<({String.Join(", ", returnParameters.Select(p => $"{p.type} {p.name}"))})>";
            }            

            stream.Write($"{indent}public {(String.IsNullOrEmpty(modifier) ? "" : modifier + " ")}{returnType} {(Compiler.DotNetNaming ? GetMappedName("", "Async") : GetMappedName("", "_async"))}(");
            var parameters = new List<string>();
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is OperationParameter op)
                {
                    var p = "";
                    var attributes = op.DataType.MappedAttributes;
                    if (attributes.Count > 0)
                    {
                        p += String.Join("", attributes) + " ";
                    }
                    switch (op.Direction)
                    {
                        case ParameterDirection.OUT:
                            continue;                            
                        case ParameterDirection.INOUT:
                            break;
                    }
                    p += $"{op.DataType.MappedType} {op.MappedName}";
                    parameters.Add(p);
                }
            }
            stream.Write(String.Join(", ", parameters));
            stream.Write(")");
            return returnParameters;
        }

        public void PrintStub(string indent, TextWriter stream)
        {
            PrintStub(indent, stream, false);
            if (IsAsync)
            {
                stream.WriteLine();
                PrintStub(indent, stream, true);
            }            
        }

        private void PrintStub(string indent, TextWriter stream, bool ami)
        {
            var opIfzName = ParentScope.Symbol.GetMappedName("I", "Operations");
            if (ami)
            {
                InternalPrintAsyncOperation(indent, stream, "async");
            }
            else
            {
                InternalPrintOperation(indent, stream, "");
            }
            stream.WriteLine();
            stream.WriteLine($"{indent}{{");
            stream.WriteLine($"{indent}\twhile(true)");
            //stream.WriteLine($"{indent}\t\tif(!this._IsLocal())");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\tIInputStream inputStream = null;");
            stream.WriteLine($"{indent}\t\tIOutputStream outputStream = null;");
            stream.WriteLine($"{indent}\t\ttry");
            stream.WriteLine($"{indent}\t\t{{");
            stream.WriteLine($"{indent}\t\t\toutputStream = _Request(\"{Name}\", true);");            
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is OperationParameter op)
                {                    
                    switch (op.Direction)
                    {
                        case ParameterDirection.IN:                 
                        case ParameterDirection.INOUT:
                            op.DataType.PrintWrite($"{indent}\t\t\t", stream, "outputStream", op.MappedName, 0);
                            break;
                    }
                }
            }            
            if (ami)
            {
                stream.WriteLine($"{indent}\t\t\tinputStream = await _InvokeAsync(outputStream);");
            }
            else
            {
                stream.WriteLine($"{indent}\t\t\tinputStream = _Invoke(outputStream);");
            }            
            if (ReturnType !=  null)
            {
                stream.WriteLine($"{indent}\t\t\t{ReturnType.MappedType} _result;");
                ReturnType.PrintRead($"{indent}\t\t\t", stream, "inputStream", "_result", 0);
            }
            var returnParameters = new List<string>();
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is OperationParameter op)
                {
                    switch (op.Direction)
                    {
                        case ParameterDirection.OUT:
                            if (ami)
                            {
                                returnParameters.Add(op.MappedName);
                                stream.WriteLine($"{indent}\t\t\t{op.DataType.MappedType} {op.MappedName};");
                            }
                            op.DataType.PrintRead($"{indent}\t\t\t", stream, "inputStream", op.MappedName, 0);
                            break;
                        case ParameterDirection.INOUT:
                            op.DataType.PrintRead($"{indent}\t\t\t", stream, "inputStream", op.MappedName, 0);
                            if (ami)
                            {
                                returnParameters.Add(op.MappedName);
                            }
                            break;
                    }
                }
            }
            if (ReturnType != null)
            {
                returnParameters.Insert(0, "_result");
            }
            if (returnParameters.Count == 1)
            {
                stream.WriteLine($"{indent}\t\t\treturn {returnParameters[0]};");
            }
            else if (returnParameters.Count > 1)
            {
                stream.WriteLine($"{indent}\t\t\treturn ({String.Join(", ", returnParameters)});");
            }
            else
            {
                stream.WriteLine($"{indent}\t\t\treturn;");
            }
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t\tcatch(RemarshalException)");
            stream.WriteLine($"{indent}\t\t{{");
            stream.WriteLine($"{indent}\t\t\tcontinue;");
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t\tcatch(CORBA.ApplicationException aex)");
            stream.WriteLine($"{indent}\t\t{{");
            stream.WriteLine($"{indent}\t\t\ttry");
            stream.WriteLine($"{indent}\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\tswitch (aex.Id)");
            stream.WriteLine($"{indent}\t\t\t\t{{");
            foreach (var ex in Raises)
            {
                stream.WriteLine($"{indent}\t\t\t\t\tcase \"{ex.RepositoryId}\":");
                stream.WriteLine($"{indent}\t\t\t\t\t\tthrow {ex.FullHelperName}.Read(aex.InputStream);");
            }
            stream.WriteLine($"{indent}\t\t\t\t\tdefault:");
            stream.WriteLine($"{indent}\t\t\t\t\t\tthrow new RuntimeException(\"Unexpected exception \" + aex.Id);");
            stream.WriteLine($"{indent}\t\t\t\t}}\t\t\t\t\t\t");
            stream.WriteLine($"{indent}\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\tfinally");
            stream.WriteLine($"{indent}\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\ttry");
            stream.WriteLine($"{indent}\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\taex.InputStream.Close();");
            stream.WriteLine($"{indent}\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\tcatch (Exception ex)");
            stream.WriteLine($"{indent}\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\tthrow new RuntimeException(\"Unexpected exception \" + ex.ToString());");
            stream.WriteLine($"{indent}\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t}}");
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t\tfinally");
            stream.WriteLine($"{indent}\t\t{{");
            stream.WriteLine($"{indent}\t\t\tif (outputStream != null)");
            stream.WriteLine($"{indent}\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\ttry");
            stream.WriteLine($"{indent}\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\toutputStream.Close();");
            stream.WriteLine($"{indent}\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\tcatch (Exception e)");
            stream.WriteLine($"{indent}\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\tthrow new RuntimeException(\"Unexpected exception \" + e.ToString());");
            stream.WriteLine($"{indent}\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\tif (inputStream != null)");
            stream.WriteLine($"{indent}\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\tthis._ReleaseReply(inputStream);");
            stream.WriteLine($"{indent}\t\t\t}}");
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t}}");            
            stream.WriteLine($"{indent}}}");
        }

        public void PrintPOAInvoke(string indent, TextWriter stream)
        {            
            if (Raises.Count > 0)
            {
                stream.WriteLine($"{indent}try");
                stream.WriteLine($"{indent}{{");
            }
            var parameters = new List<string>();
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is OperationParameter op)
                {
                    var p = "";
                    switch (op.Direction)
                    {
                        case ParameterDirection.IN:
                            stream.WriteLine($"{indent}\t{op.DataType.MappedType} {op.MappedName};");
                            op.DataType.PrintRead($"{indent}\t", stream, "inputStream", op.MappedName, 0);
                            break;
                        case ParameterDirection.INOUT:
                            stream.WriteLine($"{indent}\t{op.DataType.MappedType} {op.MappedName};");
                            op.DataType.PrintRead($"{indent}\t", stream, "inputStream", op.MappedName, 0);
                            p += "ref ";
                            break;
                        case ParameterDirection.OUT:
                            p += $"out {op.DataType.MappedType} ";
                            break;
                    }
                    p += $"{op.MappedName}";
                    parameters.Add(p);
                }
            }
            stream.WriteLine($"{indent}\toutputStream = handler.CreateReply();");            
            if (ReturnType != null)
            {
                stream.WriteLine($"{indent}\tvar _result = {MappedName}({String.Join(", ", parameters)});");
                ReturnType.PrintWrite($"{indent}\t", stream, "outputStream", "_result", 0);
            }
            else
            {
                stream.WriteLine($"{indent}\t{MappedName}({String.Join(",", parameters)});");
            }
            foreach (IDLSymbol child in NamingScope.Symbols.Values)
            {
                if (child is OperationParameter op)
                {
                    switch (op.Direction)
                    {
                        case ParameterDirection.OUT:
                        case ParameterDirection.INOUT:
                            op.DataType.PrintWrite($"{indent}\t", stream, "outputStream", op.MappedName, 0);
                            break;
                    }
                }
            }
            if (Raises.Count > 0)
            {
                stream.WriteLine($"{indent}}}");
                foreach (var ex in Raises)
                {
                    stream.WriteLine($"{indent}catch({ex.MappedType} ex)");
                    stream.WriteLine($"{indent}{{");
                    stream.WriteLine($"{indent}\toutputStream = handler.CreateExceptionReply();");
                    ex.PrintWrite($"{indent}\t", stream, "outputStream", "ex", 0);
                    stream.WriteLine($"{indent}}}");
                }
            }
        }

        public void PrintTie(string indent, TextWriter stream)
        {
            InternalPrintTie(indent, stream);
            if (IsAsync)
            {                
                InternalPrintTieAsync(indent, stream);
            }
        }

        private void InternalPrintTie(string indent, TextWriter stream)
        {
            InternalPrintOperation(indent, stream, "override");            
            stream.WriteLine();
            stream.WriteLine($"{indent}{{");

            if (ReturnType != null)
            {                                
                stream.Write($"{indent}\treturn _OperationsDelegate.{MappedName}(");                
            }
            else
            {
                stream.Write($"{indent}\t_OperationsDelegate.{MappedName}(");                
            }
            var parameters = new List<string>();
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
                    p += op.MappedName;
                    parameters.Add(p);
                }
            }
            stream.Write(String.Join(", ", parameters));
            stream.WriteLine(");");
            stream.WriteLine($"{indent}}}");
        }

        private void InternalPrintTieAsync(string indent, TextWriter stream)
        {
            var returnParameters = InternalPrintAsyncOperation(indent, stream, "override");
            stream.WriteLine();
            stream.WriteLine($"{indent}{{");
            if (returnParameters.Count > 0)
            {
                foreach (var p in returnParameters)
                {
                    switch (p.dir)
                    {
                        case ParameterDirection.OUT:
                            stream.WriteLine($"{indent}\t{p.type} {p.name};");
                            break;
                    }

                }
            }
            if (ReturnType != null)
            {
                stream.Write($"{indent}\t_result = _OperationsDelegate.{MappedName}(");
            }
            else
            {
                stream.Write($"{indent}\t_OperationsDelegate.{MappedName}(");
            }
            var parameters = new List<string>();
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
                    p += op.MappedName;
                    parameters.Add(p);
                }
            }
            stream.Write(String.Join(", ", parameters));
            stream.WriteLine(");");
            if (returnParameters.Count > 1)
            {
                stream.WriteLine($"{indent}\treturn Task.FromResult(({String.Join(", ", returnParameters.Select(p => $"{p.name}"))}));");
            }
            else if (returnParameters.Count == 1)
            {
                stream.WriteLine($"{indent}\treturn Task.FromResult({returnParameters[0].name});");
            }
            else
            {
                stream.WriteLine($"{indent}\treturn Task.CompletedTask;");
            }
            stream.WriteLine($"{indent}}}");
        }
    }
}
