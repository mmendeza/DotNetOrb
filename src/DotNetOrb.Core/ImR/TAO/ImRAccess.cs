// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.ETF;
using DotNetOrb.Core.IIOP;
using DotNetOrb.Core.Util;
using ETF;
using ImplementationRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrb.Core.ImR.TAO
{
    /// <summary>
    /// This ImR adapter class contains hooks for a DotNetOrb to register and unregister with a TAO ImR.
    /// </summary>
    public class ImRAccess : IImRAccess
    {
        private ImplementationRepository.IAdministration imrLocator = null;
        private ORB orb = null;
        private POA.POA poa = null;
        private POA.POA rootPOA = null;

        private ServerObject serverObj = null;
        private ParsedIOR pior = null;
        private IIOPProfile profile = null;
        private string corbaloc = null;

        public ImRAccess()
        {

        }

        /// <summary>
        /// Resolves the IMR and returns a new ImRAccess
        /// </summary>        
        /// <exception cref="CORBA.Internal"></exception>
        public static ImRAccess Connect(ORB orb)
        {
            var result = new ImRAccess();

            result.orb = orb;

            try
            {
                result.imrLocator = ImplementationRepository.AdministrationHelper.Narrow(orb.ResolveInitialReferences("ImplRepoService"));
            }
            catch (CORBA.InvalidName e)
            {
                throw new CORBA.Internal("ImRAccess.Connect: unable to resolve TAO ImplRepoService: " + e.ToString());
            }

            bool nonExist = true;
            if (result.imrLocator != null)
            {
                try
                {
                    nonExist = result.imrLocator._NonExistent();
                }
                catch (CORBA.SystemException)
                {
                    nonExist = true;
                }
            }

            if (nonExist)
            {
                throw new CORBA.Internal("ImRAccess.Connect: Unable to resolve reference to TAO ImplRepoService");
            }

            result.SetImRInfo();
            return result;
        }

        /// <summary>
        /// This function setup all informations that will be needed later
        /// </summary>   
        /// <exception cref="CORBA.Internal"></exception>
        private void SetImRInfo()
        {
            try
            {
                var reference = this.orb.ResolveInitialReferences("ImplRepoService");
                this.pior = new ParsedIOR(this.orb, this.orb.ObjectToString(reference));
                this.corbaloc = CorbaLoc.GenerateCorbalocForMultiIIOPProfiles(this.orb, reference);
                this.profile = (IIOPProfile)this.pior.GetEffectiveProfile();
            }
            catch (CORBA.InvalidName ex)
            {
                throw new CORBA.Internal("ImRAccess.setImRInfo: unable to resolve TAO ImplRepoService: " + ex.ToString());
            }
        }

        /// <returns>The primary address of the ImR of type ProtocolAddressBase</returns>
        public ProtocolAddressBase GetImRAddress()
        {
            if (this.profile == null)
            {
                SetImRInfo();
            }
            return this.profile.GetAddress();
        }

        /// <returns>the TAO ImR's corbaloc IOR string</returns>
        public string GetImRCorbaloc()
        {
            if (this.corbaloc == null)
            {
                SetImRInfo();
            }
            return this.corbaloc;
        }

        /// <summary>
        /// This function is just a place holder.
        /// </summary>
        /// <returns>null</returns>
        public string GetImRHost()
        {
            return null;
        }

        /// <summary>
        /// This function is just a place holder.
        /// </summary>
        /// <returns>-1</returns>
        public int GetImRPort()
        {
            return -1;
        }

        /// <returns>A list of the ImR profiles of type List&lt;Profile&gt;</returns>
        public List<IProfile> GetImRProfiles()
        {
            if (this.profile == null)
            {
                SetImRInfo();
            }
            return this.pior.Profiles;
        }

        /// <summary>
        /// This function is just a place holder.
        /// </summary>
        public void RegisterPOA(string name, string server, ProtocolAddressBase address)
        {
            
        }

        /// <summary>
        /// This function is just a place holder.
        /// </summary>
        public void RegisterPOA(string name, string server, string host, int port)
        {

        }

        /// <summary>
        /// This function provides a hook for registering the POA upon being created.
        /// </summary>
        /// <exception cref="CORBA.Internal"></exception>
        public void RegisterPOA(ORB orb, POA.POA poa, ProtocolAddressBase address, string implname)
        {
            if (address is IIOPAddress)
            {
                if (orb == null)
                {
                    throw new CORBA.Internal("ImRAccess.RegisterPOA: orb must not be null");
                }
                if (poa == null)
                {
                    throw new CORBA.Internal("ImRAccess.RegisterPOA: poa must not be null");
                }

                RegisterPOATaoImR(orb, poa, implname);
            }
            else
            {
                throw new CORBA.Internal("ImRAccess.RegisterPOA: TAO ImR only supports IIOP based POAs");
            }
        }

        private void RegisterPOATaoImR(ORB orb, POA.POA poa, string implname)
        {
            this.orb = orb;
            this.poa = poa;

            // build the server name for registering with the ImR in the form 
            // DOTNETORB:<implName>/<qualified POA name>
            string theName = "DOTNETORB:" + implname + "/" + this.poa.QualifiedName;

            try
            {
                // instantiate an instance of ServerObjectImpl.
                // org.jacorb.poa.POA root_poa = this.orb_.getRootPOA();
                this.rootPOA = this.orb.GetRootPOA();
                this.serverObj = new ServerObject(this.orb, this.poa, this.rootPOA);
                if (this.serverObj == null)
                {
                    throw new CORBA.Internal("ImRAccess.RegisterPOATaoImR: can't create an instance of ServerObject");
                }

                // activate it in rootPOA
                this.rootPOA.ActivateObject(this.serverObj);
                var objRef = this.rootPOA.ServantToReference(this.serverObj);
                string corbaLoc = CorbaLoc.GenerateCorbalocForMultiIIOPProfiles(this.orb, objRef);
                int slash = corbaLoc.IndexOf("/");

                // save the endpoint part and discard the object_key part
                string partialCorbaLoc = corbaLoc;
                if (slash > 0)
                {
                    partialCorbaLoc = corbaLoc.Substring(0, slash + 1);
                }

                IServerObject svr = ServerObjectHelper.Narrow(objRef);

                // notfy TAO ImR that we are running
                this.imrLocator.ServerIsRunning(theName, partialCorbaLoc, svr);

            }
            catch (Exception ex)
            {                
                throw new CORBA.Internal("ImRAccess.RegisterPOATaoImR: got an exception while registering " + theName + " with TAO ImR, " + ex.ToString());
            }
        }

        public void SetServerDown(string name)
        {
        }

        /// <summary>
        /// This function will unregister the server from the TAO ImR by calling 
        /// the TAO ImR locator's server_is_shutting_down function.
        /// </summary>        
        public void SetServerDown(ORB orb, POA.POA poa, string implname)
        {
            if (this.imrLocator != null)
            {
                try
                {
                    // notify ImR of being shutdown
                    this.imrLocator.ServerIsShuttingDown(poa.QualifiedName);
                }
                catch (Exception)
                {                    
                }

                // deactivate the ServerObjectImpl
                if (this.serverObj != null)
                {
                    try
                    {
                        // get root_poa from the ServerObject
                        var rootPOA = this.serverObj._DefaultPOA();
                        if (rootPOA != null)
                        {
                            byte[] id = rootPOA.ServantToId(this.serverObj);
                            rootPOA.DeactivateObject(id);
                            this.serverObj = null;
                        }
                    }
                    catch (Exception e2)
                    {                        
                        // force to exit
                        System.Environment.Exit(1);
                    }
                }
            }
        }
    }
}
