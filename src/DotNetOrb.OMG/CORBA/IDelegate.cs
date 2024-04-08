// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORBA
{
    public interface IDelegate
    {        
        IInterfaceDef GetInterface(CORBA.Object self);

        CORBA.Object GetInterfaceDef(CORBA.Object self);
        
        string RepositoryId(CORBA.Object self);

        CORBA.Object Duplicate(CORBA.Object self);

        void Release(CORBA.Object self);

        bool IsA(CORBA.Object self, string repositoryId);

        bool NonExistent(CORBA.Object self);

        bool IsEquivalent(CORBA.Object self, CORBA.Object rhs);

        int Hash(CORBA.Object self, int max);

        IRequest CreateRequest(CORBA.Object self, IContext ctx, string operation, NVList argList, NamedValue result);

        IRequest CreateRequest(CORBA.Object self, IContext ctx, string operation, NVList argList, NamedValue result, ExceptionList exclist, ContextList ctxlist);

        IRequest Request(CORBA.Object self, string operation);

        IOutputStream Request(CORBA.Object self, string operation, bool responseExpected);

        [ThrowsIdlException(typeof(ApplicationException))]
        [ThrowsIdlException(typeof(RemarshalException))]
        IInputStream Invoke(CORBA.Object self, IOutputStream os);

        [ThrowsIdlException(typeof(ApplicationException))]
        [ThrowsIdlException(typeof(RemarshalException))]
        Task<IInputStream> InvokeAsync(CORBA.Object self, IOutputStream os);

        void ReleaseReply(CORBA.Object self, IInputStream inputStream);
        
        IDomainManager[] GetDomainManagers(CORBA.Object self);               
        IPolicy GetPolicy(CORBA.Object self, uint policyType);
        IPolicy[] GetPolicyOverrides(CORBA.Object self, uint[] types);
        IPolicy GetClientPolicy(CORBA.Object self, uint policyType);
        CORBA.Object SetPolicyOverrides(CORBA.Object self, List<IPolicy> policies, SetOverrideType setAdd);
        bool ValidateConnection(CORBA.Object self, out List<IPolicy> inconsistentPolicies);
        CORBA.Object GetComponent(CORBA.Object self);
        CORBA.ORB GetOrb(CORBA.Object self);
        bool IsLocal(CORBA.Object self);

        //IServantObject ServantPreinvoke(CORBA.Object self, string operation, Type expectedType);

        //void ServantPostinvoke(CORBA.Object self, IServantObject servant);

        string ToString(CORBA.Object self);

        int HashCode(CORBA.Object self);

        bool Equals(CORBA.Object self, System.Object obj);

        bool IsNil(CORBA.Object self);

    }
}
