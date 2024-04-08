// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class ObjAdapter : SystemException
    {
        public ObjAdapter(string reason) : base(reason) { }

        public ObjAdapter() : base(null, 0, CompletionStatus.No) { }

        public ObjAdapter(int minor, CompletionStatus completed) : base(null, minor, completed) { }
        public ObjAdapter(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
