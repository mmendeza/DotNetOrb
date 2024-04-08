// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;
using TimeBase;

namespace DotNetOrb.Core.Policies
{
    public class ReplyStartTimePolicy : _ReplyStartTimePolicyLocalBase
    {
        public override UtcT StartTime { get; }

        public override uint PolicyType
        {
            get
            {
                return REPLY_END_TIME_POLICY_TYPE.Value;
            }
        }

        public ReplyStartTimePolicy(CORBA.Any value) : base()
        {
            StartTime = UtcTHelper.Extract(value);
        }

        public ReplyStartTimePolicy(UtcT endTime) : base()
        {
            StartTime = endTime;
        }

        public override IPolicy Copy()
        {
            return new ReplyStartTimePolicy(StartTime);
        }

        public override void Destroy()
        {
        }
    }
}
