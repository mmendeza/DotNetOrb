/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:38
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace ImplementationRepository
{
	public class _AdministrationExtStub: CORBA.Object, IAdministrationExt
	{
		private new string[] _ids = {"IDL:ImplementationRepository/AdministrationExt:1.0","IDL:ImplementationRepository/Administration:1.0"};

		public override string[] _Ids()
		{
			return _ids;
		}

		public static Type _opsType = typeof(IAdministrationExtOperations);

		[IdlName("activate_server")]
		[ThrowsIdlException(typeof(ImplementationRepository.NotFound))]
		[ThrowsIdlException(typeof(ImplementationRepository.CannotActivate))]
		public void ActivateServer([WideChar(false)] string server)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("activate_server", true);
					outputStream.WriteString(server);
					inputStream = _Invoke(outputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							case "IDL:ImplementationRepository/NotFound:1.0":
								throw ImplementationRepository.NotFoundHelper.Read(aex.InputStream);
							case "IDL:ImplementationRepository/CannotActivate:1.0":
								throw ImplementationRepository.CannotActivateHelper.Read(aex.InputStream);
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("add_or_update_server")]
		[ThrowsIdlException(typeof(ImplementationRepository.NotFound))]
		public void AddOrUpdateServer([WideChar(false)] string server, ImplementationRepository.StartupOptions options)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("add_or_update_server", true);
					outputStream.WriteString(server);
					ImplementationRepository.StartupOptionsHelper.Write(outputStream, options);
					inputStream = _Invoke(outputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							case "IDL:ImplementationRepository/NotFound:1.0":
								throw ImplementationRepository.NotFoundHelper.Read(aex.InputStream);
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("remove_server")]
		[ThrowsIdlException(typeof(ImplementationRepository.NotFound))]
		public void RemoveServer([WideChar(false)] string server)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("remove_server", true);
					outputStream.WriteString(server);
					inputStream = _Invoke(outputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							case "IDL:ImplementationRepository/NotFound:1.0":
								throw ImplementationRepository.NotFoundHelper.Read(aex.InputStream);
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("shutdown_server")]
		[ThrowsIdlException(typeof(ImplementationRepository.NotFound))]
		public void ShutdownServer([WideChar(false)] string server)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("shutdown_server", true);
					outputStream.WriteString(server);
					inputStream = _Invoke(outputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							case "IDL:ImplementationRepository/NotFound:1.0":
								throw ImplementationRepository.NotFoundHelper.Read(aex.InputStream);
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("server_is_running")]
		[ThrowsIdlException(typeof(ImplementationRepository.NotFound))]
		public void ServerIsRunning([WideChar(false)] string server, [WideChar(false)] string partialIor, ImplementationRepository.IServerObject serverObject)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("server_is_running", true);
					outputStream.WriteString(server);
					outputStream.WriteString(partialIor);
					ImplementationRepository.ServerObjectHelper.Write(outputStream, serverObject);
					inputStream = _Invoke(outputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							case "IDL:ImplementationRepository/NotFound:1.0":
								throw ImplementationRepository.NotFoundHelper.Read(aex.InputStream);
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("server_is_shutting_down")]
		[ThrowsIdlException(typeof(ImplementationRepository.NotFound))]
		public void ServerIsShuttingDown([WideChar(false)] string server)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("server_is_shutting_down", true);
					outputStream.WriteString(server);
					inputStream = _Invoke(outputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							case "IDL:ImplementationRepository/NotFound:1.0":
								throw ImplementationRepository.NotFoundHelper.Read(aex.InputStream);
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("find")]
		public void Find([WideChar(false)] string server, out ImplementationRepository.ServerInformation info)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("find", true);
					outputStream.WriteString(server);
					inputStream = _Invoke(outputStream);
					info = ImplementationRepository.ServerInformationHelper.Read(inputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("list")]
		public void List(uint howMany, bool determineActiveStatus, out ImplementationRepository.ServerInformation[] serverList, out ImplementationRepository.IServerInformationIterator serverIterator)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("list", true);
					outputStream.WriteULong(howMany);
					outputStream.WriteBoolean(determineActiveStatus);
					inputStream = _Invoke(outputStream);
					{
						var _capacity0 = inputStream.ReadLong();
						if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
						{
							throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
						}
						serverList = new ImplementationRepository.ServerInformation[_capacity0];
						for (int i0 = 0; i0 < _capacity0; i0++)
						{
							ImplementationRepository.ServerInformation _item0;
							_item0 = ImplementationRepository.ServerInformationHelper.Read(inputStream);
							serverList[i0] = _item0;
						}
					}
					serverIterator = ImplementationRepository.ServerInformationIteratorHelper.Read(inputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("shutdown")]
		public void Shutdown(bool activators, bool servers)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("shutdown", true);
					outputStream.WriteBoolean(activators);
					outputStream.WriteBoolean(servers);
					inputStream = _Invoke(outputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("link_servers")]
		[ThrowsIdlException(typeof(ImplementationRepository.NotFound))]
		[ThrowsIdlException(typeof(ImplementationRepository.CannotComplete))]
		public void LinkServers([WideChar(false)] string server, string[] peers)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("link_servers", true);
					outputStream.WriteString(server);
					{
						outputStream.WriteLong(peers.Length);
						for (int i0 = 0; i0 < peers.Length; i0++)
						{
							outputStream.WriteString(peers[i0]);
						}
					}
					inputStream = _Invoke(outputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							case "IDL:ImplementationRepository/NotFound:1.0":
								throw ImplementationRepository.NotFoundHelper.Read(aex.InputStream);
							case "IDL:ImplementationRepository/CannotComplete:1.0":
								throw ImplementationRepository.CannotCompleteHelper.Read(aex.InputStream);
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("kill_server")]
		[ThrowsIdlException(typeof(ImplementationRepository.NotFound))]
		[ThrowsIdlException(typeof(ImplementationRepository.CannotComplete))]
		public void KillServer([WideChar(false)] string server, short signum)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("kill_server", true);
					outputStream.WriteString(server);
					outputStream.WriteShort(signum);
					inputStream = _Invoke(outputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							case "IDL:ImplementationRepository/NotFound:1.0":
								throw ImplementationRepository.NotFoundHelper.Read(aex.InputStream);
							case "IDL:ImplementationRepository/CannotComplete:1.0":
								throw ImplementationRepository.CannotCompleteHelper.Read(aex.InputStream);
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}
		[IdlName("force_remove_server")]
		[ThrowsIdlException(typeof(ImplementationRepository.NotFound))]
		[ThrowsIdlException(typeof(ImplementationRepository.CannotComplete))]
		public void ForceRemoveServer([WideChar(false)] string server, short signum)
		{
			while(true)
			{
				IInputStream inputStream = null;
				IOutputStream outputStream = null;
				try
				{
					outputStream = _Request("force_remove_server", true);
					outputStream.WriteString(server);
					outputStream.WriteShort(signum);
					inputStream = _Invoke(outputStream);
					return;
				}
				catch(RemarshalException)
				{
					continue;
				}
				catch(CORBA.ApplicationException aex)
				{
					try
					{
						switch (aex.Id)
						{
							case "IDL:ImplementationRepository/NotFound:1.0":
								throw ImplementationRepository.NotFoundHelper.Read(aex.InputStream);
							case "IDL:ImplementationRepository/CannotComplete:1.0":
								throw ImplementationRepository.CannotCompleteHelper.Read(aex.InputStream);
							default:
								throw new RuntimeException("Unexpected exception " + aex.Id);
						}						
					}
					finally
					{
						try
						{
							aex.InputStream.Close();
						}
						catch (Exception ex)
						{
							throw new RuntimeException("Unexpected exception " + ex.ToString());
						}
					}
				}
				finally
				{
					if (outputStream != null)
					{
						try
						{
							outputStream.Close();
						}
						catch (Exception e)
						{
							throw new RuntimeException("Unexpected exception " + e.ToString());
						}
					}
					if (inputStream != null)
					{
						this._ReleaseReply(inputStream);
					}
				}
			}
		}

	}

}
