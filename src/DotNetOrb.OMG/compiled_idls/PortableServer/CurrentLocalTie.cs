/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:29
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableServer
{

	public class CurrentLocalTie: _CurrentLocalBase
	{
		public ICurrentOperations _OperationsDelegate { get; set; }

		public CurrentLocalTie(ICurrentOperations d)
		{
			_OperationsDelegate = d;
		}

		[IdlName("get_POA")]
		[ThrowsIdlException(typeof(PortableServer.Current.NoContext))]
		public override PortableServer.IPOA GetPoa()
		{
			return _OperationsDelegate.GetPoa();
		}
		[IdlName("get_object_id")]
		[ThrowsIdlException(typeof(PortableServer.Current.NoContext))]
		public override byte[] GetObjectId()
		{
			return _OperationsDelegate.GetObjectId();
		}
		[IdlName("get_reference")]
		[ThrowsIdlException(typeof(PortableServer.Current.NoContext))]
		public override CORBA.IObject GetReference()
		{
			return _OperationsDelegate.GetReference();
		}
		[IdlName("get_servant")]
		[ThrowsIdlException(typeof(PortableServer.Current.NoContext))]
		public override PortableServer.Servant GetServant()
		{
			return _OperationsDelegate.GetServant();
		}
	}
}