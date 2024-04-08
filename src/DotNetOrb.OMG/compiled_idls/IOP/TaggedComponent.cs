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
	[RepositoryID("IDL:IOP/TaggedComponent:1.0")]
	[Helper(typeof(TaggedComponentHelper))]
	public partial class TaggedComponent: CORBA.IIDLEntity, IEquatable<IOP.TaggedComponent>
	{
		[IdlName("TaggedComponent")]
		public uint Tag { get; set; }
		[IdlName("TaggedComponent")]
		public byte[] ComponentData { get; set; }

		public TaggedComponent()
		{
		}

		public TaggedComponent(TaggedComponent other)
		{
			Tag = other.Tag;
			ComponentData = other.ComponentData;
		}

		public TaggedComponent(uint tag, byte[] componentData)
		{
			this.Tag = tag;
			this.ComponentData = componentData;
		}

		public bool Equals(IOP.TaggedComponent? other)
		{
			if (other == null) return false;
			if (!Tag.Equals(other.Tag)) return false;
			if (!ComponentData.SequenceEqual(other.ComponentData)) return false;
			return true;
		}
	}

}
