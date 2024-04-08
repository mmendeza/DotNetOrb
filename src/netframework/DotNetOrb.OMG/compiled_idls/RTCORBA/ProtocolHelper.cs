/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace RTCORBA
{
	public static class ProtocolHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(ProtocolHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateStructTc(RTCORBA.ProtocolHelper.Id(), "Protocol", new CORBA.StructMember[] {new CORBA.StructMember("protocol_type", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 5), null), new CORBA.StructMember("orb_protocol_properties", RTCORBA.ProtocolPropertiesHelper.Type(), null), new CORBA.StructMember("transport_protocol_properties", RTCORBA.ProtocolPropertiesHelper.Type(), null), });
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, RTCORBA.Protocol s)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), s);
		}

		public static RTCORBA.Protocol Extract(CORBA.Any any)
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
			return "IDL:RTCORBA/Protocol:1.0";
		}

		public static RTCORBA.Protocol Read(CORBA.IInputStream inputStream)
		{
			var result = new RTCORBA.Protocol();
			result.ProtocolType = inputStream.ReadULong();
			result.OrbProtocolProperties = RTCORBA.ProtocolPropertiesHelper.Read(inputStream);
			result.TransportProtocolProperties = RTCORBA.ProtocolPropertiesHelper.Read(inputStream);
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, RTCORBA.Protocol s)
		{
			outputStream.WriteULong(s.ProtocolType);
			RTCORBA.ProtocolPropertiesHelper.Write(outputStream, s.OrbProtocolProperties);
			RTCORBA.ProtocolPropertiesHelper.Write(outputStream, s.TransportProtocolProperties);
		}

	}
}
