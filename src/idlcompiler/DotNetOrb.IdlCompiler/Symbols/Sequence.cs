// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public enum SequenceType
    {
        Array,
        List
    }

    public class Sequence: IDLSymbol, ITypeSymbol
    {
        private SequenceType sequenceType;
        public SequenceType SequenceType 
        { 
            get
            {                
                var mapping = GetAnnotation("csharp_mapping");                
                if (mapping != null && mapping.Parameters.ContainsKey("sequence_type"))
                {
                    switch (mapping.Parameters["sequence_type"].ToString())
                    {
                        case "array": return SequenceType.Array;
                        case "list": return SequenceType.List;
                    }
                }
                return sequenceType;
            }
        }
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
                return "sequence";
            }
        }

        public string MappedType
        {
            get
            {
                switch (SequenceType)
                {
                    case SequenceType.List:
                        return $"List<{DataType.MappedType}>";
                    default:
                        return $"{DataType.MappedType}[]";
                }                
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
                return $"CORBA.ORB.Init().CreateSequenceTc({Length}, {DataType.TypeCodeExp})";
            }
        }

        public Sequence(ITypeSymbol dataType, SequenceType defaultSeqType, List<Annotation> annotations = null) : base("sequence", annotations)
        {
            DataType = dataType;
            sequenceType = defaultSeqType;
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

        public void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {                        
            sw.WriteLine($"{indent}{{");
            sw.WriteLine($"{indent}\tvar _capacity{iteration} = {streamName}.ReadLong();");
            sw.WriteLine($"{indent}\tif ({streamName}.Available > 0 && _capacity{iteration} > {streamName}.Available)");
            sw.WriteLine($"{indent}\t{{");
            sw.WriteLine($"{indent}\t\tthrow new Marshal($\"Sequence length too large. Only {{{streamName}.Available}} and trying to assign {{_capacity{iteration}}}\");");
            sw.WriteLine($"{indent}\t}}");                                                
            if (DataType is IBasicType basicType)
            {
                sw.WriteLine($"{indent}\tvar _array = new {DataType.MappedType}[_capacity{iteration}];");
                basicType.PrintReadArray($"{indent}\t", sw, streamName, $"_array", $"_capacity{iteration}");
                switch (SequenceType)
                {
                    case SequenceType.List:
                        sw.WriteLine($"{indent}\t{varName} = new List<{DataType.MappedType}>(_array);");
                        break;
                    default:
                        sw.WriteLine($"{indent}\t{varName} = _array;");
                        break;
                }
            }
            else
            {
                switch (SequenceType)
                {
                    case SequenceType.List:
                        sw.WriteLine($"{indent}\t{varName} = new List<{DataType.MappedType}>(_capacity{iteration});");
                        break;
                    default:
                        var type = DataType.MappedType;
                        var index = DataType.MappedType.IndexOf("[]");
                        if (index > 0)
                        {
                            type = DataType.MappedType.Substring(0, index) + $"[_capacity{iteration}]" + DataType.MappedType.Substring(index);
                        }
                        else
                        {
                            type += $"[_capacity{iteration}]";
                        }
                        sw.WriteLine($"{indent}\t{varName} = new {type};");
                        break;
                }
                sw.WriteLine($"{indent}\tfor (int i{iteration} = 0; i{iteration} < _capacity{iteration}; i{iteration}++)");
                sw.WriteLine($"{indent}\t{{");
                sw.WriteLine($"{indent}\t\t{DataType.MappedType} _item{iteration};");
                DataType.PrintRead($"{indent}\t\t", sw, streamName, $"_item{iteration}", iteration +1);                
                switch (SequenceType)
                {
                    case SequenceType.List:
                        sw.WriteLine($"{indent}\t\t{varName}.Add(_item{iteration});");
                        break;
                    default:
                        sw.WriteLine($"{indent}\t\t{varName}[i{iteration}] = _item{iteration};");
                        break;
                }
                sw.WriteLine($"{indent}\t}}");
            }
            sw.WriteLine($"{indent}}}");
        }

        public void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            sw.WriteLine($"{indent}{{");
            string len = SequenceType == SequenceType.Array ? "Length" : "Count";
            sw.WriteLine($"{indent}\t{streamName}.WriteLong({varName}.{len});");
            if (SequenceType == SequenceType.Array && DataType is IBasicType basicType)
            {
                basicType.PrintWriteArray($"{indent}\t", sw, streamName, $"{varName}", $"{varName}.{len}");
            }
            else
            {
                sw.WriteLine($"{indent}\tfor (int i{iteration} = 0; i{iteration} < {varName}.{len}; i{iteration}++)");
                sw.WriteLine($"{indent}\t{{");                
                DataType.PrintWrite($"{indent}\t\t", sw, streamName, $"{varName}[i{iteration}]", iteration + 1);
                sw.WriteLine($"{indent}\t}}");
            }
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
