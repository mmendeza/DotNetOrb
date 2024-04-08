/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace IOP
{
	public static class EncodingHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(EncodingHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateStructTc(IOP.EncodingHelper.Id(), "Encoding", new CORBA.StructMember[] {new CORBA.StructMember("format", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 4), null), new CORBA.StructMember("major_version", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 10), null), new CORBA.StructMember("minor_version", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 10), null), });
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, IOP.Encoding s)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), s);
		}

		public static IOP.Encoding Extract(CORBA.Any any)
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
			return "IDL:IOP/Encoding:1.0";
		}

		public static IOP.Encoding Read(CORBA.IInputStream inputStream)
		{
			var result = new IOP.Encoding();
			result.Format = inputStream.ReadShort();
			result.MajorVersion = inputStream.ReadOctet();
			result.MinorVersion = inputStream.ReadOctet();
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, IOP.Encoding s)
		{
			outputStream.WriteShort(s.Format);
			outputStream.WriteOctet(s.MajorVersion);
			outputStream.WriteOctet(s.MinorVersion);
		}

	}
}
