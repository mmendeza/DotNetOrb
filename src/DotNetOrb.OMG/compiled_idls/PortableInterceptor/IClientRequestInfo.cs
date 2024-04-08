/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:37
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableInterceptor
{
	[IdlName("ClientRequestInfo")]
	[RepositoryID("IDL:PortableInterceptor/ClientRequestInfo:1.0")]
	[Helper(typeof(ClientRequestInfoHelper))]
	[Stub(typeof(_ClientRequestInfoLocalBase))]
	public interface IClientRequestInfo : CORBA.IIDLEntity, CORBA.ILocalInterface, IClientRequestInfoOperations, PortableInterceptor.IRequestInfo
	{
	}
}

