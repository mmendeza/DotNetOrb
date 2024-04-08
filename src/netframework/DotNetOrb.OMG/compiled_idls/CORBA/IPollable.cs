/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CORBA
{
	[RepositoryID("IDL:CORBA/Pollable:1.0")]
	[Helper(typeof(PollableHelper))]
	public interface IPollable : CORBA.IValueBase
	{
		[IdlName("is_ready")]
		public bool IsReady(uint timeout);
		[IdlName("create_pollable_set")]
		public CORBA.IPollableSet CreatePollableSet();
	}
}

