/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;

namespace TimeBase
{
	[RepositoryID("IDL:omg.org/TimeBase/UtcT:1.0")]
	[Helper(typeof(UtcTHelper))]
	public partial class UtcT: CORBA.IIDLEntity, IEquatable<TimeBase.UtcT>
	{
		[IdlName("UtcT")]
		public ulong Time { get; set; }
		[IdlName("UtcT")]
		public uint Inacclo { get; set; }
		[IdlName("UtcT")]
		public ushort Inacchi { get; set; }
		[IdlName("UtcT")]
		public short Tdf { get; set; }

		public UtcT()
		{
		}

		public UtcT(UtcT other)
		{
			Time = other.Time;
			Inacclo = other.Inacclo;
			Inacchi = other.Inacchi;
			Tdf = other.Tdf;
		}

		public UtcT(ulong time, uint inacclo, ushort inacchi, short tdf)
		{
			this.Time = time;
			this.Inacclo = inacclo;
			this.Inacchi = inacchi;
			this.Tdf = tdf;
		}

		public bool Equals(TimeBase.UtcT? other)
		{
			if (other == null) return false;
			if (!Time.Equals(other.Time)) return false;
			if (!Inacclo.Equals(other.Inacclo)) return false;
			if (!Inacchi.Equals(other.Inacchi)) return false;
			if (!Tdf.Equals(other.Tdf)) return false;
			return true;
		}
	}

}