// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.Core.BufferManager
{
    internal class LinearExpansionPolicy : IExpansionPolicy
    {
        public int GetExpandedSize(int requestedSize)
        {
            return requestedSize;
        }
    }
}
