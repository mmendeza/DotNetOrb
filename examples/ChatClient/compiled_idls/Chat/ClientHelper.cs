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
		public static class ClientHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(ClientHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateInterfaceTc("IDL:Chat/Client:1.0", "Client");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, Chat.IClient ifz)
			{
				any.InsertObject(ifz);
			}

			public static Chat.IClient Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:Chat/Client:1.0";
			}

			public static Chat.IClient Read(CORBA.IInputStream inputStream)
			{
				return Narrow(inputStream.ReadObject(typeof(_ClientStub)));
			}

			public static void Write(CORBA.IOutputStream outputStream, Chat.IClient ifz)
			{
				outputStream.WriteObject(ifz);
			}

			public static Chat.IClient Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is Chat.IClient)
				{
					return (Chat.IClient)obj;
				}
				else if (obj._IsA("IDL:Chat/Client:1.0"))
				{
					_ClientStub stub = new _ClientStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static Chat.IClient UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is Chat.IClient)
				{
					return (Chat.IClient)obj;
				}
				else
				{
					_ClientStub stub = new _ClientStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
			}
		}
}
