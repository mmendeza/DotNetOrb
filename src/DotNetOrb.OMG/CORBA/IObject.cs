// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CORBA
{
    public interface IObject
    {
        IRequest _CreateRequest(IContext ctx, string operation, NVList argList, NamedValue result);
        IRequest _CreateRequest(IContext ctx, string operation, NVList argList, NamedValue result, ExceptionList excList, ContextList ctxList);
        IObject _Duplicate();
        IPolicy _GetClientPolicy(uint type);
        IObject _GetComponent();
        IDomainManager[] _GetDomainManagers();
        IInterfaceDef _GetInterface();
        IObject _GetInterfaceDef();
        ORB _GetOrb();
        IPolicy _GetPolicy(uint policyType);
        IPolicy[] _GetPolicyOverrides(uint[] types);
        int _Hash(int maximum);
        string[] _Ids();
        IInputStream _Invoke(IOutputStream output);        
        Task<IInputStream> _InvokeAsync(IOutputStream output);
        bool _IsA(string repositoryId);
        bool _IsEquivalent(IObject other);
        bool _IsLocal();
        bool _IsNil();
        bool _NonExistent();
        void _Release();
        void _ReleaseReply(IInputStream input);
        string _RepositoryId();
        IRequest _Request(string operation);
        IOutputStream _Request(string operation, bool responseExpected);
        IObject _SetPolicyOverrides(List<IPolicy> policies, SetOverrideType setAdd);
        bool _ValidateConnection(out List<IPolicy> inconsistentPolicies);
        //IServantObject _ServantPreinvoke(String operation, Type expectedType);
        //void _ServantPostinvoke(IServantObject servant);
    }
}