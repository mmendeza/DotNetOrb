﻿// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class Marshal : CORBA.SystemException
    {
        public Marshal() : base(null, 0, CompletionStatus.No) { }

        public Marshal(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public Marshal(string reason) : base(reason, 0, CompletionStatus.No) { }

        public Marshal(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
