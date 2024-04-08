// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    public class ValueBaseHelper
    {
        private static string id = "IDL:omg.org/CORBA/ValueBase:1.0";
        private static TypeCode typeCode;

        public static void Insert(Any any, Object value)
        {
            IOutputStream outputStream = any.CreateOutputStream();

            try
            {
                any.Type = Type();
                Write(outputStream, value);
                any.ReadValue(outputStream.CreateInputStream(), Type());
            }
            finally
            {
                try
                {
                    outputStream.Close();
                }
                catch (Exception e)
                {
                }
            }
        }

        public static object Extract(Any any)
        {
            return Read(any.CreateInputStream());
        }

        public static TypeCode Type()
        {
            if (typeCode == null)
            {
                var orb = ORB.Init();
                typeCode = orb.GetPrimitiveTc(TCKind.TkValue);
            }
            return typeCode;
        }

        public static string Id()
        {
            return id;
        }

        public static object Read(IInputStream inputStream)
        {
            return ((IInputStream)inputStream).ReadValue();
        }

        public static void Write(IOutputStream outputStream, Object value)
        {
            ((IOutputStream)outputStream).WriteValue(value, id);
        }
    }
}
