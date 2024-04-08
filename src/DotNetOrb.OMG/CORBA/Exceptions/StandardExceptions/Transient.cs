﻿// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class Transient: SystemException
    {
        public Transient() : base(null, 0, CompletionStatus.No) { }

        public Transient(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public Transient(string reason) : base(reason, 0, CompletionStatus.No) { }

        public Transient(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
