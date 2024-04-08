// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class Unknown: SystemException
    {
        public Unknown() : base(null, 0, CompletionStatus.No) { }

        public Unknown(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public Unknown(string reason) : base(reason, 0, CompletionStatus.No) { }

        public Unknown(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
