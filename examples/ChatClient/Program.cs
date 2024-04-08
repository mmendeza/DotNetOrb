// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Chat;
using DotNetOrb.Core;
using PortableServer;

namespace ChatClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var clientName = "Client";
            if (args.Length > 0)
            {
                clientName = args[0];
            }
            Console.WriteLine("Initializing orb...");
            ORB orb = (ORB)ORB.Init();
            var poa = POAHelper.Narrow(orb.ResolveInitialReferences("RootPOA"));
            poa.ThePOAManager.Activate();
            var client = new ChatClientImpl(clientName);
            CORBA.IObject o = poa.ServantToReference(client);
            string serverIOR = File.ReadAllText("C:\\temp\\chatServer.ior");
            var chatServer = ServerHelper.Narrow(orb.StringToObject(serverIOR));            

            Console.WriteLine("Registering server...");
            try
            {
                if (chatServer.RegisterUser(client._This(orb), out int sessionId))
                {
                    Console.WriteLine("Registered with session Id: " + sessionId);
                    Console.WriteLine("To send a message type message and optionally :color");
                    Console.WriteLine("To unregister type \"exit\"");
                    while (true)
                    {
                        Console.Write($"{clientName}>");
                        var input = Console.ReadLine();
                        if (input != null)
                        {
                            if (input.Equals("exit"))
                            {
                                break;
                            }
                            var parts = input.Split(":");
                            if (parts.Length > 1)
                            {
                                var message = new Message(clientName, (ColorEnum)Enum.Parse(typeof(ColorEnum), parts[1], true), parts[0]);
                                chatServer.BroadcastMessage(sessionId, message);
                            }
                            else
                            {
                                var message = new Message(clientName, ColorEnum.Default, input);
                                chatServer.BroadcastMessage(sessionId, message);
                            }
                        }
                    }
                    Console.WriteLine("Unregistering server...");
                    chatServer.UnregisterUser(sessionId);
                    Console.WriteLine("Unregistered");
                }
                else
                {
                    Console.WriteLine("Unable to register to char server!");
                }
            }
            catch (MaxUsersReached) 
            {
                Console.WriteLine("Max users reached");
            }            
            orb.Shutdown(false);
        }
    }
}