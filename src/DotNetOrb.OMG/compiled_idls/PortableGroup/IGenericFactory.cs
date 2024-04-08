/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:30
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableGroup
{
	[IdlName("GenericFactory")]
	[RepositoryID("IDL:omg.org/PortableGroup/GenericFactory:1.0")]
	[Helper(typeof(GenericFactoryHelper))]
	[Stub(typeof(_GenericFactoryStub))]
	public interface IGenericFactory : CORBA.IIDLEntity, CORBA.IObject, IGenericFactoryOperations
	{
	}
}
