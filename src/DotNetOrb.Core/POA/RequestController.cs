// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.DSI;
using DotNetOrb.Core.POA.Exceptions;
using DotNetOrb.Core.POA.Util;
using DotNetOrb.Core.Util;
using PortableServer;
using System.Collections.Generic;
using System.Threading;
using static PortableServer.POAManager;

namespace DotNetOrb.Core.POA
{
    /// <summary>
    /// This class manages all request processing affairs. The main thread takes the requests out from the queue 
    /// and will see that the necessary steps are taken.
    /// </summary>
    public class RequestController : IConfigurable
    {
        private Thread thread;

        private object syncObject = new object();

        private POA poa;
        internal POA POA { get { return poa; } }

        private ORB orb;
        internal ORB ORB { get { return orb; } }

        private RequestQueue requestQueue = new RequestQueue();
        internal RequestQueue RequestQueue { get { return requestQueue; } }

        private AOM aom;
        internal AOM AOM { get { return aom; } }

        private RPPoolManager poolManager;
        internal RPPoolManager PoolManager { get { return poolManager; } }

        private int localRequests = 0;

        private static int count = 0;

        /** the configuration object for this controller */
        private IConfiguration configuration = null;

        /** this controller's logger instance */
        private ILogger logger;

        // stores all active requests
        private HashSet<ByteArrayKey> activeRequestTable;
        // RequestProcessor -> oid
        // for synchronisation with the object deactiviation process
        private HashSet<ByteArrayKey> deactivationList = new HashSet<ByteArrayKey>();
        // oid's

        // other synchronisation stuff
        private bool terminate;
        private bool waitForCompletionCalled;
        private bool waitForShutdownCalled;
        private object queueLog = new object();
        ThreadPriority threadPriority = ThreadPriority.Highest;

        public RequestController(POA poa, ORB orb, AOM aom, RPPoolManager poolManager)
        {
            this.poa = poa;
            this.aom = aom;
            this.poolManager = poolManager;
            this.orb = orb;
            thread = new Thread(new ThreadStart(Run));
            thread.Name = "RequestController-" + (++count);
        }

        public void Configure(IConfiguration config)
        {
            configuration = config;

            logger = configuration.GetLogger(GetType().FullName);

            int threadPoolMax = config.GetAsInteger("DotNetOrb.POA.ThreadPoolMax", 20);

            activeRequestTable = poa.IsSingleThreadModel ? new HashSet<ByteArrayKey>(1) : new HashSet<ByteArrayKey>(threadPoolMax);

            requestQueue.Configure(config);

            int threadPriority = configuration.GetAsInteger("DotNetOrb.POA.ThreadPriority", (int)ThreadPriority.Highest);

            if (threadPriority < (int)ThreadPriority.Lowest)
            {
                this.threadPriority = ThreadPriority.Lowest;
            }
            else if (threadPriority > (int)ThreadPriority.Highest)
            {
                this.threadPriority = ThreadPriority.Highest;
            }
            else
            {
                this.threadPriority = (ThreadPriority)threadPriority;
            }

            thread.Priority = this.threadPriority;
            thread.IsBackground = true;
            thread.Start();
        }

        internal ILogger GetLogger()
        {
            return logger;
        }

        internal void ClearPool()
        {
            poolManager.Destroy();
        }

        /// <summary>
        /// Rejects all queued requests with specified system exception
        /// </summary>
        /// <param name="exception"></param>
        internal void ClearQueue(CORBA.SystemException exception)
        {
            ServerRequest request;
            while ((request = requestQueue.RemoveLast()) != null)
            {
                RejectRequest(request, exception);
            }
        }

        /// <summary>
        /// Indicates that the assumptions for blocking the request controller thread have changed,
        /// a waiting request controller thread will notified
        /// </summary>
        internal void ContinueToWork()
        {
            lock (queueLog)
            {
                Monitor.PulseAll(queueLog);
            }
        }

        internal void End()
        {
            lock (syncObject)
            {
                terminate = true;
                ContinueToWork();
            }
        }

        /// <summary>
        /// Frees an object from the deactivation in progress state, 
        /// a call indicates that the object deactivation process is complete
        /// </summary>
        /// <param name="oid"></param>
        internal void FreeObject(ByteArrayKey oid)
        {
            lock (syncObject)
            {
                deactivationList.Remove(oid);
            }
        }

        internal bool IsDeactivating(ByteArrayKey oid)
        {
            lock (syncObject)
            {
                return deactivationList.Contains(oid);
            }
        }

        /// <summary>
        /// Requests will dispatched to request processors, attention, if the processor pool is empty, 
        /// this method returns only if the GetProcessor() method from RequestProcessorPool can be satisfied
        /// </summary>
        /// <param name="request"></param>
        private void ProcessRequest(ServerRequest request)
        {
            Servant servant = null;
            IServantManager servantManager = null;
            bool invalid = false;
            ByteArrayKey oid = request.ObjectIdAsByteArrayKey;

            lock (syncObject)
            {
                if (waitForCompletionCalled)
                {
                    //state has changed to holding, discarding or inactive
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info("rid: " + request.RequestId + " opname: " + request.Operation + " cannot process request because waitForCompletion was called");
                    }
                    throw new CompletionRequestedException();
                }

                if (waitForShutdownCalled)
                {
                    //poa goes down
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info("rid: " + request.RequestId + " opname: " + request.Operation + " cannot process request because POA shutdown in progress");
                    }
                    throw new ShutdownInProgressException();
                }

                /* below this point it's save that the poa is active */

                if (aom != null && aom.IsDeactivating(oid) || deactivationList.Contains(oid))
                {
                    if (!poa.IsUseServantManager && !poa.IsUseDefaultServant)
                    {
                        if (logger.IsInfoEnabled)
                        {
                            logger.Info("rid: " + request.RequestId + " opname: " + request.Operation + " objectKey: " + CorbaLoc.ParseKey(request.ObjectKey) + " cannot process request, because object is already in the deactivation process");
                        }

                        throw new ObjectNotExist();
                    }
                    invalid = true;
                }

                //below this point it's save  that the object is not in a deactivation process
                if (!invalid && poa.IsRetain)
                {
                    servant = aom.GetServant(oid);
                }

                if (servant == null)
                {
                    if (poa.IsUseDefaultServant)
                    {
                        if ((servant = poa.DefaultServant) == null)
                        {
                            if (logger.IsWarnEnabled)
                            {
                                logger.Warn("rid: " + request.RequestId + " opname: " + request.Operation + " cannot process request because default servant is not set");
                            }
                            throw new ObjAdapter();
                        }

                    }
                    else if (poa.IsUseServantManager)
                    {
                        if ((servantManager = poa.ServantManager) == null)
                        {
                            if (logger.IsWarnEnabled)
                            {
                                logger.Warn("rid: " + request.RequestId + " opname: " + request.Operation + " cannot process request because servant manager is not set");
                            }
                            throw new ObjAdapter();
                        }
                        // USE_OBJECT_MAP_ONLY is in effect but object not exists
                    }
                    else
                    {
                        if (logger.IsWarnEnabled)
                        {
                            logger.Warn("rid: " + request.RequestId + " opname: " + request.Operation + " connection: " + request.ChannelHandler.ToString() + " objectKey: " + CorbaLoc.ParseKey(request.ObjectKey) + " cannot process request, because object doesn't exist");
                        }
                        throw new ObjectNotExist();
                    }
                }
                //below this point it's save that the request is valid (all preconditions can be met)
                activeRequestTable.Add(oid);
            }

            // get and initialize a processor for request processing
            if (logger.IsDebugEnabled)
            {
                logger.Debug("rid: " + request.RequestId + " opname: " + request.Operation + " trying to get a RequestProcessor");
            }

            RequestProcessor processor = poolManager.GetProcessor();
            processor.Init(this, request, servant, servantManager);
            processor.Begin();
        }

        internal void QueueRequest(ServerRequest request)
        {
            requestQueue.Add(request);

            if (requestQueue.Size() == 1)
            {
                ContinueToWork();
            }
        }

        /// <summary>
        /// Calls the basic adapter and hands out the request 
        /// if something went wrong, the specified system exception will be set
        /// </summary>
        /// <param name="request"></param>
        /// <param name="exception"></param>
        internal void RejectRequest(ServerRequest request, CORBA.SystemException exception)
        {
            if (exception != null)
            {
                request.SystemException = exception;
            }

            orb.GetBasicAdapter().ReturnResult(request);

            if (logger.IsWarnEnabled)
            {
                logger.Warn("rid: " + request.RequestId + " opname: " + request.Operation + " request rejected with exception: " + exception.Message);
            }
        }

        /// <summary>
        ///  Resets a previous waitForCompletion call, everybody who is waiting will be notified
        /// </summary>
        internal void ResetPreviousCompletionCall()
        {
            lock (syncObject)
            {
                logger.Debug("reset a previous completion call");

                waitForCompletionCalled = false;
                /* maybe somebody waits for completion */
                Monitor.PulseAll(syncObject);
            }
        }

        /// <summary>
        /// Sends the reply of the given request.
        /// </summary>
        /// <param name="request"></param>
        internal void ReturnResult(ServerRequest request)
        {
            orb.GetBasicAdapter().ReturnResult(request);
        }

        /// <summary>
        /// Called from RequestProcessor when the request has been handled. The request is removed from the active request table.
        /// </summary>
        /// <param name="request"></param>
        internal void Finish(ServerRequest request)
        {
            lock (syncObject)
            {
                ByteArrayKey oid = request.ObjectIdAsByteArrayKey;
                activeRequestTable.Remove(oid);
                Monitor.PulseAll(syncObject);
            }
        }

        /// <summary>
        /// The main loop for dispatching requests to request processors
        /// </summary>
        private void Run()
        {
            State state;
            ServerRequest request;
            ObjAdapter closedConnectionException = new ObjAdapter("connection closed: adapter inactive");
            Transient transientException = new Transient();
            while (!terminate)
            {
                state = poa.State;
                if (POAUtil.IsActive(state))
                {
                    request = requestQueue.GetFirst();

                    /* Request available */
                    if (request != null)
                    {
                        if (request.RemainingPOAName != null)
                        {
                            orb.GetBasicAdapter().DeliverRequest(request, poa);
                            requestQueue.RemoveFirst();
                        }
                        else
                        {
                            try
                            {
                                ProcessRequest(request);
                                requestQueue.RemoveFirst();
                            }
                            catch (CompletionRequestedException e)
                            {
                                /* if waitForCompletion was called the poa state    was   changed    to   holding,
                                   discarding or  inactive, the loop don't block  in   waitForContinue,  the  loop
                                   continues  and will detect  the changed state in  the next turn  (for this turn
                                   the request will not processed) */
                            }
                            catch (ShutdownInProgressException e)
                            {
                                /* waitForShutdown was called */
                                WaitForQueue();
                            }
                            catch (ObjAdapter e)
                            {
                                requestQueue.RemoveFirst();
                                RejectRequest(request, e);
                            }
                            catch (ObjectNotExist e)
                            {
                                requestQueue.RemoveFirst();
                                RejectRequest(request, e);
                            }
                            catch (CORBA.Timeout e)
                            {
                                requestQueue.RemoveFirst();
                                RejectRequest(request, e);
                            }
                        }
                        continue;
                    }
                }
                else
                {
                    if (!waitForShutdownCalled && (POAUtil.IsDiscarding(state) || POAUtil.IsInactive(state)))
                    {
                        request = requestQueue.RemoveLast();

                        /* Request available */
                        if (request != null)
                        {
                            if (POAUtil.IsDiscarding(state))
                            {
                                RejectRequest(request, transientException);
                            }
                            else
                            {
                                RejectRequest(request, closedConnectionException);
                            }
                            continue;
                        }
                    }
                }
                /* if waitForShutdown was called the RequestController loop blocks for ALL TIME in waitForQueue (the poa
                   behaves as if he is in holding state now) ATTENTION, it's a lazy synchronisation, a request could be
                   rejected if waitForShutdown was called but couldn't be processed (it's save)
                */
                WaitForQueue();
            }
        }

        /// <summary>
        /// Called from external thread to synchronize with the request controller thread, 
        /// a caller waits for completion of all active requests, no new requests will be started from now on
        /// </summary>
        internal void WaitForCompletion()
        {
            lock (syncObject)
            {
                waitForCompletionCalled = true;

                while (waitForCompletionCalled && activeRequestTable.Count > 0)
                {
                    try
                    {
                        logger.Debug("somebody waits for completion and there are active processors");
                        Monitor.Wait(syncObject);
                    }
                    catch (ThreadInterruptedException e)
                    {
                    }
                }
            }
        }

        /// <summary>
        ///  Called from external thread to synchronize with the request controller thread, 
        ///  a caller waits for completion of all active requests on this object. 
        ///  No new requests on this object will be started from now on because a steady stream of incoming 
        ///  requests could keep the object from being deactivated, a servant may invoke recursive method calls 
        ///  on the  object it incarnates and deactivation should not necessarily prevent those invocations.
        /// </summary>
        /// <param name="oid"></param>
        internal void WaitForObjectCompletion(ByteArrayKey oid)
        {
            lock (syncObject)
            {
                while (activeRequestTable.Contains(oid))
                {
                    try
                    {
                        Monitor.Wait(syncObject);
                    }
                    catch (ThreadInterruptedException e)
                    {
                    }
                }
                if (logger.IsDebugEnabled)
                {
                    logger.Debug(POAUtil.Convert(oid.GetBytes()) + "all active processors for this object have finished");

                }

                deactivationList.Add(oid);
            }
        }

        /// <summary>
        /// Blocks the request controller thread if the queue is empty, the poa is in holding state or waitForShutdown was called,
        /// if waitForShutdown was called the RequestController loop blocks for ALL TIME in this method
        /// (the poa behaves as if he is in holding state now)
        /// </summary>
        private void WaitForQueue()
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("waiting for queue");
            }

            lock (queueLog)
            {
                while ((requestQueue.IsEmpty() ||
                        POAUtil.IsHolding(poa.ThePOAManager.GetState()) ||
                        waitForShutdownCalled) &&
                       !terminate)
                {
                    try
                    {
                        Monitor.Wait(queueLog);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        // ignored
                    }
                }
            }
        }

        /// <summary>
        ///  Called from external thread to synchronize with the request controller thread, 
        ///  a caller waits for completion of all active requests, no new requests will started for ALL TIME
        /// </summary>
        internal void WaitForShutdown()
        {
            lock (syncObject)
            {
                waitForShutdownCalled = true;

                while (waitForShutdownCalled && activeRequestTable.Count > 0 || localRequests > 0)
                {
                    try
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("somebody waits for shutdown and there are active processors");
                        }
                        Monitor.Wait(syncObject);
                    }
                    catch (ThreadInterruptedException e)
                    {
                    }
                }
            }
        }

        internal void AddLocalRequest()
        {
            lock (syncObject)
            {
                localRequests++;
            }

        }

        internal void RemoveLocalRequest()
        {
            lock (syncObject)
            {
                localRequests--;
                Monitor.PulseAll(syncObject);
            }

        }
    }
}
