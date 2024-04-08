/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:35
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;

namespace MessageRouting
{
	[RepositoryID("IDL:MessageRouting/RequestInfo:1.0")]
	[Helper(typeof(RequestInfoHelper))]
	public partial class RequestInfo: CORBA.IIDLEntity, IEquatable<MessageRouting.RequestInfo>
	{
		[IdlName("RequestInfo")]
		public MessageRouting.IRouter[] Visited { get; set; }
		[IdlName("RequestInfo")]
		public MessageRouting.IRouter[] ToVisit { get; set; }
		[IdlName("RequestInfo")]
		public CORBA.IObject Target { get; set; }
		[IdlName("RequestInfo")]
		public ushort ProfileIndex { get; set; }
		[IdlName("RequestInfo")]
		public MessageRouting.ReplyDestination ReplyDestination { get; set; }
		[IdlName("RequestInfo")]
		public Messaging.PolicyValue[] SelectedQos { get; set; }
		[IdlName("RequestInfo")]
		public MessageRouting.RequestMessage Payload { get; set; }

		public RequestInfo()
		{
		}

		public RequestInfo(RequestInfo other)
		{
			Visited = other.Visited;
			ToVisit = other.ToVisit;
			Target = other.Target;
			ProfileIndex = other.ProfileIndex;
			ReplyDestination = other.ReplyDestination;
			SelectedQos = other.SelectedQos;
			Payload = other.Payload;
		}

		public RequestInfo(MessageRouting.IRouter[] visited, MessageRouting.IRouter[] toVisit, CORBA.IObject target, ushort profileIndex, MessageRouting.ReplyDestination replyDestination, Messaging.PolicyValue[] selectedQos, MessageRouting.RequestMessage payload)
		{
			this.Visited = visited;
			this.ToVisit = toVisit;
			this.Target = target;
			this.ProfileIndex = profileIndex;
			this.ReplyDestination = replyDestination;
			this.SelectedQos = selectedQos;
			this.Payload = payload;
		}

		public bool Equals(MessageRouting.RequestInfo? other)
		{
			if (other == null) return false;
			if (!Visited.SequenceEqual(other.Visited)) return false;
			if (!ToVisit.SequenceEqual(other.ToVisit)) return false;
			if (!Target.Equals(other.Target)) return false;
			if (!ProfileIndex.Equals(other.ProfileIndex)) return false;
			if (!ReplyDestination.Equals(other.ReplyDestination)) return false;
			if (!SelectedQos.SequenceEqual(other.SelectedQos)) return false;
			if (!Payload.Equals(other.Payload)) return false;
			return true;
		}
	}

}