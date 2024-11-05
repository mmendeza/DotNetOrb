// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class OctetType: BaseType, IBasicType
    {
        public bool IsUnsigned
        {
            get
            {
                switch (IDLType)
                {
                    case BaseType.OCTET:
                    case BaseType.UINT8: return true;                    
                }
                return false;
            }
        }

        public OctetType(string name, List<Annotation> annotations = null) : base(name, annotations)
        {

        }

        public override int TCKind
        {
            get
            {
                return 10;                
            }            
        }

        public override string IDLType
        {
            get
            {
                return Name;
            }
        }

        public override string MappedType
        {
            get
            {
                if (IsUnsigned)
                {
                    return "byte";
                }
                else
                {
                    return "sbyte";
                }                
            }            
        }

        public override void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {(IsUnsigned ? "" : "(sbyte) ")}{streamName}.ReadOctet();");
        }

        public override void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{streamName}.WriteOctet({(IsUnsigned ? "" : "(byte) ")}{varName});");
        }

        public override void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            sw.WriteLine($"{indent}{varName} = {(IsUnsigned ? "" : "(sbyte) ")}{anyName}.ExtractOctet();");
        }

        public override void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.InsertOctet({(IsUnsigned ? "" : "(byte) ")}{varName});");
        }

        public void PrintReadArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.ReadOctetArray(ref {varName}, 0, {length});");
        }

        public void PrintWriteArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.WriteOctetArray({varName}, 0, {length});");
        }
    }
}
