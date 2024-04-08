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

	public abstract class IRObjectPOA: PortableServer.Servant, CORBA.IInvokeHandler, IIRObjectOperations
	{
		static private Dictionary<string,int> _opsDict = new Dictionary<string,int>();
		static IRObjectPOA()
		{
			_opsDict.Add("_get_def_kind", 0);
			_opsDict.Add("destroy", 1);
		}
		private string[] _ids = {"IDL:CORBA/IRObject:1.0"};

		[IdlName("def_kind")]
		public abstract CORBA.DefinitionKind DefKind 
		{
			get;
		}
		[IdlName("destroy")]
		public abstract void Destroy();

		public override string[] _AllInterfaces(PortableServer.IPOA poa, byte[] objId)
		{
			return _ids;
		}

		public virtual CORBA.IIRObject _This()
		{
			return CORBA.IRObjectHelper.Narrow(_ThisObject());
		}

		public virtual CORBA.IIRObject _This(CORBA.ORB orb)
		{
			return CORBA.IRObjectHelper.Narrow(_ThisObject(orb));
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
