// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;

namespace DotNetOrb.Core.POA.Policies
{
    public class ImplicitActivationPolicy : _ImplicitActivationPolicyLocalBase
    {
        public override uint PolicyType
        {
            get
            {
                return IMPLICIT_ACTIVATION_POLICY_ID.Value;
            }
        }
        public override ImplicitActivationPolicyValue Value { get; }

        public ImplicitActivationPolicy(ImplicitActivationPolicyValue value)
        {
            Value = value;
        }

        public override IPolicy Copy()
        {
            return new ImplicitActivationPolicy(Value);
        }

        public override void Destroy()
        {
        }
    }
}
