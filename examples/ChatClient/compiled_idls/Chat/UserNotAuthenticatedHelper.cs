/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 26/02/2024 12:18:44
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Chat
{
	public static class UserNotAuthenticatedHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(UserNotAuthenticatedHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateExceptionTc(Chat.UserNotAuthenticatedHelper.Id(), "UserNotAuthenticated", new CORBA.StructMember[] {});
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, Chat.UserNotAuthenticated e)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), e);
		}

		public static Chat.UserNotAuthenticated Extract(CORBA.Any any)
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
			return "IDL:Chat/UserNotAuthenticated:1.0";
		}

		public static Chat.UserNotAuthenticated Read(CORBA.IInputStream inputStream)
		{
			var id = inputStream.ReadString();
			if (!id.Equals(Id()))
			{
				throw new CORBA.Marshal("Wrong id: " + id);
			}
			var result = new Chat.UserNotAuthenticated();
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, Chat.UserNotAuthenticated e)
		{
			outputStream.WriteString(Id());
		}

	}
}