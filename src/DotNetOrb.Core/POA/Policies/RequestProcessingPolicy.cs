// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;

namespace DotNetOrb.Core.POA.Policies
{
    public class RequestProcessingPolicy : _RequestProcessingPolicyLocalBase
    {
        public override uint PolicyType
        {
            get
            {
                return REQUEST_PROCESSING_POLICY_ID.Value;
            }
        }
        public override RequestProcessingPolicyValue Value { get; }

        public RequestProcessingPolicy(RequestProcessingPolicyValue value)
        {
            Value = value;
        }

        public override IPolicy Copy()
        {
            return new RequestProcessingPolicy(Value);
        }

        public override void Destroy()
        {
        }
    }
}
