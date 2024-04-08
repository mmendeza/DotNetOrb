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
		public static class POAManagerFactoryHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(POAManagerFactoryHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateLocalInterfaceTc("IDL:PortableServer/POAManagerFactory:1.0", "POAManagerFactory");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, PortableServer.IPOAManagerFactory ifz)
			{
				any.InsertObject(ifz);
			}

			public static PortableServer.IPOAManagerFactory Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:PortableServer/POAManagerFactory:1.0";
			}

			public static PortableServer.IPOAManagerFactory Read(CORBA.IInputStream inputStream)
			{
				throw new CORBA.Marshal("Local interface");
			}

			public static void Write(CORBA.IOutputStream outputStream, PortableServer.IPOAManagerFactory ifz)
			{
				throw new CORBA.Marshal("Local interface");
			}

			public static PortableServer.IPOAManagerFactory Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is PortableServer.IPOAManagerFactory)
				{
					return (PortableServer.IPOAManagerFactory)obj;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static PortableServer.IPOAManagerFactory UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is PortableServer.IPOAManagerFactory)
				{
					return (PortableServer.IPOAManagerFactory)obj;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}
		}
}
