// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DotNetOrb.Core.Util;
using IOP;
using Messaging;
using System.Collections.Generic;
using TimeBase;

namespace DotNetOrb.Core.GIOP
{
    public class InvocationPolicies
    {
        public UtcT RequestStartTime { get; set; }
        public UtcT RequestEndTime { get; set; }
        public UtcT ReplyStartTime { get; set; }
        public UtcT ReplyEndTime { get; set; }
        public ulong RelativeRoundTripTimeout { get; set; }
        public ulong RelativeRequestTimeout { get; set; }

        public InvocationPolicies(ICollection<IPolicy> policies)
        {
            foreach (var policy in policies)
            {
                switch (policy.PolicyType)
                {
                    case REQUEST_START_TIME_POLICY_TYPE.Value:
                        {
                            RequestStartTime = ((IRequestStartTimePolicy)policy).StartTime;
                        }
                        break;
                    case REQUEST_END_TIME_POLICY_TYPE.Value:
                        {
                            RequestEndTime = ((IRequestEndTimePolicy)policy).EndTime;
                        }
                        break;
                    case REPLY_START_TIME_POLICY_TYPE.Value:
                        {
                            ReplyStartTime = ((IReplyStartTimePolicy)policy).StartTime;
                        }
                        break;
                    case REPLY_END_TIME_POLICY_TYPE.Value:
                        {
                            ReplyEndTime = ((IReplyEndTimePolicy)policy).EndTime;
                        }
                        break;
                    case RELATIVE_RT_TIMEOUT_POLICY_TYPE.Value:
                        {
                            RelativeRoundTripTimeout = ((IRelativeRoundtripTimeoutPolicy)policy).RelativeExpiry;
                        }
                        break;
                    case RELATIVE_REQ_TIMEOUT_POLICY_TYPE.Value:
                        {
                            RelativeRequestTimeout = ((IRelativeRequestTimeoutPolicy)policy).RelativeExpiry;
                        }
                        break;
                }
            }
            if (RelativeRequestTimeout > 0)
            {
                RequestEndTime = Time.Earliest(Time.CorbaFuture(RelativeRequestTimeout), RequestEndTime);
            }
            if (RelativeRoundTripTimeout > 0)
            {
                ReplyEndTime = Time.Earliest(Time.CorbaFuture(RelativeRoundTripTimeout), ReplyEndTime);
            }
        }

        public InvocationPolicies(ICollection<ServiceContext> serviceContexts)
        {
            foreach (var serviceContext in serviceContexts)
            {
                if (serviceContext.ContextId == INVOCATION_POLICIES.Value)
                {
                    var input = new CDRInputStream(serviceContext.ContextData);
                    try
                    {
                        input.OpenEncapsulatedArray();
                        var policies = PolicyValueSeqHelper.Read(input);
                        foreach (var policy in policies)
                        {
                            switch (policy.Ptype)
                            {
                                case REQUEST_START_TIME_POLICY_TYPE.Value:
                                    {
                                        RequestStartTime = Time.FromCDR(policy.Pvalue);
                                    }
                                    break;
                                case REQUEST_END_TIME_POLICY_TYPE.Value:
                                    {
                                        RequestEndTime = Time.FromCDR(policy.Pvalue);
                                    }
                                    break;
                                case REPLY_START_TIME_POLICY_TYPE.Value:
                                    {
                                        ReplyStartTime = Time.FromCDR(policy.Pvalue);
                                    }
                                    break;
                                case REPLY_END_TIME_POLICY_TYPE.Value:
                                    {
                                        ReplyEndTime = Time.FromCDR(policy.Pvalue);
                                    }
                                    break;
                                case RELATIVE_RT_TIMEOUT_POLICY_TYPE.Value:
                                    {
                                        using (var inputStream = new CDRInputStream(policy.Pvalue))
                                        {
                                            inputStream.OpenEncapsulatedArray();
                                            RelativeRoundTripTimeout = inputStream.ReadULongLong();
                                        }
                                    }
                                    break;
                                case RELATIVE_REQ_TIMEOUT_POLICY_TYPE.Value:
                                    {
                                        using (var inputStream = new CDRInputStream(policy.Pvalue))
                                        {
                                            inputStream.OpenEncapsulatedArray();
                                            RelativeRequestTimeout = inputStream.ReadULongLong();
                                        }
                                    }
                                    break;
                            }
                        }
                        if (RelativeRequestTimeout > 0)
                        {
                            RequestEndTime = Time.Earliest(Time.CorbaFuture(RelativeRequestTimeout), RequestEndTime);
                        }
                        if (RelativeRoundTripTimeout > 0)
                        {
                            ReplyEndTime = Time.Earliest(Time.CorbaFuture(RelativeRoundTripTimeout), ReplyEndTime);
                        }
                    }
                    finally { input.Close(); }
                }
            }
        }
        public ServiceContext ToServiceContext()
        {
            List<PolicyValue> policies = new List<PolicyValue>();
            if (RequestStartTime != null)
            {
                policies.Add(new PolicyValue(REQUEST_START_TIME_POLICY_TYPE.Value, Time.ToCDR(RequestStartTime)));
            }
            if (RequestEndTime != null)
            {
                policies.Add(new PolicyValue(REQUEST_END_TIME_POLICY_TYPE.Value, Time.ToCDR(RequestEndTime)));
            }
            if (ReplyStartTime != null)
            {
                policies.Add(new PolicyValue(REPLY_START_TIME_POLICY_TYPE.Value, Time.ToCDR(ReplyStartTime)));
            }
            if (ReplyEndTime != null)
            {
                policies.Add(new PolicyValue(REPLY_END_TIME_POLICY_TYPE.Value, Time.ToCDR(ReplyEndTime)));
            }
            if (RelativeRequestTimeout > 0)
            {
                using (var outputStream = new CDROutputStream())
                {
                    outputStream.BeginEncapsulatedArray();
                    outputStream.WriteULongLong(RelativeRequestTimeout);
                    policies.Add(new PolicyValue(RELATIVE_REQ_TIMEOUT_POLICY_TYPE.Value, outputStream.GetBufferCopy()));
                }
            }
            if (RelativeRoundTripTimeout > 0)
            {
                using (var outputStream = new CDROutputStream())
                {
                    outputStream.BeginEncapsulatedArray();
                    outputStream.WriteULongLong(RelativeRoundTripTimeout);
                    policies.Add(new PolicyValue(RELATIVE_RT_TIMEOUT_POLICY_TYPE.Value, outputStream.GetBufferCopy()));
                }
            }
            using (var outputStream = new CDROutputStream())
            {
                outputStream.BeginEncapsulatedArray();
                PolicyValueSeqHelper.Write(outputStream, policies.ToArray());
                return new ServiceContext(INVOCATION_POLICIES.Value, outputStream.GetBufferCopy());
            }
        }
    }
}
