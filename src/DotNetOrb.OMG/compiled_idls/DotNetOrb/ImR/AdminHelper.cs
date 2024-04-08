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
		public static class AdminHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(AdminHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateInterfaceTc("IDL:DotNetOrb/ImR/Admin:1.0", "Admin");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, DotNetOrb.ImR.IAdmin ifz)
			{
				any.InsertObject(ifz);
			}

			public static DotNetOrb.ImR.IAdmin Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:DotNetOrb/ImR/Admin:1.0";
			}

			public static DotNetOrb.ImR.IAdmin Read(CORBA.IInputStream inputStream)
			{
				return Narrow(inputStream.ReadObject(typeof(_AdminStub)));
			}

			public static void Write(CORBA.IOutputStream outputStream, DotNetOrb.ImR.IAdmin ifz)
			{
				outputStream.WriteObject(ifz);
			}

			public static DotNetOrb.ImR.IAdmin Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is DotNetOrb.ImR.IAdmin)
				{
					return (DotNetOrb.ImR.IAdmin)obj;
				}
				else if (obj._IsA("IDL:DotNetOrb/ImR/Admin:1.0"))
				{
					_AdminStub stub = new _AdminStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static DotNetOrb.ImR.IAdmin UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is DotNetOrb.ImR.IAdmin)
				{
					return (DotNetOrb.ImR.IAdmin)obj;
				}
				else
				{
					_AdminStub stub = new _AdminStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
			}
		}
}
