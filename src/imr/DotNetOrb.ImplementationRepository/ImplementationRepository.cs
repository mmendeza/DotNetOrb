// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core;
using DotNetOrb.Core.CDR;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.GIOP;
using DotNetOrb.Core.IIOP;
using DotNetOrb.Core.POA;
using DotNetOrb.Core.POA.Util;
using DotNetOrb.ImR;
using GIOP;
using IOP;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static DotNetOrb.ImR.Admin;
using static DotNetOrb.ImR.Registration;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DotNetOrb.ImplementationRepository
{
    public class ImplementationRepository : ImplementationRepositoryPOA, IConfigurable
    {
        private Core.ORB orb;

        private Core.Config.IConfiguration configuration = null;

        /** the specific logger for this component */
        private Core.ILogger logger = null;

        private string iorFile = null;
        public string IorFile { get { return iorFile; } }

        /// <summary>
        /// The file containing the serialized server table. Also  used for writing the table to on shutdown.
        /// </summary>
        private FileInfo tableFile;
        private ServerTable serverTable;
        private FileInfo tableFileBackup;

        private ServerImRConnection serverImRConnection;
        private Thread listenerThread;

        private int objectActivationRetries = 5;
        private int objectActivationSleep = 50;

        private bool allowAutoRegister = false;
        private bool checkObjectLiveness = false;

        private int poaActivationTimeout = 120000; //2 min

        private Thread wt;
        private bool isRunning = true;
        private bool updatePending;
        private ResourceLock serverTableSerLock = new ResourceLock();

        string endpointHost;
        int endpointPort;
        bool endpointSSL;

        private object syncObject = new object();


        public ImplementationRepository(Core.ORB orb)
        {
            this.orb = orb;
        }

        public void Configure(Core.Config.IConfiguration config)
        {
            configuration = config;
            wt = new Thread(new ThreadStart(Write));
            wt.Name = "IMR Write Thread";

            logger = configuration.GetLogger(GetType());

            string defaultTableFile = "table.json";
            string tableFileStr = configuration.GetValue("DotNetOrb.ImR.TableFile", defaultTableFile);

            //NOTE: deliberate use of ref equivalence check here. I need to find
            //out if the default case has taken place, in which case, i assume
            //that the default string ref is just passed through.
            if (ReferenceEquals(tableFileStr, defaultTableFile))
            {
                if (logger.IsWarnEnabled)
                {
                    logger.Warn("No file for the server table specified! Please configure the property DotNetOrb.ImR.TableFile");
                    logger.Warn("Will create \"table.dat\" in current directory, if necessary");
                }
            }
            if (Directory.Exists(tableFileStr))
            {
                throw new ConfigException("The table file is a directory! Please check " + tableFileStr);
            }
            tableFile = new FileInfo(tableFileStr);
            bool _newTable = false;

            // try to open table file
            if (!tableFile.Exists)
            {
                _newTable = true;
                if (logger.IsInfoEnabled)
                {
                    logger.Info("Table file " + tableFileStr + " does not exist - autocreating it.");
                }

                try
                {
                    tableFile.Create();
                }
                catch (IOException ex)
                {
                    throw new ConfigException("Failed to create table file", ex);
                }
            }
            else
            {
                using (var fs = new FileStream(tableFile.FullName, FileMode.Open))
                {
                    var canRead = fs.CanRead;
                    if (!canRead)
                    {
                        throw new ConfigException("The table file is not readable! Please check " + tableFile.FullName);
                    }
                    var canWrite = fs.CanWrite;
                    if (!canWrite)
                    {
                        throw new ConfigException("The table file is not writable! Please check " + tableFile.FullName);
                    }
                }
            }

            try
            {
                if (_newTable)
                {
                    serverTable = new ServerTable();
                    SaveServerTable(tableFile);
                }
                else
                {
                    try
                    {
                        serverTable = JsonSerializer.Deserialize<ServerTable>(File.ReadAllText(tableFile.FullName), new JsonSerializerOptions()
                        {
                            ReferenceHandler = ReferenceHandler.Preserve,
                            WriteIndented = true
                        });
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Failed to read ServerTable -- creating an empty one");

                        serverTable = new ServerTable();
                        SaveServerTable(tableFile);
                    }
                }
            }
            catch (FileOpFailed ex)
            {
                logger.Error("Failed to read ServerTable", ex);
            }


            //should be set. if not, throw
            iorFile = configuration.GetValue("DotNetOrb.ImR.IorFile");

            string backupFileStr = configuration.GetValue("DotNetOrb.ImR.BackupFile", "");

            //set up server table backup file
            if (backupFileStr.Length == 0)
            {
                logger.Warn("No backup file specified!. No backup file will be created");
            }
            else
            {
                tableFileBackup = new FileInfo(backupFileStr);
                if (Directory.Exists(backupFileStr))
                {
                    throw new ConfigException("The backup file is a directory! Please check " + backupFileStr);
                }
                // try to open backup file
                if (!tableFileBackup.Exists)
                {
                    _newTable = true;
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info("Backup file " + backupFileStr + " does not exist - autocreating it.");
                    }
                    try
                    {
                        tableFileBackup.Create();
                    }
                    catch (IOException ex)
                    {
                        throw new ConfigException("Failed to create backup file", ex);
                    }
                }
                else
                {
                    using (var fs = new FileStream(tableFileBackup.FullName, FileMode.Open))
                    {
                        var canRead = fs.CanRead;
                        if (!canRead)
                        {
                            throw new ConfigException("The backup file is not readable! Please check " + tableFileBackup.FullName);
                        }
                        var canWrite = fs.CanWrite;
                        if (!canWrite)
                        {
                            throw new ConfigException("The backup file is not writable! Please check " + tableFileBackup.FullName);
                        }
                    }
                }
            }

            objectActivationRetries = configuration.GetAsInteger("DotNetOrb.ImR.ObjectActivationRetries", 5);

            objectActivationSleep = configuration.GetAsInteger("DotNetOrb.ImR.ObjectActivationSleep", 50);

            allowAutoRegister = configuration.GetAsBoolean("DotNetOrb.ImR.AllowAutoRegister", false);

            checkObjectLiveness = configuration.GetAsBoolean("DotNetOrb.ImR.CheckObjectLiveness", false);

            poaActivationTimeout = configuration.GetAsInteger("DotNetOrb.ImR.Timeout", 120000);

            endpointHost = configuration.GetValue("DotNetOrb.ImR.Endpoint.Host", "0.0.0.0");
            endpointPort = configuration.GetAsInteger("DotNetOrb.ImR.Endpoint.Port", 0);
            endpointSSL = configuration.GetAsBoolean("DotNetOrb.ImR.Endpoint.IsSSL", false);

            if (endpointSSL)
            {
                if (!config.ContainsKey("DotNetOrb.ImR.Endpoint.Certificate") ||
                    !config.ContainsKey("DotNetOrb.ImR.Endpoint.CertificatePassword"))
                {
                    throw new ConfigException("ImR endpoint SSL is active. \"DotNetOrb.ImR.Endpoint.Certificate\" and \"DotNetOrb.ImR.Endpoint.CertificatePassword\" are mandatory!");
                }
            }

            var address = new IIOPAddress(endpointHost, endpointPort);
            serverImRConnection = new ServerImRConnection(orb, this, address, endpointSSL);
            serverImRConnection.Connect();

            wt.IsBackground = true;
            wt.Start();
        }

        /// <summary>
        /// runs as a background thread which will write the server table out whenever any modifications are made.
        /// </summary>
        private void Write()
        {
            while (true)
            {
                try
                {
                    serverTableSerLock.GainExclusiveLock();
                    SaveServerTable(tableFile);
                }
                catch (FileOpFailed ex)
                {
                    logger.Error("Exception while saving server table", ex);
                }
                finally
                {
                    serverTableSerLock.ReleaseExclusiveLock();
                }

                if (!isRunning)
                {
                    break;
                }

                // If by the time we have written the server table another request has arrived
                // which requires an update don't bother entering the wait state.
                if (!updatePending)
                {
                    try
                    {
                        lock (syncObject)
                        {
                            Monitor.Wait(syncObject);
                        }
                    }
                    catch (ThreadInterruptedException) { }

                    logger.Debug("ImR: IMR write thread waking up to save server table... ");
                }
            }
        }

        private void SaveServerTable(FileInfo file)
        {
            try
            {
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                serverTable.tablelock.GainExclusiveLock();
                string jsonString = JsonSerializer.Serialize(serverTable, options);
                serverTable.tablelock.ReleaseExclusiveLock();
                File.WriteAllText(file.FullName, jsonString);
            }
            catch (Exception e)
            {
                logger.Error("Exception while saving server table", e);
                throw new FileOpFailed();
            }
            updatePending = false;
        }

        /// <summary>
        /// The actual core method of the implementation repository. 
        /// Causes servers to start, looks up new POA locations in the server table.
        /// </summary>        
        internal async void ReplyNewLocation(byte[] objectKey, uint requestId, int giopMinor, ImRChannelHandler channelHandler, bool isLocateRequest)
        {
            string _poaName = POAUtil.ExtractImplName(objectKey) + '/' + POAUtil.ExtractPOAName(objectKey);

            // look up POA in table
            ImRPOAInfo _poa = serverTable.GetPOA(_poaName);
            if (_poa == null)
            {
                SendSysException(new Transient("POA " + _poaName + " unknown"), channelHandler, requestId, giopMinor);
                return;
            }

            // get server of POA
            ImRServerInfo _server = _poa.Server;

            if (logger.IsDebugEnabled)
            {
                logger.Debug("ImR: Looking up: " + _server.Name);
            }

            // There is only point pinging the remote object if server
            // is active and either the QoS to ping returned objects
            // is true or the ServerStartUpDaemon is active and there
            // is a command to run - if not, even if the server isn't
            // actually active, we can't restart it so just allow this
            // to fall through and throw the TRANSIENT below.
            bool ssdValid = _server.Command.Length != 0 && serverTable.GetHost(_server.Host) != null;

            if (_server.IsActive && (checkObjectLiveness || ssdValid))
            {
                // At this point the server *might* be running - we
                // just want to verify it.
                if (!await CheckServerActive(_poa.Host, _poa.Port, objectKey, false))
                {
                    // Server is not active so set it down
                    _server.SetDown();
                }
            }

            try
            {
                RestartServer(_server);
            }
            catch (ServerStartupFailed ssf)
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info("Object (" + _server.Name + ") on " + _poa.Host + '/' + _poa.Port + " not reachable");
                }

                SendSysException(new Transient(ssf.Reason), channelHandler, requestId, giopMinor);
                return;
            }

            // POA might not be active
            bool _oldPoaState = _poa.IsActive;

            // wait for POA to be reregistered.
            if (!_poa.AwaitActivation())
            {
                // timeout reached
                SendSysException(new Transient("Timeout exceeded"), channelHandler, requestId, giopMinor);
                return;
            }
            CDROutputStream outputStream;

            if (isLocateRequest)
            {
                outputStream = new ReplyOutputStream(orb, requestId, ReplyStatusType12.LOCATION_FORWARD, new List<ServiceContext>(), giopMinor);
            }
            else
            {
                outputStream = new LocateReplyOutputStream(orb, requestId, LocateStatusType12.OBJECT_FORWARD, giopMinor);
            }

            IIOPAddress addr = new IIOPAddress(_poa.Host, (short)_poa.Port);
            IOR _ior = null;
            try
            {
                addr.Configure(configuration);
                IIOPProfile p = new IIOPProfile(addr, objectKey, giopMinor);
                p.Configure(configuration);
                _ior = ParsedIOR.CreateObjectIOR(orb, p);
            }
            catch (ConfigurationException e)
            {
                logger.Error("Error while configuring address/profile", e);
            }

            if (!_oldPoaState)
            {
                // if POA has been reactivated, we have to wait for
                // the requested object to become ready again. This is
                // for avoiding clients to get confused by
                // OBJECT_NOT_EXIST exceptions which they might get
                // when trying to contact the server too early.

                IObject _object = orb.StringToObject(new ParsedIOR(orb, _ior).GetIORString());

                // Sort of busy waiting here, no other way possible
                for (int _i = 0; _i < objectActivationRetries; _i++)
                {
                    try
                    {
                        Thread.Sleep(objectActivationSleep);

                        // This will usually throw an OBJECT_NOT_EXIST
                        if (!_object._NonExistent()) // "CORBA ping"
                        {
                            break;
                        }
                    }
                    catch (Exception _e)
                    {
                        logger.Info("Exception while waiting for object", _e);
                    }
                }
            }

            try
            {
                // write new location to stream
                outputStream.WriteIOR(_ior);

                if (logger.IsDebugEnabled)
                {
                    logger.Debug("ImR: Sending location forward for " + _server.Name);
                }
                if (isLocateRequest)
                {
                    await channelHandler.SendLocateReplyAsync((LocateReplyOutputStream)outputStream);
                }
                else
                {
                    await channelHandler.SendReplyAsync((ReplyOutputStream)outputStream);
                }
            }
            catch (IOException ex)
            {
                logger.Error("Exception while writing new location", ex);

                SendSysException(new Unknown(ex.ToString()), channelHandler, requestId, giopMinor);
            }
        }


        private async void SendSysException(CORBA.SystemException sysEx, ImRChannelHandler channelHandler, uint requestId, int giopMinor)
        {
            var outputStream = new ReplyOutputStream(orb, requestId, ReplyStatusType12.SYSTEM_EXCEPTION, new List<ServiceContext>(), giopMinor);

            SystemExceptionHelper.Write(outputStream, sysEx);

            try
            {
                await channelHandler.SendReplyAsync(outputStream);
            }
            catch (IOException _e)
            {
                logger.Error("Exception while sending SystemException to client", _e);
            }
        }

        private async Task<bool> CheckServerActive(string host, int port, byte[] objectKey, bool endpointReused)
        {
            ConnectionManager cm = null;
            IIOPAddress address = null;
            GIOPChannelHandler channelHandler = null;
            LocateRequestOutputStream lros = null;
            //LocateReplyReceiver receiver = null;
            LocateReplyInputStream lris = null;
            bool result = false;

            cm = orb.ConnectionManager;
            try
            {
                address = new IIOPAddress(host, port);
                address.Configure(configuration);

                IIOPProfile iiopProfile = new IIOPProfile(address, objectKey, orb.GiopMinorVersion);
                iiopProfile.Configure(configuration);

                channelHandler = cm.GetChannelHandler(iiopProfile);
            }
            catch (ConfigurationException e)
            {
                logger.Error("Failed to configure", e);
            }

            if (logger.IsDebugEnabled)
            {
                logger.Debug("Pinging " + host + " / " + port);
            }

            try
            {
                lros = new LocateRequestOutputStream(orb, channelHandler, new TargetAddress() { ObjectKey = objectKey }, 2);

                lris = (LocateReplyInputStream?)await channelHandler.SendLocateRequestAsync(lros);

                switch (lris.LocateStatus)
                {
                    case LocateStatusType12.UNKNOWN_OBJECT:
                    case LocateStatusType12.LOC_SYSTEM_EXCEPTION:
                        {

                            var se = SystemExceptionHelper.Read(lris);
                            if (endpointReused && se is ObjectNotExist)
                            {
                                // Endpoint is reused by another server and the new server
                                // returned OBJECT_NOT_EXIST, so give up using this endpoint.
                                // The return value can be interpreted as not available endpoint.
                                result = false;
                            }
                            else
                            {
                                result = true;
                            }
                            break;
                        }
                    case LocateStatusType12.OBJECT_HERE:
                    case LocateStatusType12.OBJECT_FORWARD:
                    case LocateStatusType12.OBJECT_FORWARD_PERM:
                    case LocateStatusType12.LOC_NEEDS_ADDRESSING_MODE:
                    default:
                        {
                            result = true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                logger.Debug("Exception while checking server active", ex);
                result = false;
            }
            finally
            {
                if (channelHandler.Connection is ClientConnection clientConnection)
                {
                    cm.ReleaseConnection(clientConnection);
                }
            }
            return result;
        }

        private void RestartServer(ImRServerInfo server)
        {
            // server might be holding
            server.AwaitRelease();

            if (!server.IsActive)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("ImR: server " + server.Name + " is down");
                }

                if (string.IsNullOrEmpty(server.Command))
                {
                    //server can't be restarted, send exception
                    throw new ServerStartupFailed("Server " + server.Name + " can't be restarted because of missing startup command");
                }

                // we have to synchronize here to avoid a server to be restarted multiple times by requests that are
                // received in the gap between the first try to restart and the reactivation of the POAs.
                // restarting is set back to false when the first POA is reactivated and the server goes back to active
                // (see ImRPOAInfo.Reactivate()).
                if (server.ShouldBeRestarted())
                {
                    try
                    {
                        // If there is no SSD for the host, we get an NullPointerException.
                        // In a further version, we might choose another random SSD.
                        ImRHostInfo _host = serverTable.GetHost(server.Host);

                        if (_host == null)
                        {
                            throw new ServerStartupFailed("Unknown host: >>" + server.Host + "<<");
                        }

                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("ImR: will restart " + server.Name);
                        }

                        _host.StartServer(server.Command, orb);
                    }
                    catch (ServerStartupFailed ssf)
                    {
                        server.IsRestarting = false;

                        throw ssf;
                    }
                    catch (Exception e)
                    {
                        server.IsRestarting = false;

                        logger.Error("Exception while restarting server", e);

                        // sth wrong with daemon, remove from table
                        serverTable.RemoveHost(server.Host);

                        throw new ServerStartupFailed("Failed to connect to host!");
                    }
                }
                else
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("ImR: somebody else is restarting " + server.Name);
                    }
                }
            }
            else
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("ImR: server " + server.Name + " is active");
                }
            }
        }

        /// <summary>
        /// Updates the server with a new command and host. For migrating purposes.
        /// </summary>
        /// <param name="name">the server name</param>
        /// <param name="command">the new startup command for this server</param>
        /// <param name="host">the new host</param>
        /// <exception cref="UnknownServerName"></exception>
        public override void EditServer([WideChar(false)] string name, [WideChar(false)] string command, [WideChar(false)] string host)
        {
            updatePending = true;

            ImRServerInfo _server = serverTable.GetServer(name);

            _server.Command = command;
            _server.Host = host;

            if (logger.IsDebugEnabled)
            {
                logger.Debug("ImR: server " + name + " edited");
            }
            lock (wt)
            {
                Monitor.Pulse(wt);
            }
        }

        /// <summary>
        /// Get host and port (wrapped inside an ImRInfo object) of this repository.
        /// </summary>
        /// <returns>the ImRInfo object of this repository</returns>
        public override ImRInfo GetImrInfo()
        {
            return new ImRInfo(endpointHost, (uint)endpointPort);
        }

        /// <summary>
        /// Get the ServerInfo object of a specific server
        /// </summary>
        /// <param name="name">the server name</param>
        /// <returns>the ServerInfo object of the server</returns>
        /// <exception cref="UnknownServerName"></exception>
        public override ServerInfo GetServerInfo([WideChar(false)] string name)
        {
            return serverTable.GetServer(name).ToServerInfo();
        }

        /// <summary>
        /// Hold a server. This causes all requests for this server to be delayed until it is released.
        /// Holding a server is useful for migrating or maintaining it. There is not timeout set, 
        /// so requests might be delayed indefinetly (or, at least, until the communication layer protests;-).
        /// </summary>
        /// <param name="name">the server name</param>
        /// <exception cref="UnknownServerName">if no server with that name known.</exception>
        public override void HoldServer([WideChar(false)] string name)
        {
            ImRServerInfo _server = serverTable.GetServer(name);
            _server.IsHolding = true;
        }

        /// <summary>
        /// List all hosts currently registered with this repository. 
        /// It is not guaranteed that the references inside the HostInfo objects are still valid.
        /// </summary>
        /// <returns>an array containing all known hosts</returns>
        public override HostInfo[] ListHosts()
        {
            return serverTable.GetHosts();
        }

        /// <summary>
        /// List all registered server. The ServerInfo objects contain also a list of the associated POAs.
        /// </summary>
        /// <returns>an array containing all registered servers</returns>
        public override ServerInfo[] ListServers()
        {
            ServerInfo[] servers;

            if (checkObjectLiveness)
            {
                logger.Debug("ImR: Checking servers");

                servers = serverTable.GetServers();

                foreach (var server in servers)
                {
                    if (server.Active && server.Poas.Length > 0)
                    {
                        byte[] first = System.Text.Encoding.Default.GetBytes(server.Poas[0].Name);
                        byte[] id = new byte[first.Length + 1];
                        Array.Copy(first, 0, id, 0, first.Length);
                        id[first.Length] = POAConstants.ObjectKeySeparatorByte;

                        var task = CheckServerActive(server.Poas[0].Host, (int)server.Poas[0].Port, id, false);

                        if (!task.Result)
                        {
                            try
                            {
                                if (logger.IsDebugEnabled)
                                {
                                    logger.Debug("ImR: Setting server " + server.Name + " down");
                                }

                                // Server is not active so set it down
                                serverTable.GetServer(server.Name).SetDown();

                                // Save retrieving the list again.
                                server.Active = false;
                            }
                            catch (UnknownServerName e)
                            {
                                if (logger.IsErrorEnabled)
                                {
                                    logger.Error("ImR: Internal error - unknown server " + server.Name, e);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                servers = serverTable.GetServers();
            }

            return servers;
        }

        /// <summary>
        /// Register a new host with a server startup daemon
        /// </summary>
        /// <param name="info">a HostInfo object containing the hosts name 
        /// and a reference to its ServerStartupDaemon object</param>
        /// <exception cref="IllegalHostName"></exception>
        /// <exception cref="InvalidSSDRef"></exception>
        public override void RegisterHost(HostInfo info)
        {
            if (string.IsNullOrEmpty(info.Name))
            {
                throw new IllegalHostName(info.Name);
            }

            try
            {
                info.SsdRef.GetSystemLoad();
            }
            catch (Exception e)
            {
                logger.Error("Exception while getting system load", e);
                throw new InvalidSSDRef();
            }
            updatePending = true;

            serverTable.AddHost(info.Name, new ImRHostInfo(info));

            lock (wt)
            {
                Monitor.Pulse(wt);
            }
        }

        /// <summary>
        /// This method registers a POA. It has actually two functions:
        /// <list type="bullet">
        ///     <item>
        ///         <description>Register a POA that has not yet been registered.It is the added to the server table</description>
        ///     </item>
        ///     <item>
        ///         <description>Reactivating a POA that is not active, but has already an entry in the server table</description>
        ///     </item>
        /// </list>
        /// The reason for using only one method for those two tasks is that it is much more difficult for the ORB, 
        /// which does the registering, to distinguish between an newly created POA and a restarted one.
        /// </summary>
        /// <param name="name">the POAs name</param>
        /// <param name="server">the logical server name of the server the running in</param>
        /// <param name="host">the POAs host</param>
        /// <param name="port">the POAs port</param>
        /// <exception cref="IllegalPOAName">the POAs name is not valid</exception>
        /// <exception cref="DuplicatePOAName">an active POA with <paramref name="name"/> is currently registered.</exception>
        /// <exception cref="UnknownServerName">the server has not been registered</exception>
        public override void RegisterPoa([WideChar(false)] string name, [WideChar(false)] string server, [WideChar(false)] string host, uint port)
        {
            ImRServerInfo _server = null;
            ImRPOAInfo _poa = null;
            bool remap = false;

            updatePending = true;

            if (logger.IsDebugEnabled)
            {
                logger.Debug("ImR: registering poa " + name + " for server: " + server + " on " + host + ":" + port);
            }

            if (allowAutoRegister && !serverTable.HasServer(server))
            {
                try
                {
                    RegisterServer(server, "", "");
                }
                catch (IllegalServerName)
                {
                    //ignore
                }
                catch (DuplicateServerName)
                {
                    //ignore
                }
            }

            _server = serverTable.GetServer(server);
            _poa = serverTable.GetPOA(name);

            if (_poa == null)
            {
                //New POAInfo is to be created
                _poa = new ImRPOAInfo(name, host, (int)port, _server, poaActivationTimeout);

                serverTableSerLock.GainExclusiveLock();
                _server.AddPOA(_poa);
                serverTableSerLock.ReleaseExclusiveLock();

                serverTable.AddPOA(name, _poa);

                logger.Debug("ImR: new poa registered");
            }
            else
            {
                // Existing POA is reactivated

                // Need to check whether the old server is alive. if it is then
                // throw an exception otherwise we can remap the currently
                // registered name.
                if (_poa.IsActive || !server.Equals(_poa.Server.Name))
                {
                    byte[] first = System.Text.Encoding.Default.GetBytes(_poa.Name);
                    byte[] id = new byte[first.Length + 1];
                    Array.Copy(first, 0, id, 0, first.Length);
                    id[first.Length] = POAConstants.ObjectKeySeparatorByte;

                    // If host and port are the same then it must be a replacement as
                    // we could not have got to here if the original was running - we
                    // would have got a socket exception.
                    if (_poa.Host.Equals(host) && _poa.Port == port)
                    {
                        remap = true;
                    }
                    else
                    {
                        // Otherwise try a ping

                        // The ImplRepo may not get chance to update its
                        // internal state due to server non-graceful
                        // shutdown. If the address is actively using by
                        // another server, the new endpoint(host, port)
                        // will replace the old endpoint.
                        bool enpReused = serverTable.IsPOAEndpointReused(_poa.Name, _poa.Host, _poa.Port);

                        // We can not determine if the address is really
                        // using based on the server table since it may
                        // not be updated. We still need ping the server.
                        var task = CheckServerActive(_poa.Host, _poa.Port, id, enpReused);
                        remap = !task.Result;
                    }

                    if (remap == false)
                    {
                        throw new DuplicatePOAName("POA " + name + " has already been registered for server " + _poa.Server.Name);
                    }
                    logger.Debug("ImR: Remapping server/port");
                }

                _poa.Reactivate(host, (int)port);
                logger.Debug("ImR: register_poa, reactivated");
            }

            lock (wt)
            {
                Monitor.Wait(wt);
            }
        }

        /// <summary>
        /// Register a logical server. The logical server corresponds to a process which has a number of POAs.
        /// </summary>
        /// <param name="name">the server name</param>
        /// <param name="command">the startup command for this server if it should be restarted on demand.
        /// Has to be empty (NOT null) if the server should not be restarted.</param>
        /// <param name="host">the host on which the server should be restarted. Should not be null, 
        /// but is ignored if no startup command is specified.</param>
        /// <exception cref="IllegalServerName"></exception>
        /// <exception cref="DuplicateServerName"></exception>
        public override void RegisterServer([WideChar(false)] string name, [WideChar(false)] string command, [WideChar(false)] string host)
        {
            updatePending = true;

            ImRServerInfo _server = new ImRServerInfo(name, host, command);
            serverTable.AddServer(name, _server);

            if (logger.IsDebugEnabled)
            {
                logger.Debug("ImR: server " + name + " on " + host + " registered");
            }
            lock (wt)
            {
                Monitor.Pulse(wt);
            }
        }


        /// <summary>
        /// Release a server from state "holding"
        /// </summary>
        /// <param name="name">the server name</param>
        /// <exception cref="UnknownServerName">if no server with that name known.</exception>
        public override void ReleaseServer([WideChar(false)] string name)
        {
            ImRServerInfo _server = serverTable.GetServer(name);
            _server.Release();
        }

        /// <summary>
        /// Save the server table to a backup file.
        /// </summary>
        /// <exception cref="FileOpFailed"></exception>
        public override void SaveServerTable()
        {
            if (tableFileBackup != null)
            {
                SaveServerTable(tableFileBackup);
            }
        }

        /// <summary>
        /// This method sets a server down, i.e. not.active. If a request for that server is encountered,
        /// the server is tried to be restarted.
        /// </summary>
        /// <param name="name">the server name</param>
        /// <exception cref="UnknownServerName">if no server with that name known.</exception>
        public override void SetServerDown([WideChar(false)] string name)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("ImR: server " + name + " is going down... ");
            }

            ImRServerInfo _server = serverTable.GetServer(name);
            _server.SetDown();
        }

        /// <summary>
        ///  Shut the repository down orderly, i.e. with saving of the server table.        
        /// </summary>
        /// <param name="wait">If false, then the ORB is forced down, ignoring any open connection.</param>
        public override void Shutdown(bool wait)
        {
            isRunning = false;
            lock (wt)
            {
                Monitor.Pulse(wt);
            }
            if (serverImRConnection != null)
            {
                serverImRConnection.Close();
            }
            try
            {
                SaveServerTable();
            }
            catch (FileOpFailed f)
            {
                logger.Error("ImR: Failed to save backup table.", f);
            }
            logger.Debug("ImR: Finished shutting down");
        }

        /// <summary>
        /// Start a server
        /// </summary>
        /// <param name="name">the server name</param>
        /// <exception cref="UnknownServerName">if no server with that name known.</exception>
        public override void StartServer([WideChar(false)] string name)
        {
            RestartServer(serverTable.GetServer(name));
        }

        /// <summary>
        /// Remove a host from the servertable. Hosts are removed automatically on server startup, 
        /// if they can't be accessed.
        /// </summary>
        /// <param name="name">the hosts name</param>
        /// <exception cref="UnknownHostName">if no host with that name known.</exception>
        public override void UnregisterHost([WideChar(false)] string name)
        {
            if (serverTable.RemoveHost(name) == null)
            {
                throw new UnknownHostName(name);
            }
        }

        /// <summary>
        ///  Remove a logical server from the server table. If a server is removed, all of its POAs are removed as well.
        /// </summary>
        /// <param name="name">the server name</param>
        /// <exception cref="UnknownServerName">if no server with that name known.</exception>
        public override void UnregisterServer([WideChar(false)] string name)
        {
            updatePending = true;

            ImRServerInfo _server = serverTable.GetServer(name);
            string[] _poas = _server.GetPOANames();

            foreach (var poa in _poas)
            {
                serverTable.RemovePOA(poa);
            }

            serverTable.RemoveServer(name);

            if (logger.IsDebugEnabled)
            {
                logger.Debug("ImR: server " + name + " unregistered");
            }

            lock (wt)
            {
                Monitor.Pulse(wt);
            }
        }

    }
}
