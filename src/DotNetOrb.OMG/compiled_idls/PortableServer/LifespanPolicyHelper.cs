/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace PortableServer
{
		public static class LifespanPolicyHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(LifespanPolicyHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateLocalInterfaceTc("IDL:PortableServer/LifespanPolicy:1.0", "LifespanPolicy");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, PortableServer.ILifespanPolicy ifz)
			{
				any.InsertObject(ifz);
			}

			public static PortableServer.ILifespanPolicy Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:PortableServer/LifespanPolicy:1.0";
			}

			public static PortableServer.ILifespanPolicy Read(CORBA.IInputStream inputStream)
			{
				throw new CORBA.Marshal("Local interface");
			}

			public static void Write(CORBA.IOutputStream outputStream, PortableServer.ILifespanPolicy ifz)
			{
				throw new CORBA.Marshal("Local interface");
			}

			public static PortableServer.ILifespanPolicy Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is PortableServer.ILifespanPolicy)
				{
					return (PortableServer.ILifespanPolicy)obj;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static PortableServer.ILifespanPolicy UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is PortableServer.ILifespanPolicy)
				{
					return (PortableServer.ILifespanPolicy)obj;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}
		}
}