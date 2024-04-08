// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;

namespace DotNetOrb.Core.Policies
{
    public class RoutingPolicy : _RoutingPolicyLocalBase
    {
        public override RoutingTypeRange RoutingRange { get; }

        public override uint PolicyType
        {
            get
            {
                return ROUTING_POLICY_TYPE.Value;
            }
        }

        public RoutingPolicy(CORBA.Any value) : base()
        {
            RoutingRange = RoutingTypeRangeHelper.Extract(value);
        }

        public RoutingPolicy(RoutingTypeRange priorityRange) : base()
        {
            RoutingRange = priorityRange;
        }

        public override IPolicy Copy()
        {
            return new RoutingPolicy(RoutingRange);
        }

        public override void Destroy()
        {
        }
    }
}
