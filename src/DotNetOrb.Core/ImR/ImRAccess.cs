// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.ETF;
using DotNetOrb.Core.IIOP;
using DotNetOrb.Core.ImR;
using DotNetOrb.Core.POA;
using DotNetOrb.ImR;
using ETF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DotNetOrb.ImR.Registration;

namespace DotNetOrb.Core.ImR
{
    public class ImRAccess : IImRAccess
    {
        private IRegistration reg = null;
        private ImRInfo info = null;

        /// <summary>
        /// Resolves the IMR and returns a new ImRAccess
        /// </summary>
        /// <exception cref="CORBA.Internal"></exception>
        public static ImRAccess Connect(CORBA.ORB orb)
        {
            var result = new ImRAccess();
            try
            {
                result.reg = RegistrationHelper.Narrow(orb.ResolveInitialReferences("ImplementationRepository"));
            }
            catch (InvalidName e)
            {
                throw new CORBA.Internal("Unable to resolve ImplementationRepository: " + e.ToString());
            }
            bool nonExist = true;
            if (result.reg != null)
            {
                try
                {
                    nonExist = result.reg._NonExistent();
                }
                catch (CORBA.SystemException)
                {
                    nonExist = true;
                }
            }
            if (nonExist)
            {
                throw new CORBA.Internal("Unable to resolve reference to ImR");
            }
            return result;
        }

        public ProtocolAddressBase GetImRAddress()
        {
            if (info == null)
            {
                info = reg.GetImrInfo();
            }
            return new IIOPAddress(info.Host, (int)info.Port);
        }

        public string GetImRCorbaloc()
        {
            return null;
        }

        public string GetImRHost()
        {
            if (info == null)
            {
                info = reg.GetImrInfo();
            }

            return info.Host;
        }

        public int GetImRPort()
        {
            if (info == null)
            {
                info = reg.GetImrInfo();
            }

            return (int)info.Port;
        }

        public List<IProfile> GetImRProfiles()
        {
            return null;
        }

        public void RegisterPOA(string name, string server, ProtocolAddressBase address)
        {
            if (address is IIOPAddress iiopAddress)
            {
                RegisterPOA(name, server, iiopAddress.HostName, iiopAddress.Port);
            }
            else
            {
                throw new CORBA.Internal("IMR only supports IIOP based POAs");
            }
        }

        public void RegisterPOA(string name, string server, string host, int port)
        {
            try
            {
                reg.RegisterPoa(name, server, host, (uint)port);
            }
            catch (DuplicatePOAName e)
            {
                throw new CORBA.Internal("A server with the same combination of ImplName/POA-Name (" + name +
                                    ") is already registered and listed as active at the imr: " + e.ToString());
            }
            catch (IllegalPOAName e)
            {
                throw new CORBA.Internal("The ImR replied that the POA name >>" +
                                    e.Name + "<< is illegal: " + e.ToString());
            }
            catch (UnknownServerName e)
            {
                throw new CORBA.Internal("The ImR replied that the server name >>" +
                                    e.Name + "<< is unknown: " + e.ToString());
            }
        }

        public void RegisterPOA(Core.ORB orb, POA.POA poa, ProtocolAddressBase address, string implname)
        {
        }

        public void SetServerDown(string name)
        {
            try
            {
                reg.SetServerDown(name);
            }
            catch (UnknownServerName e)
            {
                throw new CORBA.Internal("The ImR replied that a server with name " + name + " is unknown: " + e.ToString());
            }
        }

        public void SetServerDown(Core.ORB orb, POA.POA poa, string implname)
        {
        }

    }
}
