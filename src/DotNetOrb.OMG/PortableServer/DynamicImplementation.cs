// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;

namespace PortableServer
{
    public abstract class DynamicImplementation: Servant
    {
        abstract public void Invoke(IServerRequest request);
    }
}
