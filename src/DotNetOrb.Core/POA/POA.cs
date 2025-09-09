// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BiDirPolicy;
using CORBA;
using DotNetOrb.SSL;
using PortableServer;
using System.Text;
using static DotNetOrb.Core.POA.POAConstants;
using CSI;
using static PortableServer.POA;
using static PortableServer.Current;
using System.Configuration;
using static PortableServer.POAManager;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.Util;
using DotNetOrb.Core.DSI;
using DotNetOrb.Core.POA.Exceptions;
using DotNetOrb.Core.POA.Policies;
using DotNetOrb.Core.POA.Util;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetOrb.Core.POA
{
    public class POA : _POALocalBase, IConfigurable
    {
        private ORB orb;
        private IConfiguration configuration;
        private ILogger logger;
        private byte[] implName;
        private string logPrefix;
        private IPOAListener poaListener;
        private IPOAMonitor monitor;
        public IPOAMonitor POAMonitor { get { return monitor; } set { monitor = value; } }

        private string name;
        public override string TheName { get { return name; }}
        private string qualifiedName;
        public string QualifiedName
        {
            get
            {
                if (qualifiedName == null)
                {
                    if (parent == null)
                    {
                        qualifiedName = "";
                    }
                    else if (parent.TheParent == null)
                    {
                        qualifiedName = name;
                    }
                    else
                    {
                        qualifiedName = parent.QualifiedName + ObjectKeySeparator + name;
                    }
                }
                return qualifiedName;
            }
        }

        private POA parent;
        public override IPOA TheParent { get { return parent; } }

        private Dictionary<string, POA> children = new Dictionary<string, POA>();
        public override IPOA[] TheChildren
        {
            get
            {
                return children.Values.ToArray();
            }
        }
        private POAManager poaManager;
        public override IPOAManager ThePOAManager { get { return poaManager; } }

        public State State
        {
            get
            {
                if (poaManager != null)
                {
                    return poaManager.GetState();
                }
                else
                {
                    return State.HOLDING;
                }
            }
        }

        private IAdapterActivator adapterActivator;
        public override IAdapterActivator TheActivator { get { return adapterActivator; } set { adapterActivator = value; } }

        private AOM aom;
        // thread for handle Requests
        private RequestController requestController;
        public RequestController RequestController
        {
            get { return requestController; }
        }
        private Servant defaultServant;
        internal Servant DefaultServant => defaultServant;

        private IServantManager servantManager;
        internal IServantManager ServantManager => servantManager;

        // poa identity and a counter for oid generation
        private byte[] poaId;
        private byte[] watermark;
        private long objectIdCount;

        public override IPOAManagerFactory ThePOAManagerFactory { get => throw new NotImplementedException(); }

        // default: ORB_CTRL_MODEL
        protected IThreadPolicy threadPolicy;
        internal bool IsSingleThreadModel
        {
            get
            {
                return threadPolicy != null && threadPolicy.Value == ThreadPolicyValue.SINGLE_THREAD_MODEL;
            }
        }

        // default: TRANSIENT
        protected ILifespanPolicy lifespanPolicy;
        public bool IsPersistent
        {
            get
            {
                return lifespanPolicy != null && lifespanPolicy.Value == LifespanPolicyValue.PERSISTENT;
            }
        }

        // default: UNIQUE_ID
        protected IIdUniquenessPolicy idUniquenessPolicy;
        protected bool IsUniqueId
        {
            get
            {
                return idUniquenessPolicy == null || idUniquenessPolicy.Value == IdUniquenessPolicyValue.UNIQUE_ID;
            }
        }
        protected bool IsMultipleId
        {
            get
            {
                return idUniquenessPolicy != null && idUniquenessPolicy.Value == IdUniquenessPolicyValue.MULTIPLE_ID;
            }
        }

        // default: SYSTEM_ID
        protected IIdAssignmentPolicy idAssignmentPolicy;
        public bool IsSystemId
        {
            get
            {
                return idAssignmentPolicy == null || idAssignmentPolicy.Value == IdAssignmentPolicyValue.SYSTEM_ID;
            }
        }

        // default: RETAIN
        protected IServantRetentionPolicy servantRetentionPolicy;

        public bool IsRetain
        {
            get
            {
                return servantRetentionPolicy == null || servantRetentionPolicy.Value == ServantRetentionPolicyValue.RETAIN;
            }
        }

        // default: USE_ACTIVE_OBJECT_MAP_ONLY
        protected IRequestProcessingPolicy requestProcessingPolicy;
        internal bool IsUseDefaultServant
        {
            get
            {
                return requestProcessingPolicy != null && requestProcessingPolicy.Value == RequestProcessingPolicyValue.USE_DEFAULT_SERVANT;
            }
        }

        protected bool UseDefaultServant
        {
            get
            {
                return IsUseDefaultServant && defaultServant != null;
            }
        }

        internal bool IsUseServantManager
        {
            get
            {
                return requestProcessingPolicy != null && requestProcessingPolicy.Value == RequestProcessingPolicyValue.USE_SERVANT_MANAGER;
            }
        }

        protected bool UseServantManager
        {
            get
            {
                return IsUseServantManager && servantManager != null;
            }
        }

        // default: NO_IMPLICIT_ACTIVATION
        protected IImplicitActivationPolicy implicitActivationPolicy;
        public bool IsImplicitActivation
        {
            get
            {
                return implicitActivationPolicy != null && implicitActivationPolicy.Value == ImplicitActivationPolicyValue.IMPLICIT_ACTIVATION;
            }
        }

        // default: NORMAL
        protected IBidirectionalPolicy bidirectionalPolicy;
        public bool IsBidirGOIP
        {
            get
            {
                return bidirectionalPolicy != null && bidirectionalPolicy.Value == BOTH.Value;
            }
        }

        // default: SSL_NOT_REQUIRED
        protected ISSLPolicy sslPolicy;
        public bool IsSSLRequired
        {
            get
            {
                return sslPolicy != null && sslPolicy.Value == SSLPolicyValue.SSL_REQUIRED;
            }
        }

        private Dictionary<uint, IPolicy> allPolicies = new Dictionary<uint, IPolicy>();

        /** key: , value: CORBA.Object */
        private Dictionary<ByteArrayKey, CORBA.Object> createdReferences = new Dictionary<ByteArrayKey, CORBA.Object>();

        // stores the etherealize_objects value from the first call of destroy
        private bool etherealize;

        // synchronisation stuff
        private ShutdownState shutdownState = ShutdownState.NotCalled;

        private object syncObject = new object();
        private object poaCreationLock = new object();
        private object poaDestructionLock = new object();
        private object unknownAdapterLock = new object();
        private bool unknownAdapterCalled;

        private bool configured = false;

        protected bool IsActive
        {
            get
            {
                return poaManager.GetState() == State.ACTIVE;
            }
        }
        protected bool IsDiscarding
        {
            get
            {
                return poaManager.GetState() == State.DISCARDING;
            }
        }
        protected bool IsHolding
        {
            get
            {
                return poaManager.GetState() == State.HOLDING;
            }
        }
        protected bool IsInactive
        {
            get
            {
                return poaManager.GetState() == State.INACTIVE;
            }
        }
        protected bool IsShutdownInProgress
        {
            get
            {
                return shutdownState >= ShutdownState.ShutdownInProgress;
            }
        }
        protected bool IsDestructionApparent
        {
            get
            {
                return shutdownState >= ShutdownState.DestructionApparent;
            }
        }

        /// <summary>
        /// Returns whether the POA has been completely destroyed (including finishing outstanding requests).
        /// This is public not protected as it is called from orb/Delegate.
        /// </summary>
        public bool IsDestructionComplete
        {
            get
            {
                return shutdownState >= ShutdownState.DestructionComplete;
            }
        }

        public override byte[] Id { get => throw new NotImplementedException(); }

        POA(ORB orb, string name, POA parent, POAManager poaManager, IPolicy[] policies) : base()
        {
            this.orb = orb;
            this.name = name;
            this.parent = parent;
            this.poaManager = poaManager;
            logPrefix = "POA " + name;

            if (policies != null)
            {
                for (int i = 0; i < policies.Length; i++)
                {
                    allPolicies.Add(policies[i].PolicyType, policies[i]);

                    switch (policies[i].PolicyType)
                    {
                        case THREAD_POLICY_ID.Value:
                            threadPolicy = (IThreadPolicy)policies[i];
                            break;
                        case LIFESPAN_POLICY_ID.Value:
                            lifespanPolicy = (ILifespanPolicy)policies[i];
                            break;
                        case ID_UNIQUENESS_POLICY_ID.Value:
                            idUniquenessPolicy = (IIdUniquenessPolicy)policies[i];
                            break;
                        case ID_ASSIGNMENT_POLICY_ID.Value:
                            idAssignmentPolicy = (IIdAssignmentPolicy)policies[i];
                            break;
                        case SERVANT_RETENTION_POLICY_ID.Value:
                            servantRetentionPolicy = (IServantRetentionPolicy)policies[i];
                            break;
                        case REQUEST_PROCESSING_POLICY_ID.Value:
                            requestProcessingPolicy = (IRequestProcessingPolicy)policies[i];
                            break;
                        case IMPLICIT_ACTIVATION_POLICY_ID.Value:
                            implicitActivationPolicy = (IImplicitActivationPolicy)policies[i];
                            break;
                        case BIDIRECTIONAL_POLICY_TYPE.Value:
                            bidirectionalPolicy = (IBidirectionalPolicy)policies[i];
                            break;
                        case SSL_POLICY_TYPE.Value:
                            sslPolicy = (ISSLPolicy)policies[i];
                            break;
                    }
                }
            }

            //check if BiDir policy tell us to use BiDirGIOP (for the whole ORB)
            if (IsBidirGOIP)
            {
                orb.IsBidir = true;
            }
        }


        /// <summary>
        /// Called from orb to obtain the RootPOA
        /// </summary>        
        public static POA POAInit(ORB orb)
        {
            var poaMgr = new POAManager(orb);

            //the only policy value that differs from default
            IPolicy[] policies = null;

            policies = new IPolicy[1];

            policies[0] = new ImplicitActivationPolicy(ImplicitActivationPolicyValue.IMPLICIT_ACTIVATION);

            // TODO: new GOA
            POA rootPOA = new POA(orb, RootPOAName, null, poaMgr, policies);

            return rootPOA;
        }

        public void Configure(IConfiguration config)
        {
            configuration = config;
            logger = configuration.GetLogger(GetType());

            string tmp = configuration.GetValue("DotNetOrb.ImplName", "");

            if (tmp.Length > 0)
            {
                implName = Encoding.Default.GetBytes(tmp);
            }
            watermark = GenerateWatermark();
            aom = IsRetain ? new AOM(IsUniqueId, logger) : null;

            requestController = new RequestController(this, orb, aom, orb.NewRPPoolManager(IsSingleThreadModel));
            requestController.Configure(configuration);

            poaManager.RegisterPOA(this);
            monitor = new POAMonitor();
            monitor.Init(this, aom, requestController.RequestQueue, requestController.PoolManager, "POA " + name);
            monitor.OpenMonitor();

            if (poaListener != null)
            {
                poaListener.PoaCreated(this);
            }

            monitor.Configure(configuration);

            if (logger.IsDebugEnabled)
            {
                logger.Debug("POA " + name + " ready");
            }
            configured = true;
        }

        private void CheckIsConfigured()
        {
            if (!configured)
            {
                throw new InvalidOperationException("POA: not configured!");
            }
        }
        protected void CheckDestructionApparent()
        {
            if (IsDestructionApparent)
            {
                throw new ObjectNotExist("POA destroyed", (int)(OMGVMCID.Value | 4), CompletionStatus.No);
            }
        }

        protected static void CheckNotLocal(IObject obj)
        {
            if (obj is LocalObject)
            {
                //Local object
                throw new WrongAdapter();
            }
        }

        /// <summary>
        /// Returns true if the current thread is in the context of executing a request from some POA 
        /// belonging to the same ORB as this POA
        /// </summary>
        public bool IsInInvocationContext()
        {
            try
            {
                if (orb.GetPOACurrent().GetORB() == orb)
                {
                    return true;
                }
            }
            catch (NoContext e)
            {
            }
            return false;
        }

        /// <summary>
        /// Returns true if the current thread is in the context of executing a request on the specified servant from this POA,
        /// if the specified servant is null, it returns true if the current thread is in an invocation context from this POA.        
        public bool IsInInvocationContext(Servant servant)
        {
            try
            {
                if (orb.GetPOACurrent().GetPoa() == this && (servant == null || orb.GetPOACurrent().GetServant() == servant))
                {
                    return true;
                }
            }
            catch (NoContext e)
            {
            }
            return false;
        }

        public void AddPOAEventListener(IEventListener listener)
        {
            CheckIsConfigured();

            if (listener is IPOAListener)
            {
                AddPOAListener((IPOAListener)listener);
            }

            if (listener is IAOMListener && aom != null)
            {
                aom.AddAOMListener((IAOMListener)listener);
            }

            if (listener is IRequestQueueListener)
            {
                requestController.RequestQueue.AddRequestQueueListener((IRequestQueueListener)listener);
            }

            if (listener is IRPPoolManagerListener)
            {
                requestController.PoolManager.AddRPPoolManagerListener((IRPPoolManagerListener)listener);
            }
        }

        public void RemovePOAEventListener(IEventListener listener)
        {
            if (listener is IPOAListener)
            {
                RemovePOAListener((IPOAListener)listener);
            }
            if (listener is IAOMListener && aom != null)
            {
                aom.RemoveAOMListener((IAOMListener)listener);
            }
            if (listener is IRequestQueueListener)
            {
                requestController.RequestQueue.RemoveRequestQueueListener((IRequestQueueListener)listener);
            }
            if (listener is IRPPoolManagerListener)
            {
                requestController.PoolManager.RemoveRPPoolManagerListener((IRPPoolManagerListener)listener);
            }
        }

        protected void AddPOAListener(IPOAListener listener)
        {
            lock (syncObject)
            {
                poaListener = (IPOAListener)EventMulticaster.Add(poaListener, listener);
            }
        }

        protected void RemovePOAListener(IPOAListener listener)
        {
            lock (syncObject)
            {
                poaListener = (IPOAListener)EventMulticaster.Remove(poaListener, listener);
            }
        }

        /// <summary>
        /// Called from Delegate. To ensure thread safety we use the AOM::Incarnate method when activating 
        /// a servant with the ServantActivator.
        /// </summary>        
        public Servant IncarnateServant(byte[] oid, IServantActivator sa)
        {
            return aom.Incarnate(new ByteArrayKey(oid), sa, this);
        }

        /// <summary>
        /// Called from orb, returns a registered child poa, if no child poa exists an adapter activator will be used 
        /// to create a new poa under this name
        /// </summary>
        public POA GetChildPOA(string adapterName)
        {
            CheckIsConfigured();
            CheckDestructionApparent();
            POA child = children[adapterName];

            if (child == null || child.IsDestructionApparent)
            {
                if (adapterActivator == null)
                {
                    if (IsHolding)
                    {
                        throw new ParentIsHoldingException();
                    }
                    throw new ObjectNotExist("No adapter activator exists for " + adapterName, (int)(OMGVMCID.Value | 2), CompletionStatus.No);
                }

                if (IsDiscarding)
                {
                    throw new Transient("a parent poa is in discarding state", (int)(OMGVMCID.Value | 1), CompletionStatus.No);
                }

                if (IsInactive)
                {
                    throw new ObjAdapter("A parent poa is in inactive state", (int)(OMGVMCID.Value | 1), CompletionStatus.No);
                }

                //poa should be active
                bool successful = false;

                if (IsSingleThreadModel)
                {
                    //all invocations an adapter activator are serialized if the single thread model is in use
                    lock (unknownAdapterLock)
                    {
                        while (unknownAdapterCalled)
                        {
                            try
                            {
                                Monitor.Wait(unknownAdapterLock);
                            }
                            catch (ThreadInterruptedException e)
                            {
                            }
                        }
                        unknownAdapterCalled = true;
                        try
                        {
                            successful = TheActivator.UnknownAdapter(this, POAUtil.UnmaskStr(adapterName));
                        }
                        finally
                        {
                            unknownAdapterCalled = false;
                            Monitor.PulseAll(unknownAdapterLock);
                        }
                    }
                }
                else
                {
                    //ORB_CTRL_MODEL
                    successful = TheActivator.UnknownAdapter(this, POAUtil.UnmaskStr(adapterName));
                }

                // UnknownAdapter doesn't return until the poa is created and initialized
                if (successful)
                {
                    child = children[adapterName];
                    if (child == null)
                    {
                        throw new POAInternalException("UnknownAdapter returns true, but the child poa doesn't exist");
                    }
                }
                else
                {
                    throw new ObjectNotExist("poa activation is failed", (int)(OMGVMCID.Value | 2), CompletionStatus.No);
                }
            }
            return child;
        }

        public override byte[] ActivateObject(Servant servant)
        {
            CheckIsConfigured();

            CheckDestructionApparent();

            if (!IsRetain || !IsSystemId)
            {
                throw new WrongPolicy();
            }

            byte[] objectId = GenerateObjectId();

            try
            {
                aom.Add(objectId, servant);

                orb.SetDelegate(servant);
            }
            catch (ObjectAlreadyActive e)
            {
                throw new POAInternalException("error: object already active (activate_object)");
            }

            return objectId;
        }

        public override void ActivateObjectWithId(byte[] oid, Servant servant)
        {
            CheckIsConfigured();

            CheckDestructionApparent();

            if (oid == null)
            {
                throw new BadParam("Cannot activate_object_with_id with null ID.", (int)(OMGVMCID.Value | 14), CompletionStatus.No);
            }

            if (!IsRetain)
            {
                throw new WrongPolicy();
            }

            if (IsSystemId && !PreviouslyGeneratedObjectId(oid))
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn(logPrefix + "oid: " + POAUtil.Convert(oid) + " - activate_object_with_id: oid not previously generated!");
                }
                throw new BadParam((int)(OMGVMCID.Value | 14), CompletionStatus.No);
            }

            aom.Add(oid, servant);

            orb.SetDelegate(servant);
        }

        internal void ChangeToActive()
        {
            if (poaListener != null)
            {
                poaListener.PoaStateChanged(this, POAState.Active);
            }

            monitor.ChangeState("changed to active...");

            // notify everybody who is waiting for request completion
            requestController.ResetPreviousCompletionCall();
            // continue the request dispatching
            requestController.ContinueToWork();

            monitor.ChangeState("active");
        }

        internal void ChangeToDiscarding()
        {

            if (poaListener != null)
            {
                poaListener.PoaStateChanged(this, POAState.Discarding);
            }

            monitor.ChangeState("changed to discarding ...");

            // notify everybody who is waiting for request completion
            requestController.ResetPreviousCompletionCall();
            // continue the request dispatching
            requestController.ContinueToWork();
            // wait for completion of all active requests
            requestController.WaitForCompletion();

            monitor.ChangeState("discarding");
        }

        internal void ChangeToHolding()
        {
            if (poaListener != null)
            {
                poaListener.PoaStateChanged(this, POAState.Holding);
            }
            monitor.ChangeState("changed to holding ...");

            // notify everybody who is waiting for request completion
            requestController.ResetPreviousCompletionCall();
            // wait for completion of all active requests
            requestController.WaitForCompletion();
            monitor.ChangeState("holding");
        }


        internal void ChangeToInactive(bool etherealizeObjects)
        {
            if (poaListener != null)
            {
                poaListener.PoaStateChanged(this, POAState.Inactive);
            }

            monitor.ChangeState("changed to inactive ...");

            // notify everybody who is waiting for request completion
            requestController.ResetPreviousCompletionCall();

            // continue the request dispatching
            requestController.ContinueToWork();

            // wait for completion of all active requests
            requestController.WaitForCompletion();

            /* etherialize all active objects */
            if (etherealize && IsRetain && IsUseServantManager && servantManager != null)
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(logPrefix + "etherialize all servants ...");
                }

                aom.RemoveAll((IServantActivator)servantManager, this, true);

                if (logger.IsInfoEnabled)
                {
                    logger.Info(logPrefix + "etherialize all servants ...");
                }

                if (monitor != null)
                {
                    monitor.ChangeState("inactive (etherialization completed)");
                }

            }
            else
            {
                if (monitor != null)
                {
                    monitor.ChangeState("inactive (no etherialization)");
                }
            }
        }

        public override IIdAssignmentPolicy CreateIdAssignmentPolicy(IdAssignmentPolicyValue value)
        {
            CheckDestructionApparent();
            return new IdAssignmentPolicy(value);
        }

        public override IIdUniquenessPolicy CreateIdUniquenessPolicy(IdUniquenessPolicyValue value)
        {
            CheckDestructionApparent();
            return new IdUniquenessPolicy(value);
        }

        public override IImplicitActivationPolicy CreateImplicitActivationPolicy(ImplicitActivationPolicyValue value)
        {
            CheckDestructionApparent();
            return new ImplicitActivationPolicy(value);
        }

        public override ILifespanPolicy CreateLifespanPolicy(LifespanPolicyValue value)
        {
            CheckDestructionApparent();
            return new LifespanPolicy(value);
        }

        public override IPOA CreatePoa(string adapterName, IPOAManager aPOAManager, IPolicy[] policies)
        {
            CheckIsConfigured();

            CheckDestructionApparent();

            if (adapterName == null)
            {
                throw new BadParam("Cannot pass null as an adapter name");
            }
            var poaName = POAUtil.MaskStr(adapterName);

            if (aPOAManager != null && !(aPOAManager is POAManager))
            {
                throw new Exceptions.ApplicationException("error: the POAManager is incompatible with type \"DotNetOrb.Core.POA.POAManager\"!");
            }

            IPolicy[] policyList = null;
            if (policies != null)
            {
                // check the policy list for inconstancies
                short index = VerifyPolicyList(policies);

                if (index != -1)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("Policy list invalid at index " + index);
                    }
                    throw new InvalidPolicy((ushort)index);
                }

                // copy the policy list
                policyList = new IPolicy[policies.Length];

                for (int i = 0; i < policies.Length; i++)
                {
                    policyList[i] = policies[i].Copy();
                }
            }

            POA child;

            lock (poaCreationLock)
            {
                children.TryGetValue(poaName, out child);
                if (child != null && !child.IsDestructionApparent)
                {
                    throw new AdapterAlreadyExists();
                }
                // wait for completion of a concurrent destruction process
                if (child != null)
                {
                    POA aChild;
                    while ((aChild = children[poaName]) != null)
                    {
                        try
                        {
                            // notification is in unregisterChild
                            Monitor.Wait(poaCreationLock);
                        }
                        catch (ThreadInterruptedException e)
                        {
                        }

                        // someone else has won the race
                        if (child != aChild)
                        {
                            throw new AdapterAlreadyExists();
                        }
                    }
                }

                if (IsShutdownInProgress)
                {
                    throw new BadInvOrder((int)(OMGVMCID.Value | 17), CompletionStatus.No);
                }

                var poaManager = aPOAManager == null ? new POAManager(orb) : (POAManager)aPOAManager;

                //TODO GOA
                child = new POA(orb, poaName, this, poaManager, policyList);

                try
                {
                    child.Configure(configuration);
                }
                catch (ConfigurationException e)
                {
                    throw new CORBA.Internal(e.ToString());
                }

                // notify a poa listener
                try
                {
                    if (poaListener != null)
                    {
                        poaListener.PoaCreated(child);
                    }
                }
                catch (CORBA.Internal e)
                {
                    poaManager.poaCreationFailed = true;
                    throw e;
                }
                // register the child poa
                children.Add(poaName, child);
            }
            return child;
        }

        /// <summary>
        ///  The specified repository id, which may be a null string, will become the typeId of the generated object reference
        /// </summary>
        public override CORBA.IObject CreateReference([WideChar(false)] string intfRepId)
        {
            CheckIsConfigured();

            CheckDestructionApparent();

            if (!IsSystemId)
            {
                throw new WrongPolicy();
            }

            return GetReference(GenerateObjectId(), intfRepId, false);
        }

        public override CORBA.IObject CreateReferenceWithId(byte[] oid, string intfRepId)
        {
            CheckIsConfigured();

            CheckDestructionApparent();

            if (IsSystemId && !PreviouslyGeneratedObjectId(oid))
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn(logPrefix + "oid: " + POAUtil.Convert(oid) + "CreateReferenceWithId : object key not previously generated!");
                }

                throw new BadParam((int)(OMGVMCID.Value | 14), CompletionStatus.No);
            }

            return GetReference(oid, intfRepId, false);
        }


        /// <summary>
        /// Called from orb for handing over a request
        /// </summary>        
        public void Invoke(ServerRequest request)
        {
            CheckIsConfigured();

            lock (poaDestructionLock)
            {
                CheckDestructionApparent();

                // if the request is for this poa check whether the object key is generated from him
                if (request.RemainingPOAName == null)
                {
                    if (IsSystemId && !PreviouslyGeneratedObjectId(request.ObjectId))
                    {
                        if (logger.IsWarnEnabled)
                        {
                            logger.Warn($"{logPrefix} rid: {request.RequestId} opname: {request.Operation} Invoke: object id not previously generated!");
                            if (logger.IsDebugEnabled)
                            {
                                logger.Debug($"{logPrefix} ObjectId: {CorbaLoc.ParseKey(request.ObjectId)} to POA Watermark: {CorbaLoc.ParseKey(watermark)} mismatch.");
                            }
                        }
                        throw new WrongAdapter();
                    }
                }

                try
                {
                    // Pass the request to the request controller. The operation returns immediately after the request is queued
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug($"{logPrefix} rid: {request.RequestId} opname: {request.Operation} Invoke: queuing request");
                    }
                    requestController.QueueRequest(request);
                }
                catch (ResourceLimitReachedException e)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("Caught " + e + " when queueing " + request.Operation);
                    }
                    throw new Transient("Resource limit reached", (int)(OMGVMCID.Value | 1), CompletionStatus.No);
                }
            }
        }

        public bool PreviouslyGeneratedObjectId(byte[] oid)
        {
            return IdUtil.Equals(watermark, ExtractWatermark(oid));
        }

        public bool PreviouslyGeneratedObjectKey(byte[] objectKey)
        {
            return IdUtil.Equals(objectKey, GetPOAId(), GetPOAId().Length);
        }

        public override IRequestProcessingPolicy CreateRequestProcessingPolicy(RequestProcessingPolicyValue value)
        {
            CheckDestructionApparent();
            return new RequestProcessingPolicy(value);
        }

        public override IServantRetentionPolicy CreateServantRetentionPolicy(ServantRetentionPolicyValue value)
        {
            CheckDestructionApparent();
            return new ServantRetentionPolicy(value);
        }

        public override IThreadPolicy CreateThreadPolicy(ThreadPolicyValue value)
        {
            CheckDestructionApparent();
            return new ThreadPolicy(value);
        }

        public override void DeactivateObject(byte[] oid)
        {
            CheckIsConfigured();
            if (!IsRetain)
            {
                throw new WrongPolicy();
            }
            //TODO
            //((GOA)this).clearGroupRegistration(oid);
            var oidbak = new ByteArrayKey(oid);
            aom.Remove(oidbak, requestController, UseServantManager ? (IServantActivator)servantManager : null, this, false);
            createdReferences.Remove(oidbak);
        }

        public override void Destroy(bool etherealizeObjects, bool waitForCompletion)
        {
            CheckIsConfigured();

            if (waitForCompletion && IsInInvocationContext())
            {
                throw new BadInvOrder((int)(OMGVMCID.Value | 3), CompletionStatus.No);
            }

            /* synchronized with creationLog */
            /* child poa creations are impossible now */
            MakeShutdownInProgress(etherealizeObjects);

            // destroy all childs first
            // Clone the collection to prevent concurrent modification problems.
            var childs = children.Values.ToArray();
            foreach (var child in childs)
            {
                child.Destroy(etherealize, waitForCompletion);
            }

            var task = Task.Run(() =>
            {
                //The apparent destruction of the POA occurs only after all executing requests in the POA have completed,
                //but before any calls to etherealize are made.
                requestController.WaitForShutdown();
                //poa behaves as if it is in the holding state now and blocks until all active request have completed
                MakeDestructionApparent();
                //unregister poa from the POAManager, any calls on the poa are impossible now especially you cannot activate
                //or deactivate objects (raises a ObjNotExist exception), but you have a race condition with currently 
                //running object (de)activation processes
                MakeDestructionComplete();
            });
            if (waitForCompletion)
            {
                try
                {
                    task.Wait();
                }
                catch (Exception e) { }
            }
        }

        private byte[] ExtractWatermark(byte[] id)
        {
            if (id.Length < watermark.Length)
            {
                return new byte[0];
            }

            return IdUtil.Extract(id, id.Length - watermark.Length, watermark.Length);
        }

        /// <summary>
        /// If the intended child poa is not found and activateIt is TRUE, it is  possible for another thread to create the same poa with
        /// CreatePOA at the same time in a race condition. Applications should be prepared to deal with failures from the manual
        /// (CreatePOA) or automatic (FindPOA or UnknownAdapter from AdapterActivator) POA creation. Another possible situation is that 
        /// the poa returned goes shutdown but the orb will notice this situation if it will proceed with request processing.
        /// </summary>        
        public override IPOA FindPoa(string adapterName, bool activateIt)
        {
            CheckDestructionApparent();

            string poaName = POAUtil.MaskStr(adapterName);

            POA child = children[poaName];

            if (child == null || child.IsDestructionApparent)
            {

                bool successful = false;

                if (activateIt && TheActivator != null)
                {
                    //all invocations an adapter activator are serialized if the single thread model is in use
                    if (IsSingleThreadModel)
                    {
                        lock (unknownAdapterLock)
                        {
                            while (unknownAdapterCalled)
                            {
                                try
                                {
                                    Monitor.Wait(unknownAdapterLock);
                                }
                                catch (ThreadInterruptedException e)
                                {
                                }
                            }
                            unknownAdapterCalled = true;
                            try
                            {
                                successful = TheActivator.UnknownAdapter(this, adapterName);
                            }
                            finally
                            {
                                unknownAdapterCalled = false;
                                Monitor.PulseAll(unknownAdapterLock);
                            }
                        }

                    }
                    else
                    { /* ORB_CTRL_MODEL */
                        successful = TheActivator.UnknownAdapter(this, adapterName);
                    }
                }

                /* unknown_adapter returns not until the poa is created and initialized */
                if (successful)
                {
                    child = children[poaName];
                    if (child == null)
                    {
                        throw new POAInternalException("error: UnknownAdapter returns true, but the child poa does'n extist");
                    }

                }
                else
                {
                    throw new AdapterNonExistent();
                }
            }
            return child;
        }

        /// <summary>
        /// Creates a new ObjectId for an object
        /// </summary>        
        internal byte[] GenerateObjectId()
        {
            lock (syncObject)
            {
                if (IsPersistent)
                {
                    return IdUtil.Concat(IdUtil.CreateId(), watermark);
                }

                // Synchonize as the increment is not an atomic operation.
                return IdUtil.Concat(IdUtil.ToId(objectIdCount++), watermark);
            }
        }

        private byte[] GenerateWatermark()
        {
            if (watermark == null)
            {
                if (IsPersistent)
                {
                    watermark = IdUtil.ToId(Encoding.Default.GetString(GetPOAId()).GetHashCode());
                }
                else
                {
                    watermark = IdUtil.CreateId();
                }
            }
            return watermark;
        }

        public override Servant GetServant()
        {
            CheckIsConfigured();

            CheckDestructionApparent();

            if (!IsUseDefaultServant)
            {
                throw new WrongPolicy();
            }

            if (defaultServant == null)
            {
                throw new NoServant();
            }

            return defaultServant;
        }

        public override IServantManager GetServantManager()
        {
            CheckIsConfigured();

            CheckDestructionApparent();

            if (!IsUseServantManager)
            {
                throw new WrongPolicy();
            }

            return servantManager;
        }

        public byte[] GetPOAId()
        {
            if (poaId == null)
            {
                byte[] implName = GetImplName();

                int inLength = implName.Length;

                byte[] poaName = Encoding.Default.GetBytes(QualifiedName);
                int pnLength = poaName.Length;

                int offset = 0;
                if (pnLength == 0)
                {
                    poaId = new byte[inLength];
                    Array.Copy(implName, 0, poaId, 0, inLength);
                }
                else
                {
                    poaId = new byte[inLength + pnLength + 1];
                    Array.Copy(implName, 0, poaId, 0, inLength);
                    offset += inLength;
                    poaId[offset] = ObjectKeySeparatorByte;
                    offset++;
                    Array.Copy(poaName, 0, poaId, offset, pnLength);
                }
            }
            return poaId;
        }

        private byte[] GetImplName()
        {
            byte[] impl;
            // If we are using a transient object then we must place some random information in the IOR so that it is unique.
            if (!IsPersistent || implName == null)
            {
                impl = orb.ServerId;

                if (logger.IsInfoEnabled)
                {
                    if (IsPersistent)
                    {
                        logger.Info("Impl name not set; using server ID: " + Encoding.Default.GetString(impl));
                    }
                    else
                    {
                        logger.Info("Using server ID (" + Encoding.Default.GetString(impl) + ") for transient POA");
                    }
                }
            }
            else
            {
                impl = implName;
            }
            var masked = POAUtil.MaskId(impl);
            return masked;
        }

        protected CORBA.Object GetReference(byte[] oid, string intfRepId, bool cache)
        {
            byte[] objectId = POAUtil.MaskId(oid);
            int pidLength = GetPOAId().Length;
            int oidLength = objectId.Length;
            byte[] objectKey = new byte[pidLength + oidLength + 1];
            int offset = 0;

            Array.Copy(GetPOAId(), 0, objectKey, offset, pidLength);
            offset += pidLength;
            objectKey[offset] = ObjectKeySeparatorByte;
            offset++;
            Array.Copy(objectId, 0, objectKey, offset, oidLength);

            var key = new ByteArrayKey(oid);

            if (createdReferences.ContainsKey(key))
            {
                return createdReferences[key];
            }
            else
            {
                var result = orb.GetReference(this, objectKey, intfRepId, !IsPersistent);

                if (cache)
                {
                    createdReferences.Add(key, result);
                }

                if (poaListener != null)
                {
                    poaListener.ReferenceCreated(result);
                }
                return result;
            }
        }

        public override CORBA.IObject IdToReference(byte[] oid)
        {
            CheckDestructionApparent();

            if (!IsRetain)
            {
                throw new WrongPolicy();
            }

            var servant = aom.GetServant(oid);
            // objectId is not active
            if (servant == null)
            {
                throw new ObjectNotActive();
            }

            //If the object with the specified ObjectId currently active, a reference encapsulating
            //the information used to activate the object is returned.
            return GetReference(oid, servant._AllInterfaces(this, oid)[0], true);
        }

        public override Servant IdToServant(byte[] oid)
        {
            CheckDestructionApparent();

            if (!IsRetain && !IsUseDefaultServant)
            {
                throw new WrongPolicy();
            }

            // servant is active
            if (IsRetain)
            {
                var servant = aom.GetServant(oid);

                if (servant != null)
                {
                    return servant;
                }
            }

            if (UseDefaultServant)
            {
                return defaultServant;
            }

            throw new ObjectNotActive();
        }

        /// <summary>
        /// Any calls on the poa are impossible now, especially you cannot activate or deactivate objects 
        /// (raises a ObjNotExist exception). The poa will unregister with the POAManager.
        /// After destruction has become apparent, the POA may be re-created via either the AdapterActivator or a call to CreatePOA
        /// </summary>
        private void MakeDestructionApparent()
        {
            lock (poaDestructionLock)
            {
                if (shutdownState < ShutdownState.DestructionApparent)
                {
                    /* do */
                    poaManager.UnregisterPOA(this);

                    /* set */
                    shutdownState = ShutdownState.DestructionApparent;

                    /* announce */
                    if (poaListener != null)
                    {
                        poaListener.PoaStateChanged(this, POAState.Destroyed);
                    }

                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug(logPrefix + "destruction is apparent");
                    }

                    monitor.ChangeState("destruction is apparent ...");
                }
            }
        }

        /// <summary>
        /// After destruction has become complete, a poa creation process under the same poa name can continue now.
        /// </summary>
        private void MakeDestructionComplete()
        {
            if (shutdownState < ShutdownState.DestructionComplete)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug(logPrefix + "clear up the queue ...");
                }

                // Clear up the queue
                requestController.ClearQueue(new ObjectNotExist("adapter destroyed", (int)(OMGVMCID.Value | 3), CompletionStatus.No));

                if (logger.IsDebugEnabled)
                {
                    logger.Debug(logPrefix + "... done");
                }

                if (aom != null)
                {
                    //Stop the AOM removal queue.
                    aom.EndAOMRemoval();
                }

                //etherialize all active objects
                if (etherealize && IsRetain && UseServantManager)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug(logPrefix + "etherialize all servants ...");
                    }

                    aom.RemoveAll((IServantActivator)servantManager, this, true);

                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug(logPrefix + "... done");
                    }
                }

                if (logger.IsDebugEnabled)
                {
                    logger.Debug(logPrefix + "remove all processors from the pool ...");
                }
                requestController.ClearPool();

                if (logger.IsDebugEnabled)
                {
                    logger.Debug(logPrefix + "... done");
                }

                //stop the request controller
                if (logger.IsDebugEnabled)
                {
                    logger.Debug(logPrefix + "stop the request controller ...");
                }

                requestController.End();

                if (logger.IsDebugEnabled)
                {
                    logger.Debug(logPrefix + "... done");
                }

                shutdownState = ShutdownState.DestructionComplete;
                if (parent != null)
                {
                    // I am not the RootPOA unregister the poa with the parent and notify a concurrent creation process
                    parent.UnregisterChild(name);
                }

                //annouce
                if (logger.IsInfoEnabled)
                {
                    logger.Info(logPrefix + " destroyed");
                }

                monitor.ChangeState("destroyed");

                //clear tables
                createdReferences.Clear();
                allPolicies.Clear();
            }
        }

        /// <summary>
        /// The etherealizeObjects parameter from the destroy method will be saved in the field etherealize 
        /// because the etherealizeObjects parameter applies only to the first call of destroy. 
        /// Subsequent calls use the field etherealize. Any calls to CreatePOA are impossible now (receives a BadInvOrder exception).
        /// </summary>
        private void MakeShutdownInProgress(bool etherealizeObjects)
        {
            lock (poaCreationLock)
            {
                if (shutdownState < ShutdownState.ShutdownInProgress)
                {
                    /* do */
                    etherealize = etherealizeObjects;

                    /* set */
                    shutdownState = ShutdownState.ShutdownInProgress;

                    /* annouce */
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug(logPrefix + "shutdown is in progress");
                    }

                    monitor.ChangeState("shutdown is in progress ...");
                }
            }
        }

        public override byte[] ReferenceToId(IObject reference)
        {
            CheckDestructionApparent();
            CheckNotLocal(reference);

            byte[] objectId;
            byte[] objectKey = orb.MapObjectKey(((Delegate)((CORBA.Object)reference)._Delegate).GetObjectKey());

            try
            {
                objectId = POAUtil.ExtractOID(objectKey);
            }
            catch (POAInternalException e)
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn(logPrefix + "Unable to extract OID from objectKey: " + Encoding.Default.GetString(objectKey));
                }
                throw new WrongAdapter();
            }

            if (IsSystemId && !PreviouslyGeneratedObjectId(objectId))
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn(logPrefix + "oid: " + POAUtil.Convert(objectId) + "ReferenceToId: oid not previously generated!");
                }
                throw new WrongAdapter();
            }
            else if (!IsSystemId && !QualifiedName.Equals(POAUtil.ExtractPOAName(objectKey)))
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn(logPrefix + "reference: " + Encoding.Default.GetString(objectKey) + "ReferenceToId: reference not previously generated for this POA!");
                }
                throw new WrongAdapter();
            }

            return objectId;
        }

        public override Servant ReferenceToServant(IObject reference)
        {
            if (reference == null)
            {
                throw new BadParam("reference may not be null");
            }

            CheckDestructionApparent();
            CheckNotLocal(reference);

            if (!IsRetain && !IsUseDefaultServant)
            {
                throw new WrongPolicy();
            }

            byte[] objectId;
            byte[] objectKey = orb.MapObjectKey(((Delegate)((CORBA.Object)reference)._Delegate).GetObjectKey());

            try
            {
                objectId = POAUtil.ExtractOID(objectKey);
            }
            catch (POAInternalException e)
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn(logPrefix + "Unable to extract OID from objectKey: " + Encoding.Default.GetString(objectKey));
                }
                throw new WrongAdapter();
            }

            if (IsSystemId && !PreviouslyGeneratedObjectId(objectId))
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn(logPrefix + "oid: " + POAUtil.Convert(objectId) + "ReferenceToServant: oid not previously generated!");
                }
                throw new WrongAdapter();
            }
            else if (!IsSystemId && !QualifiedName.Equals(POAUtil.ExtractPOAName(objectKey)))
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn(logPrefix + "reference: " + Encoding.Default.GetString(objectKey) + "ReferenceToServant: reference not previously generated for this POA!");
                }
                throw new WrongAdapter();
            }

            var oid = new ByteArrayKey(objectId);

            if (aom != null && aom.IsDeactivating(oid) || requestController.IsDeactivating(oid))
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn(logPrefix + "oid: " + POAUtil.Convert(objectId) + "cannot process request, because object is already in the deactivation process");
                }

                throw new ObjectNotExist((int)(OMGVMCID.Value | 4), CompletionStatus.No);
            }

            Servant servant = null;

            //is active servant
            if (IsRetain && (servant = aom.GetServant(oid)) != null)
            {
                return servant;
            }
            else if (UseDefaultServant)
            {
                return defaultServant;
            }
            throw new ObjectNotActive();
        }

        public override byte[] ServantToId(Servant servant)
        {
            CheckDestructionApparent();

            if (!IsUseDefaultServant && (!IsRetain || !IsUniqueId) && (!IsRetain || !IsImplicitActivation))
            {
                throw new WrongPolicy();
            }

            byte[] objectId = null;

            if (IsRetain)
            {
                if (IsUniqueId)
                {
                    objectId = aom.GetObjectId(servant);
                    if (objectId != null)
                    {
                        return objectId;
                    }
                }

                if (IsImplicitActivation && (IsMultipleId || !aom.Contains(servant)))
                {
                    objectId = GenerateObjectId();
                    //activate the servant using the generated objectId and the intfRepId associated with the servant
                    try
                    {
                        aom.Add(objectId, servant);
                    }
                    catch (ObjectAlreadyActive e)
                    {
                        throw new POAInternalException("error: object already active (ServantToId)");
                    }
                    catch (ServantAlreadyActive e)
                    {
                        //it's ok, another one was faster with activation (only occurs if unique_id is set)
                        objectId = aom.GetObjectId(servant);
                    }

                    orb.SetDelegate(servant);

                    return objectId;
                }
            }
            if (IsUseDefaultServant && servant == defaultServant && IsInInvocationContext(servant))
            {
                //objectId associated with the current invocation
                try
                {
                    objectId = orb.GetPOACurrent().GetObjectId();
                }
                catch (NoContext e)
                {
                    throw new POAInternalException("error: not in invocation context (ServantToId)");
                }
                return objectId;
            }

            throw new ServantNotActive();
        }

        public override CORBA.IObject ServantToReference(Servant servant)
        {
            CheckDestructionApparent();

            bool isInInvocationContext = IsInInvocationContext(servant);

            if ((!IsRetain || !IsUniqueId) && (!IsRetain || !IsImplicitActivation) && !isInInvocationContext)
            {
                throw new WrongPolicy();
            }

            byte[] objectId = null;

            if (isInInvocationContext)
            {
                /* reference = Reference associated with the current invocation */
                try
                {
                    objectId = orb.GetPOACurrent().GetObjectId();
                }
                catch (NoContext e)
                {
                    throw new POAInternalException("error: not in invocation context (ServantToReference)");
                }
                return GetReference(objectId, servant._AllInterfaces(this, objectId)[0], true);
            }

            if (IsRetain)
            {
                if (IsUniqueId)
                {
                    //the object reference encapsulating the information used to activate the servant is returned
                    objectId = aom.GetObjectId(servant);
                    if (objectId != null)
                    {
                        return GetReference(objectId, servant._AllInterfaces(this, objectId)[0], true);
                    }
                }

                if (IsImplicitActivation && (IsMultipleId || !aom.Contains(servant)))
                {
                    objectId = GenerateObjectId();

                    //activate the servant using a generated objectId and the intfRepId associated with the servant 
                    //and a corresponding object reference is returned

                    try
                    {
                        aom.Add(objectId, servant);
                    }
                    catch (ObjectAlreadyActive e)
                    {
                        throw new POAInternalException("error: object already active (ServantToReference)");
                    }
                    catch (ServantAlreadyActive e)
                    {
                        //it's ok, another one was faster with activation (only occurs if unique_id is set)
                        objectId = aom.GetObjectId(servant);
                    }

                    orb.SetDelegate(servant);

                    return GetReference(objectId, servant._AllInterfaces(this, objectId)[0], true);
                }
            }
            throw new ServantNotActive();
        }

        public override void SetServant(Servant servant)
        {
            CheckDestructionApparent();

            if (!IsUseDefaultServant)
            {
                throw new WrongPolicy();
            }

            defaultServant = servant;

            if (defaultServant != null)
            {
                orb.SetDelegate(defaultServant);   // set the orb
            }
        }

        /// <summary>
        /// This method makes a additional check: if the POA has the RETAIN policy and servantManager is not an instance of 
        /// IServantActivator or if the POA has the NON_RETAIN policy and servantManager is not a instance of ServantLocator 
        /// this method raises also the WrongPolicy Exception (not spec.)
        /// </summary>        
        public override void SetServantManager(IServantManager servantManager)
        {
            CheckDestructionApparent();

            if (!IsUseServantManager)
            {
                throw new WrongPolicy();
            }

            if (this.servantManager != null)
            {
                throw new BadInvOrder();
            }

            //not spec
            if (IsRetain && !(servantManager is IServantActivator))
            {
                throw new WrongPolicy();
            }

            if (!IsRetain && !(servantManager is IServantLocator))
            {
                throw new WrongPolicy();
            }

            this.servantManager = servantManager;
        }

        internal void UnregisterChild(string name)
        {
            lock (poaCreationLock)
            {
                children.Remove(name);
                Monitor.PulseAll(poaCreationLock);
            }
        }

        /// <summary>
        /// If any of the policies specified are not valid, or if conflicting policies are specified, or if any of the specified policies require
        /// prior administrative action that has not been performed, or if the policy type is unknown, this method returns the index in the policy list
        /// of the first offending policy object. By the creation of a new POA object this lead to an InvalidPolicy exception being raised containing this index.
        /// If everything's fine, this method returns -1;
        /// </summary>
        private short VerifyPolicyList(IPolicy[] policies)
        {
            IPolicy policy;
            IPolicy policy2;

            for (short i = 0; i < policies.Length; i++)
            {
                switch (policies[i].PolicyType)
                {
                    case THREAD_POLICY_ID.Value:
                        /* no dependencies */
                        break;
                    case LIFESPAN_POLICY_ID.Value:
                        // PERSISTENT -> ImplName is set
                        if (((LifespanPolicy)policies[i]).Value == LifespanPolicyValue.PERSISTENT)
                        {
                            if (implName == null)
                            {
                                logger.Error("Cannot create a persistent poa. The implname property has not been set.");
                                return i;
                            }
                        }
                        /* no dependencies */
                        break;
                    case ID_UNIQUENESS_POLICY_ID.Value:
                        /* no dependencies */
                        /* if you set NON_RETAIN the poa doesn't take any notice of the IdUniquenesPolicyValue */
                        break;
                    case ID_ASSIGNMENT_POLICY_ID.Value:
                        // SYSTEM_ID   ->   no   dependencies   USER_ID   ->
                        // NO_IMPLICIT_ACTIVATION, but  an error will detected
                        //  if  we  have  considered  the  IMPLICIT_ACTIVATION
                        // policy
                        break;

                    case SERVANT_RETENTION_POLICY_ID.Value:
                        // RETAIN -> no dependencies
                        // NON_RETAIN -> (USE_DEFAULT_SERVANT || USE_SERVANT_MANAGER)
                        if (((ServantRetentionPolicy)policies[i]).Value == ServantRetentionPolicyValue.NON_RETAIN)
                        {
                            policy = POAUtil.GetPolicy(policies, REQUEST_PROCESSING_POLICY_ID.Value);
                            if (policy == null)
                            {
                                return i; // default (USE_ACTIVE_OBJECT_MAP_ONLY) is forbidden
                            }

                            if (((RequestProcessingPolicy)policy).Value != RequestProcessingPolicyValue.USE_DEFAULT_SERVANT &&
                                ((RequestProcessingPolicy)policy).Value != RequestProcessingPolicyValue.USE_SERVANT_MANAGER)
                            {
                                return i;
                            }
                            // NON_RETAIN -> NO_IMPLICIT_ACTIVATION, but an error will
                            // be detected if we have considered the IMPLICIT_ACTIVATION policy
                        }
                        break;

                    case REQUEST_PROCESSING_POLICY_ID.Value:
                        // USE_SERVANT_MANAGER -> no dependencies
                        // USE_ACTIVE_OBJECT_MAP_ONLY -> RETAIN
                        if (((RequestProcessingPolicy)policies[i]).Value == RequestProcessingPolicyValue.USE_ACTIVE_OBJECT_MAP_ONLY)
                        {
                            policy = POAUtil.GetPolicy(policies, SERVANT_RETENTION_POLICY_ID.Value);
                            if (policy != null)
                            {
                                if (((ServantRetentionPolicy)policy).Value != ServantRetentionPolicyValue.RETAIN)
                                {
                                    return i;
                                }
                            }
                            // else: do nothing, because default (RETAIN) is ok
                            // USE_DEFAULT_SERVANT -> (MULTIPLE_ID || NON_RETAIN)  /* not spec. (NON_RETAIN) */
                        }
                        else if (((RequestProcessingPolicy)policies[i]).Value == RequestProcessingPolicyValue.USE_DEFAULT_SERVANT)
                        {
                            policy = POAUtil.GetPolicy(policies, ID_UNIQUENESS_POLICY_ID.Value);
                            policy2 = POAUtil.GetPolicy(policies, SERVANT_RETENTION_POLICY_ID.Value);
                            if (policy == null && policy2 == null)
                            {
                                return i; // default (UNIQUE_ID && RETAIN) is forbidden
                            }
                            else if (policy != null && policy2 == null)
                            {
                                if (((IdUniquenessPolicy)policy).Value != IdUniquenessPolicyValue.MULTIPLE_ID)
                                {
                                    return i;
                                }
                            }
                            else if (policy == null && policy2 != null)
                            {
                                if (((ServantRetentionPolicy)policy2).Value != ServantRetentionPolicyValue.NON_RETAIN)
                                {
                                    return i;
                                }
                            }
                            else if (policy != null && policy2 != null)
                            {
                                if (((IdUniquenessPolicy)policy).Value != IdUniquenessPolicyValue.MULTIPLE_ID &&
                                    ((ServantRetentionPolicy)policy2).Value != ServantRetentionPolicyValue.NON_RETAIN)
                                {
                                    return i;
                                }
                            }
                        }
                        break;

                    case IMPLICIT_ACTIVATION_POLICY_ID.Value:
                        {
                            // NO_IMPLICIT_ACTIVATION -> no dependencies
                            // IMPLICIT_ACTIVATION -> (SYSTEM_ID && RETAIN)
                            if (((ImplicitActivationPolicy)policies[i]).Value == ImplicitActivationPolicyValue.IMPLICIT_ACTIVATION)
                            {
                                policy = POAUtil.GetPolicy(policies, SERVANT_RETENTION_POLICY_ID.Value);
                                if (policy != null)
                                {
                                    if (((ServantRetentionPolicy)policy).Value != ServantRetentionPolicyValue.RETAIN)
                                    {
                                        return i;
                                    }
                                }
                                // else: do nothing, because default (RETAIN) is ok

                                policy = POAUtil.GetPolicy(policies, ID_ASSIGNMENT_POLICY_ID.Value);
                                if (policy != null)
                                {
                                    if (((IdAssignmentPolicy)policy).Value != IdAssignmentPolicyValue.SYSTEM_ID)
                                    {
                                        return i;
                                    }
                                }
                                // else: do nothing, because default (SYSTEM_ID) is ok
                            }
                            break;
                        }
                    case BIDIRECTIONAL_POLICY_TYPE.Value:
                        // nothing to do
                        break;
                        //ignore unknown policies

                        //              // unknown policy type -> return i
                        //          default :
                        //              return i;
                }
            }
            return -1;
        }

        public IPolicy GetPolicy(uint type)
        {
            return allPolicies[type];
        }

        public void AddLocalRequest()
        {
            requestController.AddLocalRequest();
        }

        public void RemoveLocalRequest()
        {
            requestController.RemoveLocalRequest();
        }

        public int GetNumberOfObjects()
        {
            return aom.Size();
        }
    }

}
