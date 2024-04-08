// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ImplementationRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrb.Core.ImR.TAO
{
    /// <summary>
    /// This is the implementation of the ServerObject interface, which serves as a callback 
    /// when the ORB is registered with a TAO ImR. It allows the TAO ImR to ping a DotNetOrb server 
    /// and shutdown the server when needed.
    /// </summary>
    public class ServerObject : ServerObjectPOA
    {
        private CORBA.ORB orb = null;
        PortableServer.IPOA poa = null;
        PortableServer.IPOA rootPOA = null;
        Core.ILogger logger;

        public ServerObject(CORBA.ORB orb, PortableServer.IPOA poa, PortableServer.IPOA rootPOA)
        {
            this.orb = orb;
            this.poa = poa;
            this.rootPOA = rootPOA; // This is rootPOA
        }
        public override void Ping()
        {
            throw new NotImplementedException();
        }

        public override void Shutdown()
        {
            try
            {
                // Note : We want our child POAs to be able to unregister themselves from
                // the ImR, so we must destroy them before shutting down the orb.
                if (poa is POA.POA)
                {
                    ((POA.POA)this.rootPOA).Destroy(true, false);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                orb.Shutdown(false);
            }
            catch (Exception)
            {
            }
        }
    }
}
