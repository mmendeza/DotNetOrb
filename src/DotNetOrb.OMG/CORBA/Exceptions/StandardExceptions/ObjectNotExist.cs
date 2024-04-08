// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class ObjectNotExist : SystemException
    {
        public ObjectNotExist() : base(null, 0, CompletionStatus.No) { }

        public ObjectNotExist(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public ObjectNotExist(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
