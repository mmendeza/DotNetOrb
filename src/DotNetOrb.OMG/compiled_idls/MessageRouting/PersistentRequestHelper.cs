/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:36
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace MessageRouting
{
		public static class PersistentRequestHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(PersistentRequestHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateInterfaceTc("IDL:MessageRouting/PersistentRequest:1.0", "PersistentRequest");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, MessageRouting.IPersistentRequest ifz)
			{
				any.InsertObject(ifz);
			}

			public static MessageRouting.IPersistentRequest Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:MessageRouting/PersistentRequest:1.0";
			}

			public static MessageRouting.IPersistentRequest Read(CORBA.IInputStream inputStream)
			{
				return Narrow(inputStream.ReadObject(typeof(_PersistentRequestStub)));
			}

			public static void Write(CORBA.IOutputStream outputStream, MessageRouting.IPersistentRequest ifz)
			{
				outputStream.WriteObject(ifz);
			}

			public static MessageRouting.IPersistentRequest Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is MessageRouting.IPersistentRequest)
				{
					return (MessageRouting.IPersistentRequest)obj;
				}
				else if (obj._IsA("IDL:MessageRouting/PersistentRequest:1.0"))
				{
					_PersistentRequestStub stub = new _PersistentRequestStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static MessageRouting.IPersistentRequest UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is MessageRouting.IPersistentRequest)
				{
					return (MessageRouting.IPersistentRequest)obj;
				}
				else
				{
					_PersistentRequestStub stub = new _PersistentRequestStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
			}
		}
}