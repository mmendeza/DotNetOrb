/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:32
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Messaging
{
	[IdlName("RequestStartTimePolicy")]
	[RepositoryID("IDL:Messaging/RequestStartTimePolicy:1.0")]
	[Helper(typeof(RequestStartTimePolicyHelper))]
	[Stub(typeof(_RequestStartTimePolicyLocalBase))]
	public interface IRequestStartTimePolicy : CORBA.IIDLEntity, CORBA.ILocalInterface, IRequestStartTimePolicyOperations, CORBA.IPolicy
	{
	}
}
