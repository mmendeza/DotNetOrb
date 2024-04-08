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

	public class PolicyCurrentLocalTie: _PolicyCurrentLocalBase
	{
		public IPolicyCurrentOperations _OperationsDelegate { get; set; }

		public PolicyCurrentLocalTie(IPolicyCurrentOperations d)
		{
			_OperationsDelegate = d;
		}

		[IdlName("get_policy_overrides")]
		public override CORBA.IPolicy[] GetPolicyOverrides(uint[] ts)
		{
			return _OperationsDelegate.GetPolicyOverrides(ts);
		}
		[IdlName("set_policy_overrides")]
		[ThrowsIdlException(typeof(CORBA.InvalidPolicies))]
		public override void SetPolicyOverrides(CORBA.IPolicy[] policies, CORBA.SetOverrideType setAdd)
		{
			_OperationsDelegate.SetPolicyOverrides(policies, setAdd);
		}
	}
}
