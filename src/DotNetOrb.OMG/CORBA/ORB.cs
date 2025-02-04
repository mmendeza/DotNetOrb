// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using PortableServer;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CORBA
{
    abstract public class ORB
    {
        protected static readonly string DEFAULT_ORB_ASSEMBLY = "DotNetOrb.Core";
        protected static readonly string DEFAULT_ORB_CLASS = "DotNetOrb.Core.ORB";

        protected static object InitMutex = new object();

        protected static ORB singleton = null;

        public static ORB Init()
        {
            lock (InitMutex)
            {
                if (singleton == null)
                {
                    var orbAssembly = Assembly.Load(DEFAULT_ORB_ASSEMBLY);
                    var type = orbAssembly.GetType(DEFAULT_ORB_CLASS);
                    if (type != null)
                    {
                        try
                        {
                            singleton = (ORB)Activator.CreateInstance(type);
                        }
                        catch (Exception e)
                        {
                            throw new Internal("Exception in orb instantiation: " + e);
                        }
                    }
                }
                return singleton;                
            }
        }

        public static ORB Init(Dictionary<string, string> props)
        {
            lock (InitMutex)
            {
                if (singleton == null)
                {
                    var orbAssembly = Assembly.Load(DEFAULT_ORB_ASSEMBLY);
                    var type = orbAssembly.GetType(DEFAULT_ORB_CLASS);
                    if (type != null)
                    {
                        try
                        {
                            singleton = (ORB)Activator.CreateInstance(type, props);
                        }
                        catch (Exception e)
                        {
                            throw new Internal("Exception in orb instantiation: " + e);
                        }
                    }
                }
                return singleton;
            }
        }

        public abstract string Id { get; }        
        abstract public string ObjectToString(CORBA.IObject obj);
        abstract public CORBA.Object StringToObject(string str);
        // Dynamic Invocation related operations
        abstract public NVList CreateList(int count);
        abstract public NVList CreateOperationList(CORBA.Object oper);
        abstract public NamedValue CreateNamedValue(string s, Any any, uint flags);
        abstract public ExceptionList CreateExceptionList();
        abstract public ContextList CreateContextList();
        abstract public IContext GetDefaultContext();
        abstract public Environment CreateEnvironment();
        abstract public IOutputStream CreateOutputStream();
        abstract public void SendMultipleRequestsOneway(IRequest[] req);
        abstract public void SendMultipleRequestsDeferred(IRequest[] req);
        abstract public bool PollNextResponse();

        [ThrowsIdlException(typeof(WrongTransaction))]
        abstract public IRequest GetNextResponse();

        // Service information operations
        public virtual bool GetServiceInformation(short serviceType, out ServiceInformation serviceInfo)
        {
            throw new NoImplement();
        }
        
        abstract public string[] ListInitialServices();

        [ThrowsIdlException(typeof(InvalidName))]
        public virtual void RegisterInitialReference(string objectName,CORBA.Object @object)
        {
            throw new NoImplement();
        }

        // Initial reference operation
        [ThrowsIdlException(typeof(InvalidName))]
        abstract public CORBA.IObject ResolveInitialReferences(string objectName);

        public TypeCode GetPrimitiveTc(TCKind tcKind)
        {
            return TypeCode.GetPrimitiveTypeCode(tcKind);
        }
        
        // Type code creation operations
        public TypeCode CreateStructTc(string id, string name, StructMember[] members)
        {
            return CreateStructTc(id, name, members, true);
        }

        internal TypeCode CreateStructTc(string id, string name, StructMember[] members, bool checkName)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);

            var memberNames = new HashSet<string>();
            for (int i = 0; i < members.Length; i++)
            {
                CheckTCMemberType(members[i].Type);
                if (checkName)
                {                                    
                    var fault = false;
                    try
                    {
                        CheckTCName(members[i].Name);
                    }
                    catch (BadParam e)
                    {
                        fault = true;
                    }
                    if (members[i].Name != null && memberNames.Contains(members[i].Name) || fault)
                    {
                        throw new BadParam("Illegal struct member name: " + members[i].Name + (fault ? "(Bad Param)" : ""), 17, CompletionStatus.No);
                    }
                    memberNames.Add(members[i].Name);
                }
            }        
            return new StructTypeCode(id, name, members);
        }

        public TypeCode CreateUnionTc(string id, string name, TypeCode discriminatorType, UnionMember[] members)
        {
            return CreateUnionTc(id, name, discriminatorType, members, true);
        }

        internal TypeCode CreateUnionTc(string id, string name, TypeCode discriminatorType, UnionMember[] members, bool checkName)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);

            // check discriminator type
            var discTc = discriminatorType.OriginalTypeCode;

            if (discTc == null)                
            {
                throw new BadParam("Illegal union discriminator type", 20, CompletionStatus.No);
            }
            else
            {
                switch(discTc.Kind)
                {
                    case TCKind.TkShort:
                    case TCKind.TkLong:
                    case TCKind.TkLonglong:
                    case TCKind.TkUshort:
                    case TCKind.TkUlong:
                    case TCKind.TkUlonglong:
                    case TCKind.TkChar:
                    case TCKind.TkWchar:
                    case TCKind.TkBoolean:
                    case TCKind.TkOctet:
                    case TCKind.TkEnum:
                        break;
                    default:
                        throw new BadParam("Illegal union discriminator type", 20, CompletionStatus.No);
                }
            }

            // check that member names are legal (they do not need to be unique)            
            for (int i = 0; i < members.Length; i++)
            {
                CheckTCMemberType(members[i].Type);
                if (checkName)
                {                    
                    try
                    {
                        CheckTCName(members[i].Name);
                    }
                    catch (BadParam e)
                    {
                        throw new BadParam("Illegal union member name: " + members[i].Name, 17, CompletionStatus.No);
                    }
                }

                // check that member type matches discriminator type or is default

                CORBA.Any label = members[i].Label;
                if (!discriminatorType.Equivalent(label.Type) && !(label.Type.Kind == TCKind.TkOctet && label.ExtractOctet() == (byte)0))
                {
                    throw new BadParam("Label type does not match discriminator type", 19, CompletionStatus.No);
                }

                // check that member labels are unique

                for (int j = 0; j < i; j++)
                {
                    if (label.Equal(members[j].Label))
                    {
                        throw new BadParam("Duplicate union case label", 18, CompletionStatus.No);
                    }
                }

            }
            return new UnionTypeCode(id, name, discriminatorType, members);
        }

        public TypeCode CreateEnumTc(string id, string name, string[] members)
        {
            return CreateEnumTc(id, name, members, false);
        }
        internal TypeCode CreateEnumTc(string id, string name, string[] members, bool checkName)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);

            if (checkName)
            {
                var memberNames = new HashSet<string>();
                for (int i = 0; i < members.Length; i++)
                {                    
                    var fault = false;
                    try
                    {
                        CheckTCName(members[i]);
                    }
                    catch (BadParam e)
                    {
                        fault = true;
                    }
                    if (members[i] != null && memberNames.Contains(members[i]) || fault)
                    {
                        throw new BadParam("Illegal enum member name: " + members[i] + (fault ? "(Bad Param)" : ""), 17, CompletionStatus.No);
                    }
                    memberNames.Add(members[i]);
                }
            }
            return new EnumTypeCode(id, name, members);
        }

        public TypeCode CreateAliasTc(string id, string name, TypeCode originalType)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);
            CheckTCMemberType(originalType);
            return new AliasTypeCode(id, name, originalType);
        }

        public TypeCode CreateExceptionTc(string id, string name, StructMember[] members)
        {
            return CreateExceptionTc(id, name, members, true);
        }

        internal TypeCode CreateExceptionTc(string id, string name, StructMember[] members, bool checkName)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);
            
            var memberNames = new HashSet<string>();
            for (int i = 0; i < members.Length; i++)
            {
                CheckTCMemberType(members[i].Type);
                if (checkName)
                {
                    var fault = false;
                    try
                    {
                        CheckTCName(members[i].Name);
                    }
                    catch (BadParam e)
                    {
                        fault = true;
                    }
                    if (members[i].Name != null && memberNames.Contains(members[i].Name) || fault)
                    {
                        throw new BadParam("Illegal exception member name: " + members[i].Name + (fault ? "(Bad Param)" : ""), 17, CompletionStatus.No);
                    }
                    memberNames.Add(members[i].Name);
                }
            }
            return new ExceptionTypeCode(id, name, members);
        }

        public TypeCode CreateInterfaceTc(string id, string name)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);
            return new ObjectReferenceTypeCode(id, name);
        }

        public TypeCode CreateStringTc(int bound)
        {
            return new StringTypeCode(bound);
        }

        public TypeCode CreateWStringTc(int bound)
        {
            return new WStringTypeCode(bound);
        }

        public TypeCode CreateFixedTc(ushort digits, ushort scale)
        {
            if (digits <= 0 || scale < 0 || scale > digits)
            {
                throw new BadParam("Invalid combination of digits and scale factor");
            }
            return new FixedTypeCode(digits, scale);
        }

        public TypeCode CreateSequenceTc(int bound, TypeCode elementType)
        {
            CheckTCMemberType(elementType);
            return new SequenceTypeCode(elementType, bound);
        }

        public TypeCode CreateRecursiveSequenceTc(int bound, int offset)
        {
            throw new NoImplement("CreateRecursiveSequenceTc - NYI");
        }

        public TypeCode CreateArrayTc(int length, TypeCode elementType)
        {
            CheckTCMemberType(elementType);
            return new ArrayTypeCode(elementType, length);
        }

        public TypeCode CreateValueTc(string id, string name, ValueModifier typeModifier, TypeCode concreteBase, ValueMember[] members)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);
            return new ValueTypeCode(id, name, typeModifier, concreteBase, members);
        }

        public TypeCode CreateValueBoxTc(string id, string name, TypeCode boxedType)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);
            return new ValueBoxTypeCode(id, name, boxedType);
        }

        public TypeCode CreateNativeTc(string id, string name)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);
            return new NativeTypeCode(id, name);
        }

        public TypeCode CreateRecursiveTc(string id)
        {
            CheckTCRepositoryId(id);
            return new RecursiveTypeCode(id);
        }

        public TypeCode CreateAbstractInterfaceTc(string id, string name)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);
            return new AbstractInterfaceTypeCode(id, name);
        }

        public TypeCode CreateLocalInterfaceTc(string id, string name)
        {
            CheckTCRepositoryId(id);
            CheckTCName(name, true);
            return new LocalInterfaceTypeCode(id, name);
        }

        public TypeCode CreateComponentTc(string id, string name)
        {
            throw new NoImplement("CreateComponentTc - NYI");
        }

        public TypeCode CreateHomeTc(string id, string name)
        {
            throw new NoImplement("CreateHomeTc - NYI");
        }

        public TypeCode CreateEventTc(string id, string name, ValueModifier typeModifier, TypeCode concreteBase, ValueMember[] members)
        {
            throw new NoImplement("CreateEventTc - NYI");
        }

        abstract public Any CreateAny();

        private void CheckTCName(string name, bool allowNull = false)
        {
            if (name == null)
            {
                if (allowNull)
                {
                    return;
                }
                throw new BadParam("Illegal null IDL name", 15, CompletionStatus.No);
            }
            if (name.Length > 0)
            {
                if (!IsLegalStartChar(name[0]))
                {
                    throw new BadParam("Illegal start character to IDL name: " + name, 15, CompletionStatus.No);
                }
                for (var i = 0; i < name.Length; i++)
                {
                    if (!IsLegalNameChar(name[i]))
                    {
                        throw new BadParam("Illegal IDL name: " + name, 15, CompletionStatus.No);
                    }
                }
            }
            else
            {
                throw new BadParam("Illegal blank IDL name", 15, CompletionStatus.No);
            }
        }

        private bool IsLegalStartChar(char character)
        {
            return (character >= 'a' && character <= 'z')
                || (character == '_')
                || (character == ':')
                || (character >= 'A' && character <= 'Z');
        }

        private bool IsLegalNameChar(char character)
        {
            return IsLegalStartChar(character)
                || (character == '_')
                || (character == ':')
                || (character >= '0' && character <= '9');
        }

        private void CheckTCRepositoryId(string repId)
        {
            if (repId == null || repId.IndexOf(":") == -1) 
            {
                throw new BadParam("Illegal repository id: " + repId, 16, CompletionStatus.No);
            }
        }

        private void CheckTCMemberType(TypeCode typeCode)
        {
            if (typeCode is not RecursiveTypeCode && 
                (typeCode == null || 
                typeCode.Kind == TCKind.TkNull ||
                typeCode.Kind == TCKind.TkVoid ||
                typeCode.Kind == TCKind.TkExcept))
            {
                throw new BadTypeCode("Illegal member TypeCode", 2, CompletionStatus.No);
            }
        }

        // Thread related operations

        public virtual bool WorkPending()
        {
            throw new NoImplement();
        }

        public virtual void PerformWork()
        {
            throw new NoImplement();
        }

        public virtual void Run()
        {
            throw new NoImplement();
        }

        public virtual void Shutdown(bool waitForCompletion)
        {
            throw new NoImplement();
        }

        public virtual void Destroy()
        {
            throw new NoImplement();
        }

        // Policy related operations
        [ThrowsIdlException(typeof(PolicyError))]
        public virtual IPolicy CreatePolicy(uint type, Any val)
        {
            throw new NoImplement();
        }

        // Value factory operations
        public virtual IValueFactory RegisterValueFactory(string id, IValueFactory factory)
        {
            throw new NoImplement();
        }

        public virtual void UnregisterValueFactory(string id)
        {
            throw new NoImplement();
        }

        public virtual IValueFactory LookupValueFactory(string id)
        {
            throw new NoImplement();
        }

        public virtual void SetDelegate(Servant servant)
        {
            throw new NoImplement();
        }

        public virtual string ExceptionToString(CORBA.SystemException ex)
        {
            throw new NoImplement();
        }

        public abstract IBoxedValueHelper GetBoxedValueHelper(string repId);
    }
}
