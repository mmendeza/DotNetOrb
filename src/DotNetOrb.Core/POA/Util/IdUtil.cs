// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Util;
using DotNetty.Common.Utilities;
using System;
using System.Linq;
using System.Text;

namespace DotNetOrb.Core.POA.Util
{
    internal class IdUtil
    {
        private static Random rnd = new Random(9999);

        public static byte[] Concat(byte[] first, byte[] second)
        {
            byte[] result = new byte[first.Length + second.Length];
            Array.Copy(first, 0, result, 0, first.Length);
            Array.Copy(second, 0, result, first.Length, second.Length);
            return result;
        }

        /// <summary>
        /// Creates an id as a concatenation of the current time in msec and 4 random bytes
        /// </summary>
        /// <returns></returns>
        public static byte[] CreateId()
        {
            byte[] time = ToId(Time.NanoTime());
            byte[] random = ToId(rnd.NextLong());
            return Concat(time, random);
        }

        public static bool Equals(byte[] first, byte[] second)
        {
            return first.SequenceEqual(second);
        }


        /// <summary>
        /// * compares first len bytes of two byte arrays
        /// </summary>        
        public static bool Equals(byte[] first, byte[] second, int len)
        {
            if (first == second)
            {
                return true;
            }

            if (first.Length < len || second.Length < len)
            {
                return false;
            }

            for (int i = 0; i < len; i++)
            {
                if (first[i] != second[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Extracts len bytes from id, the first byte to be copied is at index offset
        /// </summary>
        public static byte[] Extract(byte[] id, int offset, int len)
        {
            byte[] result = new byte[len];
            Array.Copy(id, offset, result, 0, len);
            return result;
        }

        /// <summary>
        /// Converts the number l into a byte array
        /// </summary>        
        public static byte[] ToId(long l)
        {
            string str = Convert.ToString(l, 8);

            if (str.Length % 2 != 0)
            {
                str = "0" + str;
            }

            byte[] result = new byte[str.Length / 2];
            StringBuilder number = new StringBuilder("xx");

            for (int i = 0; i < result.Length; i++)
            {
                number[0] = str[i * 2];
                number[1] = str[i * 2 + 1];
                result[i] = Convert.ToByte(number.ToString());

                if (result[i] == POAConstants.ObjectKeySeparator)
                {
                    result[i] = 48;     // char 0 , hex 30
                }
                else if (result[i] == POAConstants.MaskByte)
                {
                    result[i] = 19;     // char !!, hex 13
                }
            }
            return result;
        }
    }
}
