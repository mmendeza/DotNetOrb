// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BiDirPolicy;
using CORBA;

namespace DotNetOrb.Core.Policies
{
    public class BidirectionalPolicy : LocalObject, IBidirectionalPolicy
    {
        private string[] _ids = { "IDL:omg.org/BiDirPolicy/BidirectionalPolicy:1.0", "IDL:/CORBA/Policy:1.0" };

        public override string[] _Ids()
        {
            return _ids;
        }

        public uint PolicyType
        {
            get
            {
                return BIDIRECTIONAL_POLICY_TYPE.Value;
            }
            set
            {

            }
        }
        public ushort Value { get; set; }

        public BidirectionalPolicy(ushort value) : base()
        {
            Value = value;
        }

        public IPolicy Copy()
        {
            return new BidirectionalPolicy(Value);
        }

        public void Destroy()
        {
        }
    }
}
