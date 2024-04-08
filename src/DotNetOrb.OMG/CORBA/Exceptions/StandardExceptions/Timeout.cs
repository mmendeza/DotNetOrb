// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class Timeout: SystemException
    {
        public Timeout() : base(null, 0, CompletionStatus.No) { }

        public Timeout(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public Timeout(string reason) : base(reason, 0, CompletionStatus.No) { }

        public Timeout(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
