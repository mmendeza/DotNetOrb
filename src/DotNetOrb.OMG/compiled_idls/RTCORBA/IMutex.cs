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
	[IdlName("Mutex")]
	[RepositoryID("IDL:RTCORBA/Mutex:1.0")]
	[Helper(typeof(MutexHelper))]
	[Stub(typeof(_MutexLocalBase))]
	public interface IMutex : CORBA.IIDLEntity, CORBA.ILocalInterface, IMutexOperations
	{
	}
}

