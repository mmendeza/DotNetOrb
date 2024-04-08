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
	public static class LocateStatusType12Helper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(LocateStatusType12Helper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateEnumTc(GIOP.LocateStatusType12Helper.Id(),"LocateStatusType_1_2", new String[] {"UNKNOWN_OBJECT", "OBJECT_HERE", "OBJECT_FORWARD", "OBJECT_FORWARD_PERM", "LOC_SYSTEM_EXCEPTION", "LOC_NEEDS_ADDRESSING_MODE"});
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, GIOP.LocateStatusType12 value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static GIOP.LocateStatusType12 Extract(CORBA.Any any)
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
			return "IDL:GIOP/LocateStatusType_1_2:1.0";
		}

		public static GIOP.LocateStatusType12 Read(CORBA.IInputStream inputStream)
		{
			return (GIOP.LocateStatusType12) inputStream.ReadLong();
		}

		public static void Write(CORBA.IOutputStream outputStream, GIOP.LocateStatusType12 value)
		{
			outputStream.WriteLong((int) value);
		}

	}
}