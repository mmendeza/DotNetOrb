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
	public abstract class _ServantActivatorLocalBase: CORBA.LocalObject, IServantActivator
	{
		private string[] _ids = {"IDL:PortableServer/ServantActivator:1.0","IDL:PortableServer/ServantManager:1.0"};

		public override string[] _Ids()
		{
			return _ids;
		}

		[IdlName("incarnate")]
		[ThrowsIdlException(typeof(PortableServer.ForwardRequest))]
		public abstract PortableServer.Servant Incarnate(byte[] oid, PortableServer.IPOA adapter);
		[IdlName("etherealize")]
		public abstract void Etherealize(byte[] oid, PortableServer.IPOA adapter, PortableServer.Servant serv, bool cleanupInProgress, bool remainingActivations);
	}

}
