// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORBA
{
    public abstract class LocalObject : IObject
    {        

        private ORB orb;

        public ORB _GetOrb()
        {
            if (orb == null)
                throw new ObjectNotExist(0, CompletionStatus.No);
            return orb;
        }

        public LocalObject()
        { 
        }

        public LocalObject(ORB orb)
        {
            this.orb = orb;
        }

        public abstract string[] _Ids();
        
        
        public string _RepositoryId()
        {
            var ids = _Ids();
            return ids[0];
        }

        public bool _IsA(string repositoryId)
        {
            var ids = _Ids();
            for (int i = 0; i < ids.Length; i++)
            {
                if (repositoryId.Equals(ids[i]))
                    return true;
            }
            return false;
        }

        public bool _IsEquivalent(IObject other)
        {
            return Equals(other);
        }

        public bool _NonExistent()
        {
            return false;
        }

        public int _Hash(int maximum)
        {
            return GetHashCode();
        }

        public IObject _Duplicate()
        {
            throw new NoImplement();
        }

        public IPolicy _GetClientPolicy(int type)
        {
            throw new NoImplement();
        }

        public IObject _GetComponent()
        {
            throw new NoImplement();
        }

        public IDomainManager[] _GetDomainManagers()
        {
            throw new NoImplement();
        }

        public IInterfaceDef _GetInterface()
        {
            throw new NoImplement();
        }

        public IPolicy _GetPolicy(int policy_type)
        {
            throw new NoImplement();
        }

        public IPolicy[] _GetPolicyOverrides(int[] types)
        {
            throw new NoImplement();
        }

        public bool _IsNil()
        {
            throw new NoImplement();
        }

        public void _Release()
        {
            throw new NoImplement();
        }

        public IObject _SetPolicyOverrides(IPolicy[] policies, SetOverrideType set_add)
        {
            throw new NoImplement();
        }

        public bool _ValidateConnection(out IPolicy[] inconsistentPolicies)
        {
            throw new NoImplement();
        }

        public IObject _GetInterfaceDef()
        {
            throw new NoImplement();
        }

        public IRequest _CreateRequest(IContext ctx, string operation, NVList argList, NamedValue result)
        {
            throw new NoImplement();
        }

        public IRequest _CreateRequest(IContext ctx, string operation, NVList argList, NamedValue result, ExceptionList excList, ContextList ctxList)
        {
            throw new NoImplement();
        }

        public IObject _SetPolicyOverrides(List<IPolicy> policies, SetOverrideType setAdd)
        {
            throw new NoImplement();
        }

        public bool _ValidateConnection(out List<IPolicy> inconsistentPolicies)
        {
            throw new NoImplement();
        }

        public IInputStream _Invoke(IOutputStream output)
        {
            throw new NoImplement();
        }

        public Task<IInputStream> _InvokeAsync(IOutputStream output)
        {
            throw new NoImplement();
        }

        public bool _IsLocal()
        {
            throw new NoImplement();
        }

        public void _ReleaseReply(IInputStream input)
        {
            throw new NoImplement();
        }

        public IRequest _Request(string operation)
        {
            throw new NoImplement();
        }

        public IOutputStream _Request(string operation, bool responseExpected)
        {
            throw new NoImplement();
        }

        public IPolicy _GetClientPolicy(uint type)
        {
            throw new NoImplement();
        }

        public IPolicy _GetPolicy(uint policyType)
        {
            throw new NoImplement();
        }

        public IPolicy[] _GetPolicyOverrides(uint[] types)
        {
            throw new NoImplement();
        }

        //public IServantObject _ServantPreinvoke(string operation, Type expectedType)
        //{
        //    throw new NoImplement();
        //}

        //public void _ServantPostinvoke(IServantObject servant)
        //{
        //    throw new NoImplement();
        //}
    }
}
