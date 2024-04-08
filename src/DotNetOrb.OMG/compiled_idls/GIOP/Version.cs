/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;

namespace GIOP
{
	[RepositoryID("IDL:GIOP/Version:1.0")]
	[Helper(typeof(VersionHelper))]
	public partial class Version: CORBA.IIDLEntity, IEquatable<GIOP.Version>
	{
		[IdlName("Version")]
		public byte Major { get; set; }
		[IdlName("Version")]
		public byte Minor { get; set; }

		public Version()
		{
		}

		public Version(Version other)
		{
			Major = other.Major;
			Minor = other.Minor;
		}

		public Version(byte major, byte minor)
		{
			this.Major = major;
			this.Minor = minor;
		}

		public bool Equals(GIOP.Version? other)
		{
			if (other == null) return false;
			if (!Major.Equals(other.Major)) return false;
			if (!Minor.Equals(other.Minor)) return false;
			return true;
		}
	}

}
