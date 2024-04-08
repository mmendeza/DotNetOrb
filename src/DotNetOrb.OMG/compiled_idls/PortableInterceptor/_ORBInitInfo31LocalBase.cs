/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:37
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableInterceptor
{
	public abstract class _ORBInitInfo31LocalBase: CORBA.LocalObject, IORBInitInfo31
	{
		private string[] _ids = {"IDL:PortableInterceptor/ORBInitInfo_3_1:1.0","IDL:PortableInterceptor/ORBInitInfo:1.0"};

		public override string[] _Ids()
		{
			return _ids;
		}

		[IdlName("arguments")]
		public abstract string[] Arguments 
		{
			get;
		}
		[IdlName("orb_id")]
		[WideChar(false)]
		public abstract string OrbId 
		{
			get;
		}
		[IdlName("codec_factory")]
		public abstract IOP.ICodecFactory CodecFactory 
		{
			get;
		}
		[IdlName("register_initial_reference")]
		[ThrowsIdlException(typeof(PortableInterceptor.ORBInitInfo.InvalidName))]
		public abstract void RegisterInitialReference([WideChar(false)] string id, CORBA.IObject obj);
		[IdlName("resolve_initial_references")]
		[ThrowsIdlException(typeof(PortableInterceptor.ORBInitInfo.InvalidName))]
		public abstract CORBA.IObject ResolveInitialReferences([WideChar(false)] string id);
		[IdlName("add_client_request_interceptor")]
		[ThrowsIdlException(typeof(PortableInterceptor.ORBInitInfo.DuplicateName))]
		public abstract void AddClientRequestInterceptor(PortableInterceptor.IClientRequestInterceptor interceptor);
		[IdlName("add_server_request_interceptor")]
		[ThrowsIdlException(typeof(PortableInterceptor.ORBInitInfo.DuplicateName))]
		public abstract void AddServerRequestInterceptor(PortableInterceptor.IServerRequestInterceptor interceptor);
		[IdlName("add_ior_interceptor")]
		[ThrowsIdlException(typeof(PortableInterceptor.ORBInitInfo.DuplicateName))]
		public abstract void AddIorInterceptor(PortableInterceptor.IIORInterceptor interceptor);
		[IdlName("allocate_slot_id")]
		public abstract uint AllocateSlotId();
		[IdlName("register_policy_factory")]
		public abstract void RegisterPolicyFactory(uint type, PortableInterceptor.IPolicyFactory policyFactory);
		[IdlName("add_client_request_interceptor_with_policy")]
		[ThrowsIdlException(typeof(PortableInterceptor.ORBInitInfo.DuplicateName))]
		[ThrowsIdlException(typeof(CORBA.PolicyError))]
		public abstract void AddClientRequestInterceptorWithPolicy(PortableInterceptor.IClientRequestInterceptor interceptor, CORBA.IPolicy[] policies);
		[IdlName("add_server_request_interceptor_with_policy")]
		[ThrowsIdlException(typeof(PortableInterceptor.ORBInitInfo.DuplicateName))]
		[ThrowsIdlException(typeof(CORBA.PolicyError))]
		public abstract void AddServerRequestInterceptorWithPolicy(PortableInterceptor.IServerRequestInterceptor interceptor, CORBA.IPolicy[] policies);
		[IdlName("add_ior_interceptor_with_policy")]
		[ThrowsIdlException(typeof(PortableInterceptor.ORBInitInfo.DuplicateName))]
		[ThrowsIdlException(typeof(CORBA.PolicyError))]
		public abstract void AddIorInterceptorWithPolicy(PortableInterceptor.IIORInterceptor interceptor, CORBA.IPolicy[] policies);
	}

}
