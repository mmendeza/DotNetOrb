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
	[IdlName("IdUniquenessPolicy")]
	[RepositoryID("IDL:PortableServer/IdUniquenessPolicy:1.0")]
	[Helper(typeof(IdUniquenessPolicyHelper))]
	[Stub(typeof(_IdUniquenessPolicyLocalBase))]
	public interface IIdUniquenessPolicy : CORBA.IIDLEntity, CORBA.ILocalInterface, IIdUniquenessPolicyOperations, CORBA.IPolicy
	{
	}
}
