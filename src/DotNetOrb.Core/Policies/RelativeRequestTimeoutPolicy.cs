// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;

namespace DotNetOrb.Core.Policies
{
    public class RelativeRequestTimeoutPolicy : _RelativeRequestTimeoutPolicyLocalBase
    {
        public override ulong RelativeExpiry { get; }

        public override uint PolicyType
        {
            get
            {
                return RELATIVE_REQ_TIMEOUT_POLICY_TYPE.Value;
            }
        }

        public RelativeRequestTimeoutPolicy(CORBA.Any value) : base()
        {
            RelativeExpiry = value.ExtractULongLong();
        }

        public RelativeRequestTimeoutPolicy(ulong relativeExpiry) : base()
        {
            RelativeExpiry = relativeExpiry;
        }

        public override IPolicy Copy()
        {
            return new RelativeRequestTimeoutPolicy(RelativeExpiry);
        }

        public override void Destroy()
        {
        }
    }
}
