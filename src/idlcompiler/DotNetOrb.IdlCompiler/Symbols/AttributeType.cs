// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class AttributeType: IDLSymbol, IScopeSymbol
    {
        public Scope NamingScope { get; set; }        
        public bool IsReadOnly { get; set; }
        public ITypeSymbol DataType { get; set; }
        public List<ExceptionSymbol> GetRaises { get; set; }
        public List<ExceptionSymbol> SetRaises { get; set; }

        public AttributeType(string name, bool readOnly, List<Annotation> annotations = null) : base(name, annotations)
        {
            IsReadOnly = readOnly;  
            GetRaises = new List<ExceptionSymbol>();
            SetRaises = new List<ExceptionSymbol>();
        }

        public void PrintAttribute(string indent, TextWriter stream, String modifier = null)
        {
            stream.WriteLine($"{indent}[IdlName(\"{Name}\")]");
            foreach (var att in MappedAttributes)
            {
                stream.WriteLine($"{indent}{att}");
            }
            foreach (var att in DataType.MappedAttributes)
            {
                stream.WriteLine($"{indent}{att}");
            }
            stream.WriteLine($"{indent}public {(String.IsNullOrEmpty(modifier) ? "" : modifier + " ")}{DataType.MappedType} {MappedName} ");
            stream.WriteLine($"{indent}{{");
            foreach (var ex in GetRaises)
            {
                stream.WriteLine($"{indent}\t[ThrowsIdlException(typeof({ex.MappedType}))]");
            }
            stream.WriteLine($"{indent}\tget;");
            if (!this.IsReadOnly)
            {
                stream.WriteLine();
                foreach (var ex in SetRaises)
                {
                    stream.WriteLine($"{indent}\t[ThrowsIdlException(typeof({ex.MappedType}))]");
                }
                stream.WriteLine($"{indent}\tset;");
            }
            stream.WriteLine($"{indent}}}");
        }

        public void PrintStub(string indent, TextWriter stream)
        {
            var opIfzName = ParentScope.Symbol.GetMappedName("I", "Operations");
            foreach (var att in MappedAttributes)
            {
                stream.WriteLine($"{indent}{att}");
            }
            foreach (var att in DataType.MappedAttributes)
            {
                stream.WriteLine($"{indent}{att}");
            }
            stream.WriteLine($"{indent}public {DataType.MappedType} {MappedName}");
            stream.WriteLine($"{indent}{{");
            foreach (var ex in GetRaises)
            {
                stream.WriteLine($"{indent}\t[ThrowsIdlException(typeof({ex.MappedType}))]");                
            }
            //stream.WriteLine($"{indent}\t[IdlName(\"_get_{Name}\")]");
            stream.WriteLine($"{indent}\tget");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\twhile(true)");
            stream.WriteLine($"{indent}\t\t{{");
            //stream.WriteLine($"{indent}\t\t\tif(!this._IsLocal())");
            stream.WriteLine($"{indent}\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\tIInputStream inputStream = null;");
            stream.WriteLine($"{indent}\t\t\t\tIOutputStream outputStream = null;");
            stream.WriteLine($"{indent}\t\t\t\ttry");
            stream.WriteLine($"{indent}\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\toutputStream = _Request(\"_get_{Name}\", true);");
            stream.WriteLine($"{indent}\t\t\t\t\tinputStream = _Invoke(outputStream);");
            stream.WriteLine($"{indent}\t\t\t\t\t{DataType.MappedType} _result;");
            DataType.PrintRead($"{indent}\t\t\t\t\t", stream, "inputStream", "_result", 0);
            stream.WriteLine($"{indent}\t\t\t\t\treturn _result;");
            stream.WriteLine($"{indent}\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\tcatch(RemarshalException)");
            stream.WriteLine($"{indent}\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\tcontinue;");
            stream.WriteLine($"{indent}\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\tcatch(CORBA.ApplicationException aex)");
            stream.WriteLine($"{indent}\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\ttry");
            stream.WriteLine($"{indent}\t\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\t\tswitch (aex.Id)");
            stream.WriteLine($"{indent}\t\t\t\t\t\t{{");
            foreach (var ex in GetRaises)
            {
                stream.WriteLine($"{indent}\t\t\t\t\t\t\tcase \"{ex.RepositoryId}\":");
                stream.WriteLine($"{indent}\t\t\t\t\t\t\t\tthrow {ex.FullHelperName}.Read(aex.InputStream);");
            }
            stream.WriteLine($"{indent}\t\t\t\t\t\t\tdefault:");
            stream.WriteLine($"{indent}\t\t\t\t\t\t\t\tthrow new RuntimeException(\"Unexpected exception \" + aex.Id);");
            stream.WriteLine($"{indent}\t\t\t\t\t\t}}\t\t\t\t\t\t");
            stream.WriteLine($"{indent}\t\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\t\tfinally");
            stream.WriteLine($"{indent}\t\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\t\ttry");
            stream.WriteLine($"{indent}\t\t\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\t\t\taex.InputStream.Close();");
            stream.WriteLine($"{indent}\t\t\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\t\t\tcatch (Exception ex)");
            stream.WriteLine($"{indent}\t\t\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\t\t\tthrow new RuntimeException(\"Unexpected exception \" + ex.ToString());");
            stream.WriteLine($"{indent}\t\t\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\tfinally");
            stream.WriteLine($"{indent}\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\tif (outputStream != null)");
            stream.WriteLine($"{indent}\t\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\t\ttry");
            stream.WriteLine($"{indent}\t\t\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\t\t\toutputStream.Close();");
            stream.WriteLine($"{indent}\t\t\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\t\t\tcatch (Exception e)");
            stream.WriteLine($"{indent}\t\t\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\t\t\tthrow new RuntimeException(\"Unexpected exception \" + e.ToString());");
            stream.WriteLine($"{indent}\t\t\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\t\tif (inputStream != null)");
            stream.WriteLine($"{indent}\t\t\t\t\t{{");
            stream.WriteLine($"{indent}\t\t\t\t\t\tthis._ReleaseReply(inputStream);");
            stream.WriteLine($"{indent}\t\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t\t}}");
            stream.WriteLine($"{indent}\t\t\t}}");
            //stream.WriteLine($"{indent}\t\t\telse");
            //stream.WriteLine($"{indent}\t\t\t{{");
            //stream.WriteLine($"{indent}\t\t\t\tvar so = _ServantPreinvoke(\"_get_{Name}\", _opsType);");
            //stream.WriteLine($"{indent}\t\t\t\tif(so == null)");
            //stream.WriteLine($"{indent}\t\t\t\t\tcontinue;");
            //stream.WriteLine($"{indent}\t\t\t\tvar localServant = ({opIfzName}) so.Servant;");            
            //stream.WriteLine($"{indent}\t\t\t\ttry");
            //stream.WriteLine($"{indent}\t\t\t\t{{");
            //stream.WriteLine($"{indent}\t\t\t\t\tvar _result = localServant.{MappedName};");
            //stream.WriteLine($"{indent}\t\t\t\t\tso.NormalCompletion();");
            //stream.WriteLine($"{indent}\t\t\t\t\treturn _result;");
            //stream.WriteLine($"{indent}\t\t\t\t}}");
            //stream.WriteLine($"{indent}\t\t\t\tcatch (Exception ex) ");
            //stream.WriteLine($"{indent}\t\t\t\t{{");
            //stream.WriteLine($"{indent}\t\t\t\t\tso.ExceptionalCompletion(ex);");
            //stream.WriteLine($"{indent}\t\t\t\t\tthrow;");
            //stream.WriteLine($"{indent}\t\t\t\t}}\t\t\t\t\t");
            //stream.WriteLine($"{indent}\t\t\t\tfinally");
            //stream.WriteLine($"{indent}\t\t\t\t{{");
            //stream.WriteLine($"{indent}\t\t\t\t\t_ServantPostinvoke(so);");
            //stream.WriteLine($"{indent}\t\t\t\t}}");
            //stream.WriteLine($"{indent}\t\t\t}}");
            stream.WriteLine($"{indent}\t\t}}");
            stream.WriteLine($"{indent}\t}}");
            if (!IsReadOnly)
            {
                foreach (var ex in SetRaises)
                {
                    stream.WriteLine($"{indent}\t[ThrowsIdlException(typeof({ex.MappedType}))]");
                }
                //stream.WriteLine($"{indent}\t[IdlName(\"_set_{Name}\")]");
                stream.WriteLine($"{indent}\tset");
                stream.WriteLine($"{indent}\t{{");
                stream.WriteLine($"{indent}\t\twhile(true)");
                stream.WriteLine($"{indent}\t\t{{");
                //stream.WriteLine($"{indent}\t\t\tif(!this._IsLocal())");
                stream.WriteLine($"{indent}\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\tIInputStream inputStream = null;");
                stream.WriteLine($"{indent}\t\t\t\tIOutputStream outputStream = null;");
                stream.WriteLine($"{indent}\t\t\t\ttry");
                stream.WriteLine($"{indent}\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\toutputStream = _Request(\"_set_{Name}\", true);");
                DataType.PrintWrite($"{indent}\t\t\t\t\t", stream, "outputStream", "value", 0);
                stream.WriteLine($"{indent}\t\t\t\t\tinputStream = _Invoke(outputStream);");
                stream.WriteLine($"{indent}\t\t\t\t\treturn;");
                stream.WriteLine($"{indent}\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\tcatch(RemarshalException)");
                stream.WriteLine($"{indent}\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\tcontinue;");
                stream.WriteLine($"{indent}\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\tcatch(CORBA.ApplicationException aex)");
                stream.WriteLine($"{indent}\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\ttry");
                stream.WriteLine($"{indent}\t\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\t\tswitch (aex.Id)");
                stream.WriteLine($"{indent}\t\t\t\t\t\t{{");
                foreach (var ex in SetRaises)
                {
                    stream.WriteLine($"{indent}\t\t\t\t\t\t\tcase \"{ex.RepositoryId}\":");
                    stream.WriteLine($"{indent}\t\t\t\t\t\t\t\tthrow {ex.FullHelperName}.Read(aex.InputStream);");
                }
                stream.WriteLine($"{indent}\t\t\t\t\t\t\tdefault:");
                stream.WriteLine($"{indent}\t\t\t\t\t\t\t\tthrow new RuntimeException(\"Unexpected exception \" + aex.Id);");
                stream.WriteLine($"{indent}\t\t\t\t\t\t}}\t\t\t\t\t\t");
                stream.WriteLine($"{indent}\t\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\t\tfinally");
                stream.WriteLine($"{indent}\t\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\t\ttry");
                stream.WriteLine($"{indent}\t\t\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\t\t\taex.InputStream.Close();");
                stream.WriteLine($"{indent}\t\t\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\t\t\tcatch (Exception ex)");
                stream.WriteLine($"{indent}\t\t\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\t\t\tthrow new RuntimeException(\"Unexpected exception \" + ex.ToString());");
                stream.WriteLine($"{indent}\t\t\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\tfinally");
                stream.WriteLine($"{indent}\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\tif (outputStream != null)");
                stream.WriteLine($"{indent}\t\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\t\ttry");
                stream.WriteLine($"{indent}\t\t\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\t\t\toutputStream.Close();");
                stream.WriteLine($"{indent}\t\t\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\t\t\tcatch (Exception e)");
                stream.WriteLine($"{indent}\t\t\t\t\t\t{{");
                stream.WriteLine($"{indent}\t\t\t\t\t\t\tthrow new RuntimeException(\"Unexpected exception \" + e.ToString());");
                stream.WriteLine($"{indent}\t\t\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t\t\tthis._ReleaseReply(inputStream);");
                stream.WriteLine($"{indent}\t\t\t\t}}");
                stream.WriteLine($"{indent}\t\t\t}}");
                //stream.WriteLine($"{indent}\t\t\telse");
                //stream.WriteLine($"{indent}\t\t\t{{");
                //stream.WriteLine($"{indent}\t\t\t\tvar so = _ServantPreinvoke(\"_set_{Name}\", _opsType);");
                //stream.WriteLine($"{indent}\t\t\t\tif(so == null)");
                //stream.WriteLine($"{indent}\t\t\t\t\tcontinue;");
                //stream.WriteLine($"{indent}\t\t\t\tvar localServant = ({opIfzName}) so.Servant;");
                //stream.WriteLine($"{indent}\t\t\t\ttry");
                //stream.WriteLine($"{indent}\t\t\t\t{{");
                //stream.WriteLine($"{indent}\t\t\t\t\tlocalServant.{MappedName} = value;");
                //stream.WriteLine($"{indent}\t\t\t\t\tso.NormalCompletion();");                
                //stream.WriteLine($"{indent}\t\t\t\t}}");
                //stream.WriteLine($"{indent}\t\t\t\tcatch (Exception ex) ");
                //stream.WriteLine($"{indent}\t\t\t\t{{");
                //stream.WriteLine($"{indent}\t\t\t\t\tso.ExceptionalCompletion(ex);");
                //stream.WriteLine($"{indent}\t\t\t\t\tthrow;");
                //stream.WriteLine($"{indent}\t\t\t\t}}\t\t\t\t\t");
                //stream.WriteLine($"{indent}\t\t\t\tfinally");
                //stream.WriteLine($"{indent}\t\t\t\t{{");
                //stream.WriteLine($"{indent}\t\t\t\t\t_ServantPostinvoke(so);");
                //stream.WriteLine($"{indent}\t\t\t\t}}");
                //stream.WriteLine($"{indent}\t\t\t}}");
                stream.WriteLine($"{indent}\t\t}}");
                stream.WriteLine($"{indent}\t}}");
            }
            stream.WriteLine($"{indent}}}");
        }

        public void PrintPOASet(string indent, TextWriter stream)
        {
            if (SetRaises.Count > 0)
            {
                stream.WriteLine($"{indent}try");
                stream.WriteLine($"{indent}{{");
            }
            stream.WriteLine($"{indent}\toutputStream = handler.CreateReply();");
            DataType.PrintRead($"{indent}\t", stream, "inputStream", MappedName, 0);
            if (SetRaises.Count > 0)
            {
                stream.WriteLine($"{indent}}}");
                foreach (var ex in SetRaises)
                {
                    stream.WriteLine($"{indent}catch({ex.MappedType} ex)");
                    stream.WriteLine($"{indent}{{");
                    stream.WriteLine($"{indent}\toutputStream = handler.CreateExceptionReply();");
                    ex.PrintWrite($"{indent}\t", stream, "outputStream", "ex", 0);
                    stream.WriteLine($"{indent}}}");
                }
            }
        }

        public void PrintPOAGet(string indent, TextWriter stream)
        {
            if (GetRaises.Count > 0)
            {
                stream.WriteLine($"{indent}try");
                stream.WriteLine($"{indent}{{");
            }
            stream.WriteLine($"{indent}\toutputStream = handler.CreateReply();");
            DataType.PrintWrite($"{indent}\t", stream, "outputStream", MappedName, 0);
            if (GetRaises.Count > 0)
            {
                stream.WriteLine($"{indent}}}");
                foreach (var ex in GetRaises)
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
            foreach (var att in MappedAttributes)
            {
                stream.WriteLine($"{indent}{att}");
            }
            foreach (var att in DataType.MappedAttributes)
            {
                stream.WriteLine($"{indent}{att}");
            }
            stream.WriteLine($"{indent}public override {DataType.MappedType} {MappedName} ");
            stream.WriteLine($"{indent}{{");
            foreach (var ex in GetRaises)
            {
                stream.WriteLine($"{indent}\t[ThrowsIdlException(typeof({ex.MappedType}))]");
            }
            //stream.WriteLine($"{indent}\t[IdlName(\"_get_{Name}\")]");
            stream.WriteLine($"{indent}\tget");
            stream.WriteLine($"{indent}\t{{");
            stream.WriteLine($"{indent}\t\treturn _OperationsDelegate.{MappedName};");
            stream.WriteLine($"{indent}\t}}");
            if (!this.IsReadOnly)
            {
                stream.WriteLine();
                foreach (var ex in SetRaises)
                {
                    stream.WriteLine($"{indent}\t[ThrowsIdlException(typeof({ex.MappedType}))]");
                }
                //stream.WriteLine($"{indent}\t[IdlName(\"_set_{Name}\")]");
                stream.WriteLine($"{indent}\tset");
                stream.WriteLine($"{indent}\t{{");
                stream.WriteLine($"{indent}\t\t_OperationsDelegate.{MappedName} = value;");
                stream.WriteLine($"{indent}\t}}");
            }
            stream.WriteLine($"{indent}}}");
        }
    }
}
