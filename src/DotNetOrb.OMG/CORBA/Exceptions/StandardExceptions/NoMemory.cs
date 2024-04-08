// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class NoMemory: SystemException
    {
        public NoMemory() : base(null, 0, CompletionStatus.No) { }

        public NoMemory(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public NoMemory(string reason) : base(reason, 0, CompletionStatus.No) { }

        public NoMemory(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
