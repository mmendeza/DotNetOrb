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

	public class PriorityModelPolicyLocalTie: _PriorityModelPolicyLocalBase
	{
		public IPriorityModelPolicyOperations _OperationsDelegate { get; set; }

		public PriorityModelPolicyLocalTie(IPriorityModelPolicyOperations d)
		{
			_OperationsDelegate = d;
		}

		public override uint PolicyType 
		{
			get
			{
				return _OperationsDelegate.PolicyType;
			}
		}
		[IdlName("copy")]
		public override CORBA.IPolicy Copy()
		{
			return _OperationsDelegate.Copy();
		}
		[IdlName("destroy")]
		public override void Destroy()
		{
			_OperationsDelegate.Destroy();
		}
		public override RTCORBA.PriorityModel PriorityModel 
		{
			get
			{
				return _OperationsDelegate.PriorityModel;
			}
		}
		public override short ServerPriority 
		{
			get
			{
				return _OperationsDelegate.ServerPriority;
			}
		}
	}
}
