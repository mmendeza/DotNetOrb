// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;

namespace DotNetOrb.Core.Policies
{
    public class RelativeRoundtripTimeoutPolicy : _RelativeRoundtripTimeoutPolicyLocalBase
    {
        public override ulong RelativeExpiry { get; }

        public override uint PolicyType
        {
            get
            {
                return RELATIVE_RT_TIMEOUT_POLICY_TYPE.Value;
            }
        }

        public RelativeRoundtripTimeoutPolicy(CORBA.Any value) : base()
        {
            RelativeExpiry = value.ExtractULongLong();
        }

        public RelativeRoundtripTimeoutPolicy(ulong relativeExpiry) : base()
        {
            RelativeExpiry = relativeExpiry;
        }

        public override IPolicy Copy()
        {
            return new RelativeRoundtripTimeoutPolicy(RelativeExpiry);
        }

        public override void Destroy()
        {
        }
    }
}
