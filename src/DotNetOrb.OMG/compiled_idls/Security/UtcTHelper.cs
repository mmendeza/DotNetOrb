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
	public static class UtcTHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(UtcTHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateAliasTc(Security.UtcTHelper.Id(), "UtcT", TimeBase.UtcTHelper.Type());
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, TimeBase.UtcT value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static TimeBase.UtcT Extract(CORBA.Any any)
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
			return "IDL:omg.org/Security/UtcT:1.0";
		}

		public static TimeBase.UtcT Read(CORBA.IInputStream inputStream)
		{
			TimeBase.UtcT result;
			result = TimeBase.UtcTHelper.Read(inputStream);
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, TimeBase.UtcT value)
		{
			TimeBase.UtcTHelper.Write(outputStream, value);
		}

	}
}