// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.Core.BufferManager
{
    public interface IExpansionPolicy
    {
        int GetExpandedSize(int requestedSize);
    }
}
