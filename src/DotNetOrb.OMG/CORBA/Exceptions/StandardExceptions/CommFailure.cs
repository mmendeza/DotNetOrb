// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class CommFailure: SystemException
    {
        public CommFailure() : base(null, 0, CompletionStatus.No) { }

        public CommFailure(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public CommFailure(string reason) : base(reason, 0, CompletionStatus.No) { }

        public CommFailure(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
