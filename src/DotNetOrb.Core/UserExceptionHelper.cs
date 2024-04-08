// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;

namespace DotNetOrb.Core
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
                        type = CORBA.ORB.Init().CreateExceptionTc(Id(), "UserException", new StructMember[] { });
                    }
                }
            }
            return type;
        }

        public static void Insert(CORBA.Any any, UserException e)
        {
            any.Type = Type();
            Write(any.CreateOutputStream(), e);
        }

        public static UserException Extract(CORBA.Any any)
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

        public static UserException Read(IInputStream inputStream)
        {
            var id = inputStream.ReadString();
            if (!id.Equals(Id()))
            {
                throw new Marshal("Wrong id: " + id);
            }
            var result = new UserException();
            return result;
        }

        public static void Write(IOutputStream outputStream, UserException e)
        {
            outputStream.WriteString(Id());
        }

    }
}
