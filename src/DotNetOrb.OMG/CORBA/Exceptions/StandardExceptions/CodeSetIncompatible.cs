// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class CodeSetIncompatible: SystemException
    {
        public CodeSetIncompatible() : base(null, 0, CompletionStatus.No) { }

        public CodeSetIncompatible(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public CodeSetIncompatible(string reason) : base(reason, 0, CompletionStatus.No) { }

        public CodeSetIncompatible(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
