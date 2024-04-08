// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using System;

namespace DotNetOrb.Core.BufferManager
{
    internal class DefaultExpansionPolicy : IExpansionPolicy
    {
        private int scale = 4;
        private int divider = 6;

        public DefaultExpansionPolicy(IConfiguration config)
        {
            scale = config.GetAsInteger("DotNetOrb.BufferManager.DefaultExpansionPolicy.Scale", 4);
            divider = config.GetAsInteger("DotNetOrb.BufferManager.DefaultExpansionPolicy.Divider", 6);
        }

        public int GetExpandedSize(int requestedSize)
        {
            double multiplier = scale - Math.Log(requestedSize) / divider;
            multiplier = multiplier < 1.0 ? 1.0 : multiplier;
            return (int)Math.Floor(multiplier * requestedSize);
        }
    }
}
