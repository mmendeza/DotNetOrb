/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:29
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace ETF
{
	[IdlName("Profile")]
	[RepositoryID("IDL:omg.org/ETF/Profile:1.0")]
	[Helper(typeof(ProfileHelper))]
	[Stub(typeof(_ProfileLocalBase))]
	public interface IProfile : CORBA.IIDLEntity, CORBA.ILocalInterface, IProfileOperations
	{
	}
}

