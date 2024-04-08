// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;

namespace DotNetOrb.Core.POA.Policies
{
    public class IdAssignmentPolicy : _IdAssignmentPolicyLocalBase
    {
        public override uint PolicyType
        {
            get
            {
                return ID_ASSIGNMENT_POLICY_ID.Value;
            }
        }
        public override IdAssignmentPolicyValue Value { get; }

        public IdAssignmentPolicy(IdAssignmentPolicyValue value)
        {
            Value = value;
        }

        public override IPolicy Copy()
        {
            return new IdAssignmentPolicy(Value);
        }

        public override void Destroy()
        {
        }
    }
}
