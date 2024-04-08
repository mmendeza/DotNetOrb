/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:29
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Security
{
	public static class DayOfTheWeekHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(DayOfTheWeekHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateEnumTc(Security.DayOfTheWeekHelper.Id(),"DayOfTheWeek", new String[] {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"});
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, Security.DayOfTheWeek value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static Security.DayOfTheWeek Extract(CORBA.Any any)
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
			return "IDL:omg.org/Security/DayOfTheWeek:1.0";
		}

		public static Security.DayOfTheWeek Read(CORBA.IInputStream inputStream)
		{
			return (Security.DayOfTheWeek) inputStream.ReadLong();
		}

		public static void Write(CORBA.IOutputStream outputStream, Security.DayOfTheWeek value)
		{
			outputStream.WriteLong((int) value);
		}

	}
}