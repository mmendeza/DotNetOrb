/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:38
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace ImplementationRepository
{
	[IdlName("ActivationMode")]
	[RepositoryID("IDL:ImplementationRepository/ActivationMode:1.0")]
	[Helper(typeof(ActivationModeHelper))]
	public enum ActivationMode
	{
		[IdlName("NORMAL")]
		NORMAL = 0,
		[IdlName("MANUAL")]
		MANUAL = 1,
		[IdlName("PER_CLIENT")]
		PER_CLIENT = 2,
		[IdlName("AUTO_START")]
		AUTO_START = 3,
	}

}