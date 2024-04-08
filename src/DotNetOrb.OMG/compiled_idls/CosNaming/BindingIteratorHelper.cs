/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:30
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CosNaming
{
		public static class BindingIteratorHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(BindingIteratorHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateInterfaceTc("IDL:omg.org/CosNaming/BindingIterator:1.0", "BindingIterator");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, CosNaming.IBindingIterator ifz)
			{
				any.InsertObject(ifz);
			}

			public static CosNaming.IBindingIterator Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:omg.org/CosNaming/BindingIterator:1.0";
			}

			public static CosNaming.IBindingIterator Read(CORBA.IInputStream inputStream)
			{
				return Narrow(inputStream.ReadObject(typeof(_BindingIteratorStub)));
			}

			public static void Write(CORBA.IOutputStream outputStream, CosNaming.IBindingIterator ifz)
			{
				outputStream.WriteObject(ifz);
			}

			public static CosNaming.IBindingIterator Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is CosNaming.IBindingIterator)
				{
					return (CosNaming.IBindingIterator)obj;
				}
				else if (obj._IsA("IDL:omg.org/CosNaming/BindingIterator:1.0"))
				{
					_BindingIteratorStub stub = new _BindingIteratorStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static CosNaming.IBindingIterator UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is CosNaming.IBindingIterator)
				{
					return (CosNaming.IBindingIterator)obj;
				}
				else
				{
					_BindingIteratorStub stub = new _BindingIteratorStub();
					stub._Delegate = ((CORBA.Object)obj)._Delegate;
					return stub;
				}
			}
		}
}
