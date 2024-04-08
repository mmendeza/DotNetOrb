/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:30
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableGroup
{
	public static class MembershipStyleValueHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(MembershipStyleValueHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateAliasTc(PortableGroup.MembershipStyleValueHelper.Id(), "MembershipStyleValue", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 3));
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, int value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static int Extract(CORBA.Any any)
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
			return "IDL:omg.org/PortableGroup/MembershipStyleValue:1.0";
		}

		public static int Read(CORBA.IInputStream inputStream)
		{
			int result;
			result = inputStream.ReadLong();
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, int value)
		{
			outputStream.WriteLong(value);
		}

	}
}