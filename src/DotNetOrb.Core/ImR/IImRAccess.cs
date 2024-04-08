// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.ETF;
using ETF;
using System.Collections.Generic;

namespace DotNetOrb.Core.ImR
{
    public interface IImRAccess
    {
        public string GetImRHost();
        public int GetImRPort();
        public ProtocolAddressBase GetImRAddress();
        public string GetImRCorbaloc();
        public List<IProfile> GetImRProfiles();
        public void RegisterPOA(string name, string server, ProtocolAddressBase address);
        public void RegisterPOA(string name, string server, string host, int port);
        public void RegisterPOA(ORB orb, POA.POA poa, ProtocolAddressBase address, string implname);
        public void SetServerDown(string name);
        public void SetServerDown(ORB orb, POA.POA poa, string implname);
    }
}
