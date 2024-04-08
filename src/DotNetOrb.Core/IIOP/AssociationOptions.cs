// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.Core.IIOP
{
    [Flags]
    public enum AssociationOptions : ushort
    {
        NoProtection = 1,
        Integrity = 2,
        Confidentiality = 4,
        DetectReplay = 8,
        DetectMisordering = 16,
        EstablishTrustInTarget = 32,
        EstablishTrustInClient = 64,
        NoDelegation = 128,
        SimpleDelegation = 256,
        CompositeDelegation = 512,
        IdentityAssertion = 1024,
        DelegationByClient = 2048,
    }
}
