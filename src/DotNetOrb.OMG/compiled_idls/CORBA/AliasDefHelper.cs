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
		public static class AliasDefHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(AliasDefHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateInterfaceTc("IDL:CORBA/AliasDef:1.0", "AliasDef");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, CORBA.IAliasDef ifz)
			{
				any.InsertObject(ifz);
			}

			public static CORBA.IAliasDef Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:CORBA/AliasDef:1.0";
			}

			public static CORBA.IAliasDef Read(CORBA.IInputStream inputStream)
			{
				return Narrow(inputStream.ReadObject(typeof(_AliasDefStub)));
			}

			public static void Write(CORBA.IOutputStream outputStream, CORBA.IAliasDef ifz)
			{
				outputStream.WriteObject(ifz);
			}

			public static CORBA.IAliasDef Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is CORBA.IAliasDef)
				{
					return (CORBA.IAliasDef)obj;
				}
				else if (obj._IsA("IDL:CORBA/AliasDef:1.0"))
				{
					_AliasDefStub stub = new _AliasDefStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static CORBA.IAliasDef UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is CORBA.IAliasDef)
				{
					return (CORBA.IAliasDef)obj;
				}
				else
				{
					_AliasDefStub stub = new _AliasDefStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
			}
		}
}
