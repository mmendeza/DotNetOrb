/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace RTCORBA
{
	[IdlName("Current")]
	[RepositoryID("IDL:RTCORBA/Current:1.0")]
	[Helper(typeof(CurrentHelper))]
	[Stub(typeof(_CurrentLocalBase))]
	public interface ICurrent : CORBA.IIDLEntity, CORBA.ILocalInterface, ICurrentOperations, CORBA.ICurrent
	{
	}
}
