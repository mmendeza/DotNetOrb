/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:30
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CosNaming
{
	[IdlName("NamingContextExt")]
	[RepositoryID("IDL:omg.org/CosNaming/NamingContextExt:1.0")]
	[Helper(typeof(NamingContextExtHelper))]
	[Stub(typeof(_NamingContextExtStub))]
	public interface INamingContextExt : CORBA.IIDLEntity, CORBA.IObject, INamingContextExtOperations, CosNaming.INamingContext
	{
	}
}

