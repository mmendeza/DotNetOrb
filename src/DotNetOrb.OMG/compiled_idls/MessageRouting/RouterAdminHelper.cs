/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:35
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace MessageRouting
{
		public static class RouterAdminHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(RouterAdminHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateInterfaceTc("IDL:MessageRouting/RouterAdmin:1.0", "RouterAdmin");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, MessageRouting.IRouterAdmin ifz)
			{
				any.InsertObject(ifz);
			}

			public static MessageRouting.IRouterAdmin Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:MessageRouting/RouterAdmin:1.0";
			}

			public static MessageRouting.IRouterAdmin Read(CORBA.IInputStream inputStream)
			{
				return Narrow(inputStream.ReadObject(typeof(_RouterAdminStub)));
			}

			public static void Write(CORBA.IOutputStream outputStream, MessageRouting.IRouterAdmin ifz)
			{
				outputStream.WriteObject(ifz);
			}

			public static MessageRouting.IRouterAdmin Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is MessageRouting.IRouterAdmin)
				{
					return (MessageRouting.IRouterAdmin)obj;
				}
				else if (obj._IsA("IDL:MessageRouting/RouterAdmin:1.0"))
				{
					_RouterAdminStub stub = new _RouterAdminStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static MessageRouting.IRouterAdmin UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is MessageRouting.IRouterAdmin)
				{
					return (MessageRouting.IRouterAdmin)obj;
				}
				else
				{
					_RouterAdminStub stub = new _RouterAdminStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
			}
		}
}
