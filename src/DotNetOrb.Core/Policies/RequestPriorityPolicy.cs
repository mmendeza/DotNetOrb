// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;

namespace DotNetOrb.Core.Policies
{
    public class RequestPriorityPolicy : _RequestPriorityPolicyLocalBase
    {
        public override PriorityRange PriorityRange { get; }

        public override uint PolicyType
        {
            get
            {
                return REQUEST_PRIORITY_POLICY_TYPE.Value;
            }
        }

        public RequestPriorityPolicy(CORBA.Any value) : base()
        {
            PriorityRange = PriorityRangeHelper.Extract(value);
        }

        public RequestPriorityPolicy(PriorityRange priorityRange) : base()
        {
            PriorityRange = priorityRange;
        }

        public override IPolicy Copy()
        {
            return new RequestPriorityPolicy(PriorityRange);
        }

        public override void Destroy()
        {
        }
    }
}
