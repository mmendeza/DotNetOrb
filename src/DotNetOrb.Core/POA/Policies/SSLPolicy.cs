// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.SSL;

namespace DotNetOrb.Core.POA.Policies
{
    public class SSLPolicy : _SSLPolicyLocalBase
    {
        public override uint PolicyType
        {
            get
            {
                return SSL_POLICY_TYPE.Value;
            }
        }
        public override SSLPolicyValue Value { get; }

        public SSLPolicy(SSLPolicyValue value)
        {
            Value = value;
        }

        public override IPolicy Copy()
        {
            return new SSLPolicy(Value);
        }

        public override void Destroy()
        {
        }
    }
}
