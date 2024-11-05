// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class ArrayType: IDLSymbol, ITypeSymbol
    {
        public ITypeSymbol DataType { get; set; }        
        public List<long> Dimensions { get; set; }    

        public int TCKind
        {
            get
            {
                return 20;
            }
        }

        public string IDLType
        {
            get
            {
                return DataType.FullName + "[]";
            }
        }

        public string MappedType
        {
            get
            {
                return DataType.MappedType + "[]";
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


        public ArrayType(List<Annotation> annotations = null) : base("array", annotations)
        {
            Dimensions = new List<long>();
        }

        public string TypeCodeExp
        {
            get
            {                
                return GetTypeCodeExpression(0);
            }            
        }

        private string GetTypeCodeExpression(int index)
        {
            if (index == Dimensions.Count - 1)
            {
                return $"CORBA.ORB.Init().CreateArrayTc({Dimensions[index]}, {DataType.TypeCodeExp})";
            }
            else
            {
                return $"CORBA.ORB.Init().CreateArrayTc({Dimensions[index]}, {GetTypeCodeExpression(index + 1)})";
            }
        }

        public void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            PrintReadDimension(indent, sw, streamName, varName, 0);
        }

        private void PrintReadDimension(string indent, TextWriter sw, string streamName, string varName, int index)
        {
            sw.Write($"{indent}{varName}");
            for (int i = 0; i < index; i++)
            {
                sw.Write($"[i{i}]");
            }

            sw.Write($" = new {DataType.MappedType}[{Dimensions[index]}]");
            for (int i = index + 1; i < Dimensions.Count; i++)
            {
                sw.Write("[]");
            }            
            sw.WriteLine(";");
            sw.WriteLine($"{indent}{{");
            sw.WriteLine($"{indent}\tfor (int i{index} = 0; i{index} < {Dimensions[index]}; i{index}++)");
            sw.WriteLine($"{indent}\t{{");
            if (index == Dimensions.Count - 1)
            {
                var name = varName;
                for (int i = 0; i <= index; i++)
                {
                    name += $"[i{i}]";
                }
                DataType.PrintRead(indent + "\t\t",sw, streamName, name, 0);
            }
            else
            {
                PrintReadDimension(indent + "\t\t", sw, streamName, varName, index + 1);
            }
            sw.WriteLine($"{indent}\t}}");
            sw.WriteLine($"{indent}}}");
        }

        public void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            PrintWriteDimension(indent, sw, streamName, varName, 0);
        }

        private void PrintWriteDimension(string indent, TextWriter sw, string streamName, string varName, int index)
        {
            var len = varName;
            for (int i = 0; i < index; i++)
            {
                len += $"[i{i}]";
            }
            len += ".Length";
            sw.Write($"{indent}if ({len} < {Dimensions[index]})");
            sw.WriteLine($"{indent}{{");
            sw.WriteLine($"{indent}\tthrow new CORBA.Marshal($\"Incorrect array size {{{len}}}, expecting {Dimensions[index]}\");");
            sw.WriteLine($"{indent}}}");
            sw.WriteLine($"{indent}for (int i{index} = 0; i{index} < {Dimensions[index]}; i{index}++)");
            sw.WriteLine($"{indent}{{");
            if (index == Dimensions.Count - 1)
            {
                var name = varName;
                for (int i = 0; i <= index; i++)
                {
                    name += $"[i{i}]";
                }
                DataType.PrintWrite(indent + "\t", sw, streamName, name, 0);
            }
            else
            {
                PrintWriteDimension(indent + "\t", sw, streamName, varName, index + 1);
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
