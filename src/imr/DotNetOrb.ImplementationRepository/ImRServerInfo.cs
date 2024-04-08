// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.ImR;
using DotNetty.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static DotNetOrb.ImR.Admin;

namespace DotNetOrb.ImplementationRepository
{
    /// <summary>
    /// This class contains the information about a logical server.  It has methods for managing the associated POAs,
    /// holding and releasing the server, and, for the "client side", a method that blocks until the server is released.
    /// </summary>
    public class ImRServerInfo
    {
        public string Command { get; set; }
        public bool IsHolding { get; set; }
        public string Host { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsRestarting { get; set; }

        [JsonInclude]
        private List<ImRPOAInfo> poas = new List<ImRPOAInfo>();
        private ResourceLock poasLock = null;
        private object syncObject = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">logical server name</param>
        /// <param name="host">name of the host on which the server should be restarted 
        /// (ignored when no startup command is specified).</param>
        /// <param name="command">startup command for this server, passed to the server startup daemon 
        /// on host (in case there is one active).</param>
        /// <exception cref="IllegalServerName"></exception>
        public ImRServerInfo(string name, string host, string command)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new IllegalServerName(name);
            }
            Name = name;
            Host = host;
            Command = command;
            IsHolding = false;
            IsRestarting = false;
            IsActive = false;
            poasLock = new ResourceLock();
        }

        public ServerInfo ToServerInfo()
        {
            poasLock.GainExclusiveLock();
            // build array
            POAInfo[] _info;
            lock (poas)
            {
                ImRPOAInfo[] _poas = poas.ToArray();
                _info = new POAInfo[_poas.Length];
                for (int i = 0; i < _info.Length; i++)
                {
                    _info[i] = _poas[i].ToPOAInfo();
                }
            }
            poasLock.ReleaseExclusiveLock();
            return new ServerInfo(Name, Command, _info, Host, IsActive, IsHolding);
        }

        /// <summary>
        /// Adds a POA to this server
        /// </summary>
        /// <param name="poa"></param>
        public void AddPOA(ImRPOAInfo poa)
        {
            if (!IsActive)
            {
                IsActive = true;
            }

            poasLock.GainSharedLock();
            lock (poas)
            {
                poas.Add(poa);
            }
            poasLock.ReleaseSharedLock();
        }

        /// <summary>
        /// Builds an array of of the names of the POAs associated with this server.
        /// This method is needed for deleting a server since its POAs have to be as well
        /// removed from the central storage.
        /// </summary>
        /// <returns>An array of POA names</returns>
        internal string[] GetPOANames()
        {
            // build array
            string[] names;
            lock (poas)
            {
                ImRPOAInfo[] _poas = poas.ToArray();

                names = new string[_poas.Length];
                for (int i = 0; i < names.Length; i++)
                {
                    names[i] = _poas[i].Name;
                }
            }
            return names;
        }

        /// <summary>
        ///  Sets the server down, i.e. not active. If a request for a POA of this server is received, 
        ///  the repository tries to restart the server. The server is automatically set back to active 
        ///  when the first of its POAs gets reregistered.
        /// </summary>
        public void SetDown()
        {
            lock (poas)
            {
                // sets all associated to not active.
                foreach (var poa in poas)
                {
                    poa.IsActive = false;
                }
            }
            IsActive = false;
            IsRestarting = false;
        }

        /// <summary>
        /// This method blocks until the server is released, i.e. set to not holding. 
        /// This will not time out since holding a server is only done by administrators.
        /// </summary>
        public void AwaitRelease()
        {
            lock (syncObject)
            {
                while (IsHolding)
                {
                    try
                    {
                        Monitor.Wait(syncObject);
                    }
                    catch (ThreadInterruptedException)
                    {
                        // ignored
                    }
                }
            }
        }

        /// <summary>
        /// Release the server and unblock all waiting threads.
        /// </summary>
        public void Release()
        {
            lock (syncObject)
            {
                IsHolding = false;
                Monitor.PulseAll(syncObject);
            }
        }

        /// <summary>
        /// Tests if this server should be restarted. That is the case if the server is not active and 
        /// nobody else is currently trying to restart it.  If true is returned the server is set to restarting.
        /// That means the thread calling this method has to restart the server, otherwise it will stay down indefinetly.
        /// </summary>
        /// <returns>true, if the server should be restarted by the calling thread./returns>
        public bool ShouldBeRestarted()
        {
            lock (syncObject)
            {
                bool restart = !(IsActive || IsRestarting);
                if (restart)
                {
                    IsRestarting = true;
                }
                return restart;
            }
        }

    }
}
