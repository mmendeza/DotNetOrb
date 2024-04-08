// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.Core.GIOP
{
    public interface IConnectionListener
    {
        void ConnectionClosed(Exception exceptionCause);
        void StreamClosed(Exception exceptionCause);
    }
}
