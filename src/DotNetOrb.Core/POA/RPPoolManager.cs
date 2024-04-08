// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.POA.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace DotNetOrb.Core.POA
{
    public abstract class RPPoolManager
    {
        private IRPPoolManagerListener pmListener;

        /// <summary>
        /// The current for (un)registering the invocation contexts
        /// </summary>
        private Current current;

        /// <summary>
        /// The set of currently available (inactive) request processors
        /// </summary>
        private LinkedList<RequestProcessor> pool;

        /// <summary>
        /// the set of currently active processors
        /// </summary>
        private HashSet<RequestProcessor> activeProcessors;

        /// <summary>
        /// The current number of used AND unused request processors in the pools (active/inactive)
        /// </summary>
        private int numberOfProcessors;
        private int numberOfWaiters;

        /// <summary>
        /// the maximum size of the pool. This is effectively itsburst size
        /// </summary>
        private int maxPoolSize;

        /// <summary>
        /// the minimum number of request processors. This is the permanent number of processors held in the pool.
        /// </summary>
        private int minPoolSize;

        /// <summary>
        /// a flag for delay the pool initialization
        /// </summary>
        private bool inUse = false;

        private IConfiguration configuration;

        private ILogger logger;

        /// <summary>
        /// Used to add a timeout to the time it will wait for a requestprocessor.
        /// </summary>
        private int poolThreadTimeout;

        private object syncObject = new object();

        protected RPPoolManager(Current current, int min, int max, int pt, ILogger logger, IConfiguration configuration)
        {
            this.current = current;
            maxPoolSize = max;
            minPoolSize = min;
            poolThreadTimeout = pt;
            this.logger = logger;
            this.configuration = configuration;

            numberOfProcessors = 0;
            numberOfWaiters = 0;
            pool = new LinkedList<RequestProcessor>();
            activeProcessors = new HashSet<RequestProcessor>();
        }

        private void Init()
        {
            if (inUse)
            {
                return;
            }

            for (int i = 0; i < minPoolSize; i++)
            {
                AddProcessor();
            }
            inUse = true;
        }

        private void AddProcessor()
        {
            var rp = new RequestProcessor(this);

            try
            {
                rp.Configure(configuration);
            }
            catch (ConfigException ex)
            {
                throw new RuntimeException(ex.ToString());
            }
            current.AddContext(rp.Thread, rp);
            rp.Thread.IsBackground = true;
            pool.AddFirst(rp);
            ++numberOfProcessors;
            rp.Thread.Start();
        }

        internal void AddRPPoolManagerListener(IRPPoolManagerListener listener)
        {
            lock (syncObject)
            {
                pmListener = (IRPPoolManagerListener)EventMulticaster.Add(pmListener, listener);
            }
        }

        /// <summary>
        /// Invoked by clients to indicate that they won't use this poolManager anymore.
        /// </summary>
        public abstract void Destroy();

        /// <summary>
        /// Shutdown this poolManager. clients should invoke <see cref="Destroy()"/> instead.
        /// </summary>        
        /// <exception cref="POAInternalException"></exception>
        internal void _Destroy()
        {
            if (!inUse)
            {
                return;
            }

            // wait until all active processors complete
            while (activeProcessors.Count > 0)
            {
                try
                {
                    Monitor.Wait(syncObject);
                }
                catch (ThreadInterruptedException ex)
                {
                    // ignore
                }
            }

            RequestProcessor[] rps = pool.ToArray();

            for (int i = 0; i < rps.Length; i++)
            {
                if (rps[i].IsActive)
                {
                    throw new POAInternalException("error: request processor is active (RequestProcessorPM.destroy)");
                }

                pool.Remove(rps[i]);
                --numberOfProcessors;
                current.RemoveContext(rps[i].Thread);
                rps[i].End();
            }

            inUse = false;
        }

        /// <summary>
        /// returns the number of unused processors contained in the pool
        /// </summary>       
        internal int GetPoolCount()
        {
            return pool.Count;
        }

        /**
         * returns the size of the processor pool (used and unused processors)
         */

        internal int GetPoolSize()
        {
            lock (syncObject)
            {
                return numberOfProcessors;
            }
        }

        /// <summary>
        /// Returns a processor from pool, the first call causes the initialization of the processor pool,
        /// if no processor is available the number of processors will increased until the maxPoolSize is reached,
        /// this method blocks if no processor is available and the maxPoolSize is reached until a processor is released
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CORBA.Timeout"></exception>
        internal RequestProcessor GetProcessor()
        {
            lock (syncObject)
            {
                Init();

                if (pool.Count == 0 && (numberOfProcessors < maxPoolSize || maxPoolSize < 1))
                {
                    AddProcessor();
                }

                int timeout = poolThreadTimeout;
                while (pool.Count == 0)
                {
                    WarnPoolIsEmpty();

                    var sw = Stopwatch.StartNew();
                    try
                    {
                        numberOfWaiters++;
                        Monitor.Wait(syncObject, timeout);
                    }
                    catch (ThreadInterruptedException e)
                    {
                    }
                    finally
                    {
                        numberOfWaiters--;
                        sw.Stop();
                    }
                    if (timeout > 0)
                    {
                        // Timeout configured, woken up and the timeout has expired but nothing to run
                        // it with.
                        if (sw.ElapsedMilliseconds >= timeout && pool.Count == 0)
                        {
                            // A timeout has been configured, we have finished waiting still no processors.
                            // Throw an exception
                            throw new CORBA.Timeout("No request processor available to handle request");
                        }
                        // If we have woken up before the timeout and the pool is still empty - have another
                        // go and go back to sleep.
                        else if (sw.ElapsedMilliseconds < timeout && pool.Count == 0)
                        {
                            // Need to reset timeout so we finish waiting.
                            timeout -= (int)sw.ElapsedMilliseconds;
                        }
                    }

                    // BZ946: There are some corner cases with request processor sizing.
                    //
                    // If min==max then the corner cases do not occur.
                    //
                    // In the default situation of e.g. min=5, max=20 then its
                    // possible that when we reach the maximum number of processors,
                    // use 1, and then go to release it, it will not get placed back
                    // in the pool (as 19 > 5). In fact all 15 of the 'burst'
                    // processors will be ended.
                    //
                    // If we are oscillating between min and max size then its possible that this thread
                    // may be looping on isEmpty. A RequestProcessor finishes and frees itself by calling
                    // releaseProcessor. As per above this this processor may just be released and not added
                    // to the pool. Therefore the pool is still isEmpty. However if another thread now tries
                    // to get a processor it will find the pool isEmpty and there is room to create a processor
                    // Therefore the pool size increases and this thread keeps looping. While it might eventually
                    // break out of the loop adding the below test breaks this pathological condition.
                    if (pool.Count == 0 && (numberOfProcessors < maxPoolSize || maxPoolSize < 1))
                    {
                        AddProcessor();
                    }
                }

                RequestProcessor requestProcessor = pool.First();
                pool.RemoveFirst();
                activeProcessors.Add(requestProcessor);

                // notify a pool manager listener
                if (pmListener != null)
                {
                    pmListener.ProcessorRemovedFromPool(requestProcessor, pool.Count, numberOfProcessors);
                }

                return requestProcessor;
            }

        }

        internal virtual void WarnPoolIsEmpty()
        {
            if (logger.IsWarnEnabled)
            {
                logger.Warn($"Thread pool exhausted, consider increasing DotNetOrb.POA.ThreadPoolMax (currently: {maxPoolSize})");
            }
        }


        /// <summary>
        /// Gives a processor back into the pool if the number of available processors is smaller than minPoolSize, 
        /// otherwise the processor will terminate
        /// </summary>
        /// <param name="rp"></param>
        internal void ReleaseProcessor(RequestProcessor rp)
        {
            lock (syncObject)
            {
                activeProcessors.Remove(rp);

                if (pool.Count < minPoolSize || pool.Count < numberOfWaiters)
                {
                    pool.AddFirst(rp);
                }
                else
                {
                    numberOfProcessors--;
                    current.RemoveContext(rp.Thread);
                    rp.End();
                }
                // notify a pool manager listener
                if (pmListener != null)
                {
                    pmListener.ProcessorAddedToPool(rp, pool.Count, numberOfProcessors);
                }

                // notify whoever is waiting for the release of active processors
                Monitor.PulseAll(syncObject);
            }
        }

        internal void RemoveRPPoolManagerListener(IRPPoolManagerListener listener)
        {
            lock (syncObject)
            {
                pmListener = (IRPPoolManagerListener)EventMulticaster.Remove(pmListener, listener);
            }

        }
    }
}
