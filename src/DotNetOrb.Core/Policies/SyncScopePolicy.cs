// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Messaging;

namespace DotNetOrb.Core.Policies
{
    public class SyncScopePolicy : _SyncScopePolicyLocalBase
    {
        public override short Synchronization { get; }

        public override uint PolicyType
        {
            get
            {
                return SYNC_SCOPE_POLICY_TYPE.Value;
            }
        }

        public SyncScopePolicy(CORBA.Any value) : base()
        {
            Synchronization = value.ExtractShort();
        }

        public SyncScopePolicy(SyncScope scope) : base()
        {
            Synchronization = (short)scope;
        }

        public SyncScopePolicy(short scope) : base()
        {
            Synchronization = scope;
        }

        public override IPolicy Copy()
        {
            return new SyncScopePolicy(Synchronization);
        }

        public override void Destroy()
        {
        }
    }
}
