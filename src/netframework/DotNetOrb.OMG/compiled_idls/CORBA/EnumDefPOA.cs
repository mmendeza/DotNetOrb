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

	public abstract class EnumDefPOA: PortableServer.Servant, CORBA.IInvokeHandler, IEnumDefOperations
	{
		static private Dictionary<string,int> _opsDict = new Dictionary<string,int>();
		static EnumDefPOA()
		{
			_opsDict.Add("_get_def_kind", 0);
			_opsDict.Add("destroy", 1);
			_opsDict.Add("_set_id", 2);
			_opsDict.Add("_get_id", 3);
			_opsDict.Add("_set_name", 4);
			_opsDict.Add("_get_name", 5);
			_opsDict.Add("_set_version", 6);
			_opsDict.Add("_get_version", 7);
			_opsDict.Add("_get_defined_in", 8);
			_opsDict.Add("_get_absolute_name", 9);
			_opsDict.Add("_get_containing_repository", 10);
			_opsDict.Add("describe", 11);
			_opsDict.Add("move", 12);
			_opsDict.Add("_get_type", 13);
			_opsDict.Add("_set_members", 14);
			_opsDict.Add("_get_members", 15);
		}
		private string[] _ids = {"IDL:CORBA/EnumDef:1.0","IDL:CORBA/TypedefDef:1.0"};

		[IdlName("def_kind")]
		public abstract CORBA.DefinitionKind DefKind 
		{
			get;
		}
		[IdlName("destroy")]
		public abstract void Destroy();
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
		[IdlName("members")]
		public abstract string[] Members 
		{
			get;

			set;
		}

		public override string[] _AllInterfaces(PortableServer.IPOA poa, byte[] objId)
		{
			return _ids;
		}

		public virtual CORBA.IEnumDef _This()
		{
			return CORBA.EnumDefHelper.Narrow(_ThisObject());
		}

		public virtual CORBA.IEnumDef _This(CORBA.ORB orb)
		{
			return CORBA.EnumDefHelper.Narrow(_ThisObject(orb));
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
							outputStream = handler.CreateReply();
							Id = inputStream.ReadString();
					}
					break;
					case 3:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteString(Id);
					}
					break;
					case 4:
					{
							outputStream = handler.CreateReply();
							Name = inputStream.ReadString();
					}
					break;
					case 5:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteString(Name);
					}
					break;
					case 6:
					{
							outputStream = handler.CreateReply();
							Version = inputStream.ReadString();
					}
					break;
					case 7:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteString(Version);
					}
					break;
					case 8:
					{
							outputStream = handler.CreateReply();
							CORBA.ContainerHelper.Write(outputStream, DefinedIn);
					}
					break;
					case 9:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteString(AbsoluteName);
					}
					break;
					case 10:
					{
							outputStream = handler.CreateReply();
							CORBA.RepositoryHelper.Write(outputStream, ContainingRepository);
					}
					break;
					case 11:
					{
							outputStream = handler.CreateReply();
							var _result = Describe();
							CORBA.Contained.DescriptionHelper.Write(outputStream, _result);
					}
					break;
					case 12:
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
					case 13:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteTypeCode(Type);
					}
					break;
					case 14:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								Members = new string[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									string _item0;
									_item0 = inputStream.ReadString();
									Members[i0] = _item0;
								}
							}
					}
					break;
					case 15:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(Members.Length);
								for (int i0 = 0; i0 < Members.Length; i0++)
								{
									outputStream.WriteString(Members[i0]);
								}
							}
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