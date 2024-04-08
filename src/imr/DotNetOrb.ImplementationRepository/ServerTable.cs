// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.ImR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static DotNetOrb.ImR.Admin;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DotNetOrb.ImplementationRepository
{
    /// <summary>
    /// This class represents the server table of the implementation repository.
    /// It contains all servers, POAs and hosts, and is serialized on shutdown, deserialized on startup.
    /// It provides methods for adding, deleting and listing servers, POAs and hosts.
    /// </summary>
    public class ServerTable
    {
        [JsonInclude]
        private Dictionary<string, ImRServerInfo> servers = new Dictionary<string, ImRServerInfo>();
        [JsonIgnore]
        private ResourceLock serversLock = new ResourceLock();
        [JsonInclude]
        private Dictionary<string, ImRPOAInfo> poas = new Dictionary<string, ImRPOAInfo>();
        [JsonIgnore]
        private ResourceLock poasLock = new ResourceLock();
        [JsonInclude]
        private Dictionary<string, ImRHostInfo> hosts = new Dictionary<string, ImRHostInfo>();
        [JsonIgnore]
        private ResourceLock hostsLock = new ResourceLock();
        [JsonIgnore]
        public ResourceLock tablelock = new ResourceLock();

        public bool HasServer(string name)
        {
            return servers.ContainsKey(name);
        }

        public ImRServerInfo GetServer(string name)
        {
            if (servers.ContainsKey(name))
            {
                return servers[name];
            }
            else
            {
                throw new UnknownServerName(name);
            }
        }

        public void AddServer(string name, ImRServerInfo server)
        {
            if (servers.ContainsKey(name))
            {
                throw new DuplicateServerName(name);
            }
            tablelock.GainSharedLock();
            serversLock.GainSharedLock();
            servers.Add(name, server);
            serversLock.ReleaseSharedLock();
            tablelock.ReleaseSharedLock();
        }

        public void RemoveServer(string name)
        {
            if (servers.ContainsKey(name))
            {
                tablelock.GainSharedLock();
                serversLock.GainSharedLock();
                servers.Remove(name);
                serversLock.ReleaseSharedLock();
                tablelock.ReleaseSharedLock();
            }
            else
            {
                throw new UnknownServerName(name);
            }
        }

        public bool IsPOAEndpointReused(string name, string host, int port)
        {
            POAInfo[] poas = GetPOAs();
            for (int i = 0; i < poas.Length; i++)
            {
                if (poas[i].Name.Equals(name))
                    continue;

                if (poas[i].Host.Equals(host) && poas[i].Port == port && poas[i].Active == true)
                    return true;
            }
            return false;
        }

        public ImRPOAInfo GetPOA(string name)
        {
            return poas[name];
        }

        public void AddPOA(string name, ImRPOAInfo poa)
        {
            tablelock.GainSharedLock();
            poasLock.GainSharedLock();
            poas.Add(name, poa);
            poasLock.ReleaseSharedLock();
            tablelock.ReleaseSharedLock();
        }

        public void RemovePOA(string name)
        {
            tablelock.GainSharedLock();
            poasLock.GainSharedLock();
            poas.Remove(name);
            poasLock.ReleaseSharedLock();
            tablelock.ReleaseSharedLock();
        }

        public ServerInfo[] GetServers()
        {
            tablelock.GainSharedLock();
            serversLock.GainExclusiveLock();
            //build array
            ServerInfo[] _servers = new ServerInfo[servers.Count];
            int i = 0;
            foreach (var server in servers.Values)
            {
                _servers[i++] = server.ToServerInfo();
            }
            serversLock.ReleaseExclusiveLock();
            tablelock.ReleaseSharedLock();
            return _servers;
        }

        public HostInfo[] GetHosts()
        {
            tablelock.GainSharedLock();
            hostsLock.GainExclusiveLock();
            //build array
            HostInfo[] _hosts = new HostInfo[hosts.Count];
            int i = 0;
            foreach (var host in hosts.Values)
            {
                _hosts[i++] = host.ToHostInfo();
            }
            hostsLock.ReleaseExclusiveLock();
            tablelock.ReleaseSharedLock();
            return _hosts;
        }

        public POAInfo[] GetPOAs()
        {
            tablelock.GainSharedLock();
            poasLock.GainExclusiveLock();
            //build array
            POAInfo[] _poas = new POAInfo[poas.Count];
            int i = 0;
            foreach (var poa in poas.Values)
            {
                _poas[i++] = poa.ToPOAInfo();
            }
            poasLock.ReleaseExclusiveLock();
            tablelock.ReleaseSharedLock();
            return _poas;
        }

        public void AddHost(string name, ImRHostInfo host)
        {
            tablelock.GainSharedLock();
            hostsLock.GainSharedLock();
            hosts.Add(name, host);
            hostsLock.ReleaseSharedLock();
            tablelock.ReleaseSharedLock();
        }

        public ImRHostInfo RemoveHost(string name)
        {
            hosts.Remove(name, out ImRHostInfo ret);
            return ret;
        }

        public ImRHostInfo GetHost(string name)
        {
            return hosts[name];
        }
    }
}
