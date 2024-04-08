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
	public abstract class _CodecFactoryLocalBase: CORBA.LocalObject, ICodecFactory
	{
		private string[] _ids = {"IDL:IOP/CodecFactory:1.0"};

		public override string[] _Ids()
		{
			return _ids;
		}

		[IdlName("create_codec")]
		[ThrowsIdlException(typeof(IOP.CodecFactory.UnknownEncoding))]
		public abstract IOP.ICodec CreateCodec(IOP.Encoding enc);
	}

}
