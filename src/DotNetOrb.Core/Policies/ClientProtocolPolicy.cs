// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using IOP;
using RTCORBA;

namespace DotNetOrb.Core.Policies
{
    public class ClientProtocolPolicy : _ClientProtocolPolicyLocalBase
    {

        public override Protocol[] Protocols { get; }

        public override uint PolicyType
        {
            get
            {
                return CLIENT_PROTOCOL_POLICY_TYPE.Value;
            }
        }

        public uint Tag
        {
            get
            {
                if (Protocols[0] != null)
                {
                    return Protocols[0].ProtocolType;
                }
                return TAG_INTERNET_IOP.Value;
            }
        }

        public ClientProtocolPolicy(Protocol[] protos) : base()
        {
            Protocols = protos;
        }

        public ClientProtocolPolicy(CORBA.Any any) : this(ProtocolListHelper.Extract(any))
        {
        }

        public override IPolicy Copy()
        {
            return new ClientProtocolPolicy(Protocols);
        }

        public override void Destroy()
        {
        }
    }
}
