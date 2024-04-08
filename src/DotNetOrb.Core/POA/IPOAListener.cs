// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using static DotNetOrb.Core.POA.POAConstants;

namespace DotNetOrb.Core.POA
{
    public interface IPOAListener : IEventListener
    {
        void PoaCreated(POA poa);
        void PoaStateChanged(POA poa, POAState state);
        void ReferenceCreated(CORBA.Object obj);
    }
}
