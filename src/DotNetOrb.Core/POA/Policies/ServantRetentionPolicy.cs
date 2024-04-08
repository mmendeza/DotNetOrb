// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;

namespace DotNetOrb.Core.POA.Policies
{
    public class ServantRetentionPolicy : _ServantRetentionPolicyLocalBase
    {
        public override uint PolicyType
        {
            get
            {
                return SERVANT_RETENTION_POLICY_ID.Value;
            }
        }
        public override ServantRetentionPolicyValue Value { get; }

        public ServantRetentionPolicy(ServantRetentionPolicyValue value)
        {
            Value = value;
        }

        public override IPolicy Copy()
        {
            return new ServantRetentionPolicy(Value);
        }

        public override void Destroy()
        {
        }
    }
}
