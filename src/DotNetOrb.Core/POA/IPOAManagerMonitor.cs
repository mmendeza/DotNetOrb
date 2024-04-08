// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;

namespace DotNetOrb.Core.POA
{
    public interface IPOAManagerMonitor : IConfigurable
    {
        void AddPOA(string name);
        void CloseMonitor();
        void Init(POAManager poaManager);
        void OpenMonitor();
        void PrintMessage(string str);
        void RemovePOA(string name);
        void SetToActive();
        void SetToDiscarding(bool wait);
        void SetToHolding(bool wait);
        void SetToInactive(bool wait, bool etherialize);
    }
}
