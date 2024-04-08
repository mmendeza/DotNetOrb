/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CORBA
{
	public static class TCKindHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(TCKindHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateEnumTc(CORBA.TCKindHelper.Id(),"TCKind", new String[] {"tk_null", "tk_void", "tk_short", "tk_long", "tk_ushort", "tk_ulong", "tk_float", "tk_double", "tk_boolean", "tk_char", "tk_octet", "tk_any", "tk_TypeCode", "tk_Principal", "tk_objref", "tk_struct", "tk_union", "tk_enum", "tk_string", "tk_sequence", "tk_array", "tk_alias", "tk_except", "tk_longlong", "tk_ulonglong", "tk_longdouble", "tk_wchar", "tk_wstring", "tk_fixed", "tk_value", "tk_value_box", "tk_native", "tk_abstract_interface", "tk_local_interface", "tk_component", "tk_home", "tk_event"});
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, CORBA.TCKind value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static CORBA.TCKind Extract(CORBA.Any any)
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
			return "IDL:CORBA/TCKind:1.0";
		}

		public static CORBA.TCKind Read(CORBA.IInputStream inputStream)
		{
			return (CORBA.TCKind) inputStream.ReadLong();
		}

		public static void Write(CORBA.IOutputStream outputStream, CORBA.TCKind value)
		{
			outputStream.WriteLong((int) value);
		}

	}
}
