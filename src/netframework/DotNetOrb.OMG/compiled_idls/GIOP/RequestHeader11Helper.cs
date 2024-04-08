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
	public static class RequestHeader11Helper
	{
		private static volatile CORBA.TypeCode type;

		public static CORBA.TypeCode Type()
		{
			if (type == null)
			{
				lock (typeof(RequestHeader11Helper))
				{
					if (type == null)
					{
						type = CORBA.ORB.Init().CreateStructTc(GIOP.RequestHeader11Helper.Id(), "RequestHeader_1_1", new CORBA.StructMember[] {new CORBA.StructMember("service_context", CORBA.ORB.Init().CreateSequenceTc(0, IOP.ServiceContextHelper.Type()), null), new CORBA.StructMember("request_id", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 5), null), new CORBA.StructMember("response_expected", CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 8), null), new CORBA.StructMember("reserved", CORBA.ORB.Init().CreateArrayTc(3, CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 10)), null), new CORBA.StructMember("object_key", CORBA.ORB.Init().CreateSequenceTc(0, CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 10)), null), new CORBA.StructMember("operation", CORBA.ORB.Init().CreateStringTc(0), null), new CORBA.StructMember("requesting_principal", CORBA.ORB.Init().CreateSequenceTc(0, CORBA.ORB.Init().GetPrimitiveTc((CORBA.TCKind) 10)), null), });
					}
				}
			}
			return type;
		}

		public static void Insert(CORBA.Any any, GIOP.RequestHeader11 s)
		{
			any.Type = Type();
			Write(any.CreateOutputStream(), s);
		}

		public static GIOP.RequestHeader11 Extract(CORBA.Any any)
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
			return "IDL:GIOP/RequestHeader_1_1:1.0";
		}

		public static GIOP.RequestHeader11 Read(CORBA.IInputStream inputStream)
		{
			var result = new GIOP.RequestHeader11();
			{
				var _capacity0 = inputStream.ReadLong();
				if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
				{
					throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
				}
				result.ServiceContext = new IOP.ServiceContext[_capacity0];
				for (int i0 = 0; i0 < _capacity0; i0++)
				{
					IOP.ServiceContext _item0;
					_item0 = IOP.ServiceContextHelper.Read(inputStream);
					result.ServiceContext[i0] = _item0;
				}
			}
			result.RequestId = inputStream.ReadULong();
			result.ResponseExpected = inputStream.ReadBoolean();
			result.Reserved = new byte[3];
			{
				for (int i0 = 0; i0 < 3; i0++)
				{
					result.Reserved[i0] = inputStream.ReadOctet();
				}
			}
			{
				var _capacity0 = inputStream.ReadLong();
				if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
				{
					throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
				}
				var _array = new byte[_capacity0];
				inputStream.ReadOctetArray(ref _array, 0, _capacity0);
				result.ObjectKey = _array;
			}
			result.Operation = inputStream.ReadString();
			{
				var _capacity0 = inputStream.ReadLong();
				if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
				{
					throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
				}
				var _array = new byte[_capacity0];
				inputStream.ReadOctetArray(ref _array, 0, _capacity0);
				result.RequestingPrincipal = _array;
			}
			return result;
		}

		public static void Write(CORBA.IOutputStream outputStream, GIOP.RequestHeader11 s)
		{
			{
				outputStream.WriteLong(s.ServiceContext.Length);
				for (int i0 = 0; i0 < s.ServiceContext.Length; i0++)
				{
					IOP.ServiceContextHelper.Write(outputStream, s.ServiceContext[i0]);
				}
			}
			outputStream.WriteULong(s.RequestId);
			outputStream.WriteBoolean(s.ResponseExpected);
			if (s.Reserved.Length < 3)			{
				throw new CORBA.Marshal($"Incorrect array size {s.Reserved.Length}, expecting 3");
			}
			for (int i0 = 0; i0 < 3; i0++)
			{
				outputStream.WriteOctet(s.Reserved[i0]);
			}
			{
				outputStream.WriteLong(s.ObjectKey.Length);
				outputStream.WriteOctetArray(s.ObjectKey, 0, s.ObjectKey.Length);
			}
			outputStream.WriteString(s.Operation);
			{
				outputStream.WriteLong(s.RequestingPrincipal.Length);
				outputStream.WriteOctetArray(s.RequestingPrincipal, 0, s.RequestingPrincipal.Length);
			}
		}

	}
}