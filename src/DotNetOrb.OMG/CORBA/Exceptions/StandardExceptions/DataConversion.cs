// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class DataConversion : CORBA.SystemException
    {
        public DataConversion() : base(null, 0, CompletionStatus.No) { }

        public DataConversion(int minor, CompletionStatus completed) : base(null, minor, completed) { }

        public DataConversion(string reason) : base(reason, 0, CompletionStatus.No) { }

        public DataConversion(string reason, int minor, CompletionStatus completed) : base(reason, minor, completed) { }
    }
}
