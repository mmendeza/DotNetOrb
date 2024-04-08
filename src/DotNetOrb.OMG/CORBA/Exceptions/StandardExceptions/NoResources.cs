// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class NoResources: SystemException
    {
        public NoResources() : base(null, 0, CompletionStatus.No) { }

        public NoResources(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public NoResources(string reason) : base(reason, 0, CompletionStatus.No) { }

        public NoResources(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
