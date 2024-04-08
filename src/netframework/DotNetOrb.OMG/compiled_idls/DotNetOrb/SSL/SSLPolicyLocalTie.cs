/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:37
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace DotNetOrb.SSL
{

	public class SSLPolicyLocalTie: _SSLPolicyLocalBase
	{
		public ISSLPolicyOperations _OperationsDelegate { get; set; }

		public SSLPolicyLocalTie(ISSLPolicyOperations d)
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
		public override DotNetOrb.SSL.SSLPolicyValue Value 
		{
			get
			{
				return _OperationsDelegate.Value;
			}
		}
	}
}
