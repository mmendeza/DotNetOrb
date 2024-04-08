/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:27
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CORBA
{
		public static class StringDefHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(StringDefHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateInterfaceTc("IDL:CORBA/StringDef:1.0", "StringDef");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, CORBA.IStringDef ifz)
			{
				any.InsertObject(ifz);
			}

			public static CORBA.IStringDef Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:CORBA/StringDef:1.0";
			}

			public static CORBA.IStringDef Read(CORBA.IInputStream inputStream)
			{
				return Narrow(inputStream.ReadObject(typeof(_StringDefStub)));
			}

			public static void Write(CORBA.IOutputStream outputStream, CORBA.IStringDef ifz)
			{
				outputStream.WriteObject(ifz);
			}

			public static CORBA.IStringDef Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is CORBA.IStringDef)
				{
					return (CORBA.IStringDef)obj;
				}
				else if (obj._IsA("IDL:CORBA/StringDef:1.0"))
				{
					_StringDefStub stub = new _StringDefStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static CORBA.IStringDef UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is CORBA.IStringDef)
				{
					return (CORBA.IStringDef)obj;
				}
				else
				{
					_StringDefStub stub = new _StringDefStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
			}
		}
}