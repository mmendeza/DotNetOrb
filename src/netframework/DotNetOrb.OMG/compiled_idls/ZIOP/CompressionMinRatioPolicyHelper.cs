/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:37
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace ZIOP
{
		public static class CompressionMinRatioPolicyHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(CompressionMinRatioPolicyHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateLocalInterfaceTc("IDL:omg.org/ZIOP/CompressionMinRatioPolicy:1.0", "CompressionMinRatioPolicy");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, ZIOP.ICompressionMinRatioPolicy ifz)
			{
				any.InsertObject(ifz);
			}

			public static ZIOP.ICompressionMinRatioPolicy Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:omg.org/ZIOP/CompressionMinRatioPolicy:1.0";
			}

			public static ZIOP.ICompressionMinRatioPolicy Read(CORBA.IInputStream inputStream)
			{
				throw new CORBA.Marshal("Local interface");
			}

			public static void Write(CORBA.IOutputStream outputStream, ZIOP.ICompressionMinRatioPolicy ifz)
			{
				throw new CORBA.Marshal("Local interface");
			}

			public static ZIOP.ICompressionMinRatioPolicy Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is ZIOP.ICompressionMinRatioPolicy)
				{
					return (ZIOP.ICompressionMinRatioPolicy)obj;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static ZIOP.ICompressionMinRatioPolicy UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is ZIOP.ICompressionMinRatioPolicy)
				{
					return (ZIOP.ICompressionMinRatioPolicy)obj;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}
		}
}