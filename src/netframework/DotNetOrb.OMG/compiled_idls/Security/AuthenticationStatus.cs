/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:29
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Security
{
	[IdlName("AuthenticationStatus")]
	[RepositoryID("IDL:omg.org/Security/AuthenticationStatus:1.0")]
	[Helper(typeof(AuthenticationStatusHelper))]
	public enum AuthenticationStatus
	{
		[IdlName("SecAuthSuccess")]
		SecAuthSuccess = 0,
		[IdlName("SecAuthFailure")]
		SecAuthFailure = 1,
		[IdlName("SecAuthContinue")]
		SecAuthContinue = 2,
		[IdlName("SecAuthExpired")]
		SecAuthExpired = 3,
	}

}