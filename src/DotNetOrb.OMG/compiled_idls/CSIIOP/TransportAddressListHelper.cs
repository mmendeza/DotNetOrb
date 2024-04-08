/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:29
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CSIIOP
{
	public static class TransportAddressListHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(TransportAddressListHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateAliasTc(CSIIOP.TransportAddressListHelper.Id(), "TransportAddressList", CORBA.ORB.Init().CreateSequenceTc(0, CSIIOP.TransportAddressHelper.Type()));
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, CSIIOP.TransportAddress[] value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static CSIIOP.TransportAddress[] Extract(CORBA.Any any)
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
			return "IDL:CSIIOP/TransportAddressList:1.0";
		}

		public static CSIIOP.TransportAddress[] Read(CORBA.IInputStream inputStream)
		{
			CSIIOP.TransportAddress[] result;
			{
				var _capacity0 = inputStream.ReadLong();
				if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
				{
					throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
				}
				result = new CSIIOP.TransportAddress[_capacity0];
				for (int i0 = 0; i0 < _capacity0; i0++)
				{
					CSIIOP.TransportAddress _item0;
					_item0 = CSIIOP.TransportAddressHelper.Read(inputStream);
					result[i0] = _item0;
				}
			}
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, CSIIOP.TransportAddress[] value)
		{
			{
				outputStream.WriteLong(value.Length);
				for (int i0 = 0; i0 < value.Length; i0++)
				{
					CSIIOP.TransportAddressHelper.Write(outputStream, value[i0]);
				}
			}
		}

	}
}
