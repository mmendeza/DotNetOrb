// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.ImR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrb.ImplementationRepository
{
    /// <summary>
    /// This class represents a host. It contains information about a server startup daemon residing on this host 
    /// and provides a method for starting a server on that host.
    /// </summary>
    public class ImRHostInfo
    {
        public string Host { get; set; }
        public string IorString { get; set; }

        private IServerStartupDaemon ssdRef;

        public ImRHostInfo(HostInfo host)
        {
            Host = host.Name;
            ssdRef = host.SsdRef;
            IorString = host.IorString;
        }

        public HostInfo ToHostInfo()
        {
            return new HostInfo(Host, null, IorString);
        }

        /// <summary>
        /// This method tries to start a server with the daemon for this host.
        /// </summary>
        /// <param name="command">the startup command of the server</param>
        /// <param name="orb"></param>
        public void StartServer(string command, ORB orb)
        {
            if (ssdRef == null)
            {
                ssdRef = ServerStartupDaemonHelper.Narrow(orb.StringToObject(IorString));
            }
            ssdRef.StartServer(command);
        }
    }
}
