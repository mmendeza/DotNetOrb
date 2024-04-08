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
	[IdlName("SecurityContextType")]
	[RepositoryID("IDL:omg.org/Security/SecurityContextType:1.0")]
	[Helper(typeof(SecurityContextTypeHelper))]
	public enum SecurityContextType
	{
		[IdlName("SecClientSecurityContext")]
		SecClientSecurityContext = 0,
		[IdlName("SecServerSecurityContext")]
		SecServerSecurityContext = 1,
	}

}