/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:38
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace DynamicAny
{
	public static class NameDynAnyPairSeqHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(NameDynAnyPairSeqHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateAliasTc(DynamicAny.NameDynAnyPairSeqHelper.Id(), "NameDynAnyPairSeq", CORBA.ORB.Init().CreateSequenceTc(0, DynamicAny.NameDynAnyPairHelper.Type()));
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, DynamicAny.NameDynAnyPair[] value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static DynamicAny.NameDynAnyPair[] Extract(CORBA.Any any)
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
			return "IDL:DynamicAny/NameDynAnyPairSeq:1.0";
		}

		public static DynamicAny.NameDynAnyPair[] Read(CORBA.IInputStream inputStream)
		{
			DynamicAny.NameDynAnyPair[] result;
			{
				var _capacity0 = inputStream.ReadLong();
				if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
				{
					throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
				}
				result = new DynamicAny.NameDynAnyPair[_capacity0];
				for (int i0 = 0; i0 < _capacity0; i0++)
				{
					DynamicAny.NameDynAnyPair _item0;
					_item0 = DynamicAny.NameDynAnyPairHelper.Read(inputStream);
					result[i0] = _item0;
				}
			}
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, DynamicAny.NameDynAnyPair[] value)
		{
			{
				outputStream.WriteLong(value.Length);
				for (int i0 = 0; i0 < value.Length; i0++)
				{
					DynamicAny.NameDynAnyPairHelper.Write(outputStream, value[i0]);
				}
			}
		}

	}
}
