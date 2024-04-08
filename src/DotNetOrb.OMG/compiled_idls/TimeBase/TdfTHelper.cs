/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace TimeBase
{
	public static class TdfTHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(TdfTHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateAliasTc(TimeBase.TdfTHelper.Id(), "TdfT", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 4));
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, short value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static short Extract(CORBA.Any any)
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
			return "IDL:omg.org/TimeBase/TdfT:1.0";
		}

		public static short Read(CORBA.IInputStream inputStream)
		{
			short result;
			result = inputStream.ReadShort();
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, short value)
		{
			outputStream.WriteShort(value);
		}

	}
}