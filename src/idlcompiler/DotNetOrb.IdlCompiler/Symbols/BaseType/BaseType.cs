// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public abstract class BaseType : IDLSymbol, ITypeSymbol
    {
        public const string INT8 = "int8";
        public const string UINT8 = "uint8";
        public const string SHORT = "short";
        public const string INT16 = "int16";
        public const string UNSIGNED_SHORT = "unsigned short";
        public const string UINT16 = "uint16";
        public const string LONG = "long";
        public const string INT32 = "int32";
        public const string UNSIGNED_LONG = "unsigned long";
        public const string UINT32 = "uint32";
        public const string LONG_LONG = "long long";
        public const string INT64 = "int64";
        public const string UNSIGNED_LONG_LONG = "unsigned long long";
        public const string UINT64 = "uint64";
        public const string FLOAT = "float";
        public const string DOUBLE = "double";
        public const string LONG_DOUBLE = "long double";
        public const string CHAR = "char";
        public const string WCHAR = "wchar";
        public const string BOOLEAN = "boolean";
        public const string OCTET = "octet";
        public const string OBJECT = "Object";
        public const string VALUE_BASE = "ValueBase";
        public const string ANY = "any";
        public const string NATIVE = "native";
        public const string TYPECODE = "TypeCode";


        protected BaseType(string name, List<Annotation> annotations = null) : base(name, annotations)
        {
        }

        public static BaseType CreateBaseType(string name, List<Annotation> annotations = null)
        {
            name = Regex.Replace(name, @"\s+", " ").Trim();
            switch (name)
            {
                case INT8:
                    return new SignedByteType(name, annotations);
                case UINT8:
                case OCTET:
                    return new OctetType(name, annotations);
                case SHORT:
                case UNSIGNED_SHORT:                    
                case INT16:
                case UINT16:
                    return new ShortType(name, annotations);
                case LONG:                    
                case INT32:                    
                case UNSIGNED_LONG:                    
                case UINT32:
                    return new LongType(name, annotations);
                case LONG_LONG:                    
                case INT64:                    
                case UNSIGNED_LONG_LONG:                    
                case UINT64:
                    return new LongLongType(name, annotations);
                case FLOAT:
                    return new FloatType(name, annotations);
                case DOUBLE:
                    return new DoubleType(name, annotations);
                case LONG_DOUBLE:
                    //throw new IdlCompilerException("long double not supported");
                    return new LongDoubleType(name, annotations);
                case CHAR:                    
                case WCHAR:
                    return new CharType(name, annotations);
                case BOOLEAN:
                    return new BooleanType(name, annotations);
                case OBJECT:
                    return new ObjectType(name, annotations);
                case VALUE_BASE:
                    return new ValueBaseType(name, annotations);
                case ANY:
                    return new AnyType(name, annotations);
                case NATIVE:
                    return new NativeType(name, annotations);
                case TYPECODE:
                    return new TypeCodeType(name, annotations);
                default:
                    throw new IdlCompilerException("Unknown base type");
            }
        }

        public abstract int TCKind { get; }

        public abstract string IDLType { get; }

        public abstract string MappedType { get; }

        public virtual string HelperName => throw new NotImplementedException();

        public virtual string FullHelperName => throw new NotImplementedException();

        public virtual string TypeCodeExp 
        { 
            get
            {
                return $"CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) {TCKind})";
            }
        }

        public override string RepositoryId
        {
            get
            {
                return "IDL:*primitive*:1.0";
            }
        }

        public abstract void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration);

        public abstract void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration);

        public abstract void PrintInsert(string indent, TextWriter sw, string anyName, string varName);
        
        public abstract void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null);
        
    }
}
