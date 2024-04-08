// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;
using TimeBase;

namespace DotNetOrb.Core.Policies
{
    public class ReplyEndTimePolicy : _ReplyEndTimePolicyLocalBase
    {
        public override UtcT EndTime { get; }

        public override uint PolicyType
        {
            get
            {
                return REPLY_END_TIME_POLICY_TYPE.Value;
            }
        }

        public ReplyEndTimePolicy(CORBA.Any value) : base()
        {
            EndTime = UtcTHelper.Extract(value);
        }

        public ReplyEndTimePolicy(UtcT endTime) : base()
        {
            EndTime = endTime;
        }

        public override IPolicy Copy()
        {
            return new ReplyEndTimePolicy(EndTime);
        }

        public override void Destroy()
        {
        }
    }
}
