/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:26
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CORBA
{
	public interface IPolicyOperations
	{
		[IdlName("policy_type")]
		public uint PolicyType 
		{
			get;
		}
		[IdlName("copy")]
		public CORBA.IPolicy Copy();
		[IdlName("destroy")]
		public void Destroy();
	}
}

