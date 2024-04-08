/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:37
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace DotNetOrb.ImR
{

	public class ImplementationRepositoryPOATie: ImplementationRepositoryPOA
	{
		public IImplementationRepositoryOperations _OperationsDelegate { get; set; }
		private PortableServer.IPOA _poa;

		public ImplementationRepositoryPOATie(IImplementationRepositoryOperations d)
		{
			_OperationsDelegate = d;
		}

		public ImplementationRepositoryPOATie(IImplementationRepositoryOperations d, PortableServer.POA poa)
		{
			_OperationsDelegate = d;
			_poa = poa;
		}

		public override PortableServer.IPOA _DefaultPOA()
		{
			if (_poa != null)
			{
				return _poa;
			}
			return base._DefaultPOA();
		}

		public override DotNetOrb.ImR.IImplementationRepository _This()
		{
			return DotNetOrb.ImR.ImplementationRepositoryHelper.Narrow(_ThisObject());
		}

		public override DotNetOrb.ImR.IImplementationRepository _This(CORBA.ORB orb)
		{
			return DotNetOrb.ImR.ImplementationRepositoryHelper.Narrow(_ThisObject(orb));
		}

		[IdlName("register_host")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Registration.IllegalHostName))]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Registration.InvalidSSDRef))]
		public override void RegisterHost(DotNetOrb.ImR.HostInfo info)
		{
			_OperationsDelegate.RegisterHost(info);
		}
		[IdlName("set_server_down")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.UnknownServerName))]
		public override void SetServerDown([WideChar(false)] string name)
		{
			_OperationsDelegate.SetServerDown(name);
		}
		[IdlName("register_poa")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Registration.IllegalPOAName))]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Registration.DuplicatePOAName))]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.UnknownServerName))]
		public override void RegisterPoa([WideChar(false)] string name, [WideChar(false)] string server, [WideChar(false)] string host, uint port)
		{
			_OperationsDelegate.RegisterPoa(name, server, host, port);
		}
		[IdlName("get_imr_info")]
		public override DotNetOrb.ImR.ImRInfo GetImrInfo()
		{
			return _OperationsDelegate.GetImrInfo();
		}
		[IdlName("list_hosts")]
		public override DotNetOrb.ImR.HostInfo[] ListHosts()
		{
			return _OperationsDelegate.ListHosts();
		}
		[IdlName("list_servers")]
		public override DotNetOrb.ImR.ServerInfo[] ListServers()
		{
			return _OperationsDelegate.ListServers();
		}
		[IdlName("get_server_info")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.UnknownServerName))]
		public override DotNetOrb.ImR.ServerInfo GetServerInfo([WideChar(false)] string name)
		{
			return _OperationsDelegate.GetServerInfo(name);
		}
		[IdlName("shutdown")]
		public override void Shutdown(bool wait)
		{
			_OperationsDelegate.Shutdown(wait);
		}
		[IdlName("save_server_table")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Admin.FileOpFailed))]
		public override void SaveServerTable()
		{
			_OperationsDelegate.SaveServerTable();
		}
		[IdlName("register_server")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Admin.IllegalServerName))]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Admin.DuplicateServerName))]
		public override void RegisterServer([WideChar(false)] string name, [WideChar(false)] string command, [WideChar(false)] string host)
		{
			_OperationsDelegate.RegisterServer(name, command, host);
		}
		[IdlName("unregister_server")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.UnknownServerName))]
		public override void UnregisterServer([WideChar(false)] string name)
		{
			_OperationsDelegate.UnregisterServer(name);
		}
		[IdlName("edit_server")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.UnknownServerName))]
		public override void EditServer([WideChar(false)] string name, [WideChar(false)] string command, [WideChar(false)] string host)
		{
			_OperationsDelegate.EditServer(name, command, host);
		}
		[IdlName("hold_server")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.UnknownServerName))]
		public override void HoldServer([WideChar(false)] string name)
		{
			_OperationsDelegate.HoldServer(name);
		}
		[IdlName("release_server")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.UnknownServerName))]
		public override void ReleaseServer([WideChar(false)] string name)
		{
			_OperationsDelegate.ReleaseServer(name);
		}
		[IdlName("start_server")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.UnknownServerName))]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.ServerStartupFailed))]
		public override void StartServer([WideChar(false)] string name)
		{
			_OperationsDelegate.StartServer(name);
		}
		[IdlName("unregister_host")]
		[ThrowsIdlException(typeof(DotNetOrb.ImR.Admin.UnknownHostName))]
		public override void UnregisterHost([WideChar(false)] string name)
		{
			_OperationsDelegate.UnregisterHost(name);
		}
	}
}