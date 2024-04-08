// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CORBA
{
    public class TypeCode
    {
        protected static ORB orb = ORB.Init();

        protected TCKind kind;
        public virtual TCKind Kind
        {
            get
            {
                return kind;
            }            
        }

        protected static TypeCode[] primitiveTypeCodes = new TypeCode[34];

        protected static Dictionary<Type, TypeCode> primitiveTypeCodesDict = new Dictionary<Type, TypeCode>();

        //public virtual bool IsSimple { get => true; }

        public virtual bool IsPrimitive
        {
            get
            {
                return (primitiveTypeCodes[(int)kind] != null);
            }
        }

        /// <summary>
        /// return the content type if the argument is an alias, or the argument itself otherwise
        /// </summary>
        public virtual TypeCode OriginalTypeCode
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// returns a new TypeCode with all type and member information removed.
        /// RepositoryID and alias are preserved. 
        /// This method effectively clones the original typecode 
        /// Simpler than trying to work out what type so what to duplicate(and compact).
        /// </summary>
        public virtual TypeCode CompactTypeCode
        {
            get
            {
                return this;
            }
        }

        static TypeCode()
        {
            //statically create primitive TypeCodes for fast lookup
            for(int i = 0; i <= 13; i++ )
            {
                primitiveTypeCodes[i] = new TypeCode(i);
            }
            for(int i = 23; i <= 26; i++ )
            {
                primitiveTypeCodes[i] = new TypeCode(i);
            }
            primitiveTypeCodes[(int)TCKind.TkString] = new StringTypeCode(0);
            primitiveTypeCodes[(int)TCKind.TkWstring] = new WStringTypeCode(0);
            primitiveTypeCodes[(int)TCKind.TkFixed] = new FixedTypeCode(1, 0);

            // Sun's ValueHandler in JDK 1.3 and 1.4 calls
            // ORB.get_primitive_tc() for TCKind.Tk_tk_objref and TCKind.Tk_tk_value.
            // These don't exactly look "primitive" to us, but as a courtesy to
            // Sun we provide the following bogus TypeCode objects so that
            // we can return something in these cases.
            primitiveTypeCodes[(int) TCKind.TkObjref] = new ObjectReferenceTypeCode("IDL:omg.org/CORBA/Object:1.0", "Object");
            primitiveTypeCodes[(int) TCKind.TkValue] = new ValueTypeCode("IDL:omg.org/CORBA/portable/ValueBase:1.0", "ValueBase", ValueModifier.None, null, new CORBA.ValueMember[0]);

            primitiveTypeCodesDict.Add(typeof(bool), primitiveTypeCodes[(int)TCKind.TkBoolean]);
            primitiveTypeCodesDict.Add(typeof(char), primitiveTypeCodes[(int)TCKind.TkWchar]);
            primitiveTypeCodesDict.Add(typeof(byte), primitiveTypeCodes[(int)TCKind.TkOctet]);
            primitiveTypeCodesDict.Add(typeof(short), primitiveTypeCodes[(int)TCKind.TkShort]);
            primitiveTypeCodesDict.Add(typeof(ushort), primitiveTypeCodes[(int)TCKind.TkUshort]);
            primitiveTypeCodesDict.Add(typeof(int), primitiveTypeCodes[(int)TCKind.TkLong]);
            primitiveTypeCodesDict.Add(typeof(uint), primitiveTypeCodes[(int)TCKind.TkUlong]);
            primitiveTypeCodesDict.Add(typeof(long), primitiveTypeCodes[(int)TCKind.TkLonglong]);
            primitiveTypeCodesDict.Add(typeof(ulong), primitiveTypeCodes[(int)TCKind.TkUlonglong]);
            primitiveTypeCodesDict.Add(typeof(float), primitiveTypeCodes[(int)TCKind.TkFloat]);
            primitiveTypeCodesDict.Add(typeof(double), primitiveTypeCodes[(int)TCKind.TkDouble]);
        }

        public static TypeCode GetPrimitiveTypeCode(TCKind kind)
        {
            return GetPrimitiveTypeCode((int)kind);
        }

        public static TypeCode GetPrimitiveTypeCode(int kind)
        {
            if (primitiveTypeCodes[kind] == null)
            {
                throw new BadParam("No primitive TypeCode for kind " + kind);
            }
            return primitiveTypeCodes[kind];
        }



        /// <summary>
        /// Resolve any recursive TypeCodes contained within this TypeCode.
        /// </summary>
        /// <param name="actual">The actual (non-recursive) TypeCode that replaces any 
        /// recursive TypeCodes contained in the actual TypeCode that have the same RepositoryId</param>
        protected void ResolveRecursion(TypeCode actual)
        {
            try
            {
                for (int i = 0; i < MemberCount; i++)
                {
                    var typeCode = MemberType(i).OriginalTypeCode;
                    if (typeCode is RecursiveTypeCode rtc)
                    {
                        if (typeCode.Id.Equals(actual.Id))
                        {
                            rtc.ActualTypeCode = actual;
                        }
                    }
                    else if (typeCode is TypeCode tc)
                    {
                        switch (typeCode.Kind)
                        {
                            case TCKind.TkStruct:
                            case TCKind.TkUnion:
                            case TCKind.TkValue:
                                {
                                    tc.ResolveRecursion(actual);
                                    break;
                                }
                            case TCKind.TkSequence:
                                {
                                    typeCode = tc.ContentType.OriginalTypeCode;
                                    if (typeCode is RecursiveTypeCode rtc1)
                                    {
                                        if (typeCode.Id.Equals(actual.Id))
                                        {
                                            rtc1.ActualTypeCode = actual;
                                        }
                                    }
                                    else if (typeCode is TypeCode tc1)
                                    {
                                        tc1.ResolveRecursion(actual);
                                    }
                                    break;
                                }
                        }                                    
                    }
                }                            
            }
            catch (Bounds)
            {

            }
            catch (BadKind)
            {

            }
        }

        public static TypeCode CreateTypeCode(Type type)
        {
            return CreateTypeCode(type, new Dictionary<Type, TypeCode>());
        }

        private static TypeCode CreateTypeCode(Type type, Dictionary<Type,TypeCode> knownTypes)
        {
            if (type.IsPrimitive)
            {
                return primitiveTypeCodesDict[type];
            }
            else if (knownTypes.ContainsKey(type))
            {
                // recursive type code
                var newTypeCode = new RecursiveTypeCode(GetRepositoryId(type));
                newTypeCode.ActualTypeCode = knownTypes[type];
                return newTypeCode;
            }
            else if (type.IsArray)
            {
                // array is mapped to a valuebox containing an IDL sequence
                var newTypeCode = new ValueBoxTypeCode(GetRepositoryId(type), "NET_Array",
                                 new SequenceTypeCode(CreateTypeCode(type.GetElementType(), knownTypes), 0));                                              
                return newTypeCode;
            }
            else if (typeof(CORBA.Object).IsAssignableFrom(type))
            {
                return new ObjectReferenceTypeCode(GetRepositoryId(type), type.Name);
            }
            else if (typeof(IIDLEntity).IsAssignableFrom(type))
            {
                //an IDL entity has a helper class with a static method Type()
                var helperType = GetHelperType(type);
                if (helperType != null)
                {
                    try
                    {
                        var helper = Activator.CreateInstance(helperType);
                        var methodInfo = helperType.GetMethod("Type");
                        if (methodInfo != null)
                        {                            
                            var result = (TypeCode) methodInfo.Invoke(helper, null);
                            return result;
                        }
                        else
                        {
                            throw new ArgumentException($"Cannot create TypeCode for class {type.FullName}: Method Type() not found in helper class");
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ArgumentException($"Cannot create TypeCode for class {type.FullName}: {e.Message}");
                    }
                }
                else
                {
                    throw new ArgumentException($"Cannot create TypeCode for class {type.FullName}: Helper class not found");                    
                }
            }
            else
            {
                throw new ArgumentException($"Cannot create TypeCode for class {type.FullName}: class is not an IDL Entity");
            }
        }

        private static string GetRepositoryId(Type type)
        {
            if (typeof(IIDLEntity).IsAssignableFrom(type))
            {
                var idAtt = type.GetCustomAttributes(typeof(RepositoryIDAttribute)).FirstOrDefault() as RepositoryIDAttribute;
                if (idAtt != null)
                {
                    return idAtt.Id;
                }                
            }
            throw new CORBA.IntfRepos("Repository id not found for type: " + type.FullName);
        }

        private static Type GetHelperType(Type type)
        {
            var helperAtt = (HelperAttribute)type.GetCustomAttribute(typeof(HelperAttribute));
            if (helperAtt != null)
            {
                return helperAtt.HelperType;
            }
            return null;
        }       

        protected TypeCode()
        {
            kind = TCKind.TkNull;
        }

        public TypeCode(int tcKind)
        {
            kind = (TCKind) tcKind;
        }

        public TypeCode(TCKind tcKind)
        {
            kind = tcKind;
        }
        
        public virtual bool Equal(TypeCode tc)
        {
            try
            {
                if (tc is RecursiveTypeCode rtc)
                {
                    return Equal(rtc.ActualTypeCode);
                }
                return (kind == tc.Kind);
            }
            catch (Bounds)
            {
                return false;
            }
            catch (BadKind)
            { 
                return false; 
            }            
        }

        public virtual bool Equivalent(TypeCode tc)
        {
            try
            {
                if (tc is RecursiveTypeCode rtc)
                {
                    return Equivalent(rtc.ActualTypeCode);
                }
                if (tc is AliasTypeCode tcAlias)
                {
                    return Equivalent(tcAlias.ContentType);
                }
                if (kind != tc.Kind)
                {
                    return false;
                }
                return true;
            }
            catch (Bounds)
            {
                return false;
            }
            catch (BadKind)
            {
                return false;
            }
        }

        public override bool Equals(object? obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (!(obj is TypeCode))
            {
                return false;
            }

            return Equal((TypeCode) obj);
        }

        public override int GetHashCode()
        {            
            // for primitive typecodes
            return base.GetHashCode();
        }

        public virtual void RemarshalValue(IInputStream input, IOutputStream output)
        {            
            try
            {
                switch (kind)
                {
                    case TCKind.TkNull:       // 0
                    case TCKind.TkVoid:       // 1                        
                        break;
                    case TCKind.TkShort:      // 2
                        output.WriteShort(input.ReadShort());
                        break;
                    case TCKind.TkLong:       // 3
                        output.WriteLong(input.ReadLong());
                        break;
                    case TCKind.TkUshort:     // 4
                        output.WriteUShort(input.ReadUShort());
                        break;
                    case TCKind.TkUlong:      // 5
                        output.WriteULong(input.ReadULong());
                        break;
                    case TCKind.TkFloat:      // 6
                        output.WriteFloat(input.ReadFloat());
                        break;
                    case TCKind.TkDouble:     // 7
                        output.WriteDouble(input.ReadDouble());
                        break;
                    case TCKind.TkBoolean:    // 8
                        output.WriteBoolean(input.ReadBoolean());
                        break;
                    case TCKind.TkChar:       // 9
                        output.WriteChar(input.ReadChar());
                        break;
                    case TCKind.TkOctet:      // 10
                        output.WriteOctet(input.ReadOctet());
                        break;
                    case TCKind.TkAny:        // 11                        
                        output.WriteAny(input.ReadAny());
                        break;
                    case TCKind.TkTypeCode:   // 12
                        output.WriteTypeCode(input.ReadTypeCode());
                        break;
                    case TCKind.TkPrincipal:  // 13
                        throw new NoImplement("Principal deprecated");
                    case TCKind.TkLonglong:   // 23
                        output.WriteLongLong(input.ReadLongLong());
                        break;
                    case TCKind.TkUlonglong:  // 24                        
                        output.WriteULongLong(input.ReadULongLong());
                        break;
                    case TCKind.TkLongdouble: // 25                        
                        throw new BadTypeCode("type longdouble not supported");
                    case TCKind.TkWchar:      // 26                        
                        output.WriteWChar(input.ReadWChar());                        
                        break;
                    default:
                        {
                            throw new CORBA.Marshal("Cannot handle TypeCode with kind " + kind);
                        }
                }
            }
            catch (BadKind ex)
            {
                throw new CORBA.Marshal("When processing TypeCode with kind: " + kind + " caught " + ex);
            }
            catch (Bounds ex)
            {
                throw new CORBA.Marshal("When processing TypeCode with kind: " + kind + " caught " + ex);
            }
        }
        
        public static TypeCode ReadTypeCode(IInputStream inputStream, IDictionary<int, string> recursiveTCMap, IDictionary<int,TypeCode> repeatedTCMap)
        {                      
            var kind = inputStream.ReadLong();
            var startPos = inputStream.Pos - 4;

            if (kind == -1)
            {
                return RecursiveTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);

            }
            switch ((TCKind)kind)
            {                
                case TCKind.TkAbstractInterface:
                    return AbstractInterfaceTypeCode.ReadTypeCode(inputStream,recursiveTCMap,repeatedTCMap, startPos);
                case TCKind.TkAlias:
                    return AliasTypeCode.ReadTypeCode(inputStream,recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkExcept:
                    return ExceptionTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkArray:
                    return ArrayTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkEnum:
                    return EnumTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkFixed:
                    return FixedTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkLocalInterface:
                    return LocalInterfaceTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkObjref:
                    return ObjectReferenceTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkSequence:
                    return SequenceTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkString:
                    return StringTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkWstring:
                    return WStringTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkStruct:
                    return StructTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkUnion:
                    return UnionTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkValueBox:
                    return ValueBoxTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                case TCKind.TkValue:
                    return ValueTypeCode.ReadTypeCode(inputStream, recursiveTCMap, repeatedTCMap, startPos);
                default:
                    {
                        return GetPrimitiveTypeCode(kind);
                    }
            }            
        }

        public virtual void WriteTypeCode(IOutputStream outputStream, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap)
        {
            outputStream.WriteLong((int) kind);
        }      

        public virtual TypeCode ConcreteBaseType
        {
            get
            {
                throw new BadKind();
            }            
        }

        public virtual TypeCode ContentType
        {
            get
            {
                throw new BadKind();
            }            
        }

        public virtual int DefaultIndex
        {
            get
            {
                throw new BadKind();
            }            
        }

        public virtual TypeCode DiscriminatorType
        {
            get
            {
                throw new BadKind();
            }            
        }

        public virtual ushort FixedDigits
        {
            get
            {
                throw new BadKind();
            }            
        }

        public virtual ushort FixedScale
        {
            get
            {
                throw new BadKind();
            }
        }

        public virtual string Id
        {
            get
            {
                throw new BadKind();
            }            
        }
        public virtual string Name
        {
            get
            {
                throw new BadKind();
            }            
        }

        public virtual ValueModifier TypeModifier
        {
            get
            {
                throw new BadKind();
            }            
        }

        public virtual int Length
        {
            get
            {
                throw new BadKind();
            }            
        }

        public virtual int MemberCount
        {
            get
            {
                throw new BadKind();
            }            
        }

        public virtual CORBA.Any MemberLabel(int index)
        {
            throw new BadKind();
        }

        public virtual string MemberName(int index)
        {
            throw new BadKind();
        }

        public virtual TypeCode MemberType(int index)
        {
            throw new BadKind();
        }

        public virtual short MemberVisibility(int index)
        {
            throw new BadKind();
        }

    }
}
