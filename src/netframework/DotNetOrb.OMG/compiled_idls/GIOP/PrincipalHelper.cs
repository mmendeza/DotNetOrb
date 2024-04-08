/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace GIOP
{
	public static class PrincipalHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(PrincipalHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateAliasTc(GIOP.PrincipalHelper.Id(), "Principal", CORBA.ORB.Init().CreateSequenceTc(0, CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 10)));
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, byte[] value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static byte[] Extract(CORBA.Any any)
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
			return "IDL:GIOP/Principal:1.0";
		}

		public static byte[] Read(CORBA.IInputStream inputStream)
		{
			byte[] result;
			{
				var _capacity0 = inputStream.ReadLong();
				if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
				{
					throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
				}
				var _array = new byte[_capacity0];
				inputStream.ReadOctetArray(ref _array, 0, _capacity0);
				result = _array;
			}
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, byte[] value)
		{
			{
				outputStream.WriteLong(value.Length);
				outputStream.WriteOctetArray(value, 0, value.Length);
			}
		}

	}
}
