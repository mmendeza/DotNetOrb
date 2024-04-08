/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:29
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;

namespace CSIIOP
{
	[RepositoryID("IDL:CSIIOP/TLS_SEC_TRANS:1.0")]
	[Helper(typeof(TLS_SEC_TRANSHelper))]
	public partial class TLS_SEC_TRANS: CORBA.IIDLEntity, IEquatable<CSIIOP.TLS_SEC_TRANS>
	{
		[IdlName("TLS_SEC_TRANS")]
		public ushort TargetSupports { get; set; }
		[IdlName("TLS_SEC_TRANS")]
		public ushort TargetRequires { get; set; }
		[IdlName("TLS_SEC_TRANS")]
		public CSIIOP.TransportAddress[] Addresses { get; set; }

		public TLS_SEC_TRANS()
		{
		}

		public TLS_SEC_TRANS(TLS_SEC_TRANS other)
		{
			TargetSupports = other.TargetSupports;
			TargetRequires = other.TargetRequires;
			Addresses = other.Addresses;
		}

		public TLS_SEC_TRANS(ushort targetSupports, ushort targetRequires, CSIIOP.TransportAddress[] addresses)
		{
			this.TargetSupports = targetSupports;
			this.TargetRequires = targetRequires;
			this.Addresses = addresses;
		}

		public bool Equals(CSIIOP.TLS_SEC_TRANS? other)
		{
			if (other == null) return false;
			if (!TargetSupports.Equals(other.TargetSupports)) return false;
			if (!TargetRequires.Equals(other.TargetRequires)) return false;
			if (!Addresses.SequenceEqual(other.Addresses)) return false;
			return true;
		}
	}

}
