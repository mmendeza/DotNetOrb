/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:35
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Messaging
{
	[RepositoryID("IDL:omg.org/Messaging/Poller:3.1")]
	[Helper(typeof(PollerHelper))]
	public interface IPoller : CORBA.IValueBase, CORBA.IPollable
	{
		[IdlName("operation_target")]
		public CORBA.IObject OperationTarget 
		{
			get;
		}
		[IdlName("operation_name")]
		[WideChar(false)]
		public string OperationName 
		{
			get;
		}
		[IdlName("associated_handler")]
		public Messaging.IReplyHandler AssociatedHandler 
		{
			get;

			set;
		}
		[IdlName("is_from_poller")]
		public bool IsFromPoller 
		{
			get;
		}
	}
}
