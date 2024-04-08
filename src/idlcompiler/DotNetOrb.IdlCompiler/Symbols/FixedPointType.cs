// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class FixedPointType: IDLSymbol, ITypeSymbol
    {  
        public long TotalDigits { get; set; }
        public long FractionalDigits { get; set; }
        public int TCKind
        {
            get
            {
                return 28;
            }
        }

        public string IDLType
        {
            get
            {
                return "fixed";
            }
        }

        public string MappedType
        {
            get
            {
                return "Decimal";
            }
        }
        public string HelperName => throw new NotImplementedException();

        public string FullHelperName => throw new NotImplementedException();

        public string TypeCodeExp
        {
            get
            {
                return $"ORB.Init().CreateFixedTc({TotalDigits}, {FractionalDigits})";
            }
        }

        public override string RepositoryId
        {
            get
            {
                return "IDL:*fixed*:1.0";
            }
        }

        public FixedPointType(long totalDigits, long fractionalDigits, bool dotNetNaming, List<Annotation> annotations = null) : base("fixed", dotNetNaming, annotations)
        {
            TotalDigits = totalDigits;
            FractionalDigits = fractionalDigits;            
        }

        public void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = {streamName}.ReadFixed({TotalDigits}, {FractionalDigits});");
        }

        public void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{streamName}.WriteFixed({varName}, {TotalDigits}, {FractionalDigits});");
        }

        public void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {            
            sw.WriteLine($"{indent}{varName} = {anyName}.ExtractFixed();");
        }

        public void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {            
            sw.WriteLine($"{indent}{anyName}.InsertFixed({varName});");
        }

        public void PrintReadArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {            
            sw.WriteLine($"{indent}{streamName}.ReadFixedArray(ref {varName}, 0, {length});");
        }

        public void PrintWriteArray(string indent, TextWriter sw, string streamName, string varName, string length)
        {            
            sw.WriteLine($"{indent}{streamName}.WriteFixedArray({varName}, 0, {length});");
        }
    }
}
