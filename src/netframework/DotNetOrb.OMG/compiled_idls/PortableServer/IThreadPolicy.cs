/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableServer
{
	[IdlName("ThreadPolicy")]
	[RepositoryID("IDL:PortableServer/ThreadPolicy:1.0")]
	[Helper(typeof(ThreadPolicyHelper))]
	[Stub(typeof(_ThreadPolicyLocalBase))]
	public interface IThreadPolicy : CORBA.IIDLEntity, CORBA.ILocalInterface, IThreadPolicyOperations, CORBA.IPolicy
	{
	}
}

