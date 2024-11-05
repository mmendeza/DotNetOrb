// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class AnyType: BaseType
    {        
        public AnyType(string name, List<Annotation> annotations = null) : base(name, annotations)
        {

        }

        public override int TCKind
        {
            get
            {
                return 11;
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
                return "CORBA.Any";
            }
        }

        public override void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {streamName}.ReadAny();");
        }

        public override void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{streamName}.WriteAny({varName});");
        }

        public override void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            if (type == null)
            {
                throw new IdlCompilerException("type is null in Any.PrintExtract");
            }
            sw.WriteLine($"{indent}{varName} = ({type}) {anyName}.ExtractAny();");
        }

        public override void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.InsertAny({varName});");
        }
            
    }
}
