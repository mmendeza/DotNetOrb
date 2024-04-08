// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core;
using DotNetOrb.Core.Config;
using DotNetOrb.ImR;
using PortableServer;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace DotNetOrb.NameService
{
    public class NameService : BackgroundService, IConfigurable
    {
        private static Core.ORB orb = null;
        private static Core.Config.IConfiguration configuration = null;

        // the specific logger for this component
        private static Core.ILogger logger = null;

        // the file name int which the IOR will be stored
        private static string fileName = null;
        internal static string filePrefix = "_nsdb";
        private static string commandSuffix = "";

        // if this value is != 0, the name server will automatically shut down after the given time
        private static int timeout = 0;

        static string nameDelimiter = "/";
        private static bool printIOR;

        public NameService()
        {
        }

        public void Configure(Core.Config.IConfiguration config)
        {
            configuration = config;
            logger = config.GetLogger(GetType());

            printIOR = config.GetAsBoolean("DotNetOrb.Naming.PrintIor", false);

            timeout = config.GetAsInteger("DotNetOrb.Naming.Timeout", 0);

            fileName = config.GetValue("DotNetOrb.Naming.IORFilename", "");

            /* which directory to store/load in? */
            var directory = config.GetValue("DotNetOrb.Naming.DBDir", "");

            if (!directory.Equals(""))
                filePrefix = directory + Path.DirectorySeparatorChar + filePrefix;

            if (config.GetAsBoolean("DotNetOrb.UseImR", false))
            {
                // don't supply "imr_register", so a ns started by an imr_ssd won't try to register himself again.
                string command = config.GetValue("DotNetOrb.Exec", "") + commandSuffix;

                // autoregister server
                try
                {
                    IAdmin admin = AdminHelper.Narrow(orb.ResolveInitialReferences("ImplementationRepository"));
                    var server = "StandardNS";
                    var host = "";
                    try
                    {
                        host = System.Net.Dns.GetHostName();
                    }
                    catch (SocketException)
                    {                        
                    }

                    ServerInfo info = null;

                    try
                    {
                        info = admin.GetServerInfo(server);
                    }
                    catch (UnknownServerName n)
                    {
                        logger.Warn("Unknown Server name", n);
                    }

                    if (info == null)
                    {
                        admin.RegisterServer(server, command, host);
                    }
                    else if (info != null)
                    {
                        admin.EditServer(server, command, host);
                    }
                }
                catch (Exception e)
                {
                    logger.Warn("Unexpected exception", e);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                commandSuffix = " DotNetOrb.NameServer";

                var props = new Dictionary<string, string>();
                props.Add("DotNetOrb.ImplName", "StandardNS");

                /*
                 * by setting the following property, the ORB will accept client requests targeted at the object with
                 * key "NameService", so more readablee corbaloc URLs can be used
                 */
                props.Add("DotNetOrb.Orb.ObjectKeyMap.NameService", "StandardNS/NameServer-POA/_root");

                /* intialize the ORB and Root POA */
                orb = (Core.ORB)CORBA.ORB.Init(props);

                var config = orb.Configuration;

                /* configure the name service using the ORB configuration */
                Configure(config);

                PortableServer.IPOA rootPOA = PortableServer.POAHelper.Narrow(orb.ResolveInitialReferences("RootPOA"));

                /* create a user defined poa for the naming contexts */

                IPolicy[] policies = new CORBA.IPolicy[3];

                policies[0] = rootPOA.CreateIdAssignmentPolicy(PortableServer.IdAssignmentPolicyValue.USER_ID);
                policies[1] = rootPOA.CreateLifespanPolicy(PortableServer.LifespanPolicyValue.PERSISTENT);
                policies[2] = rootPOA.CreateRequestProcessingPolicy(PortableServer.RequestProcessingPolicyValue.USE_SERVANT_MANAGER);

                IPOA nsPOA = rootPOA.CreatePoa("NameServer-POA", rootPOA.ThePOAManager, policies);

                NamingContextImpl.Init(orb, rootPOA);
                NameServantActivator servantActivator = new NameServantActivator(orb);
                servantActivator.Configure(config);

                nsPOA.SetServantManager(servantActivator);
                nsPOA.ThePOAManager.Activate();

                for (int i = 0; i < policies.Length; i++)
                    policies[i].Destroy();

                /* export the root context's reference to a file */
                byte[] oid = Encoding.Default.GetBytes("_root");
                try
                {
                    IObject obj = nsPOA.CreateReferenceWithId(oid, "IDL:omg.org/CosNaming/NamingContextExt:1.0");

                    if (fileName != null && fileName.Length > 0)
                    {
                        using (StreamWriter writer = new StreamWriter(fileName))
                        {
                            writer.WriteLine(orb.ObjectToString(obj));
                        }
                    }

                    if (printIOR)
                    {
                        Console.WriteLine("SERVER IOR: " + orb.ObjectToString(obj));
                        logger.Info("SERVER IOR: " + orb.ObjectToString(obj));
                    }
                }
                catch (Exception e)
                {
                    logger.Error("unexpected exception", e);
                    throw new RuntimeException(e.Message);
                }

                if (logger.IsInfoEnabled)
                {
                    logger.Info("Name Server is UP");
                }

                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

                var sw = Stopwatch.StartNew();
                /* either block indefinitely or time out */
                while (!stoppingToken.IsCancellationRequested)
                {
                    if (timeout > 0 && timeout > sw.ElapsedMilliseconds)
                    {
                        break;
                    }
                    else
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                }
                /* shutdown. This will etherealize all servants, thus saving their state */
                orb.Shutdown(true);
            }
            catch (ConfigException e)
            {
                logger.Error(e);
                //usage();
            }
            catch (Exception e)
            {
                logger.Error(e);
                System.Environment.Exit(1);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            orb.Shutdown(true);
            return base.StopAsync(cancellationToken);
        }

        private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            orb.Shutdown(true);
        }
    }
}