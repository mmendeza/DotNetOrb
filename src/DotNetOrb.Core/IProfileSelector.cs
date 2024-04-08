// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.GIOP;
using ETF;
using System.Collections.Generic;

namespace DotNetOrb.Core
{
    /// <summary>
    /// A IProfileSelector decides, on the client side, which Profile from an object's IOR should be used to communicate with the object.
    /// </summary>
    public interface IProfileSelector
    {
        IProfile SelectProfile(List<IProfile> profiles, ConnectionManager ccm);
        IProfile SelectNextProfile(List<IProfile> profiles, IProfile lastProfile);
    }
}
