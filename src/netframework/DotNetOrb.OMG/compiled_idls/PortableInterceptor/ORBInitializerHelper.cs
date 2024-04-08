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
		public static class ORBInitializerHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(ORBInitializerHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateLocalInterfaceTc("IDL:PortableInterceptor/ORBInitializer:1.0", "ORBInitializer");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, PortableInterceptor.IORBInitializer ifz)
			{
				any.InsertObject(ifz);
			}

			public static PortableInterceptor.IORBInitializer Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:PortableInterceptor/ORBInitializer:1.0";
			}

			public static PortableInterceptor.IORBInitializer Read(CORBA.IInputStream inputStream)
			{
				throw new CORBA.Marshal("Local interface");
			}

			public static void Write(CORBA.IOutputStream outputStream, PortableInterceptor.IORBInitializer ifz)
			{
				throw new CORBA.Marshal("Local interface");
			}

			public static PortableInterceptor.IORBInitializer Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is PortableInterceptor.IORBInitializer)
				{
					return (PortableInterceptor.IORBInitializer)obj;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static PortableInterceptor.IORBInitializer UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is PortableInterceptor.IORBInitializer)
				{
					return (PortableInterceptor.IORBInitializer)obj;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}
		}
}