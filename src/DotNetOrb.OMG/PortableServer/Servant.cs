// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;

namespace PortableServer
{
    abstract public class Servant
    {
        private IDelegate _delegate;

        public IDelegate _Delegate
        {
            get
            {
                if (_delegate == null)
                {
                    throw new BadInvOrder("The Servant has not been associated with an ORBinstance");
                }
                return _delegate;
            }
            set
            {
                _delegate = value;
            }
        }

        public IObject _ThisObject()
        {
            return _Delegate.ThisObject(this);
        }

        public IObject _ThisObject(ORB orb)
        {
            orb.SetDelegate(this);
            return _ThisObject();
        }

        public CORBA.ORB _ORB()
        {
            return _Delegate.ORB(this);
        }

        public IPOA _POA()
        {
            return _Delegate.POA(this);
        }

        public byte[] _ObjectId()
        {
            return _Delegate.ObjectId(this);
        }

        public virtual IPOA _DefaultPOA()
        {
            return _Delegate.DefaultPOA(this);
        }

        public bool _IsA(string repositoryId)
        {
            return _Delegate.IsA(this, repositoryId);
        }        

        public bool _NonExistent()
        {
            return _Delegate.NonExistent(this);
        }

        public IObject _GetComponent()
        {
            return _Delegate.GetComponent(this);
        }

        public IInterfaceDef _GetInterface()
        {
            return _Delegate.GetInterface(this);
        }

        public IObject _GetInterfaceDef()
        {
            return _Delegate.GetInterfaceDef(this);
        }

        public string _RepositoryId()
        {
            return _Delegate.RepositoryId(this);
        }

        public abstract string[] _AllInterfaces(IPOA poa, byte[] objectId);
        
    }
}
