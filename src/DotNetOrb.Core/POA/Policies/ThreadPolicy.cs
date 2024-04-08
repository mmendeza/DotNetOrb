// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;

namespace DotNetOrb.Core.POA.Policies
{
    public class ThreadPolicy : _ThreadPolicyLocalBase
    {
        public override uint PolicyType
        {
            get
            {
                return THREAD_POLICY_ID.Value;
            }
        }
        public override ThreadPolicyValue Value { get; }

        public ThreadPolicy(ThreadPolicyValue value)
        {
            Value = value;
        }

        public override IPolicy Copy()
        {
            return new ThreadPolicy(Value);
        }

        public override void Destroy()
        {
        }
    }
}
