/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:34
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Messaging
{
	[IdlName("MaxHopsPolicy")]
	[RepositoryID("IDL:Messaging/MaxHopsPolicy:1.0")]
	[Helper(typeof(MaxHopsPolicyHelper))]
	[Stub(typeof(_MaxHopsPolicyLocalBase))]
	public interface IMaxHopsPolicy : CORBA.IIDLEntity, CORBA.ILocalInterface, IMaxHopsPolicyOperations, CORBA.IPolicy
	{
	}
}

