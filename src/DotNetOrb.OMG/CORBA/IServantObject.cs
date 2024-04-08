// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    public interface IServantObject
    {
        object Servant { get; }
        void NormalCompletion();
        void ExceptionalCompletion(Exception ex);
    }
}
