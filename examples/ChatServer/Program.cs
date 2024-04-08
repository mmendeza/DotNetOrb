// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core;
using PortableServer;

namespace ChatServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing orb...");
            ORB orb = (ORB)ORB.Init();
            var poa = POAHelper.Narrow(orb.ResolveInitialReferences("RootPOA"));
            poa.ThePOAManager.Activate();

            CORBA.IObject o = poa.ServantToReference(new ChatServerImpl());
            var ior = orb.ObjectToString(o);
            using (StreamWriter writer = new StreamWriter("C:\\temp\\chatServer.ior"))
            {
                writer.WriteLine(ior);
            }
            Console.WriteLine("Server IOR: " + ior);            
            Console.WriteLine("Pulse any key to end server...");
            Console.ReadLine();
            orb.Shutdown(false);
        }
    }
}