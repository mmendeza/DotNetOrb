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
	[RepositoryID("IDL:MessageRouting/RequestMessage:1.0")]
	[Helper(typeof(RequestMessageHelper))]
	public partial class RequestMessage: CORBA.IIDLEntity, IEquatable<MessageRouting.RequestMessage>
	{
		[IdlName("RequestMessage")]
		public GIOP.Version GiopVersion { get; set; }
		[IdlName("RequestMessage")]
		public IOP.ServiceContext[] ServiceContexts { get; set; }
		[IdlName("RequestMessage")]
		public byte ResponseFlags { get; set; }
		[IdlName("RequestMessage")]
		public byte[] Reserved { get; set; }
		[IdlName("RequestMessage")]
		public byte[] ObjectKey { get; set; }
		[IdlName("RequestMessage")]
		[WideChar(false)]
		public string Operation { get; set; }
		[IdlName("RequestMessage")]
		public MessageRouting.MessageBody Body { get; set; }

		public RequestMessage()
		{
		}

		public RequestMessage(RequestMessage other)
		{
			GiopVersion = other.GiopVersion;
			ServiceContexts = other.ServiceContexts;
			ResponseFlags = other.ResponseFlags;
			Reserved = other.Reserved;
			ObjectKey = other.ObjectKey;
			Operation = other.Operation;
			Body = other.Body;
		}

		public RequestMessage(GIOP.Version giopVersion, IOP.ServiceContext[] serviceContexts, byte responseFlags, byte[] reserved, byte[] objectKey, string operation, MessageRouting.MessageBody body)
		{
			this.GiopVersion = giopVersion;
			this.ServiceContexts = serviceContexts;
			this.ResponseFlags = responseFlags;
			this.Reserved = reserved;
			this.ObjectKey = objectKey;
			this.Operation = operation;
			this.Body = body;
		}

		public bool Equals(MessageRouting.RequestMessage? other)
		{
			if (other == null) return false;
			if (!GiopVersion.Equals(other.GiopVersion)) return false;
			if (!ServiceContexts.SequenceEqual(other.ServiceContexts)) return false;
			if (!ResponseFlags.Equals(other.ResponseFlags)) return false;
			if (!Reserved.SequenceEqual(other.Reserved)) return false;
			if (!ObjectKey.SequenceEqual(other.ObjectKey)) return false;
			if (!Operation.Equals(other.Operation)) return false;
			if (!Body.Equals(other.Body)) return false;
			return true;
		}
	}

}
