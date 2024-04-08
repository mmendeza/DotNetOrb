// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.DSI;

namespace DotNetOrb.Core.POA
{
    public interface IRequestQueueListener : IEventListener
    {
        void RequestAddedToQueue(ServerRequest request, int queueSize);
        void RequestRemovedFromQueue(ServerRequest request, int queueSize);
    }
}
