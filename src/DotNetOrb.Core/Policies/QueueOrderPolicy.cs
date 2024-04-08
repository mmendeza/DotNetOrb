// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;

namespace DotNetOrb.Core.Policies
{
    public class QueueOrderPolicy : _QueueOrderPolicyLocalBase
    {
        public override ushort AllowedOrders { get; }

        public override uint PolicyType
        {
            get
            {
                return QUEUE_ORDER_POLICY_TYPE.Value;
            }
        }

        public QueueOrderPolicy(CORBA.Any value) : base()
        {
            AllowedOrders = value.ExtractUShort();
        }

        public QueueOrderPolicy(ushort allowedOrders) : base()
        {
            AllowedOrders = allowedOrders;
        }

        public override IPolicy Copy()
        {
            return new QueueOrderPolicy(AllowedOrders);
        }

        public override void Destroy()
        {
        }
    }
}
