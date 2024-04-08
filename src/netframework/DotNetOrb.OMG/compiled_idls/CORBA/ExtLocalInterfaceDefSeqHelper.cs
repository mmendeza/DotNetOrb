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
	public static class ExtLocalInterfaceDefSeqHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(ExtLocalInterfaceDefSeqHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateAliasTc(CORBA.ExtLocalInterfaceDefSeqHelper.Id(), "ExtLocalInterfaceDefSeq", CORBA.ORB.Init().CreateSequenceTc(0, CORBA.ExtLocalInterfaceDefHelper.Type()));
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, CORBA.IExtLocalInterfaceDef[] value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static CORBA.IExtLocalInterfaceDef[] Extract(CORBA.Any any)
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
			return "IDL:CORBA/ExtLocalInterfaceDefSeq:1.0";
		}

		public static CORBA.IExtLocalInterfaceDef[] Read(CORBA.IInputStream inputStream)
		{
			CORBA.IExtLocalInterfaceDef[] result;
			{
				var _capacity0 = inputStream.ReadLong();
				if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
				{
					throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
				}
				result = new CORBA.IExtLocalInterfaceDef[_capacity0];
				for (int i0 = 0; i0 < _capacity0; i0++)
				{
					CORBA.IExtLocalInterfaceDef _item0;
					_item0 = CORBA.ExtLocalInterfaceDefHelper.Read(inputStream);
					result[i0] = _item0;
				}
			}
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, CORBA.IExtLocalInterfaceDef[] value)
		{
			{
				outputStream.WriteLong(value.Length);
				for (int i0 = 0; i0 < value.Length; i0++)
				{
					CORBA.ExtLocalInterfaceDefHelper.Write(outputStream, value[i0]);
				}
			}
		}

	}
}