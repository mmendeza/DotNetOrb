// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;

namespace DotNetOrb.Core.POA.Policies
{
    public class LifespanPolicy : _LifespanPolicyLocalBase
    {
        public override uint PolicyType
        {
            get
            {
                return LIFESPAN_POLICY_ID.Value;
            }
        }
        public override LifespanPolicyValue Value { get; }

        public LifespanPolicy(LifespanPolicyValue value)
        {
            Value = value;
        }

        public override IPolicy Copy()
        {
            return new LifespanPolicy(Value);
        }

        public override void Destroy()
        {
        }
    }
}
