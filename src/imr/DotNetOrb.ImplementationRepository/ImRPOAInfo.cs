// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.ImR;
using DotNetty.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DotNetOrb.ImR.Registration;

namespace DotNetOrb.ImplementationRepository
{
    public class ImRPOAInfo
    {
        public int Port { get; set; }
        public ImRServerInfo Server { get; set; }
        public string Host { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Timeout { get; set; }

        private object syncObject = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">the POAs name</param>
        /// <param name="host">the POAs host</param>
        /// <param name="port">the port the POA listens on.</param>
        /// <param name="server">the server the POA is associated with.</param>
        /// <param name="timeout">activation timeout</param>
        /// <exception cref="IllegalPOAName"></exception>
        public ImRPOAInfo(string name, string host, int port, ImRServerInfo server, int timeout)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new IllegalPOAName(name);
            }
            Name = name;
            Host = host;
            Port = port;
            Server = server;
            IsActive = true;
            Timeout = timeout;
        }

        public POAInfo ToPOAInfo()
        {
            return new POAInfo(Name, Host, (uint)Port, Server.Name, IsActive);
        }

        /// <summary>
        /// Reactivates this POA, i.e. sets it to active and unblocks any waiting threads.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Reactivate(string host, int port)
        {
            lock (syncObject)
            {
                Host = host;
                Port = port;
                IsActive = true;
                Server.IsActive = true;
                Server.IsRestarting = false;
                Monitor.PulseAll(syncObject);
            }

        }

        /// <summary>
        /// This method blocks until the POA is reactivated, or the timeout is exceeded.
        /// </summary>
        /// <returns>false, if the timeout has been exceeded, true otherwise.</returns>
        public bool AwaitActivation()
        {
            lock (syncObject)
            {
                while (!IsActive)
                {
                    try
                    {
                        var sw = Stopwatch.StartNew();
                        Monitor.Wait(syncObject, Timeout);
                        if (!IsActive && sw.ElapsedMilliseconds > Timeout)
                        {
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }

                return true;
            }
        }
    }
}
