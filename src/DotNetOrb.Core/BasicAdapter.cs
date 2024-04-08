// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using CSI;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.DSI;
using DotNetOrb.Core.ETF;
using DotNetOrb.Core.GIOP;
using DotNetOrb.Core.POA.Exceptions;
using ETF;
using System;
using System.Collections.Generic;

namespace DotNetOrb.Core
{
    public class BasicAdapter : IConfigurable
    {
        private List<Listener> listeners = new List<Listener>();

        private ServerLocalConnection serverLocalConnection;

        private TransportManager transportManager;

        private IConfiguration configuration;

        private ILogger logger = null;

        private ORB orb;

        internal BasicAdapter(ORB orb, TransportManager transportManager)
        {
            this.orb = orb;
            this.transportManager = transportManager;
            serverLocalConnection = new ServerLocalConnection(orb);
        }

        public void Configure(IConfiguration config)
        {
            configuration = config;
            logger = config.GetLogger(GetType());
            // create all Listeners
            var factories = GetListenerFactories();
            foreach (var factory in factories)
            {
                var protocol = ListenEndpoint.MapProfileTag(factory.ProfileTag);
                var listenEndpoints = transportManager.GetListenEndpoints(protocol);
                foreach (var listenEndpoint in listenEndpoints)
                {
                    var listener = factory.CreateListener(listenEndpoint);
                    //listener.Configure(config);
                    listeners.Add(listener);
                }
            }
            foreach (var listener in listeners)
            {
                listener.Listen();
            }
            serverLocalConnection.Connect();
        }

        private List<IConnectionFactory> GetListenerFactories()
        {
            var result = new List<IConnectionFactory>();
            List<string> tags = configuration.GetAsList("DotNetOrb.Transport.Server.Listeners");

            if (tags.Count == 0)
            {
                result.AddRange(transportManager.GetFactoriesList());
            }
            else
            {
                if (tags.Contains("off"))
                {
                    tags.Remove("off");
                }
                foreach (var tagStr in tags)
                {
                    uint tag = 0;
                    try
                    {
                        tag = uint.Parse(tagStr);
                    }
                    catch (FormatException ex)
                    {
                        throw new ArgumentException("could not parse profile tag for listener: " + tagStr + " (should have been a number)");
                    }
                    var factory = transportManager.GetFactory(tag);
                    if (factory == null)
                    {
                        throw new ArgumentException("could not find Factories for profile tag: " + tag);
                    }
                    result.Add(factory);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a List of endpoint profiles for all transports that listen for incoming connections.
        /// Each individual profile is a copy and can safely be modified by the caller 
        /// (e.g.add an object key, patch the address, stuff it into an IOR, etc.).
        /// </summary>        
        public List<IProfile> GetEndpointProfiles()
        {
            var result = new List<IProfile>();
            foreach (var listener in listeners)
            {
                result.Add(listener.Profile.Copy());
            }
            return result;
        }

        public void DeliverRequest(ServerRequest request, POA.POA poa)
        {
            var tmpPoa = poa;
            var scopes = request.RemainingPOAName;
            try
            {
                for (int i = 0; i < scopes.Length; i++)
                {
                    if (scopes[i].Equals(""))
                    {
                        break;
                    }
                    try
                    {
                        tmpPoa = tmpPoa.GetChildPOA(scopes[i]);
                    }
                    catch (ParentIsHoldingException p)
                    {
                        /*
                         * if one of the POAs is in holding state, we simply deliver the request to this POA.
                         * It will forward the request to its child POAs if necessary when changing back to active
                         * For the POA to be able to forward this request to its child POAa, we need to supply the
                         * remaining part of the child's POA name
                         */
                        var restOfName = new string[scopes.Length - i];
                        for (int j = 0; j < restOfName.Length; j++)
                        {
                            restOfName[j] = scopes[j + i];
                        }
                        request.RemainingPOAName = restOfName;
                        break;
                    }
                }

                if (tmpPoa == null)
                {
                    throw new CORBA.Internal("Request POA null!");
                }

                /* hand over to the POA */
                tmpPoa.Invoke(request);

            }
            catch (PortableServer.POA.WrongAdapter e)
            {
                // unknown oid (not previously generated)
                request.SystemException = new ObjectNotExist("unknown oid", (int)(OMGVMCID.Value | 2), CompletionStatus.No);
                request.Reply();
            }
            catch (CORBA.SystemException e)
            {
                request.SystemException = e;
                request.Reply();
            }
            catch (RuntimeException e)
            {
                request.SystemException = new Unknown(e.ToString());
                request.Reply();
                logger.Warn("unexpected exception", e);
            }
            catch (Exception e)
            {
                request.SystemException = new Unknown(e.ToString());
                request.Reply();
                logger.Error("unexpected exception", e);
            }
        }

        public void ReturnResult(ServerRequest request)
        {
            request.Reply();
        }

        public void StopListeners()
        {
            foreach (var listener in listeners)
            {
                listener.Destroy();
            }
            serverLocalConnection.Close();
        }


    }
}
