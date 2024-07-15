// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using ETF;
using GIOP;
using IOP;
using Messaging;
using RTCORBA;
using static PortableServer.POA;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.GIOP;
using DotNetOrb.Core.POA.Util;
using DotNetOrb.Core.DII;
using DotNetOrb.Core.CDR;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace DotNetOrb.Core
{
    public class Delegate : CORBA.IDelegate
    {
        private object bindSync = new object();

        private ParsedIOR pior = null;

        private IOR ior;

        private GIOPChannelHandler currentChannel;

        private bool isBound = false;
        private POA.POA poa;

        private ORB orb;

        private ILogger logger;

        //set after the first attempt to determine whether this reference is to a local object
        private bool resolvedLocality = false;

        private bool locateOnBindPerformed = false;

        private ConnectionManager connectionManager;

        private Dictionary<uint, IPolicy> policyOverrides = new Dictionary<uint, IPolicy>();

        private IConfiguration configuration;

        private bool locateOnBind;

        private int maxBuiltinRetries = 0;

        private int defaultGiopMinor;

        private SyncScope defaultSyncScope;

        /// <summary>
        /// specify if this Delegate should drop its connection to the remote ORB after a non-recoverable 
        /// SystemException occured.non-recoverable SystemException couldn't be handled by rebind.
        /// </summary>
        private bool disconnectAfterNonRecoverableSystemException;

        /// <summary>
        /// Always attempt to locate is_a information locally rather than remotely.
        /// </summary>
        private bool avoidIsARemoteCall = true;

        protected CORBA.Object forwardReference;

        private Delegate(ORB orb, IConfiguration config)
        {
            this.orb = orb;
            configuration = config;

            connectionManager = orb.ConnectionManager;

            logger = config.GetLogger(GetType());

            locateOnBind = config.GetAsBoolean("DotNetOrb.LocateOnBind", false);
            avoidIsARemoteCall = config.GetAsBoolean("DotNetOrb.AvoidIsARemoteCall", true);

            try
            {
                maxBuiltinRetries = config.GetAsInteger("DotNetOrb.MaxBuiltinRetries", 0);
                if (maxBuiltinRetries < 0)
                {
                    logger.Error("Configuration error - max builtin retries < 0");
                    throw new CORBA.Internal("Configuration error - max builtin retries < 0");
                }
            }
            catch (ConfigException ex)
            {
                logger.Error("Configuration exception retrieving max builtin retries", ex);
                throw new CORBA.Internal("Configuration exception retrieving max builtin retries" + ex);
            }
            disconnectAfterNonRecoverableSystemException = config.GetAsBoolean("DotNetOrb.Connection.Client.DisconnectAfterSystemexception", true);

            try
            {
                defaultGiopMinor = configuration.GetAsInteger("DotNetOrb.Giop.MinorVersion", 2);
            }
            catch (ConfigException ex)
            {
                logger.Error("Configuration exception retrieving giop minor version", ex);
                throw new CORBA.Internal("Configuration exception retrieving giop minor version" + ex);
            }

            try
            {
                defaultSyncScope = (SyncScope)Enum.Parse(typeof(SyncScope), config.GetValue("DotNetOrb.DefaultSyncScope", "TRANSPORT"));
            }
            catch (ConfigException ex)
            {
                logger.Error("Configuration exception retrieving default sync scope ", ex);
                throw new CORBA.Internal("Configuration exception retrieving default sync scope " + ex);
            }
        }

        private Delegate(ORB orb) : this(orb, orb.Configuration)
        {
        }

        public Delegate(ORB orb, ParsedIOR pior) : this(orb)
        {
            this.pior = pior;
        }

        public Delegate(ORB orb, IOR ior, bool parseIORLazy) : this(orb)
        {
            this.ior = ior;

            if (parseIORLazy)
            {
                // postpone parsing of IOR.
                // see getParsedIOR
            }
            else
            {
                GetParsedIOR();
            }
        }

        public Delegate(ORB orb, IOR ior) : this(orb, ior, false)
        {
        }

        private void Bind()
        {
            lock (bindSync)
            {
                if (isBound)
                {
                    return;
                }

                ParsedIOR ior = GetParsedIOR();
                ior.PatchSSL();

                // Check if ClientProtocolPolicy set, if so, set profile
                // selector for IOR that selects effective profile for protocol
                Protocol[] protocols = GetClientProtocols();
                if (protocols != null && !ior.IsProfileSelectorSet)
                {
                    ior.SetProfileSelector(new SpecificProfileSelector(protocols));
                }

                IProfile profile = ior.GetEffectiveProfile();

                if (profile == null)
                {
                    throw new CommFailure("No effective profile");
                }

                //MIOP
                //if (profile is MIOPProfile)
                //{
                //    connections[TransportType.MIOP.ordinal()] = conn_mg.getConnection(profile);
                //    profile = ((MIOPProfile)profile).getGroupIIOPProfile();
                //}

                if (profile != null)
                {
                    currentChannel = connectionManager.GetChannelHandler(profile);
                }

                isBound = true;

                /* The delegate could query the server for the object
                 *  location using a GIOP locate request to make sure the
                 *  first call will get through without redirections
                 *  (provided the server's answer is definite):
                 */
                if (locateOnBind && !locateOnBindPerformed)
                {
                    //only locate once, because bind is called from the
                    //switch statement below again.
                    locateOnBindPerformed = true;

                    try
                    {
                        var os = new LocateRequestOutputStream(orb, currentChannel, new TargetAddress() { ObjectKey = ior.GetObjectKey() }, ior.GetEffectiveProfile().Version.Minor);
                        var locateTask = currentChannel.SendLocateRequestAsync(os);

                        GetParsedIOR().MarkLastUsedProfile();

                        var locateReply = locateTask.Result as LocateReplyInputStream;

                        switch (locateReply.LocateStatus)
                        {

                            case LocateStatusType12.UNKNOWN_OBJECT:
                                {
                                    throw new Unknown("Could not bind to object, server does not know it!");
                                }
                            case LocateStatusType12.OBJECT_HERE:
                                {
                                    break;
                                }
                            case LocateStatusType12.OBJECT_FORWARD:
                            case LocateStatusType12.OBJECT_FORWARD_PERM:
                                {
                                    forwardReference = (CORBA.Object)locateReply.ReadObject();
                                    break;
                                }

                            case LocateStatusType12.LOC_SYSTEM_EXCEPTION:
                                {
                                    throw SystemExceptionHelper.Read(locateReply);
                                }

                            case LocateStatusType12.LOC_NEEDS_ADDRESSING_MODE:
                                {
                                    throw new NoImplement("Server responded to LocateRequest with a status of LOC_NEEDS_ADDRESSING_MODE, but this isn't yet implemented");
                                }

                            default:
                                {
                                    throw new ArgumentException("Unknown reply status for LOCATE_REQUEST: " + locateReply.LocateStatus);
                                }
                        }
                    }
                    catch (CORBA.SystemException se)
                    {
                        //rethrow
                        throw se;
                    }
                    catch (AggregateException e)
                    {
                        if (e.InnerException is CORBA.SystemException)
                        {
                            throw e.InnerException;
                        }
                    }
                    catch (Exception e)
                    {
                        if (logger.IsWarnEnabled)
                        {
                            logger.Warn(e.ToString());
                        }
                    }
                }
            }
        }


        private void Rebind()
        {
            RebindWithProto(null);
        }

        private void RebindWithProto(Protocol[] protocols)
        {
            lock (bindSync)
            {
                ParsedIOR pior = GetParsedIOR();
                // Check if ClientProtocolPolicy set, if so, set profile
                // selector for IOR that selects effective profile for protocol.
                //

                // If rebind has been passed in a new set of protocols use that.
                if (protocols != null)
                {
                    pior.SetProfileSelector(new SpecificProfileSelector(protocols));
                }
                else
                {
                    Protocol[] thisProtocols = GetClientProtocols();

                    if (thisProtocols != null && !pior.IsProfileSelectorSet)
                    {
                        pior.SetProfileSelector(new SpecificProfileSelector(thisProtocols));
                    }
                }

                if (logger.IsDebugEnabled)
                    logger.Debug("Delegate.rebind, releasing connection");

                if (currentChannel.Connection is ClientConnection clientConnection)
                {
                    connectionManager.ReleaseConnection(clientConnection);
                }
                currentChannel = null;

                //to tell bind() that it has to take action
                isBound = false;

                Bind();
            }
        }

        /// <summary>
        /// Call WworkPending as that does a simple boolean check to establish if the ORB has been shutdown
        /// otherwise it throws BAD_INV_ORDER.
        /// </summary>
        private void CheckORB()
        {
            orb.WorkPending();
        }

        public override string ToString()
        {
            lock (bindSync)
            {
                return GetParsedIOR().GetIORString();
            }
        }
        public string ToString(CORBA.Object self)
        {
            return ToString();
        }

        public string GetIDString()
        {
            return GetParsedIOR().GetIDString();
        }

        public override int GetHashCode()
        {
            return GetIDString().GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is CORBA.Object && ToString().Equals(obj.ToString());
        }

        public bool Equals(CORBA.Object self, object obj)
        {
            return Equals(obj);
        }

        public IRequest CreateRequest(CORBA.Object self, IContext ctx, string operation, NVList argList, NamedValue result)
        {
            CheckORB();
            if (forwardReference != null)
            {
                return forwardReference._Delegate.CreateRequest(self, ctx, operation, argList, result);
            }
            Bind();
            return new Request(self, orb, currentChannel, GetParsedIOR().GetObjectKey(), operation, argList, ctx, result);
        }

        public IRequest CreateRequest(CORBA.Object self, IContext ctx, string operation, NVList argList, NamedValue result, ExceptionList exclist, ContextList ctxlist)
        {
            CheckORB();
            if (forwardReference != null)
            {
                return forwardReference._Delegate.CreateRequest(self, ctx, operation, argList, result, exclist, ctxlist);
            }
            Bind();
            return new Request(self, orb, currentChannel, GetParsedIOR().GetObjectKey(), operation, argList, ctx, result, exclist, ctxlist);
        }

        public CORBA.Object Duplicate(CORBA.Object self)
        {
            return orb.GetDelegate(new ParsedIOR(orb, ToString()));
        }

        /// <summary>
        /// The GetPolicy operation returns the policy object of the specified type, which applies to this object. 
        /// It returns the effective Policy for the object reference.The effective Policy is the one that would be used 
        /// if a request were made. This Policy is determined first by obtaining the effective override for the PolicyType 
        /// as returned by GetClientPolicy. The effective override is then compared with the Policy as specified in the IOR.     
        /// The effective Policy is determined by reconciling the effective override and the IOR-specified Policy.
        /// If the two policies cannot be reconciled, the standard system exception INV_POLICY is raised with standard minor code 1. 
        /// The absence of a Policy value in the IOR implies that any legal value may be used.
        /// </summary>
        public IPolicy GetPolicy(CORBA.Object self, uint policyType)
        {
            var result = GetClientPolicy(self, policyType);
            if (result != null)
            {
                // TODO: "reconcile" with server-side policy
                return result;
            }
            // if not locally overridden, ask the server
            return GetPolicy(self, policyType, Request(self, "_get_policy", true));
        }

        /// <summary>
        ///  Gets the effective overriding policy with the given type from the client-side, or null if this policy type is unset.
        /// </summary>        
        public IPolicy GetClientPolicy(CORBA.Object self, uint policyType)
        {
            return GetClientPolicy(policyType);
        }

        private IPolicy GetClientPolicy(uint policyType)
        {
            IPolicy result = null;

            lock (policyOverrides)
            {
                policyOverrides.TryGetValue(policyType, out result);
            }

            if (result == null)
            {
                // no override at the object level for this type, now
                // check at the thread level, ie PolicyCurrent.
                // TODO: currently not implemented

                // check at the ORB-level
                IPolicyManager policyManager = orb.PolicyManager;
                if (policyManager != null)
                {
                    IPolicy[] orbPolicies = policyManager.GetPolicyOverrides(new uint[] { policyType });
                    if (orbPolicies != null && orbPolicies.Length == 1)
                    {
                        result = orbPolicies[0];
                    }
                }
            }

            return result;
        }


        public IPolicy GetPolicy(CORBA.Object self, uint policyType, IOutputStream outputStream)
        {
            // ask object implementation
            while (true)
            {
                try
                {
                    outputStream.WriteObject(self);
                    outputStream.WriteULong(policyType);
                    var inputStream = Invoke(self, outputStream);
                    return PolicyHelper.Narrow(inputStream.ReadObject());
                }
                catch (RemarshalException r)
                {
                    // Ignored
                }
                catch (CORBA.ApplicationException _ax)
                {
                    throw new CORBA.Internal("Unexpected exception " + _ax.Id);
                }
            }
        }

        public IPolicy[] GetPolicyOverrides(CORBA.Object self, uint[] types)
        {
            var result = new List<IPolicy>();
            lock (policyOverrides)
            {
                foreach (var id in types)
                {
                    if (policyOverrides.ContainsKey(id))
                    {
                        result.Add(policyOverrides[id]);
                    }
                }
            }
            return result.ToArray();
        }

        public CORBA.Object SetPolicyOverrides(CORBA.Object self, List<IPolicy> policies, SetOverrideType setAdd)
        {
            // According to CORBA 3, 4.3.9.1 this should return a new Object with
            // the policies applied to that.
            CORBA.Object result = Duplicate(self);
            Delegate delResult = (Delegate)result._Delegate;

            lock (policyOverrides)
            {
                if (setAdd == SetOverrideType.ADD_OVERRIDE)
                {
                    // Need to add the overrides within this object to the new one.
                    foreach (var kvp in policyOverrides)
                    {
                        delResult.policyOverrides.Add(kvp.Key, kvp.Value);
                    }
                }

                foreach (var policy in policies)
                {
                    delResult.policyOverrides.Add(policy.PolicyType, policy);
                }
            }
            return result;
        }

        public bool ValidateConnection(CORBA.Object self, out List<IPolicy> inconsistentPolicies)
        {
            inconsistentPolicies = new List<IPolicy>();
            if (NonExistent(self))
            {
                return false;
            }
            else
            {
                lock (bindSync)
                {
                    try
                    {
                        Bind();
                    }
                    catch (InvalidPolicy e)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public short GetSyncScope()
        {
            IPolicy policy = GetClientPolicy(SYNC_SCOPE_POLICY_TYPE.Value);
            if (policy != null)
            {
                return ((ISyncScopePolicy)policy).Synchronization;
            }
            return (short)defaultSyncScope;
        }

        public Protocol[] GetClientProtocols()
        {
            var policy = GetClientPolicy(CLIENT_PROTOCOL_POLICY_TYPE.Value);
            if (policy != null)
            {
                return ((IClientProtocolPolicy)policy).Protocols;
            }
            return null;
        }

        public CORBA.Object GetComponent(CORBA.Object self)
        {
            IInputStream inputStream = InvokeBuiltin(self, "_get_component", null);
            return inputStream == null ? null : (CORBA.Object)inputStream.ReadObject();
        }

        public IDomainManager[] GetDomainManagers(CORBA.Object self)
        {
            throw new NotImplementedException();
        }

        public IInterfaceDef GetInterface(CORBA.Object self)
        {
            return InterfaceDefHelper.Narrow(GetInterfaceDef(self));
        }

        public CORBA.Object GetInterfaceDef(CORBA.Object self)
        {
            CheckORB();
            var inputStream = InvokeBuiltin(self, "_interface", null);
            return (CORBA.Object)(inputStream == null ? null : inputStream.ReadObject());
        }

        //GIOPConnection GetConnection()
        //{
        //    lock(bindSync)
        //    {
        //        Bind();
        //        return currentConnection;
        //    }
        //}

        public IOR GetIOR()
        {
            lock (bindSync)
            {
                return pior.IOR;
            }
        }

        public byte[] GetObjectId()
        {
            lock (bindSync)
            {
                Bind();

                return POAUtil.ExtractOID(pior.GetObjectKey());
            }
        }

        public byte[] GetObjectKey()
        {
            lock (bindSync)
            {
                Bind();
                return GetParsedIOR().GetObjectKey();
            }
        }

        public ParsedIOR GetParsedIOR()
        {
            lock (bindSync)
            {
                // If the _pior has not been initialised due to the lazy
                // initialisation use the ior to create one.
                if (pior == null)
                {
                    if (ior == null)
                    {
                        // should never happen due to the checks in configure
                        throw new CORBA.Internal("Internal error - unable to initialise ParsedIOR as IOR is null");
                    }
                    else
                    {
                        pior = new ParsedIOR(orb, ior);
                        ior = null;
                    }
                }
                return pior;
            }
        }

        public void ResolvePOA(CORBA.Object self)
        {
            if (!resolvedLocality)
            {
                resolvedLocality = true;
                POA.POA local_poa = orb.FindPOA(this, self);

                if (local_poa != null)
                {
                    poa = local_poa;
                }
            }
        }

        public POA.POA GetPOA()
        {
            return poa;
        }

        public CORBA.Object GetReference(POA.POA _poa)
        {
            if (_poa != null)
            {
                poa = _poa;
            }
            var typeId = ior == null ? TypeId() : ior.TypeId;
            var reference = new CORBA.Object(typeId);
            reference._Delegate = this;
            return reference;
        }

        public CORBA.ORB GetOrb(CORBA.Object self)
        {
            return orb;
        }

        public int Hash(CORBA.Object self, int max)
        {
            return GetHashCode();
        }

        public int HashCode(CORBA.Object self)
        {
            return GetHashCode();
        }

        public IInputStream Invoke(CORBA.Object self, IOutputStream os)
        {
            CheckORB();

            var ros = os as RequestOutputStream;
            if (forwardReference != null)
            {
                try
                {
                    return forwardReference._Delegate.Invoke(self, ros);
                }
                // An Object Not Exist on a forwarded reference should retry
                // on the original to get forwarded again to the right server
                catch (ObjectNotExist ne)
                {
                    if (ne.Minor != 0)
                    {
                        forwardReference = null;
                        throw new RemarshalException();
                    }
                }
            }
            GIOPChannelHandler channelToUse = null;
            var isLocal = IsLocal(self);
            try
            {
                if (isLocal)
                {
                    channelToUse = connectionManager.GetLocalChannelHandler();
                }
                else
                {
                    lock (bindSync)
                    {
                        if (!isBound)
                        {
                            // Somehow the connection got closed under us
                            throw new CommFailure("Connection closed");
                        }
                        else if (ros.ChannelHandler == currentChannel)
                        {
                            // RequestOutputStream has been created for exactly this connection
                            channelToUse = currentChannel;
                            if (channelToUse.Connection is ClientConnection clientConnection)
                            {
                                clientConnection.IncClients();
                            }
                        }
                        else
                        {
                            logger.Debug("Invoke: RemarshalException");
                            // RequestOutputStream has been created for
                            // another connection, so try again
                            throw new RemarshalException();
                        }
                    }
                }
                if (!ros.ResponseExpected)  // oneway op
                {
                    channelToUse.SendRequestAsync(ros);
                    return new CDRInputStream(new byte[0]);
                }
                else
                {
                    var task = channelToUse.SendRequestAsync(ros);
                    GetParsedIOR().MarkLastUsedProfile();
                    try
                    {
                        var result = task.Result;
                        return result;
                    }
                    catch (AggregateException ex)
                    {
                        throw ex.InnerException;
                    }
                }
            }
            catch (Transient ex)
            {
                if (!isLocal && CheckNextProfile())
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("Delegate.Invoke: looping on " + ex);
                    }
                    throw new RemarshalException();
                }
                else
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("Delegate.Invoke: Not looping on " + ex);
                    }
                    throw ex;
                }
            }
            catch (CORBA.Timeout ex)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Delegate.Invoke: Closing connection due to " + ex);
                }
                if (!isLocal && channelToUse.Connection is ClientConnection cc)
                {
                    Disconnect(cc);
                }
                throw ex;
            }
            catch (CORBA.SystemException ex)
            {
                logger.Debug("Delegate.Invoke: SystemException", ex);
                if (!isLocal && channelToUse.Connection is ClientConnection cc)
                {
                    Disconnect(cc);
                }
                throw ex;
            }
            catch (PortableServer.ForwardRequest ex)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Received forward request to " + ex.ForwardReference);
                }
                forwardReference = (CORBA.Object?)ex.ForwardReference;
                throw new RemarshalException();
            }
            finally
            {
                if (channelToUse != null && channelToUse.Connection is ClientConnection clientConnection)
                {
                    connectionManager.ReleaseConnection(clientConnection);
                }
            }
            return null;
        }

        public Task<IInputStream> InvokeAsync(CORBA.Object self, IOutputStream os)
        {
            CheckORB();

            var ros = os as RequestOutputStream;
            if (forwardReference != null)
            {
                try
                {
                    return forwardReference._Delegate.InvokeAsync(self, ros);
                }
                // An Object Not Exist on a forwarded reference should retry
                // on the original to get forwarded again to the right server
                catch (ObjectNotExist ne)
                {
                    if (ne.Minor != 0)
                    {
                        forwardReference = null;
                        throw new RemarshalException();
                    }
                }
            }
            GIOPChannelHandler channelToUse = null;
            var isLocal = IsLocal(self);
            if (isLocal)
            {
                channelToUse = connectionManager.GetLocalChannelHandler();
            }
            else
            {
                lock (bindSync)
                {
                    if (!isBound)
                    {
                        // Somehow the connection got closed under us
                        throw new CommFailure("Connection closed");
                    }
                    else if (ros.ChannelHandler == currentChannel)
                    {
                        // RequestOutputStream has been created for exactly this connection
                        channelToUse = currentChannel;
                        if (channelToUse.Connection is ClientConnection clientConnection)
                        {
                            clientConnection.IncClients();
                        }
                    }
                    else
                    {
                        logger.Debug("Invoke: RemarshalException");
                        // RequestOutputStream has been created for
                        // another connection, so try again
                        throw new RemarshalException();
                    }
                }
            }
            var task = channelToUse.SendRequestAsync(ros).ContinueWith(t =>
            {
                if (channelToUse != null && channelToUse.Connection is ClientConnection clientConnection)
                {
                    connectionManager.ReleaseConnection(clientConnection);
                }
                if (t.IsCompleted && !t.IsFaulted)
                {
                    return t.Result;
                }
                else if (t.Exception != null)
                {
                    var ex = t.Exception.InnerException;
                    if (ex is Transient)
                    {
                        if (!isLocal && CheckNextProfile())
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.Debug("Delegate.Invoke: looping on " + ex);
                            }
                            throw new RemarshalException();
                        }
                        else
                        {
                            if (logger.IsDebugEnabled)
                            {
                                logger.Debug("Delegate.Invoke: Not looping on " + ex);
                            }
                            throw ex;
                        }
                    }
                    else if (ex is CORBA.Timeout)
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("Delegate.Invoke: Closing connection due to " + ex);
                        }
                        if (!isLocal && channelToUse?.Connection is ClientConnection cc)
                        {
                            Disconnect(cc);
                        }
                        throw ex;
                    }
                    else if (ex is CORBA.SystemException)
                    {
                        logger.Debug("Delegate.Invoke: SystemException", ex);
                        if (!isLocal && channelToUse?.Connection is ClientConnection cc)
                        {
                            Disconnect(cc);
                        }
                        throw ex;
                    }
                    else if (ex is PortableServer.ForwardRequest fre)
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("Received forward request to " + fre.ForwardReference);
                        }
                        forwardReference = (CORBA.Object?)fre.ForwardReference;
                        throw new RemarshalException();
                    }
                    else if (ex is CORBA.ApplicationException aex)
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("Received CORBA Application exception " + aex.Id);
                        }
                        throw aex;
                    }
                }
                return t.Result;
            });
            GetParsedIOR().MarkLastUsedProfile();
            return task;
        }

        private void Disconnect(ClientConnection connectionInUse)
        {
            if (connectionInUse == null)
            {
                return;
            }

            if (!disconnectAfterNonRecoverableSystemException)
            {
                return;
            }

            lock (bindSync)
            {
                if (currentChannel == null)
                {
                    return;
                }

                if (currentChannel != connectionInUse.ChannelHandler)
                {
                    return;
                }

                logger.Debug("Releasing the connection...");

                if (currentChannel.Connection is ClientConnection clientConnection)
                {
                    connectionManager.ReleaseConnection(clientConnection);
                }
                currentChannel = null;
                isBound = false;
            }
        }

        private bool CheckNextProfile()
        {
            bool doRebind = false;
            lock (bindSync)
            {
                if (GetParsedIOR().Profiles.Count > 1)
                {
                    IProfile curProfile = GetParsedIOR().GetNextEffectiveProfile();
                    if (logger.IsDebugEnabled)
                    {
                        IProfile lastProfile = GetParsedIOR().GetLastUsedProfile();
                        logger.Debug("checkNextProfile: new = " + curProfile + " last = " + lastProfile);
                    }

                    if (curProfile != null &&
                        !curProfile.Equals(GetParsedIOR().GetLastUsedProfile()))
                    {
                        doRebind = true;
                    }
                }
                if (doRebind)
                {
                    Rebind();
                }
                return doRebind;
            }
        }

        private IInputStream InvokeBuiltin(CORBA.Object self, string op, string arg)
        {
            IOutputStream os;
            // The old behavior was to loop forever, which will happen here if maxBuiltinRetries is 0.
            for (int retries = 0; maxBuiltinRetries == 0 || retries < maxBuiltinRetries; retries++)
            {
                try
                {
                    os = Request(self, op, true);
                    if (arg != null)
                        os.WriteString(arg);
                    return Invoke(self, os);
                }
                catch (RemarshalException re)
                {
                    // Ignored
                }
                catch (CORBA.ApplicationException ax)
                {
                    throw new CORBA.Internal("Unexpected exception " + ax.Id);
                }
            }
            return null;
        }

        public string RepositoryId(CORBA.Object self)
        {
            return GetParsedIOR().GetTypeId();
        }

        /// <summary>
        /// Determines whether the object denoted by self has type repositoryId or a subtype of it
        /// </summary>        
        public bool IsA(CORBA.Object self, string repositoryId)
        {
            /* First, try to find out without a remote invocation. */

            /* check most derived type as defined in the IOR first
             * (this type might otherwise not be found if the helper
             * is consulted and the reference was not narrowed to
             * the most derived type. In this case, the ids returned by
             * the helper won't contain the most derived type
             */

            if (ior != null && ior.TypeId.Equals(repositoryId))
            {
                return true;
            }

            ParsedIOR pior = GetParsedIOR();

            if (pior.GetTypeId().Equals(repositoryId))
            {
                return true;
            }

            /*   The Ids in Object will at least contain the type id
                 found in the object reference itself.
            */
            string[] ids = self._Ids();

            /* the last id will be CORBA.Object, and we know that already... */
            for (int i = 0; i < ids.Length - 1; i++)
            {
                if (ids[i].Equals(repositoryId))
                {
                    return true;
                }
            }

            /* ok, we could not affirm by simply looking at the locally available
               type ids, so ask the object itself */

            IInputStream inputStream = InvokeBuiltin(self, "_is_a", repositoryId);
            return inputStream == null ? false : inputStream.ReadBoolean();
        }


        public bool IsEquivalent(CORBA.Object self, CORBA.Object rhs)
        {
            throw new NotImplementedException();
        }

        public bool IsLocal(CORBA.Object self)
        {
            if (poa == null)
            {
                ResolvePOA(self);
            }
            return poa != null;
        }

        public bool IsNil(CORBA.Object self)
        {
            ParsedIOR pior = GetParsedIOR();

            return pior.IOR.TypeId.Equals("") && pior.IOR.Profiles.Length == 0;
        }

        public bool NonExistent(CORBA.Object self)
        {
            IInputStream inputStream = null;
            try
            {
                inputStream = InvokeBuiltin(self, "_non_existent", null);
            }
            catch (ObjectNotExist e)
            {
                return true;
            }
            return inputStream == null ? false : inputStream.ReadBoolean();
        }

        /// <summary>
        /// Called to indicate that this Delegate will no longer be used by the client.
        /// The Delegate unregisters itself from the underlying GIOPConnection.
        /// If there are no other Delegates using that connection, 
        /// it will be closed and disposed of altogether.
        /// </summary>        
        public void Release(CORBA.Object self)
        {
            lock (bindSync)
            {
                if (!isBound)
                {
                    return;
                }

                if (currentChannel != null)
                {
                    if (currentChannel.Connection is ClientConnection clientConnection)
                    {
                        connectionManager.ReleaseConnection(clientConnection);
                    }
                    currentChannel = null;
                }
                isBound = false;

                // Call using string rather than this to prevent data race warning.
                orb.Release(GetParsedIOR().GetIORString());

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Delegate released!");
                }
            }
        }

        public void ReleaseReply(CORBA.Object self, IInputStream inputStream)
        {
            if (inputStream != null)
            {
                try
                {
                    inputStream.Close();
                }
                catch (Exception io)
                {
                    // ignored
                }
            }
        }

        public IRequest Request(CORBA.Object self, string operation)
        {
            orb.PerformWork();
            if (IsLocal(self))
            {
                return new Request(self, orb, connectionManager.GetLocalChannelHandler(), GetParsedIOR().GetObjectKey(), operation);
            }
            else
            {
                lock (bindSync)
                {
                    Bind();
                    return new Request(self, orb, currentChannel, GetParsedIOR().GetObjectKey(), operation);
                }
            }

        }

        public IOutputStream Request(CORBA.Object self, string operation, bool responseExpected)
        {
            orb.PerformWork();

            ParsedIOR ior = GetParsedIOR();

            IProfile profile = ior.GetEffectiveProfile();
            byte[] objectKey = profile.GetObjectKey();

            if (IsLocal(self))
            {
                var outputStream = new RequestOutputStream(orb,
                             connectionManager.GetLocalChannelHandler(),
                             operation,
                             responseExpected,
                             new TargetAddress()
                             {
                                 ObjectKey = objectKey
                             },
                             self,
                             orb.GiopMinorVersion);

                return outputStream;
            }
            else
            {
                if (forwardReference != null)
                {
                    return forwardReference._Delegate.Request(self, operation, responseExpected);
                }
                else
                {
                    lock (bindSync)
                    {
                        if (currentChannel != null && currentChannel.Connection.IsClosed)
                        {
                            Release(self);
                        }

                        Bind();

                        // Default to the version within the EffectiveProfile
                        int giopMinor = profile.Version.Minor;

                        var outputStream = new RequestOutputStream(orb,
                                                     currentChannel,
                                                     operation,
                                                     responseExpected,
                                                     new TargetAddress()
                                                     {
                                                         ObjectKey = objectKey
                                                     },
                                                     self,
                                                     giopMinor);

                        return outputStream;
                    }
                }
            }

        }

        //public IServantObject ServantPreinvoke(CORBA.Object self, string operation, Type expectedType)
        //{
        //    throw new NotImplementedException();
        //}

        //public void ServantPostinvoke(CORBA.Object self, IServantObject servant)
        //{
        //    throw new NotImplementedException();
        //}

        public string TypeId()
        {
            lock (bindSync)
            {
                return GetParsedIOR().IOR.TypeId;
            }
        }


    }
}
