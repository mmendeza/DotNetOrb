/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableServer
{
	[IdlName("ForwardRequest")]
	[RepositoryID("IDL:PortableServer/ForwardRequest:1.0")]
	[Helper(typeof(ForwardRequestHelper))]
	public partial class ForwardRequest: CORBA.UserException, CORBA.IIDLEntity, IEquatable<PortableServer.ForwardRequest>
	{
		public ForwardRequest()
		{
		}

		public ForwardRequest(string _msg): base(_msg)
		{
		}

		public ForwardRequest(ForwardRequest other)
		{
			ForwardReference = other.ForwardReference;
		}

		public ForwardRequest(CORBA.IObject forwardReference, string _msg = ""): base(_msg)
		{
			this.ForwardReference = forwardReference;
		}

		public bool Equals(PortableServer.ForwardRequest? other)
		{
			if (other == null) return false;
			if (!ForwardReference.Equals(other.ForwardReference)) return false;
			return true;
		}
		[IdlName("forward_reference")]
		public CORBA.IObject ForwardReference { get; set; }
	}

}
