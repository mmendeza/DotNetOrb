// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;
using TimeBase;

namespace DotNetOrb.Core.Policies
{
    public class RequestEndTimePolicy : _RequestEndTimePolicyLocalBase
    {
        public override UtcT EndTime { get; }

        public override uint PolicyType
        {
            get
            {
                return REQUEST_END_TIME_POLICY_TYPE.Value;
            }
        }

        public RequestEndTimePolicy(CORBA.Any value) : base()
        {
            EndTime = UtcTHelper.Extract(value);
        }

        public RequestEndTimePolicy(UtcT endTime) : base()
        {
            EndTime = endTime;
        }

        public override IPolicy Copy()
        {
            return new RequestEndTimePolicy(EndTime);
        }

        public override void Destroy()
        {
        }
    }
}
