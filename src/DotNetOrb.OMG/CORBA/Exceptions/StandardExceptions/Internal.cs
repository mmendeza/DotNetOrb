// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class Internal : CORBA.SystemException
    {
        public Internal() : base(null, 0, CompletionStatus.No) { }

        public Internal(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public Internal(string reason) : base(reason, 0, CompletionStatus.No) { }

        public Internal(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
