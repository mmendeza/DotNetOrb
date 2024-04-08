// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;

namespace DotNetOrb.Core.POA.Policies
{
    public class IdUniquenessPolicy : _IdUniquenessPolicyLocalBase
    {
        public override uint PolicyType
        {
            get
            {
                return ID_UNIQUENESS_POLICY_ID.Value;
            }
        }
        public override IdUniquenessPolicyValue Value { get; }

        public IdUniquenessPolicy(IdUniquenessPolicyValue value)
        {
            Value = value;
        }

        public override IPolicy Copy()
        {
            return new IdUniquenessPolicy(Value);
        }

        public override void Destroy()
        {
        }
    }
}
