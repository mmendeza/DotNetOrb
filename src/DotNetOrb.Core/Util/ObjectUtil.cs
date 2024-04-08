// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Net;
using System.Text;

namespace DotNetOrb.Core.Util
{
    public static class ObjectUtil
    {
        //for byte -> hexchar
        private static char[] HEX_LOOKUP = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        public static string? ReadURL(string url)
        {
            StreamReader reader = CreateStreamReader(url);

            try
            {
                return reader?.ReadLine();
            }
            finally
            {
                reader.Close();
            }
        }

        private static StreamReader CreateStreamReader(string url)
        {
            string token = "file://";
            StreamReader sr = null;
            if (url.StartsWith(token))
            {
                try
                {
                    sr = new StreamReader(url.Substring(token.Length));
                }
                catch (Exception)
                {
                    // no worries, let the URL handle it
                }
            }

            if (sr == null)
            {
                sr = new StreamReader(WebRequest.Create(url).GetResponse().GetResponseStream());
            }
            return sr;
        }

        public static string BufToString(byte[] values, int start, int len)
        {
            StringBuilder result = new StringBuilder();
            StringBuilder chars = new StringBuilder();

            for (int i = start; i < start + len; i++)
            {
                if (i % 16 == 0)
                {
                    result.Append(chars.ToString());
                    result.Append('\n');
                    chars.Length = 0;
                }

                chars.Append(ToAscii(values[i]));
                result.Append(ToHex(values[i]));

                if (i % 4 == 3)
                {
                    chars.Append(' ');
                    result.Append(' ');
                }
            }

            if (len % 16 != 0)
            {
                int pad = 0;
                int delta_bytes = 16 - len % 16;

                //rest of line (no of bytes)
                //each byte takes two chars plus one ws
                pad = delta_bytes * 3;

                //additional whitespaces after four bytes
                pad += delta_bytes / 4;

                // additional whitespace after completed 4 byte block
                if (delta_bytes % 4 > 0)
                {
                    pad += 1;
                }

                for (int i = 0; i < pad; i++)
                {
                    chars.Insert(0, ' ');
                }
            }

            result.Append(chars.ToString());
            return result.ToString();
        }

        public static void AppendHex(StringBuilder buffer, int value)
        {
            buffer.Append(HEX_LOOKUP[value]);
        }

        public static string ToHex(byte value)
        {
            StringBuilder buffer = new StringBuilder();

            int upper = value >> 4 & 0x0F;
            AppendHex(buffer, upper);

            int lower = value & 0x0F;
            AppendHex(buffer, lower);

            buffer.Append(' ');

            return buffer.ToString();
        }

        public static char ToAscii(byte value)
        {
            if (value > 31 && value < 127)
            {
                return (char)value;
            }

            return '.';
        }

        public static object? GetInstance(string strFullyQualifiedName, params object[] paramArray)
        {
            if (strFullyQualifiedName != null)
            {
                var type = Type.GetType(strFullyQualifiedName, false, true);
                if (type != null)
                    return Activator.CreateInstance(type, paramArray);
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = asm.GetType(strFullyQualifiedName, false, true);
                    if (type != null)
                        return Activator.CreateInstance(type, paramArray);
                }
            }
            return null;
        }
    }
}
