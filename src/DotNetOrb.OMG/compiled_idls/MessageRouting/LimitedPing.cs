/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:37
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace MessageRouting
{
	[IdlName("LimitedPing")]
	[RepositoryID("IDL:MessageRouting/LimitedPing:1.0")]
	[Helper(typeof(LimitedPingHelper))]
	public abstract class LimitedPing : MessageRouting.UnlimitedPing, CORBA.IStreamableValue
	{
		public override string[] _TruncatableIds { get => new[] {"IDL:MessageRouting/LimitedPing:1.0"}; } 

		public override CORBA.TypeCode _Type { get => MessageRouting.LimitedPingHelper.Type(); }


		[IdlName("interval_limit")]
		public uint IntervalLimit { get; set; }


		public override void _Write(CORBA.IOutputStream outputStream)
		{
			base._Write(outputStream);
			outputStream.WriteULong(IntervalLimit);
		}

		public override void _Read(CORBA.IInputStream inputStream)
		{
			base._Read(inputStream);
			IntervalLimit = inputStream.ReadULong();
		}
	}
}
