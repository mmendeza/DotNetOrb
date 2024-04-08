// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class BadTypeCode : CORBA.SystemException
    {
        public BadTypeCode() : base(null, 0, CompletionStatus.No) { }

        public BadTypeCode(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public BadTypeCode(string reason) : base(reason, 0, CompletionStatus.No) { }

        public BadTypeCode(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
