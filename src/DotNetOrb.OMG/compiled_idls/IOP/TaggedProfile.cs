/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;

namespace IOP
{
	[RepositoryID("IDL:IOP/TaggedProfile:1.0")]
	[Helper(typeof(TaggedProfileHelper))]
	public partial class TaggedProfile: CORBA.IIDLEntity, IEquatable<IOP.TaggedProfile>
	{
		[IdlName("TaggedProfile")]
		public uint Tag { get; set; }
		[IdlName("TaggedProfile")]
		public byte[] ProfileData { get; set; }

		public TaggedProfile()
		{
		}

		public TaggedProfile(TaggedProfile other)
		{
			Tag = other.Tag;
			ProfileData = other.ProfileData;
		}

		public TaggedProfile(uint tag, byte[] profileData)
		{
			this.Tag = tag;
			this.ProfileData = profileData;
		}

		public bool Equals(IOP.TaggedProfile? other)
		{
			if (other == null) return false;
			if (!Tag.Equals(other.Tag)) return false;
			if (!ProfileData.SequenceEqual(other.ProfileData)) return false;
			return true;
		}
	}

}
