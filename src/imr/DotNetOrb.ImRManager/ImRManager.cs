// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using DotNetOrb.ImR;
using System.Net.Sockets;
using System.Text;

namespace DotNetOrb.ImRManager
{
    public class ImRManager
    {
        private static DotNetOrb.Core.ORB orb;
        private static Core.ILogger logger;

        /// <summary>
        /// This method registers a server with the imr. To be called from within a program.
        /// Leave command and host to "" (not null), if automatic startup is not desired.
        /// </summary>
        /// <param name="orb"></param>
        /// <param name="server"></param>
        /// <param name="command"></param>
        /// <param name="host"></param>
        /// <param name="editExisting">if set to true and the server already exist, 
        /// the entry will be set to the supplied new values.</param>
        public static void AutoRegisterServer(CORBA.ORB orb, string server, string command, string host, bool editExisting)
        {
            try
            {
                IAdmin admin = AdminHelper.Narrow(orb.ResolveInitialReferences("ImplementationRepository"));

                IConfiguration config = ((DotNetOrb.Core.ORB)orb).Configuration;
                var logger = config.GetLogger("DotNetOrb.ImR.ImRManager");

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
                else if ((info != null) && editExisting)
                {
                    admin.EditServer(server, command, host);
                }
            }
            catch (Exception e)
            {
                logger.Warn("unexpected exception", e);
            }
        }

        /// <summary>
        /// Returns the name of the local host to be supplied to the imr.
        /// If this can't be queried, an empty String is returned.
        /// </summary>        
        public static string GetLocalHostName()
        {
            try
            {
                return System.Net.Dns.GetHostName();
            }
            catch (SocketException)
            {
                // ignore, was: Debug.output(3, e);
            }

            return "";
        }

        /// <summary>
        /// Returns an arbitrary host, on which an imr ssd is running, or an empty String, if none is present.
        /// </summary>
        public static string GetAnyHostName(CORBA.ORB orb)
        {
            try
            {
                IAdmin admin = AdminHelper.Narrow(orb.ResolveInitialReferences("ImplementationRepository"));

                HostInfo[] hosts = admin.ListHosts();

                if (hosts.Length > 0)
                {
                    return hosts[0].Name;
                }
            }
            catch (Exception e)
            {
                logger.Warn("unexpected exception", e);
            }

            return "";
        }

        private static IAdmin GetAdmin()
        {
            IAdmin admin = null;
            try
            {
                admin = AdminHelper.Narrow(orb.ResolveInitialReferences("ImplementationRepository"));
            }
            catch (Exception ex)
            {
                if (logger.IsWarnEnabled)
                    logger.Warn("Could not contact Impl. Repository!", ex);
            }

            if (admin == null)
            {
                Console.WriteLine("Unable to connect to repository process!");
                System.Environment.Exit(-1);
            }
            return admin;
        }

        /// <summary>
        /// Add a server to the repository or edit an existing server.
        /// </summary>        
        private static void AddServer(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("Please specify at least server name");
                PrintShortUsage();
            }

            string serverName = args[1];
            string host = null;
            string command = null;

            try
            {
                //evaluate parameters
                for (int i = 2; i < args.Length; i++)
                {
                    if (args[i].Equals("-h"))
                    {
                        //is next arg available?
                        if ((++i) < args.Length)
                        {
                            host = args[i];
                        }
                        else
                        {
                            Console.WriteLine("Please provide a hostname after the -h switch");
                            PrintShortUsage();
                        }
                    }
                    else if (args[i].Equals("-c"))
                    {
                        var sb = new StringBuilder();

                        for (int j = (i + 1); j < args.Length; j++)
                        {
                            sb.Append(args[j]);

                            if (j < (args.Length - 1))
                            {
                                sb.Append(' '); //append whitespace
                                                //don't append, if last arg
                            }
                        }

                        command = sb.ToString();
                        break; //definitely at end of args
                    }
                    else
                    {
                        Console.WriteLine("Unrecognized switch: " + args[i]);
                        PrintShortUsage();
                    }
                }

                if (host == null)
                {
                    host = GetLocalHostName();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                PrintUsage();
            }

            if (command == null)
            {
                command = "";
            }

            IAdmin admin = GetAdmin();

            try
            {
                if (args[0].Equals("add"))
                {
                    admin.RegisterServer(serverName, command, host);
                }
                else
                {
                    //else case already checked in main
                    admin.EditServer(serverName, command, host);
                }

                Console.WriteLine("Server " + serverName + " successfully " + args[0] + "ed");
                Console.WriteLine("Host: >>" + host + "<<");

                if (command.Length > 0)
                {
                    Console.WriteLine("Command: >>" + command + "<<");
                }
                else
                {
                    Console.WriteLine("No command specified. Server can't be restarted!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }

            System.Environment.Exit(0);
        }

        /// <summary>
        /// Remove a server or host from the repository.
        /// </summary>
        private static void Remove(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine(" Please specify if you want to remove a server or a host");
                PrintShortUsage();
            }
            if (args.Length == 2)
            {
                Console.WriteLine(" Please specify a servername / hostname");
                PrintShortUsage();
            }
            IAdmin admin = GetAdmin();
            if (args[1].Equals("server"))
            {
                try
                {
                    admin.UnregisterServer(args[2]);

                    Console.WriteLine("Server " + args[2] + " successfully removed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
            }
            else if (args[1].Equals("host"))
            {
                try
                {
                    admin.UnregisterHost(args[2]);

                    Console.WriteLine("Host " + args[2] + " successfully removed");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex}");
                }
            }
            else
            {
                Console.WriteLine("Unknown command " + args[1]);
                PrintShortUsage();
            }
            System.Environment.Exit(0);
        }

        /// <summary>
        /// List servers or hosts.
        /// </summary>
        private static void List(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("Please use (servers | hosts) in command");
                PrintShortUsage();
            }
            IAdmin admin = GetAdmin();
            try
            {
                if (args[1].Equals("servers"))
                {
                    ServerInfo[] _info = admin.ListServers();
                    Console.WriteLine("Servers (total: " + _info.Length + "):");
                    for (int _i = 0; _i < _info.Length; _i++)
                    {
                        Console.WriteLine((_i + 1) + ") " + _info[_i].Name);
                        Console.WriteLine("   " + "Host: " + _info[_i].Host);
                        Console.WriteLine("   " + "Command: " + _info[_i].Command);
                        Console.WriteLine("   " + "active: " + ((_info[_i].Active) ? "yes" : "no"));
                        Console.WriteLine("   " + "holding: " + ((_info[_i].Holding) ? "yes" : "no"));
                    }
                }
                else if (args[1].Equals("hosts"))
                {
                    HostInfo[] _info = admin.ListHosts();
                    Console.WriteLine("Hosts (total: " + _info.Length + "):");
                    for (int _i = 0; _i < _info.Length; _i++)
                    {
                        Console.WriteLine((_i + 1) + ") " + _info[_i].Name);
                    }
                }
                else
                {
                    Console.WriteLine("Unrecognized option: " + args[1]);
                    PrintShortUsage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            System.Environment.Exit(0);
        }

        private static void HoldServer(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("Please specify a server name");
                PrintShortUsage();
            }

            String serverName = args[1];
            int timeout = 0;

            IAdmin admin = GetAdmin();

            try
            {
                if (args.Length == 3)
                {
                    timeout = int.Parse(args[2]);
                }

                admin.HoldServer(serverName);
                Console.WriteLine("Server " + serverName + " set to holding");

                if (timeout > 0)
                {
                    Thread.Sleep(timeout);

                    admin.ReleaseServer(serverName);

                    Console.WriteLine("Server " + serverName + " released");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}"); ;
            }
            System.Environment.Exit(0);
        }

        private static void ReleaseServer(string[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("Please specify a server name");
                PrintShortUsage();
            }

            string serverName = args[1];            
            IAdmin admin = GetAdmin();
            try
            {
                admin.ReleaseServer(serverName);

                Console.WriteLine("Server " + serverName + " released");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");;
            }
            System.Environment.Exit(0);
        }

        private static void StartServer(String[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("Please specify a server name");
                PrintShortUsage();
            }
            string serverName = args[1];
            IAdmin admin = GetAdmin();
            try
            {
                admin.StartServer(serverName);

                Console.WriteLine("Server " + serverName + " started");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");;
            }
            System.Environment.Exit(0);
        }

        private static void SaveTable()
        {
            IAdmin admin = GetAdmin();
            try
            {
                admin.SaveServerTable();

                Console.WriteLine("Backup of server table was successfull");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");;
            }
            System.Environment.Exit(0);
        }

        private static void ShutdownImR(string[] args)
        {
            IAdmin admin = GetAdmin();
            bool wait = true;
            if (args.Length == 2)
            {
                if (args[1].ToLower().Equals("force"))
                {
                    wait = false;
                }
                else
                {
                    Console.WriteLine("Unrecognized option: " + args[1]);
                    Console.WriteLine("The only possible option is \"force\"");
                    PrintShortUsage();
                }
            }
            try
            {
                admin.Shutdown(wait);
                Console.WriteLine("The Implementation Repository has been shut down without exceptions");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");;
            }
            System.Environment.Exit(0);
        }

        private static void SetDown(String[] args)
        {
            if (args.Length == 1)
            {
                Console.WriteLine("Please specify a server name");
                PrintShortUsage();
            }
            IRegistration reg = RegistrationHelper.Narrow(GetAdmin());

            try
            {
                reg.SetServerDown(args[1]);

                Console.WriteLine("Server " + args[1] + " set down");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");;
            }
            System.Environment.Exit(0);
        }


        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
            }

            orb = (Core.ORB)CORBA.ORB.Init();

            logger = orb.Configuration.GetLogger("DotNetOrb.ImR.ImRManager");

            try
            {
                if (args[0].Equals("add") || args[0].Equals("edit"))
                    AddServer(args);
                else if (args[0].Equals("remove"))
                    Remove(args);
                else if (args[0].Equals("list"))
                    List(args);
                else if (args[0].Equals("hold"))
                    HoldServer(args);
                else if (args[0].Equals("release"))
                    ReleaseServer(args);
                else if (args[0].Equals("start"))
                    StartServer(args);
                else if (args[0].Equals("savetable"))
                    SaveTable();
                else if (args[0].Equals("shutdown"))
                    ShutdownImR(args);
                else if (args[0].Equals("setdown"))
                    SetDown(args);
                else if (args[0].Equals("help"))
                    PrintUsage();
                else
                {
                    Console.WriteLine("Unrecognized command: " + args[0]);
                    PrintUsage();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");;
                System.Environment.Exit(0);
            }
        }

        private static void PrintShortUsage()
        {
            Console.WriteLine("\nYour command has not been understood possibly due to\n" +
                               "one or more missing arguments");
            Console.WriteLine("Type \"imr_mg help\" to display the help screen");
            System.Environment.Exit(-1);
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: ImRManager <command> [<servername>] [switches]");
            Console.WriteLine("Command: (add | edit) <servername> [-h <hostname> -c <startup cmd>]");
            Console.WriteLine("\t -h <hostname> Restart server on this host");
            Console.WriteLine("\t -c <command> Restart server with this command");
            Console.WriteLine("\t If -h is not set, the local hosts name (that of the manager) is used.");
            Console.WriteLine("\t Note: The -c switch must always follow after the -h switch,");
            Console.WriteLine("\t because all arguments after -c are interpreted as the");
            Console.WriteLine("\t startup command.");

            Console.WriteLine("\nCommand: remove (server | host) <name>");
            Console.WriteLine("\t Removes the server or host <name> from the repository");

            Console.WriteLine("\nCommand: list (servers | hosts)");
            Console.WriteLine("\t Lists all servers or all hosts");

            Console.WriteLine("\nCommand: hold <servername> [<time>]");
            Console.WriteLine("\t Holds the server <servername> (if <time> is specified,");
            Console.WriteLine("\t it is released automatically)");

            Console.WriteLine("\nCommand: release <servername>");
            Console.WriteLine("\t Releases the server <servername>");

            Console.WriteLine("\nCommand: start <servername>");
            Console.WriteLine("\t Starts the server <servername> on its given host with " +
                               "its given command");

            Console.WriteLine("\nCommand: setdown <servername>");
            Console.WriteLine("\t Declares the server <servername> as \"down\" to the repository.");
            Console.WriteLine("\t This means that the repository tries to start the server up after ");
            Console.WriteLine("\t receiving the next request for it.");
            Console.WriteLine("\t This is actually an operation only committed by the ORB, but it");
            Console.WriteLine("\t might be useful for server migration and recovery of crashed servers.");
            Console.WriteLine("\t Note: Use \"hold\" before, to avoid the server being restarted at the");
            Console.WriteLine("\t wrong moment.");

            Console.WriteLine("\nCommand: savetable");
            Console.WriteLine("\t Makes a backup of the server table");

            Console.WriteLine("\nCommand: shutdown [force]");
            Console.WriteLine("\t Shuts the ImR down orderly. If \"force\" is specified, the ORB ");
            Console.WriteLine("\t is forced down, ignoring open connections.");

            Console.WriteLine("\nCommand: help");
            Console.WriteLine("\t This screen");

            System.Environment.Exit(1);
        }
    }
}
