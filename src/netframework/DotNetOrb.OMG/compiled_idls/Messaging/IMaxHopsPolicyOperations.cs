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
	public interface IMaxHopsPolicyOperations : CORBA.IPolicyOperations
	{
		[IdlName("max_hops")]
		public ushort MaxHops 
		{
			get;
		}
	}
}
