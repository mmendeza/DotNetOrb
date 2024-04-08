// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class BadParam : CORBA.SystemException
    {
        public BadParam() : base(null, 0, CompletionStatus.No) { }

        public BadParam(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public BadParam(string reason) : base(reason, 0, CompletionStatus.No) { }

        public BadParam(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
