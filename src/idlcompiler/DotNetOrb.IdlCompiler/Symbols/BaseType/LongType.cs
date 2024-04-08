// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class LongType: BaseType, IBasicType
    {
        public bool IsUnsigned
        {
            get
            {
                switch (IDLType)
                {
                    case BaseType.UINT32:
                    case BaseType.UNSIGNED_LONG: return true;
                }
                return false;
            }
        }
        public LongType(string name, bool dotNetNaming, List<Annotation> annotations = null) : base(name, dotNetNaming, annotations)
        {

        }

        public override int TCKind
        {
            get
            {
                if (IsUnsigned)
                {
                    return 5;
                }
                else
                {
                    return 3;
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
                    return "uint";
                }
                else
                {
                    return "int";
                }
            }
        }

        public override void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {streamName}.{(IsUnsigned ? "ReadULong();" : "ReadLong();")}");
        }

        public override void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsUnsigned ? "WriteULong" : "WriteLong")}({varName});");
        }

        public override void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            sw.WriteLine($"{indent}{varName} = {anyName}.{(IsUnsigned ? "ExtractULong();" : "ExtractLong();")}");
        }

        public override void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.{(IsUnsigned ? "InsertULong" : "InsertLong")}({varName});");
        }
        public void PrintReadArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsUnsigned ? "ReadULongArray" : "ReadLongArray")}(ref {varName}, 0, {length});");
        }

        public void PrintWriteArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsUnsigned ? "WriteULongArray" : "WriteLongArray")}({varName}, 0, {length});");
        }

    }
}
