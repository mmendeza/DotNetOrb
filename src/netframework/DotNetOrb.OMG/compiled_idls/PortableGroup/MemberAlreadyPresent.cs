/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:30
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableGroup
{
	[IdlName("MemberAlreadyPresent")]
	[RepositoryID("IDL:omg.org/PortableGroup/MemberAlreadyPresent:1.0")]
	[Helper(typeof(MemberAlreadyPresentHelper))]
	public partial class MemberAlreadyPresent: CORBA.UserException, CORBA.IIDLEntity, IEquatable<PortableGroup.MemberAlreadyPresent>
	{
		public MemberAlreadyPresent()
		{
		}

		public MemberAlreadyPresent(string _msg): base(_msg)
		{
		}

		public MemberAlreadyPresent(MemberAlreadyPresent other)
		{
		}

		public bool Equals(PortableGroup.MemberAlreadyPresent? other)
		{
			if (other == null) return false;
			return true;
		}
	}

}