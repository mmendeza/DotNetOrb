// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class DoubleType: BaseType, IBasicType
    {        
        public DoubleType(string name, List<Annotation> annotations = null) : base(name, annotations)
        {

        }

        public override int TCKind
        {
            get
            {
                return 7;
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
                return "double";
            }
        }

        public override void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {streamName}.ReadDouble();");
        }

        public override void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{streamName}.WriteDouble({varName});");
        }

        public override void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            sw.WriteLine($"{indent}{varName} = {anyName}.ExtractDouble();");
                
        }

        public override void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.InsertDouble({varName});");
        }

        public void PrintReadArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.ReadDoubleArray(ref {varName}, 0, {length});");
        }

        public void PrintWriteArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {
            sw.WriteLine($"{indent}{streamName}.WriteDoubleArray({varName}, 0, {length});");
        }

    }
}
