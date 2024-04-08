/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace IOP
{
	public static class MultipleComponentProfileHelper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(MultipleComponentProfileHelper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateAliasTc(IOP.MultipleComponentProfileHelper.Id(), "MultipleComponentProfile", CORBA.ORB.Init().CreateSequenceTc(0, IOP.TaggedComponentHelper.Type()));
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, IOP.TaggedComponent[] value)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), value);
		}

		public static IOP.TaggedComponent[] Extract(CORBA.Any any)
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
			return "IDL:IOP/MultipleComponentProfile:1.0";
		}

		public static IOP.TaggedComponent[] Read(CORBA.IInputStream inputStream)
		{
			IOP.TaggedComponent[] result;
			{
				var _capacity0 = inputStream.ReadLong();
				if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
				{
					throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
				}
				result = new IOP.TaggedComponent[_capacity0];
				for (int i0 = 0; i0 < _capacity0; i0++)
				{
					IOP.TaggedComponent _item0;
					_item0 = IOP.TaggedComponentHelper.Read(inputStream);
					result[i0] = _item0;
				}
			}
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, IOP.TaggedComponent[] value)
		{
			{
				outputStream.WriteLong(value.Length);
				for (int i0 = 0; i0 < value.Length; i0++)
				{
					IOP.TaggedComponentHelper.Write(outputStream, value[i0]);
				}
			}
		}

	}
}
