// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using DotNetOrb.ImR;
using DotNetty.Common.Utilities;
using Microsoft.Extensions.Logging;
using PortableServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrb.ServerStartupDaemon
{
    internal class ServerStartupDaemon : ServerStartupDaemonPOA, IConfigurable
    {
        private ORB orb = null;
        private static string outPrefix = ">> ";


        private Core.ILogger logger;

        public ServerStartupDaemon(ORB orb)
        {
            this.orb = orb;
        }

        public void Configure(Core.Config.IConfiguration config)
        {
            logger = config.GetLogger(GetType());

            try
            {
                IRegistration registration = null;

                registration = RegistrationHelper.Narrow(orb.ResolveInitialReferences("ImplementationRepository"));

                if (registration == null)
                    throw new ConfigException("ImR not found");

                IPOA poa = POAHelper.Narrow(orb.ResolveInitialReferences("RootPOA"));
                poa.ThePOAManager.Activate();

                IServerStartupDaemon ssd = ServerStartupDaemonHelper.Narrow(poa.ServantToReference(this));

                var hostname = System.Net.Dns.GetHostName();

                HostInfo me = new HostInfo(hostname, ssd, orb.ObjectToString(ssd));

                registration.RegisterHost(me);
            }
            catch (Exception e)
            {
                throw new ConfigException("Caught Exception", e);
            }
        }

        /// <summary>
        /// NOT IMPLEMENTED, but currently used for "pinging" purposes.
        /// </summary>
        /// <returns>0 always</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override int GetSystemLoad()
        {
            return 0;
        }

        /// <summary>
        /// This method starts a server on this host as specified by 'command'.
        /// </summary>
        /// <param name="command">The server startup command, i.e. the servers class name and parameters 
        /// for its main method.The interpreter is inserted automatically.</param>
        /// <exception cref="ServerStartupFailed"></exception>
        public override void StartServer(string command)
        {
            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Starting: " + command);
                }

                var argsPrepend = "";
                var shellName = "/bin/bash";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    shellName = "cmd";
                    argsPrepend = "/c ";
                }

                Process server = new Process();
                server.StartInfo.FileName = shellName;
                server.StartInfo.Arguments = argsPrepend + command;
                server.StartInfo.UseShellExecute = false;
                server.StartInfo.RedirectStandardOutput = true;
                server.StartInfo.CreateNoWindow = true;
                server.Start();

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    while (!server.StandardOutput.EndOfStream)
                    {
                        var line = server.StandardOutput.ReadLine();
                        if (line != null)
                        {
                            Console.WriteLine(outPrefix + line);
                            logger.Debug(outPrefix + line);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                logger.Error("Server startup failed", ex);
                throw new ServerStartupFailed(ex.ToString());
            }
        }

    }
}
