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
	public static class SecurityContextTypeHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(SecurityContextTypeHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateEnumTc(Security.SecurityContextTypeHelper.Id(),"SecurityContextType", new String[] {"SecClientSecurityContext", "SecServerSecurityContext"});
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, Security.SecurityContextType value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static Security.SecurityContextType Extract(CORBA.Any any)
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
			return "IDL:omg.org/Security/SecurityContextType:1.0";
		}

		public static Security.SecurityContextType Read(CORBA.IInputStream inputStream)
		{
			return (Security.SecurityContextType) inputStream.ReadLong();
		}

		public static void Write(CORBA.IOutputStream outputStream, Security.SecurityContextType value)
		{
			outputStream.WriteLong((int) value);
		}

	}
}
