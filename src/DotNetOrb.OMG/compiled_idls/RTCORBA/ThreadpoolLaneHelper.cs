/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace RTCORBA
{
	public static class ThreadpoolLaneHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(ThreadpoolLaneHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateStructTc(RTCORBA.ThreadpoolLaneHelper.Id(), "ThreadpoolLane", new CORBA.StructMember[] {new CORBA.StructMember("lane_priority", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 4), null), new CORBA.StructMember("static_threads", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 5), null), new CORBA.StructMember("dynamic_threads", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 5), null), });
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, RTCORBA.ThreadpoolLane s)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), s);
		}

		public static RTCORBA.ThreadpoolLane Extract(CORBA.Any any)
		{
			var inputStream = any.CreateInputStream();
			try
			{
				return Read(inputStream);
			}
			finally
			{
				inputStream.Close();
			}
		}

		public static string Id()
		{
			return "IDL:RTCORBA/ThreadpoolLane:1.0";
		}

		public static RTCORBA.ThreadpoolLane Read(CORBA.IInputStream inputStream)
		{
			var result = new RTCORBA.ThreadpoolLane();
			result.LanePriority = inputStream.ReadShort();
			result.StaticThreads = inputStream.ReadULong();
			result.DynamicThreads = inputStream.ReadULong();
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, RTCORBA.ThreadpoolLane s)
		{
			outputStream.WriteShort(s.LanePriority);
			outputStream.WriteULong(s.StaticThreads);
			outputStream.WriteULong(s.DynamicThreads);
		}

	}
}