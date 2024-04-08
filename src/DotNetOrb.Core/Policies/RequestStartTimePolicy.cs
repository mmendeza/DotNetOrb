// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;
using TimeBase;

namespace DotNetOrb.Core.Policies
{
    public class RequestStartTimePolicy : _RequestStartTimePolicyLocalBase
    {
        public override UtcT StartTime { get; }

        public override uint PolicyType
        {
            get
            {
                return REQUEST_START_TIME_POLICY_TYPE.Value;
            }
        }

        public RequestStartTimePolicy(CORBA.Any value) : base()
        {
            StartTime = UtcTHelper.Extract(value);
        }

        public RequestStartTimePolicy(UtcT endTime) : base()
        {
            StartTime = endTime;
        }

        public override IPolicy Copy()
        {
            return new RequestStartTimePolicy(StartTime);
        }

        public override void Destroy()
        {
        }
    }
}
