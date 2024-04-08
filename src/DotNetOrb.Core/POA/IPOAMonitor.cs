// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;

namespace DotNetOrb.Core.POA
{
    public interface IPOAMonitor : IConfigurable
    {
        void ChangeState(string state);

        void CloseMonitor();

        void Init(POA poa, AOM aom, RequestQueue queue, RPPoolManager pm, string prefix);

        void OpenMonitor();
    }
}
