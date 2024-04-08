// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using PortableServer;

namespace DotNetOrb.Core.POA
{
    public interface IInvocationContext
    {
        byte[] ObjectId { get; }
        ORB ORB { get; }
        POA POA { get; }
        Servant Servant { get; }
    }
}
