// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class ShortType: BaseType, IBasicType
    {
        public bool IsUnsigned
        {
            get
            {
                switch (IDLType)
                {
                    case BaseType.UINT16:
                    case BaseType.UNSIGNED_SHORT: return true;
                }
                return false;
            }
        }
        public ShortType(string name, List<Annotation> annotations = null) : base(name, annotations)
        {

        }

        public override int TCKind
        {
            get
            {
                if (IsUnsigned)
                {
                    return 2;
                }
                else
                {
                    return 4;
                }
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
                    return "ushort";
                }
                else
                {
                    return "short";
                }
            }
        }

        public override void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {streamName}.{(IsUnsigned ? "ReadUShort();" : "ReadShort();")}");
        }

        public override void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsUnsigned ? "WriteUShort" : "WriteShort")}({varName});");
        }

        public override void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            sw.WriteLine($"{indent}{varName} = {anyName}.{(IsUnsigned ? "ExtractUShort();" : "ExtractShort();")}");
        }

        public override void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.{(IsUnsigned ? "InsertUShort" : "InsertShort")}({varName});");
        }
        public void PrintReadArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsUnsigned ? "ReadUShortArray" : "ReadShortArray")}(ref {varName}, 0, {length});");
        }

        public void PrintWriteArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsUnsigned ? "WriteUShortArray" : "WriteShortArray")}({varName}, 0, {length});");
        }
    }
}
