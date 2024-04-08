/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 26/02/2024 12:18:44
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Chat
{
	public interface IServerOperations
	{
		[IdlName("Name")]
		[WideChar(false)]
		public string Name 
		{
			get;
		}
		[IdlName("RegisterUser")]
		[ThrowsIdlException(typeof(Chat.MaxUsersReached))]
		public bool RegisterUser(Chat.IClient clientRef, out int sessionId);
		[IdlName("UnregisterUser")]
		public void UnregisterUser(int sessionId);
		[IdlName("BroadcastMessage")]
		[ThrowsIdlException(typeof(Chat.UserNotAuthenticated))]
		public void BroadcastMessage(int sessionId, Chat.Message msg);
	}
}

