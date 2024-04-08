// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.DSI;
using DotNetOrb.Core.POA.Exceptions;
using DotNetOrb.Core.Util;
using PortableServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static PortableServer.ServantLocator;

namespace DotNetOrb.Core.POA
{
    public class RequestProcessor : IInvocationContext, IConfigurable
    {
        private static int count = 0;

        private Thread thread;

        public Thread Thread => thread;

        private object syncObject = new object();

        private RPPoolManager poolManager;
        private bool start;
        internal bool IsActive
        {
            get
            {
                return start;
            }
        }
        private bool terminate;
        private RequestController controller;
        public ORB ORB
        {
            get
            {
                if (!start)
                {
                    throw new POAInternalException("error: RequestProcessor not started (get ORB)");
                }
                return controller.ORB;
            }
        }
        public POA POA
        {
            get
            {
                if (!start)
                {
                    throw new POAInternalException("error: RequestProcessor not started (get POA)");
                }
                return controller.POA;
            }
        }
        private ServerRequest request;
        public byte[] ObjectId
        {
            get
            {
                if (!start)
                {
                    throw new POAInternalException("error: RequestProcessor not started (get ObjectId)");
                }
                return request.ObjectId;
            }
        }
        private Servant servant;
        public Servant Servant
        {
            get
            {
                if (!start)
                {
                    throw new POAInternalException("error: RequestProcessor not started (getServant)");
                }
                return servant;
            }
        }
        private IServantManager servantManager;
        private Cookie cookie;

        /// <summary>
        /// Whether to check for expiry of any ReplyEndTimePolicy.
        /// Normally, it is sufficient to check this on the client side, but the additional
        /// check on the server side can save the server and the network some work.
        /// It requires that the clocks of the client and server machine are synchronized, though.
        /// </summary>
        private bool checkReplyEndTime = false;

        private ILogger logger;

        public static class SpecialOps
        {
            public static HashSet<string> operations = new HashSet<string>();

            public static string IsA => "_is_a";
            public static string Interface => "_interface";
            public static string GetPolicy => "_get_policy";
            public static string NonExistent => "_non_existent";
            public static string GetComponent => "_get_component";
            public static string RepositoryId => "_repository_id";
            public static string SetPolicy => "_set_policy_overrides";

            static SpecialOps()
            {
                operations.Add(IsA);
                operations.Add(Interface);
                operations.Add(GetPolicy);
                operations.Add(NonExistent);
                operations.Add(GetComponent);
                operations.Add(RepositoryId);
                operations.Add(SetPolicy);
            }

            public static bool IsSpecialOperation(string operation)
            {
                return operations.Contains(operation);
            }
        }

        public RequestProcessor(RPPoolManager poolManager)
        {
            thread = new Thread(new ThreadStart(Run));
            this.poolManager = poolManager;
        }

        public void Configure(IConfiguration config)
        {
            checkReplyEndTime = config.GetAsBoolean("DotNetOrb.POA.CheckReplyEndTime", false);
        }

        internal void Begin()
        {
            lock (syncObject)
            {
                start = true;
                Monitor.Pulse(syncObject);
            }
        }
        internal void End()
        {
            lock (syncObject)
            {
                terminate = true;
                Monitor.Pulse(syncObject);
                thread.Interrupt();
            }
        }

        internal void Init(RequestController requestController, ServerRequest serverRequest, Servant servant, IServantManager manager)
        {
            controller = requestController;
            request = serverRequest;
            this.servant = servant;
            servantManager = manager;
            cookie = null;
            logger = requestController.GetLogger();
        }

        private void Clear()
        {
            controller = null;
            request = null;
            servant = null;
            servantManager = null;
            cookie = null;
        }

        /// <summary>
        /// Causes the AOM to perform the incarnate call on a servant activator
        /// </summary>
        private void InvokeIncarnate()
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("rid: " + request.RequestId + " opname: " + request.Operation + " invoke incarnate on servant activator");
            }
            try
            {
                servant = controller.AOM.Incarnate(request.ObjectIdAsByteArrayKey, (IServantActivator)servantManager, controller.POA);
                if (servant == null)
                {
                    if (logger.IsWarnEnabled)
                    {
                        logger.Warn("rid: " + request.RequestId + " opname: " + request.Operation + " incarnate: returns null");
                    }

                    request.SystemException = new ObjAdapter();
                }
            }
            catch (CORBA.SystemException e)
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn("rid: " + request.RequestId + " opname: " + request.Operation + " incarnate: system exception was thrown.", e);
                }
                request.SystemException = e;
            }
            catch (PortableServer.ForwardRequest e)
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn("rid: " + request.RequestId + " opname: " + request.Operation + " incarnate: forward exception was thrown.", e);
                }
                request.LocationForward = e;

            }
            catch (Exception e)
            {
                /* not spec. */
                if (logger.IsErrorEnabled)
                {
                    logger.Error("rid: " + request.RequestId + " opname: " + request.Operation + " incarnate: throwable was thrown.", e);
                }
                request.SystemException = new ObjAdapter(e.ToString());
            }
        }

        /// <summary>
        /// Invokes the operation on servant
        /// </summary>
        private void InvokeOperation()
        {
            string operation = request.Operation;
            bool specialOperation = false;
            try
            {
                if (servant is IInvokeHandler)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("rid: " + request.RequestId + " opname: " + operation + " invokeOperation on servant (stream based)");
                    }

                    if (SpecialOps.IsSpecialOperation(operation))
                    {
                        if (operation == SpecialOps.GetPolicy)
                        {
                            // Check the number of args. If zero, then we assuming it's a "_get_policy"
                            // operation that was generated for an attribute named 'policy', instead
                            // of the 'get_policy' method on CORBA::Object
                            if (request.Arguments != null && request.Arguments.Count() >= 1)
                            {
                                specialOperation = true;
                            }
                        }
                        else
                        {
                            specialOperation = true;
                        }
                    }

                    if (specialOperation)
                    {
                        ((ServantDelegate)servant._Delegate)._Invoke(servant, operation, request.GetInputStream(), request);
                    }
                    else
                    {
                        ((IInvokeHandler)servant)._Invoke(operation, request.GetInputStream(), request);
                    }

                }
                else if (servant is DynamicImplementation)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("rid: " + request.RequestId + " opname: " + operation + " invoke operation on servant (dsi based)");
                    }

                    if (SpecialOps.IsSpecialOperation(operation) &&
                        !(operation.Equals(SpecialOps.RepositoryId) || operation.Equals(SpecialOps.GetComponent)))
                    {
                        ((ServantDelegate)servant._Delegate)._Invoke(servant, operation, request.GetInputStream(), request);
                    }
                    else
                    {
                        ((DynamicImplementation)servant).Invoke(request);
                    }
                }
                else
                {
                    if (logger.IsWarnEnabled)
                    {
                        logger.Warn("rid: " + request.RequestId + " opname: " + operation + " unknown servant type (neither stream nor dsi based)");
                    }
                }

            }
            catch (CORBA.SystemException e)
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info("rid: " + request.RequestId + " opname: " + operation + " invocation: system exception was thrown.", e);
                }
                request.SystemException = e;
            }
            catch (OutOfMemoryException e)
            {
                /* not spec. */
                if (logger.IsErrorEnabled)
                {
                    logger.Error("rid: " + request.RequestId + " opname: " + operation + " invocation: Caught OutOfMemory invoking operation.", e);
                }
                request.SystemException = new NoMemory(e.ToString());
            }
            catch (Exception e)
            {
                /* not spec. */
                if (logger.IsErrorEnabled)
                {
                    logger.Error("rid: " + request.RequestId + " opname: " + operation + " invocation: throwable was thrown.", e);
                }
                request.SystemException = new Unknown(e.ToString());
            }
        }


        /// <summary>
        /// Performs the postinvoke call on a servant locator
        /// </summary>
        private void InvokePostInvoke()
        {
            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("rid: " + request.RequestId + " opname: " + request.Operation + " invoke postinvoke on servant locator");
                }

                ((ServantLocator)servantManager).Postinvoke(request.ObjectId, controller.POA, request.Operation, cookie, servant);
            }
            catch (CORBA.SystemException e)
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info("rid: " + request.RequestId + " opname: " + request.Operation + " postinvoke: system exception was thrown.", e);
                }
                request.SystemException = e;

            }
            catch (Exception e)
            {
                /* not spec. */
                if (logger.IsWarnEnabled)
                {
                    logger.Warn("rid: " + request.RequestId + " opname: " + request.Operation + " postinvoke: throwable was thrown.", e);
                }
                request.SystemException = new ObjAdapter(e.ToString());
            }
        }

        /// <summary>
        /// Performs the preinvoke call on a servant locator
        /// </summary>
        private void InvokePreInvoke()
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("rid: " + request.RequestId + " opname: " + request.Operation + " invoke preinvoke on servant locator");
            }
            try
            {
                servant = ((ServantLocator)servantManager).Preinvoke(request.ObjectId, controller.POA, request.Operation, out cookie);
                if (servant == null)
                {
                    if (logger.IsWarnEnabled)
                    {
                        logger.Warn("rid: " + request.RequestId + " opname: " + request.Operation + " preinvoke: returns null");
                    }
                    request.SystemException = new ObjAdapter();
                }
                controller.ORB.SetDelegate(servant);        // set the orb
            }
            catch (CORBA.SystemException e)
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info("rid: " + request.RequestId + " opname: " + request.Operation + " preinvoke: system exception was thrown.", e);
                }
                request.SystemException = e;

            }
            catch (PortableServer.ForwardRequest e)
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info("rid: " + request.RequestId + " opname: " + request.Operation + " preinvoke: forward exception was thrown.", e);
                }
                request.LocationForward = e;
            }
            catch (Exception e)
            {
                /* not spec. */
                if (logger.IsWarnEnabled)
                {
                    logger.Warn("rid: " + request.RequestId + " opname: " + request.Operation + " preinvoke: throwable was thrown.", e);
                }
                request.SystemException = new ObjAdapter(e.ToString());
            }
        }



        /// <summary>
        /// The main request processing routine
        /// </summary>
        private void Process()
        {
            ORB orb = controller.ORB;
            //ServerRequestInfo info = null;

            if (Time.HasPassed(request.InvocationPolicies.RequestEndTime))
            {
                request.SystemException = new CORBA.Timeout("Request End Time exceeded", 0, CompletionStatus.No);
                return;
            }
            if (checkReplyEndTime && Time.HasPassed(request.InvocationPolicies.ReplyEndTime))
            {
                request.SystemException = new CORBA.Timeout("Reply End Time exceeded", 0, CompletionStatus.No);
                return;
            }

            Time.WaitFor(request.InvocationPolicies.RequestStartTime);

            if (servantManager != null)
            {
                if (servantManager is IServantActivator)
                {
                    InvokeIncarnate();
                }
                else
                {
                    InvokePreInvoke();
                }
                orb.SetDelegate(servant);
            }

            if (servant != null)
            {
                InvokeOperation();
            }

            // preinvoke and postinvoke are always called in pairs
            // but what happens if the servant is null

            if (cookie != null)
            {
                InvokePostInvoke();
            }

            if (checkReplyEndTime && Time.HasPassed(request.InvocationPolicies.ReplyEndTime))
            {
                request.SystemException = new CORBA.Timeout("Reply End Time exceeded after invocation", 0, CompletionStatus.Yes);
            }
        }

        private void Run()
        {
            while (true)
            {
                lock (syncObject)
                {
                    while (!terminate && !start)
                    {
                        try
                        {
                            /* waits for the next task */
                            Monitor.Wait(syncObject);
                        }
                        catch (ThreadInterruptedException e)
                        {
                            // ignored
                        }
                    }

                    if (terminate)
                    {
                        return;
                    }
                }

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("rid: " + request.RequestId + " opname: " + request.Operation + " starts with request processing");
                }

                if (request.SyncScope == SyncScope.SERVER)
                {
                    controller.ReturnResult(request);
                    Process();
                }
                else
                {
                    Process();
                    controller.ReturnResult(request);
                }

                // return the request to the request controller
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("rid: " + request.RequestId + " opname: " + request.Operation + " ends with request processing");
                }

                controller.Finish(request);

                start = false;
                Clear();

                // give back the processor into the pool
                poolManager.ReleaseProcessor(this);
            }
        }
    }
}
