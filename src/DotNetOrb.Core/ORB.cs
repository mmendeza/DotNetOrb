// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CONV_FRAME;
using CORBA;
using CSIIOP;
using DotNetOrb.Core.CDR;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.DII;
using DotNetOrb.Core.DynAny;
using DotNetOrb.Core.ETF;
using DotNetOrb.Core.GIOP;
using DotNetOrb.Core.IIOP;
using DotNetOrb.Core.ImR;
using DotNetOrb.Core.MIOP;
using DotNetOrb.Core.POA;
using DotNetOrb.Core.POA.Exceptions;
using DotNetOrb.Core.POA.Util;
using DotNetOrb.Core.Policies;
using DotNetty.Transport.Channels.Local;
using ETF;
using IOP;
using Messaging;
using PortableInterceptor;
using PortableServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Configuration = DotNetOrb.Core.Config.Configuration;
using TypeCode = CORBA.TypeCode;

namespace DotNetOrb.Core
{
    public class ORB : CORBA.ORB, IConfigurable, IPOAListener
    {
        private static string nullIORString = "IOR:00000000000000010000000000000000";

        private static string[] services = new string[] { "RootPOA", "POACurrent", "DynAnyFactory", "PICurrent", "CodecFactory", "RTORB", };

        private ILogger logger;

        private CodeSet defaultCodeSetWChar = CodeSet.UTF16_CODESET;

        public CodeSet DefaultCodeSetWChar { get => defaultCodeSetWChar; }

        private CodeSet defaultCodeSetChar = CodeSet.ISO8859_1_CODESET; // will select from platform default;

        public CodeSet DefaultCodeSetChar { get => defaultCodeSetChar; }

        private CodeSetComponentInfo localCodeSetComponentInfo;
        public CodeSetComponentInfo LocalCodeSetComponentInfo { get => localCodeSetComponentInfo; }

        private IConfiguration configuration;
        public IConfiguration Configuration
        {
            get
            {
                if (configuration != null)
                {
                    return configuration;
                }
                else
                {
                    throw new Initialize("DotNetOrb not initialized");
                }
            }
        }

        //private IBufferManager bufferManager;
        //public IBufferManager BufferManager
        //{
        //    get
        //    {
        //        if (bufferManager != null)
        //        {
        //            return bufferManager;
        //        }
        //        else
        //        {
        //            throw new Initialize("DotNetOrb not initialized");
        //        }
        //    }
        //}

        private bool doStrictCheckOnTypecodeCreation;
        private bool cacheReferences;
        private string implName;
        public string ImplName => implName;
        private int giopMinorVersion;
        public int GiopMinorVersion
        {
            get => giopMinorVersion;
        }

        private bool giopAdd_1_0_Profiles;
        private bool useImR;
        private bool useTaoImR;
        private bool failOnORBInitializerError;
        private bool inORBInitializer;

        private ProtocolAddressBase imrProxyAddress = null;
        private ProtocolAddressBase iorProxyAddress;


        public ConcurrentDictionary<string, List<(TypeCode typeCode, int position)>> TypeCodeCache = new ConcurrentDictionary<string, List<(TypeCode typeCode, int position)>>();
        bool cacheTypeCodes = false;
        public bool CacheTypeCodes { get => cacheTypeCodes; }

        public ConcurrentDictionary<string, TypeCode> CompactedTypeCodeCache = new ConcurrentDictionary<string, TypeCode>();
        bool compactTypeCodes = false;
        public bool CompactTypeCodes { get => compactTypeCodes; }

        private Dictionary<string, CORBA.Object> initialReferences = new Dictionary<string, CORBA.Object>();

        private POA.POA rootpoa;
        private POA.Current poaCurrent;
        private BasicAdapter basicAdapter;

        private Dictionary<string, CORBA.Object> knownReferences = new Dictionary<string, CORBA.Object>();

        private ConnectionManager connectionManager;
        public ConnectionManager ConnectionManager => connectionManager;

        private TransportManager transportManager = null;

        public TransportManager TransportManager => transportManager;

        /// <summary>
        /// Maps repository ids (strings) to objects that implement IValueFactory.
        /// This map is used by Register/UnregisterValueFactory() and LookupValueFactory().
        /// </summary>
        private Dictionary<string, IValueFactory> valueFactories = new Dictionary<string, IValueFactory>();

        /// <summary>
        /// Maps repository ids (strings) of boxed value types to BoxedValueHelper instances for those types.
        /// </summary>
        private Dictionary<string, IBoxedValueHelper> boxedValueHelpers = new Dictionary<string, IBoxedValueHelper>();

        private ObjectKeyMap objectKeyMap;

        //command like args
        private string[] arguments;

        //for run() and shutdown()
        private object runSync = new object();
        private bool run = true;
        private bool IsRunning
        {
            get
            {
                lock (runSync)
                {
                    return run;
                }
            }
        }

        private bool shutdownInProgress = false;
        private bool destroyed = false;
        private object shutdownSync = new object();

        /**
         *  for registering POAs with the ImR
         */
        private IImRAccess imr = null;
        private int persistentPOACount;

        private string orbId = "DotNetOrb";
        public override string Id => orbId;

        /// <summary>
        /// Outstanding dii requests awaiting completion
        /// </summary>
        private ConcurrentDictionary<Request, byte> requests = new ConcurrentDictionary<Request, byte>();

        /// <summary>
        /// Most recently completed dii request found during poll
        /// </summary>
        private Request request = null;

        private PolicyManager policyManager;

        public PolicyManager PolicyManager => policyManager;

        /// <summary>
        /// Policy factories, from portable interceptor spec
        /// </summary>
        private ConcurrentDictionary<uint, IPolicyFactory> policyFactories = new ConcurrentDictionary<uint, IPolicyFactory>();

        public bool IsBidir = false;

        /// <summary>
        /// Unique ID that will be used to identify this server.
        /// </summary>
        private string serverIdStr;

        public string ServerIdString => serverIdStr;

        /// <summary>
        /// bytes form of serverIdStr.
        /// </summary>
        private byte[] serverId;

        public byte[] ServerId => serverId;

        private RPPoolManagerFactory poolManagerFactory;

        /// <summary>
        ///  The ORB default initial reference argument, -ORBDefaultInitRef, assists in resolution of initial references not explicitly specified with -ORBInitRef.
        ///  -ORBDefaultInitRef requires a URL that, after appending a slash ‘/’ character and a stringified object key, 
        ///  forms a new URL to identify an initial object reference.
        /// </summary>
        private string defaultInitRef;

        /// <summary>
        /// Indicates that the root POA manager was not yet activated.
        /// </summary>
        private bool firstConnection = true;

        /// <summary>
        /// Associates connected objects to their servants. The servant associated with a connected object is retrieved 
        /// from this map when disconnect is called on the object.
        /// </summary>
        private Dictionary<CORBA.Object, Servant> connectedObjects = new Dictionary<CORBA.Object, Servant>();

        private object syncObject = new object();

        public LocalAddress LocalAddress { get; private set; }

        public ORB(): this(new Dictionary<string, string>())
        {
        }

        public ORB(Dictionary<string, string> props)
        {
            var rnd = new Random();
            var buffer = new byte[sizeof(UInt64)];
            rnd.NextBytes(buffer);            
            serverIdStr = BitConverter.ToUInt64(buffer, 0).ToString();
            serverId = System.Text.Encoding.Default.GetBytes(serverIdStr);
            LocalAddress = new LocalAddress("giop.endpoint." + serverIdStr);
            objectKeyMap = new ObjectKeyMap(this);
            configuration = new Configuration(props, this);
            Configure(configuration);
            Init();
        }

        public void Configure(IConfiguration config)
        {
            configuration = config;
            if (configuration.ContainsKey("ORBid"))
            {
                orbId = configuration.GetValue("ORBid");
            }
            if (configuration.ContainsKey("ORBDefaultInitRef"))
            {
                defaultInitRef = configuration.GetValue("ORBDefaultInitRef");
            }
            logger = configuration.GetLogger(GetType().FullName);
            doStrictCheckOnTypecodeCreation = configuration.GetAsBoolean("DotNetOrb.Interop.StrictCheckOnTcCreation", true);
            //bufferManager = configuration.GetAsObject("DotNetOrb.BufferManager", "DotNetOrb.Core.Orb.BufferManager.DefaultBufferManager") as IBufferManager;
            cacheReferences = configuration.GetAsBoolean("DotNetOrb.ReferenceCaching", false);
            implName = configuration.GetValue("DotNetOrb.ImplName", "");
            giopMinorVersion = configuration.GetAsInteger("DotNetOrb.GiopMinorVersion", 2);
            giopAdd_1_0_Profiles = configuration.GetAsBoolean("DotNetOrb.Giop.Add_1_0_Profiles", false);
            //hashTableClassName = configuration.getAttribute("DotNetOrb.hashtable_class", HashMap.class.getName());
            IIOPAddress.NetworkVirtualInterfaces = config.GetAsStringArray("DotNetOrb.Network.Virtual", "VirtualBox,VMWare,VMware,Virtual,virbr0,vboxnet,docker");
            useImR = configuration.GetAsBoolean("DotNetOrb.UseImR", false);
            useTaoImR = configuration.GetAsBoolean("DotNetOrb.UseTaoImR", false);
            if (useTaoImR && useImR)
            {
                throw new ConfigException("Ambiguous ImR property settings: DotNetOrb.UseImR and DotNetOrb.UseTaoImR are both true");
            }
            string host = configuration.GetValue("DotNetOrb.ImR.IORProxyHost", null);
            int port = configuration.GetAsInteger("DotNetOrb.ImR.IORProxyPort", -1);
            string address = configuration.GetValue("DotNetOrb.ImR.IORPRoxyAddress", null);

            imrProxyAddress = CreateAddress(host, port, address);
            if (useTaoImR && imrProxyAddress != null)
            {
                // ignore IMR proxy setting when TAO ImR is used
                imrProxyAddress = null;
            }
            host = configuration.GetValue("DotNetOrb.IORProxyHost", null);
            port = configuration.GetAsInteger("DotNetOrb.IORProxyPort", -1);
            address = configuration.GetValue("DotNetOrb.IORProxyAddress", null);

            iorProxyAddress = CreateAddress(host, port, address);
            if (useTaoImR && iorProxyAddress != null)
            {
                // ignore IOR proxy setting when TAO ImR is used
                iorProxyAddress = null;
            }
            failOnORBInitializerError = configuration.GetAsBoolean("DotNetOrb.ORBInitializer.FailOnError", false);

            cacheTypeCodes = configuration.GetAsBoolean("DotNetOrb.CacheTypecodes", false);
            compactTypeCodes = configuration.GetAsBoolean("DotNetOrb.CompactTypecodes", false);
            bool printVersion = configuration.GetAsBoolean("DotNetOrb.ORB.PrintVersion", false);
            if (printVersion)
            {
                logger.Info($"{GetType().Assembly.GetName().Name} {GetType().Assembly.GetName().Version?.ToString()}");
            }

            ConfigureObjectKeyMap(configuration);

            if (poolManagerFactory != null)
            {
                // currently the ORB is only re-configured during
                // test runs. in this case an existing the poolManagerFactory
                // should be shut down properly to give its threads the
                // change to exit.

                // doesn't work this way as configure is invoked during
                // a request. causes the test to hang (alphonse)

                //poolManagerFactory.destroy();
            }

            poolManagerFactory = new RPPoolManagerFactory(this);
            ConfigureCodeset();
        }

        private void ConfigureCodeset()
        {
            string ncsc = configuration.GetValue("DotNetOrb.NativeCharCodeset", "");
            string ncsw = configuration.GetValue("DotNetOrb.NativeWCharCodeset", "");

            if (ncsc != null && !"".Equals(ncsc))
            {
                try
                {
                    CodeSet codeset = CodeSet.GetCodeSet(ncsc);
                    defaultCodeSetChar = codeset;
                }
                catch (CodeSetIncompatible)
                {

                }
            }
            // Fallback from above if we failed to set nativeCodeSetChar
            if (defaultCodeSetChar == null)
            {
                string sysenc = CodeSet.DefaultPlatformEncoding;
                for (int i = 0; i < CodeSet.KnownEncodings.Length; i++)
                {
                    CodeSet codeset = CodeSet.KnownEncodings[i];
                    if (codeset.SupportsCharacterData( /* wide */ false) && sysenc.Equals(codeset.Name))
                    {
                        defaultCodeSetChar = codeset;
                    }
                }
                if (defaultCodeSetChar == null)
                {
                    defaultCodeSetChar = CodeSet.ISO8859_1_CODESET;
                }
            }

            if (ncsw != null && !"".Equals(ncsw))
            {
                try
                {
                    CodeSet codeset = CodeSet.GetCodeSet(ncsw);
                    defaultCodeSetWChar = codeset;
                }
                catch (CodeSetIncompatible)
                {

                }
                if (defaultCodeSetWChar == null)
                {
                    defaultCodeSetWChar = CodeSet.UTF16_CODESET;
                }
            }

            localCodeSetComponentInfo = new CodeSetComponentInfo();
            localCodeSetComponentInfo.ForCharData = CodeSet.CreateCodeSetComponent( /* wide */ false, DefaultCodeSetChar);
            localCodeSetComponentInfo.ForWcharData = CodeSet.CreateCodeSetComponent( /* wide */ true, DefaultCodeSetWChar);
        }

        /// <summary>
        /// A helper method supplied to initialize the object key map. This  replaces functionality from 
        /// the defunct Environment class to populatea hash map based on the names starting with "DotNetOrb.Orb.ObjectKeyMap"
        /// </summary>
        private void ConfigureObjectKeyMap(IConfiguration config)
        {
            objectKeyMap.ConfigureObjectKeyMap(config);
        }

        /// <summary>
        /// proprietary method that allows the internal objectKeyMap to be altered programmatically.
        /// The objectKeyMap allows more readable corbaloc URLs by mapping the actual object key to 
        /// an arbitary string. 
        /// </summary>
        /// <param name="keyName">a string value e.g.NameService</param>
        /// <param name="fullPath">a string value e.g.file:/home/rnc/NameSingleton.ior</param>
        public void AddObjectKey(string keyName, string fullPath)
        {
            objectKeyMap.AddObjectKey(keyName, fullPath);
        }

        /// <summary>
        /// proprietary method that allows the internal objectKeyMap to be altered programmatically.
        /// The objectKeyMap allows more readable corbaloc URLs by mapping the actual object key to
        /// an arbitary string.         
        /// </summary>
        /// <param name="keyName">a string value e.g.NameService</param>
        /// <param name="target">the object whose object key should be used</param>
        public void AddObjectKey(string keyName, CORBA.Object target)
        {
            objectKeyMap.AddObjectKey(keyName, target);
        }

        /// <summary>
        /// Map an object key to another, as defined by the value of a corresponding configuration property 
        /// e.g.map "NameService" to "StandardNS/NameServer-POA/_root"
        /// </summary>
        /// <param name="originalKey">byte[] value containing the original key</param>
        /// <returns>a byte[] value containing the mapped key, if mapping is defined, originalKey otherwise</returns>
        public byte[] MapObjectKey(byte[] originalKey)
        {
            return objectKeyMap.MapObjectKey(originalKey);
        }

        public override CORBA.Any CreateAny()
        {
            return new Any(this);
        }


        /// <summary>
        /// Create an address based on addressString OR (host AND port) if neither addressString NOR(host AND port) are specified 
        /// this method will return null.
        /// </summary>
        private ProtocolAddressBase CreateAddress(string host, int port, string addressString)
        {
            ProtocolAddressBase address;

            try
            {
                if (addressString == null)
                {
                    if (host != null || port != -1)
                    {
                        address = new IIOPAddress(host, port);
                        address.Configure(configuration);
                        if (host != null)
                        {
                            ((IIOPAddress)address).HostName = host;
                        }
                        if (port != -1)
                        {
                            ((IIOPAddress)address).Port = port;
                        }
                    }
                    else
                    {
                        address = null;
                    }
                }
                else
                {
                    address = CreateAddress(addressString);
                }
            }
            catch (Exception ex)
            {
                logger.Error("error initializing ProxyAddress", ex);
                throw new Initialize(ex.ToString());
            }
            return address;
        }

        private ProtocolAddressBase CreateAddress(string address)
        {
            var factorylist = transportManager.GetFactoriesList();
            ProtocolAddressBase result = null;
            foreach (var factory in factorylist)
            {
                result = factory.CreateProtocolAddress(address);
                if (result != null)
                    return result;
            }
            return result;
        }

        /// <summary>
        /// This method is used for references that have arrived over the network and is called from CDRInputStream.
        /// It does not cache entries as we are delaying creating a ParsedIOR.
        /// </summary>        
        public CORBA.Object GetObject(IOR ior)
        {
            Delegate @delegate = new Delegate(this, ior, true);
            return @delegate.GetReference(null);
        }

        public CORBA.Object GetDelegate(ParsedIOR pior)
        {
            lock (syncObject)
            {
                var key = pior.GetIORString();
                CORBA.Object obj;

                if (cacheReferences && knownReferences.ContainsKey(key))
                {
                    obj = knownReferences[key];

                    if (obj != null)
                    {
                        var del = (Delegate)obj._Delegate;
                        if (del != null)
                        {
                            ParsedIOR delpior = del.GetParsedIOR();

                            if (delpior == null)
                            {
                                knownReferences.Remove(key);

                                logger.Debug("Removing an invalid reference from cache.");
                            }
                            else if (pior.GetEffectiveProfile().IsMatch(delpior.GetEffectiveProfile()))
                            {
                                return (CORBA.Object)obj._Duplicate();
                            }
                        }
                        else
                        {
                            logger.Debug("Remove stale reference from cache ");
                            knownReferences.Remove(key);
                        }
                    }
                }

                var @delegate = new Delegate(this, pior);

                obj = @delegate.GetReference(null);

                if (cacheReferences)
                {
                    knownReferences.Add(key, obj);
                }
                return obj;
            }
        }

        /// <summary>
        /// Find a local POA for a delegate (called from IsLocal()) returns non-null only if a root POA is already activated 
        /// and all POAs along the path on the poa name are active, i.e. returns null for POAs in the holding state
        /// </summary>
        internal POA.POA FindPOA(Delegate del, CORBA.Object reference)
        {
            if (rootpoa == null || basicAdapter == null)
            {
                return null;
            }

            string refImplName = null;
            byte[] delegateObjectKey = del.GetObjectKey();

            try
            {
                refImplName = POAUtil.ExtractImplName(delegateObjectKey);
            }
            catch (POAInternalException e)
            {
                logger.Debug($"FindPOA: reference generated by foreign POA {delegateObjectKey} ", e);
                return null;
            }

            if (refImplName == null)
            {
                if (implName.Length > 0 || serverIdStr.Length > 0)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("FindPOA: implName mismatch - null != " + implName);
                    }
                    return null;
                }
            }
            else
            {
                if (!implName.Equals(refImplName) && !serverIdStr.Equals(refImplName))
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("findPOA: implName mismatch - " + refImplName + " != " + implName);
                    }
                    return null;
                }
            }

            try
            {
                POA.POA tmpPoa = rootpoa;
                string poaName = POAUtil.ExtractPOAName(delegateObjectKey);

                //strip scoped poa name (first part of the object key before "::", will be empty for the root poa                
                var scopes = POAUtil.ExtractScopedPOANames(poaName);

                for (int i = 0; i < scopes.Count; i++)
                {
                    var res = scopes[i];

                    if ("".Equals(res))
                    {
                        break;
                    }

                    //the following is a  call to a method in the private interface between the ORB and the POA. It does the
                    //necessary synchronization between incoming, potentially concurrent requests to activate a POA using its 
                    //adapter activator. This call will block until the correct POA is activated and ready to service requests.
                    //Thus, concurrent calls originating from a single, multi-threaded client will be serialized because the 
                    //thread that accepts incoming requests from the client process is blocked. Concurrent calls from other destinations
                    //are not serialized unless they involve activating the same adapter.
                    try
                    {
                        tmpPoa = tmpPoa.GetChildPOA(res);
                    }
                    catch (ParentIsHoldingException p)
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("FindPOA: holding adapter");
                        }
                        return null;
                    }
                }
                byte[] objectId = POAUtil.ExtractOID(reference);

                if (tmpPoa.IsSystemId && !tmpPoa.PreviouslyGeneratedObjectId(objectId))
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("FindPOA: not a previously generated object key.");
                    }
                    return null;
                }

                return tmpPoa;
            }
            catch (Exception e)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("FindPOA exception: ", e);
                }
            }

            if (logger.IsDebugEnabled)
            {
                logger.Debug("FindPOA: nothing found");
            }
            return null;
        }
        internal void Release(string iorString)
        {
            lock (syncObject)
            {
                if (cacheReferences)
                {
                    knownReferences.Remove(iorString);
                }
            }
        }

        public override IPolicy CreatePolicy(uint type, CORBA.Any value)
        {
            switch (type)
            {
                case MAX_HOPS_POLICY_TYPE.Value:
                    return new MaxHopsPolicy(value);
                case QUEUE_ORDER_POLICY_TYPE.Value:
                    return new QueueOrderPolicy(value);
                case REBIND_POLICY_TYPE.Value:
                    return new RebindPolicy(value);
                case RELATIVE_REQ_TIMEOUT_POLICY_TYPE.Value:
                    return new RelativeRequestTimeoutPolicy(value);
                case RELATIVE_RT_TIMEOUT_POLICY_TYPE.Value:
                    return new RelativeRoundtripTimeoutPolicy(value);
                case REPLY_END_TIME_POLICY_TYPE.Value:
                    return new ReplyEndTimePolicy(value);
                case REPLY_PRIORITY_POLICY_TYPE.Value:
                    return new ReplyPriorityPolicy(value);
                case REPLY_START_TIME_POLICY_TYPE.Value:
                    return new ReplyStartTimePolicy(value);
                case REQUEST_END_TIME_POLICY_TYPE.Value:
                    return new RequestEndTimePolicy(value);
                case REQUEST_PRIORITY_POLICY_TYPE.Value:
                    return new RequestPriorityPolicy(value);
                case REQUEST_START_TIME_POLICY_TYPE.Value:
                    return new RequestStartTimePolicy(value);
                case ROUTING_POLICY_TYPE.Value:
                    return new RoutingPolicy(value);
                case SYNC_SCOPE_POLICY_TYPE.Value:
                    return new SyncScopePolicy(value);
                case RTCORBA.CLIENT_PROTOCOL_POLICY_TYPE.Value:
                    return new ClientProtocolPolicy(value);
                default:
                    var factory = policyFactories[type];

                    if (factory == null)
                    {
                        throw new PolicyError();
                    }
                    return factory.CreatePolicy(type, value);
            }
        }

        public bool HasPolicyFactoryForType(uint type)
        {
            return policyFactories.ContainsKey(type);
        }

        public override ContextList CreateContextList()
        {
            WorkPending();
            return new ContextList();
        }

        public override CORBA.Environment CreateEnvironment()
        {
            WorkPending();
            throw new NoImplement();
        }

        public override IOutputStream CreateOutputStream()
        {
            WorkPending();
            return new CDROutputStream(this);
        }

        internal IOR CreateIOR(string repId, byte[] objectKey, bool transient, POA.POA poa, Dictionary<uint, IPolicy> policyOverrides)
        {
            var profiles = new List<IProfile>();
            var componentMap = new Dictionary<uint, TaggedComponentList>();
            var endpointProfiles = basicAdapter.GetEndpointProfiles();
            int[] profileTags = new int[endpointProfiles.Count];
            int n = 0;
            foreach (var profile in endpointProfiles)
            {
                //TODO
                //MIOP
                //if it is a group profile, it can't be added to the object ior so do nothing
                if (profile is MIOPProfile)
                {
                    continue;
                }
                profile.SetObjectKey(objectKey);
                profiles.Add(profile);
                profileTags[n++] = profile.Tag;

                TaggedComponentList profileComponents = new TaggedComponentList();
                profileComponents.AddComponent(CreateOrbTypeId());
                profileComponents.AddComponent(TAG_CODE_SETS.Value, LocalCodeSetComponentInfo, typeof(CodeSetComponentInfoHelper));
                componentMap.Add((uint)profile.Tag, profileComponents);
                //add codeset                

                if (profile is ProfileBase)
                {
                    // use proxy or ImR address if necessary
                    PatchAddress((ProfileBase)profile, repId, transient);

                    // patch primary address port to 0 if SSL is required
                    if (poa.IsSSLRequired)
                    {
                        ((ProfileBase)profile).PatchPrimaryAddress(new IIOPAddress(null, 0));
                    }
                }
            }

            TaggedComponentList multipleComponents = new TaggedComponentList();
            componentMap.Add(TAG_MULTIPLE_COMPONENTS.Value, multipleComponents);

            // add GIOP 1.0 profile if necessary

            IIOPProfile iiopProfile = FindIIOPProfile(profiles);
            if (iiopProfile != null && (giopMinorVersion == 0 || giopAdd_1_0_Profiles))
            {
                IProfile profile_1_0 = iiopProfile.ToGIOP10();
                profiles.Add(profile_1_0);

                // shuffle all components over into the multiple components profile
                TaggedComponentList iiopComponents = componentMap[TAG_INTERNET_IOP.Value];

                multipleComponents.AddAll(iiopProfile.GetComponents());
                multipleComponents.AddAll(iiopComponents);

                // if we only want GIOP 1.0, remove the other profile
                if (giopMinorVersion == 0)
                {
                    profiles.Remove(iiopProfile);
                }
            }

            if (iiopProfile != null)
            {
                TaggedComponentList components = componentMap[TAG_INTERNET_IOP.Value];

                // patch primary address port to 0 if SSL is required
                if (IsSSLRequiredInComponentList(components))
                {
                    iiopProfile.PatchPrimaryAddress(new IIOPAddress(null, 0));
                }
            }

            // marshal the profiles into the IOR and return
            TaggedProfile[] tps = null;
            if (multipleComponents.IsEmpty())
            {
                tps = new TaggedProfile[profiles.Count];
            }
            else
            {
                tps = new TaggedProfile[profiles.Count + 1];
                tps[tps.Length - 1] = CreateMultipleComponentsProfile(multipleComponents);
            }

            TaggedComponent[] tc;
            TaggedProfile tp = new TaggedProfile();

            for (int i = 0; i < profiles.Count; i++)
            {
                var p = profiles[i];
                TaggedComponentList clist = componentMap[(uint)p.Tag];
                TaggedComponentList c;
                if (p is ProfileBase)
                {
                    // If ImR is used, then get rid of direct alternate addresses
                    // and patch in the ImR alternate addresses.
                    c = PatchTagAlternateIIOPAddresses((ProfileBase)p, clist, repId, transient);
                }
                else
                {
                    c = clist;
                }
                tc = c.AsArray();
                p.Marshal(ref tp, ref tc);
                tps[i] = tp;
            }
            return new IOR(repId, tps);
        }

        /// <summary>
        ///  This function will remove all TAG_ALTERNATE_IIOP_ADDRESS tags for direct addressing from the profile and the tagged component list;
        ///  and if the TAO ImR is used, it will patch in the TAG_ALTERNATE_IIOP_ADDRESS tags of the TAO ImR.
        /// </summary>
        private TaggedComponentList PatchTagAlternateIIOPAddresses(ProfileBase profile, TaggedComponentList taggedCompList, string repId, bool transient)
        {
            if ("IDL:/DotNetOrb/imr/ImplementationRepository:1.0".Equals(repId))
            {
                return taggedCompList;
            }
            else if (transient || !useImR && !useTaoImR)
            {
                return taggedCompList;
            }

            // remove all TAG_ALTERNATE_IIOP_ADDRESS tags for direct addressing
            profile.RemoveComponents(TAG_ALTERNATE_IIOP_ADDRESS.Value);
            taggedCompList.RemoveComponents(TAG_ALTERNATE_IIOP_ADDRESS.Value);

            TaggedComponentList list = new TaggedComponentList();
            list.AddAll(taggedCompList);

            // When TAO ImR is used, the alternate addresses from its profiles are added to the list of tags.
            if (useTaoImR)
            {
                GetImR();
                if (imr != null)
                {
                    var plist = imr.GetImRProfiles();
                    int cnt = 0;
                    foreach (IIOPProfile p in plist)
                    {
                        // add all IMR endpoints (except the first primary one) to the alternate endpoint list as well
                        if (cnt++ != 0)
                        {
                            try
                            {
                                var address = new IIOPAddress(((IIOPAddress)p.GetAddress()).HostName,
                                                              ((IIOPAddress)p.GetAddress()).Port);
                                address.Configure(configuration);
                                list.AddComponent(TAG_ALTERNATE_IIOP_ADDRESS.Value, address.ToCDR());
                            }
                            catch (ConfigException e)
                            {
                                logger.Warn("PatchTagAlternateIIOPAddresses: got an exception, " + e.ToString());
                            }
                        }

                        List<IIOPAddress> addrList = p.GetAlternateAddresses();
                        if (addrList == null)
                        {
                            continue;
                        }

                        foreach (IIOPAddress imrAddr in addrList)
                        {
                            if (imrAddr == null)
                            {
                                continue;
                            }

                            try
                            {
                                IIOPAddress address = new IIOPAddress(imrAddr.HostName, imrAddr.Port);
                                address.Configure(configuration);
                                list.AddComponent(TAG_ALTERNATE_IIOP_ADDRESS.Value, address.ToCDR());
                            }
                            catch (ConfigException e)
                            {
                                logger.Warn("PatchTagAlternateIIOPAddresses: got an exception, " + e.ToString());
                            }
                        }
                    }
                }
            }
            return list;
        }

        public bool IsSSLRequiredInComponentList(TaggedComponentList components)
        {
            int minimumOptions = Integrity.Value | Confidentiality.Value | DetectReplay.Value | DetectMisordering.Value;

            if (components == null)
            {
                return false;
            }

            CompoundSecMechList csmList = (CompoundSecMechList)components.GetComponent(TAG_CSI_SEC_MECH_LIST.Value, typeof(CompoundSecMechListHelper));

            if (csmList != null && csmList.MechanismList.Length > 0 && csmList.MechanismList[0].TransportMech.Tag == CSIIOP.TAG_TLS_SEC_TRANS.Value)
            {
                byte[] tlsSecTransData = csmList.MechanismList[0].TransportMech.ComponentData;
                var inputStream = new CDRInputStream(this, tlsSecTransData);
                try
                {
                    inputStream.OpenEncapsulatedArray();
                    TLS_SEC_TRANS tls = TLS_SEC_TRANSHelper.Read(inputStream);
                    return (tls.TargetRequires & minimumOptions) != 0;
                }
                catch (Exception ex)
                {
                    throw new CORBA.Internal(ex.ToString());
                }
            }
            return false;
        }

        private TaggedProfile CreateMultipleComponentsProfile(TaggedComponentList components)
        {
            CDROutputStream outputStream = new CDROutputStream(this);
            outputStream.BeginEncapsulatedArray();
            MultipleComponentProfileHelper.Write(outputStream, components.AsArray());
            return new TaggedProfile(TAG_MULTIPLE_COMPONENTS.Value, outputStream.GetBufferCopy());
        }

        /// <summary>
        /// Finds the first IIOPProfile in the given List of Profiles, and returns it.
        /// If no such profile is found, this method returns null.
        /// </summary>
        private IIOPProfile FindIIOPProfile(ICollection<IProfile> profiles)
        {
            foreach (var p in profiles)
            {
                if (p is IIOPProfile)
                {
                    return (IIOPProfile)p;
                }
            }
            return null;
        }

        public override IContext GetDefaultContext()
        {
            WorkPending();
            throw new NoImplement();
        }

        public BasicAdapter GetBasicAdapter()
        {
            if (basicAdapter == null)
            {
                throw new Initialize("Adapters not initialized; resolve RootPOA.");
            }
            return basicAdapter;
        }

        public POA.Current GetPOACurrent()
        {
            lock (syncObject)
            {
                if (poaCurrent == null)
                {
                    poaCurrent = new POA.Current();
                }
                return poaCurrent;
            }
        }

        /// <summary>
        /// Called by POA to create an IOR
        /// </summary>
        /// <param name="poa">The calling poa</param>
        /// <param name="objectKey">Object key</param>
        /// <param name="repId">Repository id</param>
        /// <param name="transient">transient or persistent</param>
        /// <returns>a new CORBA Object reference</returns>
        public CORBA.Object GetReference(POA.POA poa, byte[] objectKey, string repId, bool transient)
        {
            IOR ior = CreateIOR(repId == null ? "IDL:omg.org/CORBA/Object:1.0" : repId, objectKey, transient, poa, null);

            if (ior == null)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error("Interal error: createIOR returns null");
                }
            }

            Delegate d = new Delegate(this, ior);

            return d.GetReference(poa);
        }

        public POA.POA GetRootPOA()
        {
            lock (syncObject)
            {
                if (rootpoa == null)
                {
                    var tmppoa = POA.POA.POAInit(this);

                    basicAdapter = new BasicAdapter(this, transportManager);

                    try
                    {
                        tmppoa.Configure(configuration);
                        basicAdapter.Configure(configuration);
                    }
                    catch (ConfigException ce)
                    {
                        throw new Initialize("ConfigurationException: " + ce.ToString(), 0, CompletionStatus.No, ce);
                    }

                    rootpoa = tmppoa;
                    rootpoa.AddPOAEventListener(this);

                }
                return rootpoa;
            }
        }

        public override string[] ListInitialServices()
        {
            WorkPending();
            var list = new List<string>(initialReferences.Count + services.Length);
            list.AddRange(services);
            list.AddRange(initialReferences.Keys);
            return list.ToArray();
        }

        #region IPOAListener
        /// <summary>
        ///  An operation from the IPOAListener interface. 
        ///  Whenever a new POA is created, the ORB is notified.
        /// </summary>        
        public void PoaCreated(POA.POA poa)
        {
            /*
            * Add this orb as the child poa's event listener. This means that the
            * ORB is always a listener to all poa events!
            */
            poa.AddPOAEventListener(this);

            /* If the new POA has a persistent lifetime policy, it is registered
             * with the implementation repository if there is one and the
             * use imr policy is set via the "DotNetOrb.Orb.UseImR" property
             */
            if (poa.IsPersistent)
            {
                persistentPOACount++;
                if (useImR)
                {
                    GetImR();

                    if (imr != null)
                    {
                        /* Register the POA */
                        var serverName = implName;
                        ProtocolAddressBase sep = GetServerAddress();
                        if (sep is IIOPAddress)
                        {
                            string sepHost = ((IIOPAddress)sep).HostName;
                            int sepPort = ((IIOPAddress)sep).Port;

                            imr.RegisterPOA(serverName + "/" + poa.QualifiedName,
                                            serverName, // logical server name
                                            sepHost,
                                            sepPort);
                        }
                    }
                }
                else if (useTaoImR)
                {
                    GetImR();

                    if (imr != null)
                    {
                        /* Register the POA */
                        ProtocolAddressBase sep = GetServerAddress();
                        if (sep is IIOPAddress)
                        {
                            imr.RegisterPOA(this, poa, sep, implName);
                        }
                    }
                }
                else
                {
                    // ignore
                }
            }
        }

        public void PoaStateChanged(POA.POA poa, POAConstants.POAState state)
        {
            if ((state == POAConstants.POAState.Destroyed ||
                state == POAConstants.POAState.Inactive) &&
                poa.IsPersistent && imr != null)
            {
                //if all persistent POAs in this server have gone down, unregister the server
                if (--persistentPOACount == 0)
                {
                    if (useImR)
                    {
                        imr.SetServerDown(implName);
                    }
                    else if (useTaoImR)
                    {
                        imr.SetServerDown(this, poa, implName);
                    }
                }
            }
        }

        public void ReferenceCreated(CORBA.Object obj)
        {
        }
        #endregion

        private void GetImR()
        {
            lock (syncObject)
            {
                //Lookup the implementation repository
                if (imr == null && (useImR || useTaoImR))
                {
                    try
                    {                        
                        if (useImR)
                        {
                            imr = ImRAccess.Connect(this);
                        }
                        else if (useTaoImR)
                        {
                            imr = ImR.TAO.ImRAccess.Connect(this);
                        }
                    }
                    catch (Exception e)
                    {
                        // If we failed to resolve the IMR set the reference to null.
                        if (logger.IsWarnEnabled)
                        {
                            logger.Warn("Error: No connection to ImplementationRepository");
                        }
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug(e.ToString());
                        }

                        if (e is CORBA.Internal)
                        {
                            throw new ObjAdapter("Unable to resolve ImR");
                        }
                        else if (e is Transient)
                        {
                            throw (Transient)e;
                        }
                        else
                        {
                            throw new ObjAdapter(e.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Replace the server address in profile with a proxy address if necessary.
        /// </summary>        
        private void PatchAddress(ProfileBase profile, string repId, bool transient)
        {
            if ("IDL:DotNetOrb/imr/ImplementationRepository:1.0".Equals(repId))
            {
                profile.PatchPrimaryAddress(imrProxyAddress);
            }
            else if (!transient && (useImR || useTaoImR))
            {
                GetImR();
                if (useImR)
                {
                    // The double call to patchPrimaryAddress ensures that either the
                    // actual imr address or the environment values are patched into the
                    // address, giving precedence to the latter.
                    profile.PatchPrimaryAddress(imr.GetImRAddress());
                    profile.PatchPrimaryAddress(imrProxyAddress);
                }
                else if (useTaoImR)
                {
                    profile.PatchPrimaryAddress(imr.GetImRAddress());
                    // Do not allow patching ImR proxy address for TAO ImR
                    // because it might have multiple enpoints.
                }
            }
            if (transient && (useImR || useTaoImR))
            {
                logger.Warn("IMR configured but Object %s is transient " + profile.ToString());
            }

            profile.PatchPrimaryAddress(iorProxyAddress);
        }

        /// <summary>
        /// Creates an ORB_TYPE_ID tagged component
        /// </summary>        
        private TaggedComponent CreateOrbTypeId()
        {
            var outputStream = new CDROutputStream(this);
            try
            {
                outputStream.BeginEncapsulatedArray();
                outputStream.WriteLong(ORBConstants.DOTNETORB_ORB_ID);
                return new TaggedComponent(TAG_ORB_TYPE.Value, outputStream.GetBufferCopy());
            }
            finally
            {
                outputStream.Close();
            }
        }

        /// <summary>
        /// Returns the address to use to locate the server.
        /// Note that this address will be overwritten by the ImR address in the IOR 
        /// of persistent servers if the useImR and useImREndpoint properties are switched on
        /// </summary>        
        private ProtocolAddressBase GetServerAddress()
        {
            lock (syncObject)
            {
                ProtocolAddressBase address = iorProxyAddress;

                if (address == null)
                {
                    //property not set
                    var eplist = GetBasicAdapter().GetEndpointProfiles();
                    foreach (var p in eplist)
                    {
                        if (p is IIOPProfile iiopProfile)
                        {
                            address = iiopProfile.GetAddress();
                            break;
                        }
                    }
                }
                else
                {
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info("Using proxy address " + address.ToString() + " in IOR");
                    }
                }
                return address;
            }
        }

        public override bool GetServiceInformation(short serviceType, out ServiceInformation serviceInfo)
        {
            throw new NoImplement();
        }

        public override IObject ResolveInitialReferences(string identifier)
        {
            WorkPending();

            if (initialReferences.ContainsKey(identifier))
            {
                return initialReferences[identifier];
            }

            CORBA.Object obj = ResolveEnvRef(identifier);
            if (obj == null)
            {
                obj = ResolveConfigInitRef(identifier);
            }

            if (obj == null)
            {
                if ("RootPOA".Equals(identifier))
                {
                    return GetRootPOA();
                }
                else if ("POACurrent".Equals(identifier))
                {
                    return GetPOACurrent();
                }
                else if ("SecurityCurrent".Equals(identifier))
                {
                    throw new InvalidName("Level2 SecurityImplementation has been removed");
                }
                else if ("DynAnyFactory".Equals(identifier))
                {
                    obj = new DynAnyFactoryImpl(this);
                }
                else if ("ORBPolicyManager".Equals(identifier))
                {
                    if (policyManager == null)
                    {
                        policyManager = new PolicyManager(Configuration);
                    }
                    return policyManager;
                }                
                else if (defaultInitRef != null)
                {
                    return StringToObject(defaultInitRef + "/" + identifier);
                }
                else
                {
                    throw new InvalidName();
                }
            }

            if (obj != null)
            {
                initialReferences.Add(identifier, obj);
            }

            return obj;
        }

        private CORBA.Object ResolveEnvRef(string identifier)
        {
            string ior = System.Environment.GetEnvironmentVariable(identifier + "IOR");
            if (ior == null)
            {
                return null;
            }
            try
            {
                return StringToObject(ior);
            }
            catch (Exception e)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error("Could not create initial reference for \"" +
                                  identifier + "\"" + System.Environment.NewLine +
                                  "Please check environment variable \"" +
                                  identifier + "IOR\"", e);
                }

                throw new InvalidName();
            }
        }

        private CORBA.Object ResolveConfigInitRef(string identifier)
        {
            string url = configuration.GetValue("ORBInitRef." + identifier, null);

            if (url == null)
            {
                return null;
            }

            try
            {
                return StringToObject(url);
            }
            catch (Exception e)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error("Could not create initial reference for \"" +
                                  identifier + "\"" + System.Environment.NewLine +
                                  "Please check property \"ORBInitRef." +
                                  identifier + '\"', e);
                }

                throw new InvalidName();
            }
        }

        /// <summary>
        /// Register a reference, that will be returned on subsequent calls to ResoveInitialReferences(id).
        /// The references "RootPOA", "POACurrent" and "PICurrent" can be set, but will not be resolved 
        /// with the passed in references.
        /// </summary>
        /// <param name="objectName">The references human-readable id, e.g. "MyService"</param>
        /// <param name="object">The objects reference</param>
        /// <exception cref="InvalidName">A reference with id has already been registered.</exception>
        public override void RegisterInitialReference(string objectName, CORBA.Object @object)
        {
            if (objectName == null || objectName.Length == 0 || initialReferences.ContainsKey(objectName))
            {
                throw new InvalidName();
            }

            if (logger.IsDebugEnabled)
            {
                logger.Debug("Registering initial ref " + objectName);
            }

            initialReferences.Add(objectName, @object);
        }

        public override void Run()
        {
            logger.Debug("ORB run");
            lock (runSync)
            {
                while (run)
                {
                    try
                    {
                        Monitor.Wait(runSync);
                    }
                    catch (ThreadInterruptedException)
                    {
                    }
                }
            }
            logger.Debug("ORB run, exit");
        }

        public override void SendMultipleRequestsOneway(IRequest[] req)
        {
            WorkPending();
            foreach (var r in req)
            {
                r.SendOneWay();
            }
        }

        public override void SendMultipleRequestsDeferred(IRequest[] req)
        {
            WorkPending();
            foreach (var r in req)
            {
                r.SendDeferred();
            }
        }

        public override bool PollNextResponse()
        {
            WorkPending();
            if (requests.Count == 0)
            {
                throw new BadInvOrder(11, CompletionStatus.No);
            }

            lock (requests)
            {
                foreach (var req in requests.Keys)
                {
                    if (req.PollResponse())
                    {
                        request = req;
                        return true;
                    }
                }
            }
            return false;
        }

        public override IRequest GetNextResponse()
        {
            WorkPending();
            if (requests.Count == 0)
            {
                throw new BadInvOrder(11, CompletionStatus.No);
            }

            lock (requests)
            {
                if (request != null)
                {
                    request.GetResponse();
                    var req = request;
                    request = null;
                    return req;
                }
                while (true)
                {
                    foreach (var req in requests.Keys)
                    {
                        if (req.PollResponse())
                        {
                            req.GetResponse();
                            return req;
                        }
                    }
                }
            }
        }

        public void AddRequest(Request req)
        {
            requests[req] = 0;
        }

        public void RemoveRequest(Request req)
        {
            requests.TryRemove(req, out byte b);
        }

        private void Init()
        {
            inORBInitializer = true;
            try
            {
                objectKeyMap = new ObjectKeyMap(this);

                transportManager = new TransportManager();
                transportManager.Configure(configuration);

                connectionManager = new ConnectionManager(this, transportManager);
                connectionManager.Configure(configuration);
            }
            finally
            {
                inORBInitializer = false;
            }
        }

        public override void Shutdown(bool waitForCompletion)
        {
            if (logger.IsInfoEnabled)
            {
                logger.Info("prepare ORB for shutdown...");
            }

            lock (shutdownSync)
            {
                if (shutdownInProgress && !waitForCompletion)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("ORB is already shutting down.");
                    }
                    return;
                }

                while (shutdownInProgress)
                {
                    try
                    {
                        Monitor.Wait(shutdownSync);
                    }
                    catch (ThreadInterruptedException ie)
                    {
                        // ignore
                    }
                }

                shutdownInProgress = true;
            }

            if (!IsRunning)
            {
                // ORB is already down.
                lock (shutdownSync)
                {
                    shutdownInProgress = false;
                    Monitor.PulseAll(shutdownSync);
                }
                return;
            }

            if (rootpoa != null)
            {
                rootpoa.Destroy(true, waitForCompletion);
                rootpoa = null;
            }

            if (basicAdapter != null)
            {
                basicAdapter.StopListeners();
            }

            connectionManager.Shutdown();
            knownReferences.Clear();
            //bufferManager.release();

            poolManagerFactory.Destroy();

            //if (interceptor_manager != null)
            //{
            //    interceptor_manager.destroy();
            //}

            // notify all threads waiting in orb.run()
            lock (runSync)
            {
                run = false;
                Monitor.PulseAll(runSync);
            }

            // notify all threads waiting for shutdown to complete
            lock (shutdownSync)
            {
                shutdownInProgress = false;
                Monitor.PulseAll(shutdownSync);
            }

            if (logger.IsInfoEnabled)
            {
                logger.Info("ORB shutdown complete");
            }
        }

        public override void Destroy()
        {
            if (destroyed)
            {
                throw new ObjectNotExist();
            }
            lock (runSync)
            {
                if (run)
                {
                    Shutdown(true);
                }
            }
            destroyed = true;
        }

        public override CORBA.Object StringToObject(string str)
        {
            WorkPending();

            if (str == null)
            {
                return null;
            }

            try
            {
                ParsedIOR pior = new ParsedIOR(this, str);
                if (pior.IsNull())
                {
                    return null;
                }

                return GetDelegate(pior);
            }
            catch (Exception e)
            {
                logger.Error("Exception while converting string to object", e);
                throw new BadParam(10, CompletionStatus.No);
            }
        }

        /// <summary>
        /// Called by RequestProcessor
        /// </summary>
        /// <param name="servant"></param>
        public override void SetDelegate(Servant servant)
        {
            if (!(servant is Servant))
            {
                throw new BadParam("Argument must be of type PortableServer.Servant");
            }

            try
            {
                var del = servant._Delegate;
            }
            catch (BadInvOrder)
            {
                // only set the delegate if it has not been set already
                var @delegate = new ServantDelegate(this);
                servant._Delegate = @delegate;
            }
            if (servant._Delegate.ORB(servant) != this)
            {
                ServantDelegate @delegate = new ServantDelegate(this);
                servant._Delegate = @delegate;
            }
        }

        public override string ObjectToString(IObject obj)
        {
            WorkPending();

            if (obj == null)
            {
                return nullIORString;
            }

            if (obj is LocalObject)
            {
                throw new Marshal("Attempt to stringify a local object");
            }

            var @delegate = ((CORBA.Object)obj)._Delegate;

            if (@delegate is Delegate)
            {
                return @delegate.ToString();
            }

            throw new BadParam("Argument has a delegate whose class is " + @delegate.GetType().FullName + ", a DotNetOrb.Orb.Delegate was expected");
        }

        public override void PerformWork()
        {
            WorkPending();
        }

        public override bool WorkPending()
        {
            if (!IsRunning)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("ORB has been shutdown");
                }

                throw new BadInvOrder(4, CompletionStatus.No);
            }
            return false;
        }

        public override IValueFactory RegisterValueFactory(string id, IValueFactory factory)
        {
            lock (syncObject)
            {
                valueFactories.Add(id, factory);
                return factory;
            }
        }

        public override void UnregisterValueFactory(string id)
        {
            lock (syncObject)
            {
                valueFactories.Remove(id);
            }
        }

        public override IValueFactory LookupValueFactory(string id)
        {
            if (valueFactories.ContainsKey(id))
            {
                return valueFactories[id];
            }
            if (id.StartsWith("IDL"))
            {
                var result = FindValueFactory(id);
                if (result != null)
                {
                    valueFactories.Add(id, result);
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds a ValueFactory for class valueName by trying standard class names.
        /// </summary>
        /// <param name="valueName">value class name</param>
        /// <returns></returns>
        private IValueFactory FindValueFactory(string repId)
        {
            var type = RepositoryId.GetValueFactoryType(repId);
            if (type != null)
                return (IValueFactory)Activator.CreateInstance(type);

            var valueType = RepositoryId.GetTypeFromRepositoryId(repId);
            var assignableTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => valueType.IsAssignableFrom(p) && !p.IsAbstract);

            // Extension of the standard: Handle the common case when the Impl class is its own factory...
            foreach (var assignableType in assignableTypes)
            {
                if (assignableType.IsAssignableFrom(typeof(IValueFactory)))
                {
                    return (IValueFactory)Activator.CreateInstance(assignableType);
                }
            }
            //Generate default factory for streamable and custom values
            var implementation = assignableTypes.FirstOrDefault();
            if (implementation != null)
            {
                return new DefaultValueFactory(implementation);
            }
            return null;
        }

        /// <summary>
        /// Returns a BoxedValueHelper for the type specified by repId, or null if no such BoxedValueHelper can be found.
        /// This method uses an internal cache of BoxedValueHelpers so that each class needs only be looked up once.
        /// </summary>
        /// <param name="repId">the repository id of the type for which a BoxedValueHelper should be returned.
        /// It is assumed that repId is the repository id of a boxed value type. Otherwise, the result will be null.</param>
        /// <returns>an instance of the BoxedValueHelper class that corresponds to repId</returns>
        public override IBoxedValueHelper GetBoxedValueHelper(string repId)
        {
            var result = boxedValueHelpers[repId];
            if (result == null)
            {
                if (boxedValueHelpers.ContainsKey(repId))
                {
                    return null;
                }

                result = RepositoryId.CreateBoxedValueHelper(repId);
                boxedValueHelpers.Add(repId, result);
            }
            return result;
        }

        public override ExceptionList CreateExceptionList()
        {
            WorkPending();
            return new ExceptionList();
        }

        public override NVList CreateList(int count)
        {
            WorkPending();
            return new NVList(this, count);
        }

        public override NamedValue CreateNamedValue(string s, CORBA.Any any, uint flags)
        {
            WorkPending();
            return new NamedValue(s, any, flags);
        }

        public override NVList CreateOperationList(CORBA.Object obj)
        {
            IOperationDef oper;
            WorkPending();

            if (obj is IOperationDef)
            {
                oper = (IOperationDef)obj;
            }
            else
            {
                throw new BadParam("Argument must be of type OperationDef");
            }
            int no = 0;

            var @params = oper.Params;

            if (@params != null)
            {
                no = @params.Length;
            }

            var list = new NVList(this, no);

            for (int i = 0; i < no; i++)
            {
                var param = @params[i];
                CORBA.Any any = CreateAny();
                any.Type = param.Type;
                switch (param.Mode)
                {
                    case ParameterMode.PARAM_IN:
                        {
                            list.AddValue(param.Name, any, (uint)ARG_IN.Value);
                            break;
                        }
                    case ParameterMode.PARAM_OUT:
                        {
                            list.AddValue(param.Name, any, (uint)ARG_OUT.Value);
                            break;
                        }
                    case ParameterMode.PARAM_INOUT:
                        {
                            list.AddValue(param.Name, any, (uint)ARG_IN_OUT.Value);
                            break;
                        }
                    default:
                        {
                            throw new BadParam("Invalid value for ParamaterMode");
                        }
                }
            }
            return list;
        }

        public RPPoolManager NewRPPoolManager(bool isSingleThreaded)
        {
            return poolManagerFactory.NewRPPoolManager(isSingleThreaded);
        }
    }
}
