// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class CharType: BaseType, IBasicType
    {
        public bool IsWide
        {
            get
            {
                switch (IDLType)
                {
                    case BaseType.WCHAR: return true;
                }
                return false;
            }
        }
        public CharType(string name, List<Annotation> annotations = null) : base(name, annotations)
        {

        }

        public override int TCKind
        {
            get
            {
                if (IsWide)
                {
                    return 26;
                }
                else
                {
                    return 9;
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
                return "char";
            }            
        }

        public override void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {streamName}.{(IsWide ? "ReadWChar();" : "ReadChar();")}");
        }

        public override void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsWide ? "WriteWChar" : "WriteChar")}({varName});");
        }

        public override void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            sw.WriteLine($"{indent}{varName} = {anyName}.{(IsWide ? "ExtractWChar();" : "ExtractChar();")}");
        }

        public override void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.{(IsWide ? "InsertWChar" : "InsertChar")}({varName});");
        }

        public void PrintReadArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsWide ? "ReadWCharArray" : "ReadCharArray")}(ref {varName}, 0, {length});");
        }

        public void PrintWriteArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.{(IsWide ? "WriteWCharArray" : "WriteCharArray")}({varName}, 0, {length});");
        }

    }
}
