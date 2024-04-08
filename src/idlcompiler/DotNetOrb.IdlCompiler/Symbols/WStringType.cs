// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class WStringType: IDLSymbol, ITypeSymbol
    {

        public long Length { get; set; }

        public int TCKind
        {
            get
            {
                return 27;
            }
        }

        public string IDLType
        {
            get
            {
                return "wstring";
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

        public WStringType(long length, bool dotNetNaming, List<Annotation> annotations = null) : base("wstring", dotNetNaming, annotations)
        {
            Length = length;
        }

        public WStringType(bool dotNetNaming, List<Annotation> annotations = null) : base("wstring", dotNetNaming, annotations)
        {
        }

        public override List<string> MappedAttributes
        {
            get
            {
                var ret = base.MappedAttributes;
                ret.Add($"[WideChar(true)]");
                if (Length > 0)
                {
                    ret.Add($"[StringLength({Length})]");
                }
                return ret;
            }
        }

        public void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {streamName}.ReadWString();");
        }

        public void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{streamName}.WriteWString({varName});");
        }

        public void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type)
        {
            sw.WriteLine($"{indent}{varName} = {anyName}.ExtractWString();");
        }

        public void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.InsertWString({varName});");
        }

    }
}
