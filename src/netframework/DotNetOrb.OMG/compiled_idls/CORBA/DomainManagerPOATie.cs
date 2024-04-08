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

	public class DomainManagerPOATie: DomainManagerPOA
	{
		public IDomainManagerOperations _OperationsDelegate { get; set; }
		private PortableServer.IPOA _poa;

		public DomainManagerPOATie(IDomainManagerOperations d)
		{
			_OperationsDelegate = d;
		}

		public DomainManagerPOATie(IDomainManagerOperations d, PortableServer.POA poa)
		{
			_OperationsDelegate = d;
			_poa = poa;
		}

		public override PortableServer.IPOA _DefaultPOA()
		{
			if (_poa != null)
			{
				return _poa;
			}
			return base._DefaultPOA();
		}

		public override CORBA.IDomainManager _This()
		{
			return CORBA.DomainManagerHelper.Narrow(_ThisObject());
		}

		public override CORBA.IDomainManager _This(CORBA.ORB orb)
		{
			return CORBA.DomainManagerHelper.Narrow(_ThisObject(orb));
		}

		[IdlName("get_domain_policy")]
		public override CORBA.IPolicy GetDomainPolicy(uint policyType)
		{
			return _OperationsDelegate.GetDomainPolicy(policyType);
		}
	}
}
