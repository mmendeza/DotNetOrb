// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;

namespace DotNetOrb.Core.Policies
{
    public class MaxHopsPolicy : _MaxHopsPolicyLocalBase
    {
        public override ushort MaxHops { get; }

        public override uint PolicyType
        {
            get
            {
                return MAX_HOPS_POLICY_TYPE.Value;
            }
        }

        public MaxHopsPolicy(CORBA.Any value) : base()
        {
            MaxHops = value.ExtractUShort();
        }

        public MaxHopsPolicy(ushort maxHops) : base()
        {
            MaxHops = maxHops;
        }

        public override IPolicy Copy()
        {
            return new MaxHopsPolicy(MaxHops);
        }

        public override void Destroy()
        {
        }
    }
}
