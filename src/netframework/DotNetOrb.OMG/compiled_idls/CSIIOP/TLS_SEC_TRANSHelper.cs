/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:29
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CSIIOP
{
	public static class TLS_SEC_TRANSHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(TLS_SEC_TRANSHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateStructTc(CSIIOP.TLS_SEC_TRANSHelper.Id(), "TLS_SEC_TRANS", new CORBA.StructMember[] {new CORBA.StructMember("target_supports", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 2), null), new CORBA.StructMember("target_requires", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 2), null), new CORBA.StructMember("addresses", CORBA.ORB.Init().CreateSequenceTc(0, CSIIOP.TransportAddressHelper.Type()), null), });
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, CSIIOP.TLS_SEC_TRANS s)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), s);
		}

		public static CSIIOP.TLS_SEC_TRANS Extract(CORBA.Any any)
		{
			var inputStream = any.CreateInputStream();
			try
			{
				return Read(inputStream);
			}
			finally
			{
				inputStream.Close();
			}
		}

		public static string Id()
		{
			return "IDL:CSIIOP/TLS_SEC_TRANS:1.0";
		}

		public static CSIIOP.TLS_SEC_TRANS Read(CORBA.IInputStream inputStream)
		{
			var result = new CSIIOP.TLS_SEC_TRANS();
			result.TargetSupports = inputStream.ReadUShort();
			result.TargetRequires = inputStream.ReadUShort();
			{
				var _capacity0 = inputStream.ReadLong();
				if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
				{
					throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
				}
				result.Addresses = new CSIIOP.TransportAddress[_capacity0];
				for (int i0 = 0; i0 < _capacity0; i0++)
				{
					CSIIOP.TransportAddress _item0;
					_item0 = CSIIOP.TransportAddressHelper.Read(inputStream);
					result.Addresses[i0] = _item0;
				}
			}
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, CSIIOP.TLS_SEC_TRANS s)
		{
			outputStream.WriteUShort(s.TargetSupports);
			outputStream.WriteUShort(s.TargetRequires);
			{
				outputStream.WriteLong(s.Addresses.Length);
				for (int i0 = 0; i0 < s.Addresses.Length; i0++)
				{
					CSIIOP.TransportAddressHelper.Write(outputStream, s.Addresses[i0]);
				}
			}
		}

	}
}
