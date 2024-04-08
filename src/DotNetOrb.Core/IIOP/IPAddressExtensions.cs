// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;

namespace DotNetOrb.Core.IIOP
{
    internal static class IPAddressExtensions
    {
        public static bool IsLinkLocal(this IPAddress address)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                if (address.ToString().StartsWith("169.254."))
                {
                    return true;
                }
            }
            else if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                return address.IsIPv6LinkLocal;
            }
            return false;
        }
    }
}
