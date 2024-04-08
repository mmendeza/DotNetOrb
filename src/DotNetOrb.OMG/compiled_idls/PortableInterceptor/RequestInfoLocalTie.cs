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

	public class RequestInfoLocalTie: _RequestInfoLocalBase
	{
		public IRequestInfoOperations _OperationsDelegate { get; set; }

		public RequestInfoLocalTie(IRequestInfoOperations d)
		{
			_OperationsDelegate = d;
		}

		public override uint RequestId 
		{
			get
			{
				return _OperationsDelegate.RequestId;
			}
		}
		[WideChar(false)]
		public override string Operation 
		{
			get
			{
				return _OperationsDelegate.Operation;
			}
		}
		public override Dynamic.Parameter[] Arguments 
		{
			get
			{
				return _OperationsDelegate.Arguments;
			}
		}
		public override CORBA.TypeCode[] Exceptions 
		{
			get
			{
				return _OperationsDelegate.Exceptions;
			}
		}
		public override string[] Contexts 
		{
			get
			{
				return _OperationsDelegate.Contexts;
			}
		}
		public override string[] OperationContext 
		{
			get
			{
				return _OperationsDelegate.OperationContext;
			}
		}
		public override CORBA.Any Result 
		{
			get
			{
				return _OperationsDelegate.Result;
			}
		}
		public override bool ResponseExpected 
		{
			get
			{
				return _OperationsDelegate.ResponseExpected;
			}
		}
		public override short SyncScope 
		{
			get
			{
				return _OperationsDelegate.SyncScope;
			}
		}
		public override short ReplyStatus 
		{
			get
			{
				return _OperationsDelegate.ReplyStatus;
			}
		}
		public override CORBA.IObject ForwardReference 
		{
			get
			{
				return _OperationsDelegate.ForwardReference;
			}
		}
		[IdlName("get_slot")]
		[ThrowsIdlException(typeof(PortableInterceptor.InvalidSlot))]
		public override CORBA.Any GetSlot(uint id)
		{
			return _OperationsDelegate.GetSlot(id);
		}
		[IdlName("get_request_service_context")]
		public override IOP.ServiceContext GetRequestServiceContext(uint id)
		{
			return _OperationsDelegate.GetRequestServiceContext(id);
		}
		[IdlName("get_reply_service_context")]
		public override IOP.ServiceContext GetReplyServiceContext(uint id)
		{
			return _OperationsDelegate.GetReplyServiceContext(id);
		}
	}
}
