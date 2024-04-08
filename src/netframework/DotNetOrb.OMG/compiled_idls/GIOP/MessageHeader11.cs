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
	[RepositoryID("IDL:GIOP/MessageHeader_1_1:1.0")]
	[Helper(typeof(MessageHeader11Helper))]
	public partial class MessageHeader11: CORBA.IIDLEntity, IEquatable<GIOP.MessageHeader11>
	{
		[IdlName("MessageHeader_1_1")]
		public char[] Magic { get; set; }
		[IdlName("MessageHeader_1_1")]
		public GIOP.Version GiopVersion { get; set; }
		[IdlName("MessageHeader_1_1")]
		public byte Flags { get; set; }
		[IdlName("MessageHeader_1_1")]
		public byte MessageType { get; set; }
		[IdlName("MessageHeader_1_1")]
		public uint MessageSize { get; set; }

		public MessageHeader11()
		{
		}

		public MessageHeader11(MessageHeader11 other)
		{
			Magic = other.Magic;
			GiopVersion = other.GiopVersion;
			Flags = other.Flags;
			MessageType = other.MessageType;
			MessageSize = other.MessageSize;
		}

		public MessageHeader11(char[] magic, GIOP.Version giopVersion, byte flags, byte messageType, uint messageSize)
		{
			this.Magic = magic;
			this.GiopVersion = giopVersion;
			this.Flags = flags;
			this.MessageType = messageType;
			this.MessageSize = messageSize;
		}

		public bool Equals(GIOP.MessageHeader11? other)
		{
			if (other == null) return false;
			if (!Magic.SequenceEqual(other.Magic)) return false;
			if (!GiopVersion.Equals(other.GiopVersion)) return false;
			if (!Flags.Equals(other.Flags)) return false;
			if (!MessageType.Equals(other.MessageType)) return false;
			if (!MessageSize.Equals(other.MessageSize)) return false;
			return true;
		}
	}

}