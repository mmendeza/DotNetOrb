/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 26/02/2024 12:18:44
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Chat
{
	[IdlName("Client")]
	[RepositoryID("IDL:Chat/Client:1.0")]
	[Helper(typeof(ClientHelper))]
	[Stub(typeof(_ClientStub))]
	public interface IClient : CORBA.IIDLEntity, CORBA.IObject, IClientOperations
	{
	}
}

