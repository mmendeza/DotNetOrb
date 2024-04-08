// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public static class UserExceptionHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(UserExceptionHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateExceptionTc(CORBA.UserExceptionHelper.Id(), "UserException", new CORBA.StructMember[] {});
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, CORBA.UserException e)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), e);
		}

		public static CORBA.UserException Extract(CORBA.Any any)
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
			return "IDL:CORBA/UserException:1.0";
		}

		public static CORBA.UserException Read(CORBA.IInputStream inputStream)
		{
			var id = inputStream.ReadString();
			if (!id.Equals(Id()))
			{
				throw new CORBA.Marshal("Wrong id: " + id);
			}
			var result = new CORBA.UserException();
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, CORBA.UserException e)
		{
			outputStream.WriteString(Id());
		}

	}
}
