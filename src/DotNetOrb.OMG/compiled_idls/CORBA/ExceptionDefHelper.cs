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
		public static class ExceptionDefHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(ExceptionDefHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateInterfaceTc("IDL:CORBA/ExceptionDef:1.0", "ExceptionDef");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, CORBA.IExceptionDef ifz)
			{
				any.InsertObject(ifz);
			}

			public static CORBA.IExceptionDef Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:CORBA/ExceptionDef:1.0";
			}

			public static CORBA.IExceptionDef Read(CORBA.IInputStream inputStream)
			{
				return Narrow(inputStream.ReadObject(typeof(_ExceptionDefStub)));
			}

			public static void Write(CORBA.IOutputStream outputStream, CORBA.IExceptionDef ifz)
			{
				outputStream.WriteObject(ifz);
			}

			public static CORBA.IExceptionDef Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is CORBA.IExceptionDef)
				{
					return (CORBA.IExceptionDef)obj;
				}
				else if (obj._IsA("IDL:CORBA/ExceptionDef:1.0"))
				{
					_ExceptionDefStub stub = new _ExceptionDefStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static CORBA.IExceptionDef UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is CORBA.IExceptionDef)
				{
					return (CORBA.IExceptionDef)obj;
				}
				else
				{
					_ExceptionDefStub stub = new _ExceptionDefStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
			}
		}
}