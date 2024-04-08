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
	public static class ValueMemberHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(ValueMemberHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateStructTc(CORBA.ValueMemberHelper.Id(), "ValueMember", new CORBA.StructMember[] {new CORBA.StructMember("name", CORBA.ORB.Init().CreateStringTc(0), null), new CORBA.StructMember("id", CORBA.ORB.Init().CreateStringTc(0), null), new CORBA.StructMember("defined_in", CORBA.ORB.Init().CreateStringTc(0), null), new CORBA.StructMember("version", CORBA.ORB.Init().CreateStringTc(0), null), new CORBA.StructMember("type", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 12), null), new CORBA.StructMember("type_def", CORBA.IDLTypeHelper.Type(), null), new CORBA.StructMember("access", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 4), null), });
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, CORBA.ValueMember s)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), s);
		}

		public static CORBA.ValueMember Extract(CORBA.Any any)
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
			return "IDL:CORBA/ValueMember:1.0";
		}

		public static CORBA.ValueMember Read(CORBA.IInputStream inputStream)
		{
			var result = new CORBA.ValueMember();
			result.Name = inputStream.ReadString();
			result.Id = inputStream.ReadString();
			result.DefinedIn = inputStream.ReadString();
			result.Version = inputStream.ReadString();
			result.Type = inputStream.ReadTypeCode();
			result.TypeDef = CORBA.IDLTypeHelper.Read(inputStream);
			result.Access = inputStream.ReadShort();
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, CORBA.ValueMember s)
		{
			outputStream.WriteString(s.Name);
			outputStream.WriteString(s.Id);
			outputStream.WriteString(s.DefinedIn);
			outputStream.WriteString(s.Version);
			outputStream.WriteTypeCode(s.Type);
			CORBA.IDLTypeHelper.Write(outputStream, s.TypeDef);
			outputStream.WriteShort(s.Access);
		}

	}
}