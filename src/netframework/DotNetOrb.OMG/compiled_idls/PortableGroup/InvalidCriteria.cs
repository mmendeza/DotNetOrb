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
	[IdlName("InvalidCriteria")]
	[RepositoryID("IDL:omg.org/PortableGroup/InvalidCriteria:1.0")]
	[Helper(typeof(InvalidCriteriaHelper))]
	public partial class InvalidCriteria: CORBA.UserException, CORBA.IIDLEntity, IEquatable<PortableGroup.InvalidCriteria>
	{
		public InvalidCriteria()
		{
		}

		public InvalidCriteria(string _msg): base(_msg)
		{
		}

		public InvalidCriteria(InvalidCriteria other)
		{
			_InvalidCriteria = other._InvalidCriteria;
		}

		public InvalidCriteria(PortableGroup.Property[] invalidCriteria, string _msg = ""): base(_msg)
		{
			this._InvalidCriteria = invalidCriteria;
		}

		public bool Equals(PortableGroup.InvalidCriteria? other)
		{
			if (other == null) return false;
			if (!_InvalidCriteria.SequenceEqual(other._InvalidCriteria)) return false;
			return true;
		}
		[IdlName("invalid_criteria")]
		public PortableGroup.Property[] _InvalidCriteria { get; set; }
	}

}