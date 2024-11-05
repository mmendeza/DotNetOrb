// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Map: IDLSymbol, ITypeSymbol
    {
        public ITypeSymbol KeyDataType { get; set; }
        public ITypeSymbol DataType { get; set; }
        public long Length { get; set; }

        public int TCKind
        {
            get
            {
                return 19;
            }
        }

        public string IDLType
        {
            get
            {
                return "map";
            }
        }

        public string MappedType
        {
            get
            {
                return $"IDictionary<{KeyDataType.MappedType}, {DataType.MappedType}>";
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
                return $"CORBA.ORB.Init().CreateSequenceTc({Length}, " +
                    $"CORBA.ORB.Init().CreateStructTc(\"IDL:*kvp*:1.0\", \"kvp\", new CORBA.StructMember[] {{ " +
                    $"new CORBA.StructMember(\"key\", {KeyDataType.TypeCodeExp}, null), " +
                    $"new CORBA.StructMember(\"value\", {DataType.TypeCodeExp}, null) " +
                    $"}})";
            }
        }

        public IList<String> GetMappedAttributes()
        {
            var attributes = new List<String>();
            if (Length > 0)
            {
                attributes.Add($"[MaxLength({Length})]");
            }
            return attributes;
        }

        public Map(ITypeSymbol keyType, ITypeSymbol dataType, List<Annotation> annotations = null) : base("map", annotations)
        {
            KeyDataType = keyType;
            DataType = dataType;
        }

        public void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{varName} = new Dictionary<{KeyDataType.MappedType}, {DataType.MappedType}>();");
            sw.WriteLine($"{indent}{{");
            sw.WriteLine($"{indent}\tvar _length{iteration} = {streamName}.ReadLong();");
            sw.WriteLine($"{indent}\tif ({streamName}.Available > 0 && _length{iteration} > {streamName}.Available)");
            sw.WriteLine($"{indent}\t{{;");
            sw.WriteLine($"{indent}\t\tthrow new Marshal($\"Map length too large. Only {{{streamName}.Available}} and trying to assign {{{varName}.Capacity}}\");");
            sw.WriteLine($"{indent}\t}};");                                    
            sw.WriteLine($"{indent}\tfor (int i{iteration} = 0; i{iteration} < _length{iteration}; i{iteration}++)");
            sw.WriteLine($"{indent}\t{{");
            sw.WriteLine($"{indent}\t\t{KeyDataType.MappedType} _key{iteration};");
            sw.WriteLine($"{indent}\t\t{DataType.MappedType} _value{iteration};");
            KeyDataType.PrintRead($"{indent}\t\t", sw, streamName, $"_key{iteration}", iteration + 1);
            DataType.PrintRead($"{indent}\t\t", sw, streamName, $"_value{iteration}", iteration + 1);
            sw.WriteLine($"{indent}\t\t{varName}.Add(_key{iteration}, _value{iteration});");
            sw.WriteLine($"{indent}\t}}");
            sw.WriteLine($"{indent}}}");
        }

        public void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{{");
            sw.WriteLine($"{indent}\t{streamName}.WriteLong({varName}.Count);");            
            sw.WriteLine($"{indent}\tforeach (var _kvp{iteration} in {varName})");
            sw.WriteLine($"{indent}\t{{");
            KeyDataType.PrintWrite($"{indent}\t\t", sw, streamName, $"_kvp{iteration}.Key", iteration + 1);
            DataType.PrintWrite($"{indent}\t\t", sw, streamName, $"_kvp{iteration}.Value", iteration + 1);
            sw.WriteLine($"{indent}\t}}");
            sw.WriteLine($"{indent}}}");
        }

        public void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            sw.WriteLine($"{indent}{anyName}.Type = {TypeCodeExp};");
            sw.WriteLine($"{indent}var outputStream = {anyName}.CreateOutputStream();");
            PrintWrite(indent, sw, "outputStream", varName, 0);
        }

        public void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            sw.WriteLine($"{indent}var inputStream = {anyName}.CreateInputStream();");
            sw.WriteLine($"{indent}try");
            sw.WriteLine($"{indent}{{");
            PrintRead(indent + "\t", sw, "inputStream", varName, 0);
            sw.WriteLine($"{indent}}}");
            sw.WriteLine($"{indent}finally");
            sw.WriteLine($"{indent}{{");
            sw.WriteLine($"{indent}\tinputStream.Close();");
            sw.WriteLine($"{indent}}}");
        }
    }
}