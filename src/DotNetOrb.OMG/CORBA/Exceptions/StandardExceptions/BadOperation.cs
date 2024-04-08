// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class BadOperation : CORBA.SystemException
    {
        public BadOperation() : base(null, 0, CompletionStatus.No) { }

        public BadOperation(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public BadOperation(string reason) : base(reason, 0, CompletionStatus.No) { }

        public BadOperation(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
