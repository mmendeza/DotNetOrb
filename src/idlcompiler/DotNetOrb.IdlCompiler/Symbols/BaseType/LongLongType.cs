// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class LongLongType: BaseType, IBasicType
    {
        public bool IsUnsigned
        {
            get
            {
                switch (IDLType)
                {
                    case BaseType.UINT64:
                    case BaseType.UNSIGNED_LONG_LONG: return true;
                }
                return false;
            }
        }
        public LongLongType(string name, bool dotNetNaming, List<Annotation> annotations = null) : base(name, dotNetNaming, annotations)
        {

        }

        public override int TCKind
        {
            get
            {
                if (IsUnsigned)
                {
                    return 24;
                }
                else
                {
                    return 23;
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
                    return "ulong";
                }
                else
                {
                    return "long";
                }
            }
        }

        public override void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {streamName}.{(IsUnsigned ? "ReadULongLong();" : "ReadLongLong();")}");
        }

        public override void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsUnsigned ? "WriteULongLong" : "WriteLongLong")}({varName});");
        }

        public override void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            sw.WriteLine($"{indent}{varName} = {anyName}.{(IsUnsigned ? "ExtractULongLong();" : "ExtractLongLong();")}");
        }

        public override void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.{(IsUnsigned ? "InsertULongLong" : "InsertLongLong")}({varName});");
        }

        public void PrintReadArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsUnsigned ? "ReadULongLongArray" : "ReadLongLongArray")}(ref {varName}, 0, {length});");
        }

        public void PrintWriteArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsUnsigned ? "WriteULongLongArray" : "WriteLongLongArray")}({varName}, 0, {length});");
        }

    }
}
