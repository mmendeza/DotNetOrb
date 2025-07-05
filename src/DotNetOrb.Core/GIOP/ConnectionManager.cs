// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.IIOP;
using ETF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetOrb.Core.GIOP
{
    public class ConnectionManager : IConfigurable
    {
        private ORB orb;

        private Dictionary<IProfile, ClientConnection> clientConnections = new Dictionary<IProfile, ClientConnection>();

        private Dictionary<IProfile, GIOPChannelHandler> bidirChannels = new Dictionary<IProfile, GIOPChannelHandler>();

        private ClientLocalConnection localConnection;

        private TransportManager transportManager;

        private ILogger logger;

        private object syncObject = new object();

        public ConnectionManager(ORB orb, TransportManager transportManager)
        {
            this.orb = orb;
            this.transportManager = transportManager;
        }

        public void Configure(IConfiguration configuration)
        {
            logger = configuration.GetLogger(GetType());
        }

        public GIOPChannelHandler GetLocalChannelHandler()
        {
            if (localConnection == null)
            {
                localConnection = new ClientLocalConnection(orb);
            }
            return localConnection.ChannelHandler;
        }

        public GIOPChannelHandler GetChannelHandler(IProfile profile)
        {
            lock (syncObject)
            {
                if (bidirChannels.ContainsKey(profile))
                {
                    var bidirConnection = bidirChannels[profile];
                    return bidirConnection;
                }

                clientConnections.TryGetValue(profile, out ClientConnection clientConnection);

                if (clientConnection == null && profile is IIOPProfile iiopProfile)
                {
                    if (iiopProfile.GetSSL() != null)
                    {
                        IIOPProfile sslProfile = iiopProfile.ToNonSSL();

                        clientConnections.TryGetValue(sslProfile, out clientConnection);
                    }
                }

                // Don't return a closed connection.
                if (clientConnection != null && clientConnection.IsClosed)
                {
                    ReleaseConnection(clientConnection);
                    clientConnection = null;
                }

                if (clientConnection == null)
                {
                    uint tag = (uint)profile.Tag;
                    var factory = transportManager.GetFactory(tag);
                    if (factory == null)
                    {
                        throw new BadParam("No transport plugin for profile tag " + tag);
                    }
                    clientConnection = factory.CreateConnection(profile);

                    logger.Debug($"ClientConnectionManager: created new {clientConnection}");

                    clientConnections.Add(profile, clientConnection);
                }
                else
                {
                    logger.Debug($"ClientConnectionManager: found {clientConnection}");
                }

                clientConnection.IncClients();
                return clientConnection.ChannelHandler;
            }
        }

        /// <summary>
        /// Only used by Delegate for client-initiated connections.
        /// </summary>
        public void ReleaseConnection(ClientConnection connection)
        {
            lock (syncObject)
            {
                if (connection.DecClients() || connection.IsClosed)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("ClientConnectionManager: releasing " + (connection.IsClosed ? "closed connection " : "") + connection.ToString());
                    }
                    connection.Close();
                    clientConnections.Remove(connection.Profile);
                }
                else
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("ClientConnectionManager: cannot release "
                                      + connection.ToString()
                                      + " (still has " + connection.ClientCount + " client(s))");
                    }
                }
            }
        }

        public void AddBidirConnection(GIOPChannelHandler channelHandler, IProfile profile)
        {
            lock (syncObject)
            {
                if (!clientConnections.ContainsKey(profile))
                {
                    //this is a bit of a hack: the bidirectional client
                    //connections have to persist until their underlying GIOP
                    //connection is closed. Therefore, we set the initial
                    //client count to 1, so the connection will be kept even
                    //if there are currently no associated Delegates.
                    bidirChannels.Add(profile, channelHandler);
                    channelHandler.ConnectionClosed += BidirConnectionClosed;
                }
            }
        }

        private void BidirConnectionClosed(object? sender, EventArgs e)
        {
            lock (syncObject)
            {
                foreach (var item in bidirChannels.Where(kvp => kvp.Value == sender).ToList())
                {
                    bidirChannels.Remove(item.Key);
                }
            }
        }

        public void Shutdown()
        {
            lock (syncObject)
            {
                //release all open connections
                foreach (var connection in clientConnections.Values)
                {
                    connection.Close();
                }

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("ConnectionManager shut down (all connections released)");
                }

                clientConnections.Clear();
                bidirChannels.Clear();
            }
        }
    }
}
