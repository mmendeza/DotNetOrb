/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:37
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableInterceptor
{
	[IdlName("InvalidSlot")]
	[RepositoryID("IDL:PortableInterceptor/InvalidSlot:1.0")]
	[Helper(typeof(InvalidSlotHelper))]
	public partial class InvalidSlot: CORBA.UserException, CORBA.IIDLEntity, IEquatable<PortableInterceptor.InvalidSlot>
	{
		public InvalidSlot()
		{
		}

		public InvalidSlot(string _msg): base(_msg)
		{
		}

		public InvalidSlot(InvalidSlot other)
		{
		}

		public bool Equals(PortableInterceptor.InvalidSlot? other)
		{
			if (other == null) return false;
			return true;
		}
	}

}