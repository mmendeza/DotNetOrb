// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class BadInvOrder : CORBA.SystemException
    {
        public BadInvOrder() : base(null, 0, CompletionStatus.No) { }

        public BadInvOrder(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public BadInvOrder(string reason) : base(reason, 0, CompletionStatus.No) { }

        public BadInvOrder(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
