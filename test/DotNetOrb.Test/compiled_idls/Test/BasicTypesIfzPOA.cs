/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 26/02/2024 12:18:44
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace Test
{

	public abstract class BasicTypesIfzPOA: PortableServer.Servant, CORBA.IInvokeHandler, IBasicTypesIfzOperations
	{
		static private Dictionary<string,int> _opsDict = new Dictionary<string,int>();
		static BasicTypesIfzPOA()
		{
			_opsDict.Add("_set_a_sbyte", 0);
			_opsDict.Add("_get_a_sbyte", 1);
			_opsDict.Add("_set_a_sbyte_seq", 2);
			_opsDict.Add("_get_a_sbyte_seq", 3);
			_opsDict.Add("_set_a_byte", 4);
			_opsDict.Add("_get_a_byte", 5);
			_opsDict.Add("_set_a_byte_seq", 6);
			_opsDict.Add("_get_a_byte_seq", 7);
			_opsDict.Add("_set_a_short", 8);
			_opsDict.Add("_get_a_short", 9);
			_opsDict.Add("_set_a_short_seq", 10);
			_opsDict.Add("_get_a_short_seq", 11);
			_opsDict.Add("_set_a_ushort", 12);
			_opsDict.Add("_get_a_ushort", 13);
			_opsDict.Add("_set_a_ushort_seq", 14);
			_opsDict.Add("_get_a_ushort_seq", 15);
			_opsDict.Add("_set_a_long", 16);
			_opsDict.Add("_get_a_long", 17);
			_opsDict.Add("_set_a_long_seq", 18);
			_opsDict.Add("_get_a_long_seq", 19);
			_opsDict.Add("_set_a_ulong", 20);
			_opsDict.Add("_get_a_ulong", 21);
			_opsDict.Add("_set_a_ulong_seq", 22);
			_opsDict.Add("_get_a_ulong_seq", 23);
			_opsDict.Add("_set_a_long_long", 24);
			_opsDict.Add("_get_a_long_long", 25);
			_opsDict.Add("_set_a_long_long_seq", 26);
			_opsDict.Add("_get_a_long_long_seq", 27);
			_opsDict.Add("_set_a_ulong_long", 28);
			_opsDict.Add("_get_a_ulong_long", 29);
			_opsDict.Add("_set_a_ulong_long_seq", 30);
			_opsDict.Add("_get_a_ulong_long_seq", 31);
			_opsDict.Add("_set_a_float", 32);
			_opsDict.Add("_get_a_float", 33);
			_opsDict.Add("_set_a_float_seq", 34);
			_opsDict.Add("_get_a_float_seq", 35);
			_opsDict.Add("_set_a_double", 36);
			_opsDict.Add("_get_a_double", 37);
			_opsDict.Add("_set_a_double_seq", 38);
			_opsDict.Add("_get_a_double_seq", 39);
			_opsDict.Add("_set_a_char", 40);
			_opsDict.Add("_get_a_char", 41);
			_opsDict.Add("_set_a_char_seq", 42);
			_opsDict.Add("_get_a_char_seq", 43);
			_opsDict.Add("_set_a_wchar", 44);
			_opsDict.Add("_get_a_wchar", 45);
			_opsDict.Add("_set_a_wchar_seq", 46);
			_opsDict.Add("_get_a_wchar_seq", 47);
			_opsDict.Add("_set_a_string", 48);
			_opsDict.Add("_get_a_string", 49);
			_opsDict.Add("_set_a_string_seq", 50);
			_opsDict.Add("_get_a_string_seq", 51);
			_opsDict.Add("_set_a_wstring", 52);
			_opsDict.Add("_get_a_wstring", 53);
			_opsDict.Add("_set_a_wstring_seq", 54);
			_opsDict.Add("_get_a_wstring_seq", 55);
			_opsDict.Add("_set_a_boolean", 56);
			_opsDict.Add("_get_a_boolean", 57);
			_opsDict.Add("_set_a_boolean_seq", 58);
			_opsDict.Add("_get_a_boolean_seq", 59);
			_opsDict.Add("_set_a_octet", 60);
			_opsDict.Add("_get_a_octet", 61);
			_opsDict.Add("_set_a_octet_seq", 62);
			_opsDict.Add("_get_a_octet_seq", 63);
			_opsDict.Add("_set_a_fixed", 64);
			_opsDict.Add("_get_a_fixed", 65);
			_opsDict.Add("_set_a_fixed_seq", 66);
			_opsDict.Add("_get_a_fixed_seq", 67);
		}
		private string[] _ids = {"IDL:Test/BasicTypesIfz:1.0"};

		[IdlName("a_sbyte")]
		public abstract sbyte ASbyte 
		{
			get;

			set;
		}
		[IdlName("a_sbyte_seq")]
		public abstract sbyte[] ASbyteSeq 
		{
			get;

			set;
		}
		[IdlName("a_byte")]
		public abstract byte AByte 
		{
			get;

			set;
		}
		[IdlName("a_byte_seq")]
		public abstract byte[] AByteSeq 
		{
			get;

			set;
		}
		[IdlName("a_short")]
		public abstract short AShort 
		{
			get;

			set;
		}
		[IdlName("a_short_seq")]
		public abstract short[] AShortSeq 
		{
			get;

			set;
		}
		[IdlName("a_ushort")]
		public abstract ushort AUshort 
		{
			get;

			set;
		}
		[IdlName("a_ushort_seq")]
		public abstract ushort[] AUshortSeq 
		{
			get;

			set;
		}
		[IdlName("a_long")]
		public abstract int ALong 
		{
			get;

			set;
		}
		[IdlName("a_long_seq")]
		public abstract int[] ALongSeq 
		{
			get;

			set;
		}
		[IdlName("a_ulong")]
		public abstract uint AUlong 
		{
			get;

			set;
		}
		[IdlName("a_ulong_seq")]
		public abstract uint[] AUlongSeq 
		{
			get;

			set;
		}
		[IdlName("a_long_long")]
		public abstract long ALongLong 
		{
			get;

			set;
		}
		[IdlName("a_long_long_seq")]
		public abstract long[] ALongLongSeq 
		{
			get;

			set;
		}
		[IdlName("a_ulong_long")]
		public abstract ulong AUlongLong 
		{
			get;

			set;
		}
		[IdlName("a_ulong_long_seq")]
		public abstract ulong[] AUlongLongSeq 
		{
			get;

			set;
		}
		[IdlName("a_float")]
		public abstract float AFloat 
		{
			get;

			set;
		}
		[IdlName("a_float_seq")]
		public abstract float[] AFloatSeq 
		{
			get;

			set;
		}
		[IdlName("a_double")]
		public abstract double ADouble 
		{
			get;

			set;
		}
		[IdlName("a_double_seq")]
		public abstract double[] ADoubleSeq 
		{
			get;

			set;
		}
		[IdlName("a_char")]
		public abstract char AChar 
		{
			get;

			set;
		}
		[IdlName("a_char_seq")]
		public abstract char[] ACharSeq 
		{
			get;

			set;
		}
		[IdlName("a_wchar")]
		public abstract char AWchar 
		{
			get;

			set;
		}
		[IdlName("a_wchar_seq")]
		public abstract char[] AWcharSeq 
		{
			get;

			set;
		}
		[IdlName("a_string")]
		[WideChar(false)]
		public abstract string AString 
		{
			get;

			set;
		}
		[IdlName("a_string_seq")]
		public abstract string[] AStringSeq 
		{
			get;

			set;
		}
		[IdlName("a_wstring")]
		[WideChar(true)]
		public abstract string AWstring 
		{
			get;

			set;
		}
		[IdlName("a_wstring_seq")]
		public abstract string[] AWstringSeq 
		{
			get;

			set;
		}
		[IdlName("a_boolean")]
		public abstract bool ABoolean 
		{
			get;

			set;
		}
		[IdlName("a_boolean_seq")]
		public abstract bool[] ABooleanSeq 
		{
			get;

			set;
		}
		[IdlName("a_octet")]
		public abstract byte AOctet 
		{
			get;

			set;
		}
		[IdlName("a_octet_seq")]
		public abstract byte[] AOctetSeq 
		{
			get;

			set;
		}
		[IdlName("a_fixed")]
		public abstract Decimal AFixed 
		{
			get;

			set;
		}
		[IdlName("a_fixed_seq")]
		public abstract Decimal[] AFixedSeq 
		{
			get;

			set;
		}

		public override string[] _AllInterfaces(PortableServer.IPOA poa, byte[] objId)
		{
			return _ids;
		}

		public virtual Test.IBasicTypesIfz _This()
		{
			return Test.BasicTypesIfzHelper.Narrow(_ThisObject());
		}

		public virtual Test.IBasicTypesIfz _This(CORBA.ORB orb)
		{
			return Test.BasicTypesIfzHelper.Narrow(_ThisObject(orb));
		}

		public CORBA.IOutputStream _Invoke(string method, CORBA.IInputStream inputStream, CORBA.IResponseHandler handler)
		{
			CORBA.IOutputStream outputStream = null;
			int opIndex;
			if (_opsDict.TryGetValue(method, out opIndex))
			{
				switch (opIndex)
				{
					case 0:
					{
							outputStream = handler.CreateReply();
							ASbyte = (sbyte) inputStream.ReadOctet();
					}
					break;
					case 1:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteOctet((byte) ASbyte);
					}
					break;
					case 2:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								ASbyteSeq = new sbyte[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									sbyte _item0;
									_item0 = (sbyte) inputStream.ReadOctet();
									ASbyteSeq[i0] = _item0;
								}
							}
					}
					break;
					case 3:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(ASbyteSeq.Length);
								for (int i0 = 0; i0 < ASbyteSeq.Length; i0++)
								{
									outputStream.WriteOctet((byte) ASbyteSeq[i0]);
								}
							}
					}
					break;
					case 4:
					{
							outputStream = handler.CreateReply();
							AByte = inputStream.ReadOctet();
					}
					break;
					case 5:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteOctet(AByte);
					}
					break;
					case 6:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new byte[_capacity0];
								inputStream.ReadOctetArray(ref _array, 0, _capacity0);
								AByteSeq = _array;
							}
					}
					break;
					case 7:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AByteSeq.Length);
								outputStream.WriteOctetArray(AByteSeq, 0, AByteSeq.Length);
							}
					}
					break;
					case 8:
					{
							outputStream = handler.CreateReply();
							AShort = inputStream.ReadShort();
					}
					break;
					case 9:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteShort(AShort);
					}
					break;
					case 10:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new short[_capacity0];
								inputStream.ReadShortArray(ref _array, 0, _capacity0);
								AShortSeq = _array;
							}
					}
					break;
					case 11:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AShortSeq.Length);
								outputStream.WriteShortArray(AShortSeq, 0, AShortSeq.Length);
							}
					}
					break;
					case 12:
					{
							outputStream = handler.CreateReply();
							AUshort = inputStream.ReadUShort();
					}
					break;
					case 13:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteUShort(AUshort);
					}
					break;
					case 14:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new ushort[_capacity0];
								inputStream.ReadUShortArray(ref _array, 0, _capacity0);
								AUshortSeq = _array;
							}
					}
					break;
					case 15:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AUshortSeq.Length);
								outputStream.WriteUShortArray(AUshortSeq, 0, AUshortSeq.Length);
							}
					}
					break;
					case 16:
					{
							outputStream = handler.CreateReply();
							ALong = inputStream.ReadLong();
					}
					break;
					case 17:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteLong(ALong);
					}
					break;
					case 18:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new int[_capacity0];
								inputStream.ReadLongArray(ref _array, 0, _capacity0);
								ALongSeq = _array;
							}
					}
					break;
					case 19:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(ALongSeq.Length);
								outputStream.WriteLongArray(ALongSeq, 0, ALongSeq.Length);
							}
					}
					break;
					case 20:
					{
							outputStream = handler.CreateReply();
							AUlong = inputStream.ReadULong();
					}
					break;
					case 21:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteULong(AUlong);
					}
					break;
					case 22:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new uint[_capacity0];
								inputStream.ReadULongArray(ref _array, 0, _capacity0);
								AUlongSeq = _array;
							}
					}
					break;
					case 23:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AUlongSeq.Length);
								outputStream.WriteULongArray(AUlongSeq, 0, AUlongSeq.Length);
							}
					}
					break;
					case 24:
					{
							outputStream = handler.CreateReply();
							ALongLong = inputStream.ReadLongLong();
					}
					break;
					case 25:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteLongLong(ALongLong);
					}
					break;
					case 26:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new long[_capacity0];
								inputStream.ReadLongLongArray(ref _array, 0, _capacity0);
								ALongLongSeq = _array;
							}
					}
					break;
					case 27:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(ALongLongSeq.Length);
								outputStream.WriteLongLongArray(ALongLongSeq, 0, ALongLongSeq.Length);
							}
					}
					break;
					case 28:
					{
							outputStream = handler.CreateReply();
							AUlongLong = inputStream.ReadULongLong();
					}
					break;
					case 29:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteULongLong(AUlongLong);
					}
					break;
					case 30:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new ulong[_capacity0];
								inputStream.ReadULongLongArray(ref _array, 0, _capacity0);
								AUlongLongSeq = _array;
							}
					}
					break;
					case 31:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AUlongLongSeq.Length);
								outputStream.WriteULongLongArray(AUlongLongSeq, 0, AUlongLongSeq.Length);
							}
					}
					break;
					case 32:
					{
							outputStream = handler.CreateReply();
							AFloat = inputStream.ReadFloat();
					}
					break;
					case 33:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteFloat(AFloat);
					}
					break;
					case 34:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new float[_capacity0];
								inputStream.ReadFloatArray(ref _array, 0, _capacity0);
								AFloatSeq = _array;
							}
					}
					break;
					case 35:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AFloatSeq.Length);
								outputStream.WriteFloatArray(AFloatSeq, 0, AFloatSeq.Length);
							}
					}
					break;
					case 36:
					{
							outputStream = handler.CreateReply();
							ADouble = inputStream.ReadDouble();
					}
					break;
					case 37:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteDouble(ADouble);
					}
					break;
					case 38:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new double[_capacity0];
								inputStream.ReadDoubleArray(ref _array, 0, _capacity0);
								ADoubleSeq = _array;
							}
					}
					break;
					case 39:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(ADoubleSeq.Length);
								outputStream.WriteDoubleArray(ADoubleSeq, 0, ADoubleSeq.Length);
							}
					}
					break;
					case 40:
					{
							outputStream = handler.CreateReply();
							AChar = inputStream.ReadChar();
					}
					break;
					case 41:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteChar(AChar);
					}
					break;
					case 42:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new char[_capacity0];
								inputStream.ReadCharArray(ref _array, 0, _capacity0);
								ACharSeq = _array;
							}
					}
					break;
					case 43:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(ACharSeq.Length);
								outputStream.WriteCharArray(ACharSeq, 0, ACharSeq.Length);
							}
					}
					break;
					case 44:
					{
							outputStream = handler.CreateReply();
							AWchar = inputStream.ReadWChar();
					}
					break;
					case 45:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteWChar(AWchar);
					}
					break;
					case 46:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new char[_capacity0];
								inputStream.ReadWCharArray(ref _array, 0, _capacity0);
								AWcharSeq = _array;
							}
					}
					break;
					case 47:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AWcharSeq.Length);
								outputStream.WriteWCharArray(AWcharSeq, 0, AWcharSeq.Length);
							}
					}
					break;
					case 48:
					{
							outputStream = handler.CreateReply();
							AString = inputStream.ReadString();
					}
					break;
					case 49:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteString(AString);
					}
					break;
					case 50:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								AStringSeq = new string[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									string _item0;
									_item0 = inputStream.ReadString();
									AStringSeq[i0] = _item0;
								}
							}
					}
					break;
					case 51:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AStringSeq.Length);
								for (int i0 = 0; i0 < AStringSeq.Length; i0++)
								{
									outputStream.WriteString(AStringSeq[i0]);
								}
							}
					}
					break;
					case 52:
					{
							outputStream = handler.CreateReply();
							AWstring = inputStream.ReadWString();
					}
					break;
					case 53:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteWString(AWstring);
					}
					break;
					case 54:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								AWstringSeq = new string[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									string _item0;
									_item0 = inputStream.ReadWString();
									AWstringSeq[i0] = _item0;
								}
							}
					}
					break;
					case 55:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AWstringSeq.Length);
								for (int i0 = 0; i0 < AWstringSeq.Length; i0++)
								{
									outputStream.WriteWString(AWstringSeq[i0]);
								}
							}
					}
					break;
					case 56:
					{
							outputStream = handler.CreateReply();
							ABoolean = inputStream.ReadBoolean();
					}
					break;
					case 57:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteBoolean(ABoolean);
					}
					break;
					case 58:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new bool[_capacity0];
								inputStream.ReadBooleanArray(ref _array, 0, _capacity0);
								ABooleanSeq = _array;
							}
					}
					break;
					case 59:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(ABooleanSeq.Length);
								outputStream.WriteBooleanArray(ABooleanSeq, 0, ABooleanSeq.Length);
							}
					}
					break;
					case 60:
					{
							outputStream = handler.CreateReply();
							AOctet = inputStream.ReadOctet();
					}
					break;
					case 61:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteOctet(AOctet);
					}
					break;
					case 62:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								var _array = new byte[_capacity0];
								inputStream.ReadOctetArray(ref _array, 0, _capacity0);
								AOctetSeq = _array;
							}
					}
					break;
					case 63:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AOctetSeq.Length);
								outputStream.WriteOctetArray(AOctetSeq, 0, AOctetSeq.Length);
							}
					}
					break;
					case 64:
					{
							outputStream = handler.CreateReply();
							AFixed = inputStream.ReadFixed(5, 2);
					}
					break;
					case 65:
					{
							outputStream = handler.CreateReply();
							outputStream.WriteFixed(AFixed, 5, 2);
					}
					break;
					case 66:
					{
							outputStream = handler.CreateReply();
							{
								var _capacity0 = inputStream.ReadLong();
								if (inputStream.Available > 0 && _capacity0 > inputStream.Available)
								{
									throw new Marshal($"Sequence length too large. Only {inputStream.Available} and trying to assign {_capacity0}");
								}
								AFixedSeq = new Decimal[_capacity0];
								for (int i0 = 0; i0 < _capacity0; i0++)
								{
									Decimal _item0;
									_item0 = inputStream.ReadFixed(5, 2);
									AFixedSeq[i0] = _item0;
								}
							}
					}
					break;
					case 67:
					{
							outputStream = handler.CreateReply();
							{
								outputStream.WriteLong(AFixedSeq.Length);
								for (int i0 = 0; i0 < AFixedSeq.Length; i0++)
								{
									outputStream.WriteFixed(AFixedSeq[i0], 5, 2);
								}
							}
					}
					break;
				}
				return outputStream;
			}
			else
			{
				throw new CORBA.BadOperation(method + " not found");
			}
		}
	}
}