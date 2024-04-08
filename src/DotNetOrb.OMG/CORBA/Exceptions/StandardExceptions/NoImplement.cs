// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class NoImplement: SystemException
    {
        public NoImplement() : base(null, 0, CompletionStatus.No) { }

        public NoImplement(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public NoImplement(string reason) : base(reason, 0, CompletionStatus.No) { }

        public NoImplement(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
