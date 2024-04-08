// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.Core.POA
{
    public interface IRPPoolManagerListener : IEventListener
    {
        void ProcessorAddedToPool(RequestProcessor processor, int poolCount, int poolSize);
        void ProcessorRemovedFromPool(RequestProcessor processor, int poolCount, int poolSize);
    }
}
