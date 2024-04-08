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
	public static class DoubleSeqHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(DoubleSeqHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateAliasTc(CORBA.DoubleSeqHelper.Id(), "DoubleSeq", CORBA.ORB.Init().CreateSequenceTc(0, CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 7)));
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, double[] value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static double[] Extract(CORBA.Any any)
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
			return "IDL:CORBA/DoubleSeq:1.0";
		}

		public static double[] Read(CORBA.IInputStream inputStream)
		{
			double[] result;
			{
				var _capacity0 = inputStream.ReadLong();
				if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
				{
					throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
				}
				var _array = new double[_capacity0];
				inputStream.ReadDoubleArray(ref _array, 0, _capacity0);
				result = _array;
			}
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, double[] value)
		{
			{
				outputStream.WriteLong(value.Length);
				outputStream.WriteDoubleArray(value, 0, value.Length);
			}
		}

	}
}