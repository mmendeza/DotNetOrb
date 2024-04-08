// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.DSI;
using DotNetOrb.Core.POA.Exceptions;
using DotNetOrb.Core.POA.Util;
using DotNetOrb.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DotNetOrb.Core.POA
{
    public class RequestQueue : IConfigurable
    {
        private IRequestQueueListener queueListener;
        private IConfiguration configuration;
        private ILogger logger;
        private int queueMin;
        private int queueMax;
        private bool queueWait;

        private bool configured;

        private object syncObject = new object();

        private LinkedList<ServerRequest> queue = new LinkedList<ServerRequest>();

        public void Configure(IConfiguration config)
        {
            if (configured)
            {
                return;
            }

            configuration = config;
            logger = config.GetLogger(GetType().FullName);
            queueMax = configuration.GetAsInteger("DotNetOrb.POA.QueueMax", 100);
            queueMin = configuration.GetAsInteger("DotNetOrb.POA.QueueMin", 10);
            queueWait = configuration.GetAsBoolean("DotNetOrb.POA.QueueWait", false);
            configured = true;

            var queueListeners = configuration.GetAsList("DotNetOrb.POA.QueueListeners");

            foreach (var listener in queueListeners)
            {
                try
                {
                    var rql = (IRequestQueueListener)ObjectUtil.GetInstance(listener);
                    if (rql != null)
                    {
                        AddRequestQueueListener(rql);
                    }
                }
                catch (Exception ex)
                {
                    throw new ConfigException("Could not instantiate queue listener: " + listener, ex);
                }
            }
        }

        internal void Add(ServerRequest request)
        {
            lock (syncObject)
            {
                CheckIsConfigured();
                if (queueMax > 0 && queue.Count >= queueMax)
                {
                    if (logger.IsWarnEnabled)
                    {
                        logger.Warn($"Request queue is full, consider increasing DotNetOrb.POA.QueueMax (currently: {queueMax})");
                    }

                    if (queueWait)
                    {
                        while (queue.Count > queueMin)
                        {
                            try
                            {
                                Monitor.Wait(queue);
                            }
                            catch (ThreadInterruptedException ex)
                            {
                                // ignore
                            }
                        }
                    }
                    else
                    {
                        throw new ResourceLimitReachedException();
                    }
                }
                queue.AddLast(request);

                if (logger.IsDebugEnabled)
                {
                    logger.Debug($"rid: {request.RequestId} opname: {request.Operation} is queued (queue size: {queue.Count})");
                }

                // notify a queue listener
                if (queueListener != null)
                {
                    queueListener.RequestAddedToQueue(request, queue.Count);
                }

            }
        }

        internal void AddRequestQueueListener(IRequestQueueListener listener)
        {
            lock (syncObject)
            {
                CheckIsConfigured();

                queueListener = (IRequestQueueListener)EventMulticaster.Add(queueListener, listener);
            }

        }

        internal StringPair[] DeliverContent()
        {
            lock (syncObject)
            {
                CheckIsConfigured();

                List<StringPair> result = new List<StringPair>(queue.Count);
                foreach (var sr in queue)
                {
                    result.Add(new StringPair(sr.RequestId.ToString(), sr.ObjectId?.ToString()));
                }
                return result.ToArray();
            }
        }

        ServerRequest GetElementAndRemove(int rid)
        {
            lock (syncObject)
            {
                CheckIsConfigured();

                if (queue.Count > 0)
                {
                    foreach (var sr in queue.ToArray())
                    {
                        if (sr.RequestId == rid)
                        {
                            queue.Remove(sr);
                            Monitor.PulseAll(syncObject);
                            // notify a queue listener
                            if (queueListener != null)
                            {
                                queueListener.RequestRemovedFromQueue(sr, queue.Count);
                            }
                            return sr;
                        }
                    }
                }
                return null;
            }
        }

        internal ServerRequest GetFirst()
        {
            lock (syncObject)
            {
                CheckIsConfigured();

                if (queue.Count > 0)
                {
                    return queue.First();
                }
                return null;
            }
        }
        internal bool IsEmpty()
        {
            CheckIsConfigured();

            return queue.Count == 0;
        }
        internal ServerRequest RemoveFirst()
        {
            lock (syncObject)
            {
                CheckIsConfigured();

                if (queue.Count > 0)
                {
                    ServerRequest result = queue.First();
                    queue.RemoveFirst();
                    Monitor.PulseAll(syncObject);
                    // notify a queue listener
                    if (queueListener != null)
                    {
                        queueListener.RequestRemovedFromQueue(result, queue.Count);
                    }
                    return result;
                }
                return null;
            }
        }

        internal ServerRequest RemoveLast()
        {
            lock (syncObject)
            {
                CheckIsConfigured();

                if (queue.Count > 0)
                {
                    ServerRequest result = queue.Last();
                    queue.RemoveLast();
                    Monitor.PulseAll(syncObject);
                    // notify a queue listener
                    if (queueListener != null)
                    {
                        queueListener.RequestRemovedFromQueue(result, queue.Count);
                    }
                    return result;
                }
                return null;
            }
        }
        internal void RemoveRequestQueueListener(IRequestQueueListener listener)
        {
            lock (syncObject)
            {
                CheckIsConfigured();
                queueListener = (IRequestQueueListener)EventMulticaster.Remove(queueListener, listener);
            }
        }

        internal int Size()
        {
            return queue.Count;
        }

        private void CheckIsConfigured()
        {
            if (!configured)
            {
                throw new BadInvOrder("RequestQueue is not configured yet.");
            }
        }
    }
}
