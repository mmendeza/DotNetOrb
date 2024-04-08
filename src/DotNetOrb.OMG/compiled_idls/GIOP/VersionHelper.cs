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
	public static class VersionHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(VersionHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateStructTc(GIOP.VersionHelper.Id(), "Version", new CORBA.StructMember[] {new CORBA.StructMember("major", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 10), null), new CORBA.StructMember("minor", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 10), null), });
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, GIOP.Version s)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), s);
		}

		public static GIOP.Version Extract(CORBA.Any any)
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
			return "IDL:GIOP/Version:1.0";
		}

		public static GIOP.Version Read(CORBA.IInputStream inputStream)
		{
			var result = new GIOP.Version();
			result.Major = inputStream.ReadOctet();
			result.Minor = inputStream.ReadOctet();
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, GIOP.Version s)
		{
			outputStream.WriteOctet(s.Major);
			outputStream.WriteOctet(s.Minor);
		}

	}
}
