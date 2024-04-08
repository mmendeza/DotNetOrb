/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:34
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Messaging
{
		public static class QueueOrderPolicyHelper
		{
			private static volatile CORBA.TypeCode type;

			public static CORBA.TypeCode Type()
			{
				if (type == null)
				{
					lock (typeof(QueueOrderPolicyHelper))
					{
						if (type == null)
						{
							type = CORBA.ORB.Init().CreateLocalInterfaceTc("IDL:Messaging/QueueOrderPolicy:1.0", "QueueOrderPolicy");
						}
					}
				}
				return type;
			}

			public static void Insert(CORBA.Any any, Messaging.IQueueOrderPolicy ifz)
			{
				any.InsertObject(ifz);
			}

			public static Messaging.IQueueOrderPolicy Extract(CORBA.Any any)
			{
				return Narrow(any.ExtractObject());
			}

			public static string Id()
			{
				return "IDL:Messaging/QueueOrderPolicy:1.0";
			}

			public static Messaging.IQueueOrderPolicy Read(CORBA.IInputStream inputStream)
			{
				throw new CORBA.Marshal("Local interface");
			}

			public static void Write(CORBA.IOutputStream outputStream, Messaging.IQueueOrderPolicy ifz)
			{
				throw new CORBA.Marshal("Local interface");
			}

			public static Messaging.IQueueOrderPolicy Narrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is Messaging.IQueueOrderPolicy)
				{
					return (Messaging.IQueueOrderPolicy)obj;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}

			public static Messaging.IQueueOrderPolicy UncheckedNarrow(CORBA.IObject obj)
			{
				if (obj == null)
				{
					return null;
				}
				else if (obj is Messaging.IQueueOrderPolicy)
				{
					return (Messaging.IQueueOrderPolicy)obj;
				}
				else
				{
					throw new CORBA.BadParam("Narrow failed");
				}
			}
		}
}
