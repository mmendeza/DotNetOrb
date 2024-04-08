// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;

namespace DotNetOrb.Core.Policies
{
    public class ReplyPriorityPolicy : _ReplyPriorityPolicyLocalBase
    {
        public override PriorityRange PriorityRange { get; }

        public override uint PolicyType
        {
            get
            {
                return REPLY_PRIORITY_POLICY_TYPE.Value;
            }
        }

        public ReplyPriorityPolicy(CORBA.Any value) : base()
        {
            PriorityRange = PriorityRangeHelper.Extract(value);
        }

        public ReplyPriorityPolicy(PriorityRange priorityRange) : base()
        {
            PriorityRange = priorityRange;
        }

        public override IPolicy Copy()
        {
            return new ReplyPriorityPolicy(PriorityRange);
        }

        public override void Destroy()
        {
        }
    }
}
