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
	public abstract class _PriorityBandedConnectionPolicyLocalBase: CORBA.LocalObject, IPriorityBandedConnectionPolicy
	{
		private string[] _ids = {"IDL:RTCORBA/PriorityBandedConnectionPolicy:1.0","IDL:CORBA/Policy:1.0"};

		public override string[] _Ids()
		{
			return _ids;
		}

		[IdlName("policy_type")]
		public abstract uint PolicyType 
		{
			get;
		}
		[IdlName("copy")]
		public abstract CORBA.IPolicy Copy();
		[IdlName("destroy")]
		public abstract void Destroy();
		[IdlName("priority_bands")]
		public abstract RTCORBA.PriorityBand[] PriorityBands 
		{
			get;
		}
	}

}
