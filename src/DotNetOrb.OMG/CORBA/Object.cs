// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORBA
{
    public class Object : IObject
    {
        private IDelegate _delegate;

        public IDelegate _Delegate
        {
            get
            {
                if (_delegate == null)
                {
                    throw new BadOperation();
                }
                return _delegate;
            }
            set
            {
                _delegate = value;
            }
        }
        protected string[] _ids = { "", "IDL:omg.org/CORBA/Object:1.0" };

        public virtual string[] _Ids()
        {
            return _ids;
        }

        public Object()
        {           
        }

        public Object(string typeId)
        {
            _ids[0] = typeId;
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

        public IObject _Duplicate()
        {
            return _Delegate.Duplicate(this);
        }

        public void _Release()
        {
            _Delegate.Release(this);
        }

        public bool _IsA(string repositoryId)
        {
            return _Delegate.IsA(this, repositoryId);
        }

        public bool _IsEquivalent(IObject other)
        {
            return _Delegate.IsEquivalent(this, (CORBA.Object) other);
        }

        public bool _NonExistent()
        {
            return _Delegate.NonExistent(this);
        }

        public int _Hash(int maximum)
        {
            return _Delegate.Hash(this, maximum);
        }

        public IRequest _Request(string operation)
        {
            return _Delegate.Request(this, operation);
        }

        public IOutputStream _Request(string operation, bool responseExpected)
        {
            return _Delegate.Request(this, operation, responseExpected);
        }

        [ThrowsIdlException(typeof(ApplicationException))]
        [ThrowsIdlException(typeof(RemarshalException))]
        public IInputStream _Invoke(IOutputStream output)
        {
            return _Delegate.Invoke(this, output);
        }

        [ThrowsIdlException(typeof(ApplicationException))]
        [ThrowsIdlException(typeof(RemarshalException))]
        public Task<IInputStream> _InvokeAsync(IOutputStream output)
        {
            return _Delegate.InvokeAsync(this, output);
        }

        public void _ReleaseReply(IInputStream input)
        {
            _Delegate.ReleaseReply(this, input);
        }

        public IRequest _CreateRequest(IContext ctx, string operation, NVList argList, NamedValue result)
        {
            return _Delegate.CreateRequest(this, ctx, operation, argList, result);
        }
        public IRequest _CreateRequest(IContext ctx, string operation, NVList argList, NamedValue result, ExceptionList excList, ContextList ctxList)
        {
            return _Delegate.CreateRequest(this, ctx, operation, argList, result, excList, ctxList);
        }

        public IPolicy _GetPolicy(uint policyType)
        {
            return _Delegate.GetPolicy(this, policyType);
        }

        public IDomainManager[] _GetDomainManagers()
        {
            return _Delegate.GetDomainManagers(this);
        }

        public IObject _SetPolicyOverrides(List<IPolicy> policies, SetOverrideType setAdd)
        {
            return _Delegate.SetPolicyOverrides(this, policies, setAdd);
        }

        public ORB _GetOrb()
        {
            return _Delegate.GetOrb(this);
        }

        public bool _IsLocal()
        {
            return _Delegate.IsLocal(this);
        }

        //public IServantObject _ServantPreinvoke(String operation, Type expectedType)
        //{
        //    return _Delegate.ServantPreinvoke(this, operation, expectedType);
        //}

        //public void _ServantPostinvoke(IServantObject servant)
        //{
        //    _Delegate.ServantPostinvoke(this, servant);
        //}

        public override string ToString()
        {
            if (_delegate != null)
                return _delegate.ToString(this);
            else
                return this.GetType().Name + ":no delegate set";
        }

        public override int GetHashCode()
        {
            if (_delegate != null)
                return _delegate.HashCode(this);
            else
                return base.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (_delegate != null)
                return _delegate.Equals(this, obj);
            else
                return (this == obj);
        }

        public IPolicy _GetClientPolicy(uint type)
        {
            return _Delegate.GetClientPolicy(this, type);
        }

        public IPolicy[] _GetPolicyOverrides(uint[] types)
        {
            return _Delegate.GetPolicyOverrides(this, types);
        }

        public bool _ValidateConnection(out List<IPolicy> inconsistentPolicies)
        {
            return _Delegate.ValidateConnection(this, out inconsistentPolicies);
        }

        public IObject _GetComponent()
        {
            return _Delegate.GetComponent(this);
        }

        public bool _IsNil()
        {
            return _Delegate.IsNil(this);
        }

    }
}
