/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:27
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CORBA
{
	public interface IContainerOperations : CORBA.IIRObjectOperations
	{
		[IdlName("lookup")]
		public CORBA.IContained Lookup([WideChar(false)] string searchName);
		[IdlName("contents")]
		public CORBA.IContained[] Contents(CORBA.DefinitionKind limitType, bool excludeInherited);
		[IdlName("lookup_name")]
		public CORBA.IContained[] LookupName([WideChar(false)] string searchName, int levelsToSearch, CORBA.DefinitionKind limitType, bool excludeInherited);
		[IdlName("describe_contents")]
		public CORBA.Container.Description[] DescribeContents(CORBA.DefinitionKind limitType, bool excludeInherited, int maxReturnedObjs);
		[IdlName("create_module")]
		public CORBA.IModuleDef CreateModule([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version);
		[IdlName("create_constant")]
		public CORBA.IConstantDef CreateConstant([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType type, CORBA.Any value);
		[IdlName("create_struct")]
		public CORBA.IStructDef CreateStruct([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.StructMember[] members);
		[IdlName("create_union")]
		public CORBA.IUnionDef CreateUnion([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType discriminatorType, CORBA.UnionMember[] members);
		[IdlName("create_enum")]
		public CORBA.IEnumDef CreateEnum([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, string[] members);
		[IdlName("create_alias")]
		public CORBA.IAliasDef CreateAlias([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType originalType);
		[IdlName("create_interface")]
		public CORBA.IInterfaceDef CreateInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IInterfaceDef[] baseInterfaces, bool isAbstract);
		[IdlName("create_value")]
		public CORBA.IValueDef CreateValue([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, bool isCustom, bool isAbstract, CORBA.IValueDef baseValue, bool isTruncatable, CORBA.IValueDef[] abstractBaseValues, CORBA.IInterfaceDef[] supportedInterfaces, CORBA.Initializer[] initializers);
		[IdlName("create_value_box")]
		public CORBA.IValueBoxDef CreateValueBox([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType originalTypeDef);
		[IdlName("create_exception")]
		public CORBA.IExceptionDef CreateException([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.StructMember[] members);
		[IdlName("create_native")]
		public CORBA.INativeDef CreateNative([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version);
		[IdlName("create_abstract_interface")]
		public CORBA.IAbstractInterfaceDef CreateAbstractInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IAbstractInterfaceDef[] baseInterfaces);
		[IdlName("create_local_interface")]
		public CORBA.ILocalInterfaceDef CreateLocalInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IInterfaceDef[] baseInterfaces);
		[IdlName("create_ext_value")]
		public CORBA.IExtValueDef CreateExtValue([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, bool isCustom, bool isAbstract, CORBA.IValueDef baseValue, bool isTruncatable, CORBA.IValueDef[] abstractBaseValues, CORBA.IInterfaceDef[] supportedInterfaces, CORBA.ExtInitializer[] initializers);
	}
}

