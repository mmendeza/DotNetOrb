/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CORBA
{
		public static class WstringDefHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(WstringDefHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateInterfaceTc("IDL:CORBA/WstringDef:1.0", "WstringDef");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, CORBA.IWstringDef ifz)
			{
				any.InsertObject(ifz);
			}

			public static CORBA.IWstringDef Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:CORBA/WstringDef:1.0";
			}

			public static CORBA.IWstringDef Read(CORBA.IInputStream inputStream)
			{
				return Narrow(inputStream.ReadObject(typeof(_WstringDefStub)));
			}

			public static void Write(CORBA.IOutputStream outputStream, CORBA.IWstringDef ifz)
			{
				outputStream.WriteObject(ifz);
			}

			public static CORBA.IWstringDef Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is CORBA.IWstringDef)
				{
					return (CORBA.IWstringDef)obj;
				}
				else if (obj._IsA("IDL:CORBA/WstringDef:1.0"))
				{
					_WstringDefStub stub = new _WstringDefStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static CORBA.IWstringDef UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is CORBA.IWstringDef)
				{
					return (CORBA.IWstringDef)obj;
				}
				else
				{
					_WstringDefStub stub = new _WstringDefStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
			}
		}
}