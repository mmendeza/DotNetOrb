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
	[IdlName("ObjectNotAdded")]
	[RepositoryID("IDL:omg.org/PortableGroup/ObjectNotAdded:1.0")]
	[Helper(typeof(ObjectNotAddedHelper))]
	public partial class ObjectNotAdded: CORBA.UserException, CORBA.IIDLEntity, IEquatable<PortableGroup.ObjectNotAdded>
	{
		public ObjectNotAdded()
		{
		}

		public ObjectNotAdded(string _msg): base(_msg)
		{
		}

		public ObjectNotAdded(ObjectNotAdded other)
		{
		}

		public bool Equals(PortableGroup.ObjectNotAdded? other)
		{
			if (other == null) return false;
			return true;
		}
	}

}