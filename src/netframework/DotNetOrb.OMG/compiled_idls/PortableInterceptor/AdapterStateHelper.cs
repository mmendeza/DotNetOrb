/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:37
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableInterceptor
{
	public static class AdapterStateHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(AdapterStateHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateAliasTc(PortableInterceptor.AdapterStateHelper.Id(), "AdapterState", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 4));
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
			return "IDL:PortableInterceptor/AdapterState:1.0";
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