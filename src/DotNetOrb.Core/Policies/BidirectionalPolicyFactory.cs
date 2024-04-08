// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BiDirPolicy;
using CORBA;
using PortableInterceptor;

namespace DotNetOrb.Core.Policies
{
    public class BidirectionalPolicyFactory : LocalObject, IPolicyFactory
    {
        private string[] _ids = { "IDL:/PortableInterceptor/PolicyFactory:1.0" };

        public override string[] _Ids()
        {
            return _ids;
        }

        public IPolicy CreatePolicy(uint type, CORBA.Any value)
        {
            if (type != BIDIRECTIONAL_POLICY_TYPE.Value)
            {
                throw new PolicyError();
            }
            return new BidirectionalPolicy(BidirectionalPolicyValueHelper.Extract(value));
        }
    }
}
