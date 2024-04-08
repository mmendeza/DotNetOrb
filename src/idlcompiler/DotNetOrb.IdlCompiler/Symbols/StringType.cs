// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class StringType: IDLSymbol, ITypeSymbol
    {

        public long Length { get; set; }

        public int TCKind
        {
            get
            {
                return 18;
            }
        }

        public string IDLType
        {
            get
            {
                return "string";
            }
        }

        public string MappedType
        {
            get
            {
                return "string";
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
                return $"CORBA.ORB.Init().CreateStringTc({Length})";
            }
        }

        public StringType(long length, bool dotNetNaming, List<Annotation> annotations = null) : base("string", dotNetNaming, annotations)
        {
            Length = length;
        }

        public StringType(bool dotNetNaming, List<Annotation> annotations = null) : base("string", dotNetNaming, annotations)
        {
        }

        public override List<string> MappedAttributes
        {
            get
            {
                var ret = base.MappedAttributes;
                ret.Add($"[WideChar(false)]");
                if (Length > 0)
                {
                    ret.Add($"[StringLength({Length})]");
                }
                return ret;
            }
        }

        public void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {streamName}.ReadString();");
        }

        public void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{streamName}.WriteString({varName});");
        }

        public void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type)
        {
            sw.WriteLine($"{indent}{varName} = {anyName}.ExtractString();");
        }

        public void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.InsertString({varName});");
        }

    }
}
