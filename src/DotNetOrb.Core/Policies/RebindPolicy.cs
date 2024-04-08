// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;

namespace DotNetOrb.Core.Policies
{
    public class RebindPolicy : _RebindPolicyLocalBase
    {
        public override short RebindMode { get; }

        public override uint PolicyType
        {
            get
            {
                return REBIND_POLICY_TYPE.Value;
            }
        }

        public RebindPolicy(CORBA.Any value) : base()
        {
            RebindMode = value.ExtractShort();
        }

        public RebindPolicy(short rebindMode) : base()
        {
            RebindMode = rebindMode;
        }

        public override IPolicy Copy()
        {
            return new RebindPolicy(RebindMode);
        }

        public override void Destroy()
        {
        }
    }
}
