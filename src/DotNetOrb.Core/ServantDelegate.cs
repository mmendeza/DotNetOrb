// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;
using System;
using static PortableServer.Current;
using static PortableServer.POA;


namespace DotNetOrb.Core
{
    public class ServantDelegate : PortableServer.IDelegate
    {
        private ORB orb;
        private IRepository ir = null;
        private PortableServer.ICurrent current = null;
        private IPOA poa = null;
        private object syncObject = new object();

        internal ServantDelegate(ORB orb)
        {
            this.orb = orb;
        }

        private void Check()
        {
            if (orb == null)
            {
                throw new BadInvOrder("The Servant has not been associated with an ORB instance");
            }
        }

        public CORBA.Object ThisObject(Servant self)
        {
            Check();

            try
            {
                poa = POA(self);
            }
            catch (ObjAdapter e)
            {
                // Use servants default POA. Operation may be re-implemented
                // by servant implementation.
                poa = self._DefaultPOA();
            }

            if (poa == null)
            {
                throw new ObjAdapter("null value returned by  _default_POA() on Servant " + self);
            }

            try
            {
                return (CORBA.Object)poa.ServantToReference(self);
            }
            catch (ServantNotActive e)
            {
                throw new ObjAdapter(e.ToString());
            }
            catch (WrongPolicy e)
            {
                throw new ObjAdapter(e.ToString());
            }
        }


        public CORBA.ORB ORB(Servant self)
        {
            Check();
            return orb;
        }

        public IPOA POA(Servant self)
        {
            Check();

            GetPOACurrent();

            try
            {
                if (current.GetServant() != self)
                {
                    throw new ObjAdapter();
                }

                return current.GetPoa();
            }
            catch (NoContext e)
            {
                throw new ObjAdapter(e.ToString());
            }
        }

        public byte[] ObjectId(Servant self)
        {
            Check();

            GetPOACurrent();

            try
            {
                return current.GetObjectId();
            }
            catch (NoContext e)
            {
                throw new ObjAdapter(e.ToString());
            }
        }

        private void GetPOACurrent()
        {
            lock (syncObject)
            {
                if (current == null)
                {
                    try
                    {
                        current = PortableServer.CurrentHelper.Narrow(orb.ResolveInitialReferences("POACurrent"));
                    }
                    catch (Exception e)
                    {
                        throw new Initialize(e.ToString());
                    }
                }
            }

        }

        public IPOA DefaultPOA(Servant self)
        {
            Check();
            try
            {
                return POAHelper.Narrow(ORB(self).ResolveInitialReferences("RootPOA"));
            }
            catch (InvalidName e)
            {
                throw new Initialize(e.ToString());
            }
        }

        public bool NonExistent(Servant self)
        {
            Check();
            return false;
        }

        public CORBA.Object GetComponent(Servant self)
        {
            Check();
            return null;
        }

        public CORBA.Object GetInterfaceDef(Servant self)
        {
            Check();
            if (ir == null)
            {
                try
                {
                    ir = RepositoryHelper.Narrow(orb.ResolveInitialReferences("InterfaceRepository"));
                }
                catch (Exception e)
                {
                    throw new Initialize(e.ToString());
                }
            }
            return (CORBA.Object)ir.LookupId(((CORBA.Object)self._ThisObject())._Ids()[0]);
        }

        public IInterfaceDef GetInterface(Servant self)
        {
            return InterfaceDefHelper.Narrow(GetInterfaceDef(self));
        }

        public bool IsA(Servant self, string repid)
        {
            string[] intf = self._AllInterfaces(POA(self), ObjectId(self));

            for (int i = 0; i < intf.Length; i++)
            {
                if (intf[i].Equals(repid))
                {
                    return true;
                }
            }
            return "IDL:omg.org/CORBA/Object:1.0".Equals(repid);
        }

        public IPolicy GetPolicy(CORBA.Object self, uint policyType)
        {
            return poa != null ? ((POA.POA)poa).GetPolicy(policyType) : null;
        }

        public IDomainManager[] GetDomainManagers(CORBA.Object self)
        {
            return null;
        }

        /// <summary>
        /// Similar to invoke in InvokeHandler, which is ultimately implement by skeletons.
        /// This method is used by the POA to handle operations that are "special", i.e.not implemented by skeletons
        /// </summary>
        /// <exception cref="BadParam">If the operation is not "special"</exception>
        public IOutputStream _Invoke(Servant self, string method, IInputStream inputStream, IResponseHandler handler)
        {
            IOutputStream outputStream = null;

            if ("_get_policy".Equals(method))
            {
                outputStream = handler.CreateReply();
                outputStream.WriteObject(GetPolicy((CORBA.Object)inputStream.ReadObject(), inputStream.ReadULong()));
            }
            else if ("_is_a".Equals(method))
            {
                outputStream = handler.CreateReply();
                outputStream.WriteBoolean(self._IsA(inputStream.ReadString()));
            }
            else if ("_interface".Equals(method))
            {
                outputStream = handler.CreateReply();
                outputStream.WriteObject(self._GetInterfaceDef());
            }
            else if ("_non_existent".Equals(method))
            {
                outputStream = handler.CreateReply();
                outputStream.WriteBoolean(self._NonExistent());
            }
            else if ("_get_component".Equals(method))
            {
                outputStream = handler.CreateReply();
                outputStream.WriteObject(self._GetComponent());
            }
            else if ("_repository_id".Equals(method))
            {
                outputStream = handler.CreateReply();
                outputStream.WriteString(self._RepositoryId());
            }
            else
            {
                throw new BadParam("Unknown operation: " + method);
            }
            return outputStream;
        }

        public string RepositoryId(Servant self)
        {
            return ThisObject(self)._RepositoryId();
        }
    }
}
