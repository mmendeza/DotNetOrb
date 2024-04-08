// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class StringValueHelper: IBoxedValueHelper
    {
        public static string Id 
        { 
            get
            {
               return "IDL:omg.org/CORBA/StringValue:1.0";
            }

        }

        public string GetId()
        {
            return StringValueHelper.Id;
        }

        private static TypeCode type = ORB.Init().CreateValueBoxTc(Id, "StringValue", ORB.Init().CreateStringTc(0));
        public static TypeCode Type
        {
            get
            {
                return type;
            }
        }

        public static string Read(IInputStream inputStream)
        {
            string result;
            result = inputStream.ReadString();
            return result;
        }
        public static void Write(IOutputStream outputStream, string value)
        {
            outputStream.WriteString(value);
        }

        public object ReadValue(IInputStream inputStream)
        {
            return Read(inputStream);
        }

        public void WriteValue(IOutputStream outputStream, object value)
        {
            Write(outputStream, (string) value);
        }
    }

}
