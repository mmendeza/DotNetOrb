/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CORBA
{

	public abstract class ValueDefPOA: PortableServer.Servant, CORBA.IInvokeHandler, IValueDefOperations
	{
		static private Dictionary<string,int> _opsDict = new Dictionary<string,int>();
		static ValueDefPOA()
		{
			_opsDict.Add("_get_def_kind", 0);
			_opsDict.Add("destroy", 1);
			_opsDict.Add("lookup", 2);
			_opsDict.Add("contents", 3);
			_opsDict.Add("lookup_name", 4);
			_opsDict.Add("describe_contents", 5);
			_opsDict.Add("create_module", 6);
			_opsDict.Add("create_constant", 7);
			_opsDict.Add("create_struct", 8);
			_opsDict.Add("create_union", 9);
			_opsDict.Add("create_enum", 10);
			_opsDict.Add("create_alias", 11);
			_opsDict.Add("create_interface", 12);
			_opsDict.Add("create_value", 13);
			_opsDict.Add("create_value_box", 14);
			_opsDict.Add("create_exception", 15);
			_opsDict.Add("create_native", 16);
			_opsDict.Add("create_abstract_interface", 17);
			_opsDict.Add("create_local_interface", 18);
			_opsDict.Add("create_ext_value", 19);
			_opsDict.Add("_set_id", 20);
			_opsDict.Add("_get_id", 21);
			_opsDict.Add("_set_name", 22);
			_opsDict.Add("_get_name", 23);
			_opsDict.Add("_set_version", 24);
			_opsDict.Add("_get_version", 25);
			_opsDict.Add("_get_defined_in", 26);
			_opsDict.Add("_get_absolute_name", 27);
			_opsDict.Add("_get_containing_repository", 28);
			_opsDict.Add("describe", 29);
			_opsDict.Add("move", 30);
			_opsDict.Add("_get_type", 31);
			_opsDict.Add("_set_supported_interfaces", 32);
			_opsDict.Add("_get_supported_interfaces", 33);
			_opsDict.Add("_set_initializers", 34);
			_opsDict.Add("_get_initializers", 35);
			_opsDict.Add("_set_base_value", 36);
			_opsDict.Add("_get_base_value", 37);
			_opsDict.Add("_set_abstract_base_values", 38);
			_opsDict.Add("_get_abstract_base_values", 39);
			_opsDict.Add("_set_is_abstract", 40);
			_opsDict.Add("_get_is_abstract", 41);
			_opsDict.Add("_set_is_custom", 42);
			_opsDict.Add("_get_is_custom", 43);
			_opsDict.Add("_set_is_truncatable", 44);
			_opsDict.Add("_get_is_truncatable", 45);
			_opsDict.Add("is_a", 46);
			_opsDict.Add("describe_value", 47);
			_opsDict.Add("create_value_member", 48);
			_opsDict.Add("create_attribute", 49);
			_opsDict.Add("create_operation", 50);
		}
		private string[] _ids = {"IDL:CORBA/ValueDef:1.0","IDL:CORBA/Container:1.0","IDL:CORBA/Contained:1.0","IDL:CORBA/IDLType:1.0"};

		[IdlName("def_kind")]
		public abstract CORBA.DefinitionKind DefKind 
		{
			get;
		}
		[IdlName("destroy")]
		public abstract void Destroy();
		[IdlName("lookup")]
		public abstract CORBA.IContained Lookup([WideChar(false)] string searchName);
		[IdlName("contents")]
		public abstract CORBA.IContained[] Contents(CORBA.DefinitionKind limitType, bool excludeInherited);
		[IdlName("lookup_name")]
		public abstract CORBA.IContained[] LookupName([WideChar(false)] string searchName, int levelsToSearch, CORBA.DefinitionKind limitType, bool excludeInherited);
		[IdlName("describe_contents")]
		public abstract CORBA.Container.Description[] DescribeContents(CORBA.DefinitionKind limitType, bool excludeInherited, int maxReturnedObjs);
		[IdlName("create_module")]
		public abstract CORBA.IModuleDef CreateModule([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version);
		[IdlName("create_constant")]
		public abstract CORBA.IConstantDef CreateConstant([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType type, CORBA.Any value);
		[IdlName("create_struct")]
		public abstract CORBA.IStructDef CreateStruct([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.StructMember[] members);
		[IdlName("create_union")]
		public abstract CORBA.IUnionDef CreateUnion([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType discriminatorType, CORBA.UnionMember[] members);
		[IdlName("create_enum")]
		public abstract CORBA.IEnumDef CreateEnum([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, string[] members);
		[IdlName("create_alias")]
		public abstract CORBA.IAliasDef CreateAlias([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType originalType);
		[IdlName("create_interface")]
		public abstract CORBA.IInterfaceDef CreateInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IInterfaceDef[] baseInterfaces, bool isAbstract);
		[IdlName("create_value")]
		public abstract CORBA.IValueDef CreateValue([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, bool isCustom, bool isAbstract, CORBA.IValueDef baseValue, bool isTruncatable, CORBA.IValueDef[] abstractBaseValues, CORBA.IInterfaceDef[] supportedInterfaces, CORBA.Initializer[] initializers);
		[IdlName("create_value_box")]
		public abstract CORBA.IValueBoxDef CreateValueBox([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType originalTypeDef);
		[IdlName("create_exception")]
		public abstract CORBA.IExceptionDef CreateException([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.StructMember[] members);
		[IdlName("create_native")]
		public abstract CORBA.INativeDef CreateNative([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version);
		[IdlName("create_abstract_interface")]
		public abstract CORBA.IAbstractInterfaceDef CreateAbstractInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IAbstractInterfaceDef[] baseInterfaces);
		[IdlName("create_local_interface")]
		public abstract CORBA.ILocalInterfaceDef CreateLocalInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IInterfaceDef[] baseInterfaces);
		[IdlName("create_ext_value")]
		public abstract CORBA.IExtValueDef CreateExtValue([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, bool isCustom, bool isAbstract, CORBA.IValueDef baseValue, bool isTruncatable, CORBA.IValueDef[] abstractBaseValues, CORBA.IInterfaceDef[] supportedInterfaces, CORBA.ExtInitializer[] initializers);
		[IdlName("id")]
		[WideChar(false)]
		public abstract string Id 
		{
			get;

			set;
		}
		[IdlName("name")]
		[WideChar(false)]
		public abstract string Name 
		{
			get;

			set;
		}
		[IdlName("version")]
		[WideChar(false)]
		public abstract string Version 
		{
			get;

			set;
		}
		[IdlName("defined_in")]
		public abstract CORBA.IContainer DefinedIn 
		{
			get;
		}
		[IdlName("absolute_name")]
		[WideChar(false)]
		public abstract string AbsoluteName 
		{
			get;
		}
		[IdlName("containing_repository")]
		public abstract CORBA.IRepository ContainingRepository 
		{
			get;
		}
		[IdlName("describe")]
		public abstract CORBA.Contained.Description Describe();
		[IdlName("move")]
		public abstract void Move(CORBA.IContainer newContainer, [WideChar(false)] string newName, [WideChar(false)] string newVersion);
		[IdlName("type")]
		public abstract CORBA.TypeCode Type 
		{
			get;
		}
		[IdlName("supported_interfaces")]
		public abstract CORBA.IInterfaceDef[] SupportedInterfaces 
		{
			get;

			set;
		}
		[IdlName("initializers")]
		public abstract CORBA.Initializer[] Initializers 
		{
			get;

			set;
		}
		[IdlName("base_value")]
		public abstract CORBA.IValueDef BaseValue 
		{
			get;

			set;
		}
		[IdlName("abstract_base_values")]
		public abstract CORBA.IValueDef[] AbstractBaseValues 
		{
			get;

			set;
		}
		[IdlName("is_abstract")]
		public abstract bool IsAbstract 
		{
			get;

			set;
		}
		[IdlName("is_custom")]
		public abstract bool IsCustom 
		{
			get;

			set;
		}
		[IdlName("is_truncatable")]
		public abstract bool IsTruncatable 
		{
			get;

			set;
		}
		[IdlName("is_a")]
		public abstract bool IsA([WideChar(false)] string id);
		[IdlName("describe_value")]
		public abstract CORBA.ValueDef.FullValueDescription DescribeValue();
		[IdlName("create_value_member")]
		public abstract CORBA.IValueMemberDef CreateValueMember([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType type, short access);
		[IdlName("create_attribute")]
		public abstract CORBA.IAttributeDef CreateAttribute([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType type, CORBA.AttributeMode mode);
		[IdlName("create_operation")]
		public abstract CORBA.IOperationDef CreateOperation([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, CORBA.IIDLType result, CORBA.OperationMode mode, CORBA.ParameterDescription[] @params, CORBA.IExceptionDef[] exceptions, string[] contexts);

		public override string[] _AllInterfaces(PortableServer.IPOA poa, byte[] objId)
		{
			return _ids;
		}

		public virtual CORBA.IValueDef _This()
		{
			return CORBA.ValueDefHelper.Narrow(_ThisObject());
		}

		public virtual CORBA.IValueDef _This(CORBA.ORB orb)
		{
			return CORBA.ValueDefHelper.Narrow(_ThisObject(orb));
		}

		public CORBA.IOutputStream _Invoke(string method, CORBA.IInputStream inputStream, CORBA.IResponseHandler handler)
		{
			CORBA.IOutputStream outputStream = null;
			int opIndex;
			if (_opsDict.TryGetValue(method, out opIndex))
			{
				switch (opIndex)
				{
					case 0:
					{
							outputStream = handler.CreateReply();
							CORBA.DefinitionKindHelper.Write(outputStream, DefKind);
					}
					break;
					case 1:
					{
							outputStream = handler.CreateReply();
							Destroy();
					}
					break;
					case 2:
					{
							string searchName;
							searchName = inputStream.ReadString();
							outputStream = handler.CreateReply();
							var _result = Lookup(searchName);
							CORBA.ContainedHelper.Write(outputStream, _result);
					}
					break;
					case 3:
					{
							CORBA.DefinitionKind limitType;
							limitType = CORBA.DefinitionKindHelper.Read(inputStream);
							bool excludeInherited;
							excludeInherited = inputStream.ReadBoolean();
							outputStream = handler.CreateReply();
							var _result = Contents(limitType, excludeInherited);
							{
								outputStream.WriteLong(_result.Length);
								for (int i0 = 0; i0 < _result.Length; i0++)
								{
									CORBA.ContainedHelper.Write(outputStream, _result[i0]);
								}
							}
					}
					break;
					case 4:
					{
							string searchName;
							searchName = inputStream.ReadString();
							int levelsToSearch;
							levelsToSearch = inputStream.ReadLong();
							CORBA.DefinitionKind limitType;
							limitType = CORBA.DefinitionKindHelper.Read(inputStream);
							bool excludeInherited;
							excludeInherited = inputStream.ReadBoolean();
							outputStream = handler.CreateReply();
							var _result = LookupName(searchName, levelsToSearch, limitType, excludeInherited);
							{
								outputStream.WriteLong(_result.Length);
								for (int i0 = 0; i0 < _result.Length; i0++)
								{
									CORBA.ContainedHelper.Write(outputStream, _result[i0]);
								}
							}
					}
					break;
					case 5:
					{
							CORBA.DefinitionKind limitType;
							limitType = CORBA.DefinitionKindHelper.Read(inputStream);
							bool excludeInherited;
							excludeInherited = inputStream.ReadBoolean();
							int maxReturnedObjs;
							maxReturnedObjs = inputStream.ReadLong();
							outputStream = handler.CreateReply();
							var _result = DescribeContents(limitType, excludeInherited, maxReturnedObjs);
							{
								outputStream.WriteLong(_result.Length);
								for (int i0 = 0; i0 < _result.Length; i0++)
								{
									CORBA.Container.DescriptionHelper.Write(outputStream, _result[i0]);
								}
							}
					}
					break;
					case 6:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							outputStream = handler.CreateReply();
							var _result = CreateModule(id, name, version);
							CORBA.ModuleDefHelper.Write(outputStream, _result);
					}
					break;
					case 7:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.IIDLType type;
							type = CORBA.IDLTypeHelper.Read(inputStream);
							CORBA.Any value;
							value = inputStream.ReadAny();
							outputStream = handler.CreateReply();
							var _result = CreateConstant(id, name, version, type, value);
							CORBA.ConstantDefHelper.Write(outputStream, _result);
					}
					break;
					case 8:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.StructMember[] members;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								members = new CORBA.StructMember[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.StructMember _item0;
									_item0 = CORBA.StructMemberHelper.Read(inputStream);
									members[i0] = _item0;
								}
							}
							outputStream = handler.CreateReply();
							var _result = CreateStruct(id, name, version, members);
							CORBA.StructDefHelper.Write(outputStream, _result);
					}
					break;
					case 9:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.IIDLType discriminatorType;
							discriminatorType = CORBA.IDLTypeHelper.Read(inputStream);
							CORBA.UnionMember[] members;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								members = new CORBA.UnionMember[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.UnionMember _item0;
									_item0 = CORBA.UnionMemberHelper.Read(inputStream);
									members[i0] = _item0;
								}
							}
							outputStream = handler.CreateReply();
							var _result = CreateUnion(id, name, version, discriminatorType, members);
							CORBA.UnionDefHelper.Write(outputStream, _result);
					}
					break;
					case 10:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							string[] members;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								members = new string[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									string _item0;
									_item0 = inputStream.ReadString();
									members[i0] = _item0;
								}
							}
							outputStream = handler.CreateReply();
							var _result = CreateEnum(id, name, version, members);
							CORBA.EnumDefHelper.Write(outputStream, _result);
					}
					break;
					case 11:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.IIDLType originalType;
							originalType = CORBA.IDLTypeHelper.Read(inputStream);
							outputStream = handler.CreateReply();
							var _result = CreateAlias(id, name, version, originalType);
							CORBA.AliasDefHelper.Write(outputStream, _result);
					}
					break;
					case 12:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.IInterfaceDef[] baseInterfaces;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								baseInterfaces = new CORBA.IInterfaceDef[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.IInterfaceDef _item0;
									_item0 = CORBA.InterfaceDefHelper.Read(inputStream);
									baseInterfaces[i0] = _item0;
								}
							}
							bool isAbstract;
							isAbstract = inputStream.ReadBoolean();
							outputStream = handler.CreateReply();
							var _result = CreateInterface(id, name, version, baseInterfaces, isAbstract);
							CORBA.InterfaceDefHelper.Write(outputStream, _result);
					}
					break;
					case 13:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							bool isCustom;
							isCustom = inputStream.ReadBoolean();
							bool isAbstract;
							isAbstract = inputStream.ReadBoolean();
							CORBA.IValueDef baseValue;
							baseValue = CORBA.ValueDefHelper.Read(inputStream);
							bool isTruncatable;
							isTruncatable = inputStream.ReadBoolean();
							CORBA.IValueDef[] abstractBaseValues;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								abstractBaseValues = new CORBA.IValueDef[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.IValueDef _item0;
									_item0 = CORBA.ValueDefHelper.Read(inputStream);
									abstractBaseValues[i0] = _item0;
								}
							}
							CORBA.IInterfaceDef[] supportedInterfaces;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								supportedInterfaces = new CORBA.IInterfaceDef[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.IInterfaceDef _item0;
									_item0 = CORBA.InterfaceDefHelper.Read(inputStream);
									supportedInterfaces[i0] = _item0;
								}
							}
							CORBA.Initializer[] initializers;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								initializers = new CORBA.Initializer[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.Initializer _item0;
									_item0 = CORBA.InitializerHelper.Read(inputStream);
									initializers[i0] = _item0;
								}
							}
							outputStream = handler.CreateReply();
							var _result = CreateValue(id, name, version, isCustom, isAbstract, baseValue, isTruncatable, abstractBaseValues, supportedInterfaces, initializers);
							CORBA.ValueDefHelper.Write(outputStream, _result);
					}
					break;
					case 14:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.IIDLType originalTypeDef;
							originalTypeDef = CORBA.IDLTypeHelper.Read(inputStream);
							outputStream = handler.CreateReply();
							var _result = CreateValueBox(id, name, version, originalTypeDef);
							CORBA.ValueBoxDefHelper.Write(outputStream, _result);
					}
					break;
					case 15:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.StructMember[] members;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								members = new CORBA.StructMember[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.StructMember _item0;
									_item0 = CORBA.StructMemberHelper.Read(inputStream);
									members[i0] = _item0;
								}
							}
							outputStream = handler.CreateReply();
							var _result = CreateException(id, name, version, members);
							CORBA.ExceptionDefHelper.Write(outputStream, _result);
					}
					break;
					case 16:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							outputStream = handler.CreateReply();
							var _result = CreateNative(id, name, version);
							CORBA.NativeDefHelper.Write(outputStream, _result);
					}
					break;
					case 17:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.IAbstractInterfaceDef[] baseInterfaces;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								baseInterfaces = new CORBA.IAbstractInterfaceDef[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.IAbstractInterfaceDef _item0;
									_item0 = CORBA.AbstractInterfaceDefHelper.Read(inputStream);
									baseInterfaces[i0] = _item0;
								}
							}
							outputStream = handler.CreateReply();
							var _result = CreateAbstractInterface(id, name, version, baseInterfaces);
							CORBA.AbstractInterfaceDefHelper.Write(outputStream, _result);
					}
					break;
					case 18:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.IInterfaceDef[] baseInterfaces;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								baseInterfaces = new CORBA.IInterfaceDef[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.IInterfaceDef _item0;
									_item0 = CORBA.InterfaceDefHelper.Read(inputStream);
									baseInterfaces[i0] = _item0;
								}
							}
							outputStream = handler.CreateReply();
							var _result = CreateLocalInterface(id, name, version, baseInterfaces);
							CORBA.LocalInterfaceDefHelper.Write(outputStream, _result);
					}
					break;
					case 19:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							bool isCustom;
							isCustom = inputStream.ReadBoolean();
							bool isAbstract;
							isAbstract = inputStream.ReadBoolean();
							CORBA.IValueDef baseValue;
							baseValue = CORBA.ValueDefHelper.Read(inputStream);
							bool isTruncatable;
							isTruncatable = inputStream.ReadBoolean();
							CORBA.IValueDef[] abstractBaseValues;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								abstractBaseValues = new CORBA.IValueDef[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.IValueDef _item0;
									_item0 = CORBA.ValueDefHelper.Read(inputStream);
									abstractBaseValues[i0] = _item0;
								}
							}
							CORBA.IInterfaceDef[] supportedInterfaces;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								supportedInterfaces = new CORBA.IInterfaceDef[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.IInterfaceDef _item0;
									_item0 = CORBA.InterfaceDefHelper.Read(inputStream);
									supportedInterfaces[i0] = _item0;
								}
							}
							CORBA.ExtInitializer[] initializers;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								initializers = new CORBA.ExtInitializer[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.ExtInitializer _item0;
									_item0 = CORBA.ExtInitializerHelper.Read(inputStream);
									initializers[i0] = _item0;
								}
							}
							outputStream = handler.CreateReply();
							var _result = CreateExtValue(id, name, version, isCustom, isAbstract, baseValue, isTruncatable, abstractBaseValues, supportedInterfaces, initializers);
							CORBA.ExtValueDefHelper.Write(outputStream, _result);
					}
					break;
					case 20:
					{
							outputStream = handler.CreateReply();
							Id = inputStream.ReadString();
					}
					break;
					case 21:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteString(Id);
					}
					break;
					case 22:
					{
							outputStream = handler.CreateReply();
							Name = inputStream.ReadString();
					}
					break;
					case 23:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteString(Name);
					}
					break;
					case 24:
					{
							outputStream = handler.CreateReply();
							Version = inputStream.ReadString();
					}
					break;
					case 25:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteString(Version);
					}
					break;
					case 26:
					{
							outputStream = handler.CreateReply();
							CORBA.ContainerHelper.Write(outputStream, DefinedIn);
					}
					break;
					case 27:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteString(AbsoluteName);
					}
					break;
					case 28:
					{
							outputStream = handler.CreateReply();
							CORBA.RepositoryHelper.Write(outputStream, ContainingRepository);
					}
					break;
					case 29:
					{
							outputStream = handler.CreateReply();
							var _result = Describe();
							CORBA.Contained.DescriptionHelper.Write(outputStream, _result);
					}
					break;
					case 30:
					{
							CORBA.IContainer newContainer;
							newContainer = CORBA.ContainerHelper.Read(inputStream);
							string newName;
							newName = inputStream.ReadString();
							string newVersion;
							newVersion = inputStream.ReadString();
							outputStream = handler.CreateReply();
							Move(newContainer,newName,newVersion);
					}
					break;
					case 31:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteTypeCode(Type);
					}
					break;
					case 32:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								SupportedInterfaces = new CORBA.IInterfaceDef[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.IInterfaceDef _item0;
									_item0 = CORBA.InterfaceDefHelper.Read(inputStream);
									SupportedInterfaces[i0] = _item0;
								}
							}
					}
					break;
					case 33:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(SupportedInterfaces.Length);
								for (int i0 = 0; i0 < SupportedInterfaces.Length; i0++)
								{
									CORBA.InterfaceDefHelper.Write(outputStream, SupportedInterfaces[i0]);
								}
							}
					}
					break;
					case 34:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								Initializers = new CORBA.Initializer[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.Initializer _item0;
									_item0 = CORBA.InitializerHelper.Read(inputStream);
									Initializers[i0] = _item0;
								}
							}
					}
					break;
					case 35:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(Initializers.Length);
								for (int i0 = 0; i0 < Initializers.Length; i0++)
								{
									CORBA.InitializerHelper.Write(outputStream, Initializers[i0]);
								}
							}
					}
					break;
					case 36:
					{
							outputStream = handler.CreateReply();
							BaseValue = CORBA.ValueDefHelper.Read(inputStream);
					}
					break;
					case 37:
					{
							outputStream = handler.CreateReply();
							CORBA.ValueDefHelper.Write(outputStream, BaseValue);
					}
					break;
					case 38:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								AbstractBaseValues = new CORBA.IValueDef[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.IValueDef _item0;
									_item0 = CORBA.ValueDefHelper.Read(inputStream);
									AbstractBaseValues[i0] = _item0;
								}
							}
					}
					break;
					case 39:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AbstractBaseValues.Length);
								for (int i0 = 0; i0 < AbstractBaseValues.Length; i0++)
								{
									CORBA.ValueDefHelper.Write(outputStream, AbstractBaseValues[i0]);
								}
							}
					}
					break;
					case 40:
					{
							outputStream = handler.CreateReply();
							IsAbstract = inputStream.ReadBoolean();
					}
					break;
					case 41:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteBoolean(IsAbstract);
					}
					break;
					case 42:
					{
							outputStream = handler.CreateReply();
							IsCustom = inputStream.ReadBoolean();
					}
					break;
					case 43:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteBoolean(IsCustom);
					}
					break;
					case 44:
					{
							outputStream = handler.CreateReply();
							IsTruncatable = inputStream.ReadBoolean();
					}
					break;
					case 45:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteBoolean(IsTruncatable);
					}
					break;
					case 46:
					{
							string id;
							id = inputStream.ReadString();
							outputStream = handler.CreateReply();
							var _result = IsA(id);
							outputStream.WriteBoolean(_result);
					}
					break;
					case 47:
					{
							outputStream = handler.CreateReply();
							var _result = DescribeValue();
							CORBA.ValueDef.FullValueDescriptionHelper.Write(outputStream, _result);
					}
					break;
					case 48:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.IIDLType type;
							type = CORBA.IDLTypeHelper.Read(inputStream);
							short access;
							access = inputStream.ReadShort();
							outputStream = handler.CreateReply();
							var _result = CreateValueMember(id, name, version, type, access);
							CORBA.ValueMemberDefHelper.Write(outputStream, _result);
					}
					break;
					case 49:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.IIDLType type;
							type = CORBA.IDLTypeHelper.Read(inputStream);
							CORBA.AttributeMode mode;
							mode = CORBA.AttributeModeHelper.Read(inputStream);
							outputStream = handler.CreateReply();
							var _result = CreateAttribute(id, name, version, type, mode);
							CORBA.AttributeDefHelper.Write(outputStream, _result);
					}
					break;
					case 50:
					{
							string id;
							id = inputStream.ReadString();
							string name;
							name = inputStream.ReadString();
							string version;
							version = inputStream.ReadString();
							CORBA.IIDLType result;
							result = CORBA.IDLTypeHelper.Read(inputStream);
							CORBA.OperationMode mode;
							mode = CORBA.OperationModeHelper.Read(inputStream);
							CORBA.ParameterDescription[] @params;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								@params = new CORBA.ParameterDescription[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.ParameterDescription _item0;
									_item0 = CORBA.ParameterDescriptionHelper.Read(inputStream);
									@params[i0] = _item0;
								}
							}
							CORBA.IExceptionDef[] exceptions;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								exceptions = new CORBA.IExceptionDef[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									CORBA.IExceptionDef _item0;
									_item0 = CORBA.ExceptionDefHelper.Read(inputStream);
									exceptions[i0] = _item0;
								}
							}
							string[] contexts;
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								contexts = new string[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									string _item0;
									_item0 = inputStream.ReadString();
									contexts[i0] = _item0;
								}
							}
							outputStream = handler.CreateReply();
							var _result = CreateOperation(id, name, version, result, mode, @params, exceptions, contexts);
							CORBA.OperationDefHelper.Write(outputStream, _result);
					}
					break;
				}
				return outputStream;
			}
			else
			{
				throw new CORBA.BadOperation(method + " not found");
			}
		}
	}
}
