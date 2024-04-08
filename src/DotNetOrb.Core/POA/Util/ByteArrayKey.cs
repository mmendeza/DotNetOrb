// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;

namespace DotNetOrb.Core.POA.Util
{
    public class ByteArrayKey
    {
        private int cacheH = 0;
        private byte[] bytes = null;
        private string cacheS = null;

        public ByteArrayKey(byte[] array)
        {
            bytes = array;
        }

        public ByteArrayKey(ByteArrayKey bak)
        {
            cacheH = bak.cacheH;
            cacheS = bak.cacheS;
            bytes = bak.bytes;
        }

        public byte[] GetBytes()
        {
            return bytes;
        }

        public override int GetHashCode()
        {
            if (cacheH == 0)
            {
                long h = 1234;

                if (bytes != null && bytes.Length > 0)
                {
                    for (int i = bytes.Length; --i >= 0;)
                    {
                        h ^= bytes[i] * (i + 1);
                    }
                    cacheH = (int)(h >> 32 ^ h);
                }
            }
            return cacheH;
        }

        public override bool Equals(object? obj)
        {
            bool result = false;

            if (obj is ByteArrayKey)
            {
                ByteArrayKey key = (ByteArrayKey)obj;

                if (bytes == key.bytes || bytes == null && key.bytes == null)
                {
                    result = true;
                }
                else if (bytes != null && key.bytes != null)
                {
                    if (bytes.Length == key.bytes.Length)
                    {
                        result = true;
                        for (int i = 0; i < bytes.Length; i++)
                        {
                            if (bytes[i] != key.bytes[i])
                            {
                                result = false;
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public override string ToString()
        {
            if (cacheS == null)
            {
                if (bytes == null)
                {
                    cacheS = "";
                }
                else
                {
                    cacheS = Encoding.Default.GetString(bytes);
                }
            }
            return cacheS;
        }
    }
}
