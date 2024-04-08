/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:29
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace ETF
{
	public abstract class _BufferListLocalBase: CORBA.LocalObject, IBufferList
	{
		private string[] _ids = {"IDL:omg.org/ETF/BufferList:1.0"};

		public override string[] _Ids()
		{
			return _ids;
		}

		[IdlName("add_buffer")]
		public abstract uint AddBuffer(uint size, ref byte[] buf);
		[IdlName("num_buffers")]
		public abstract uint NumBuffers 
		{
			get;
		}
		[IdlName("get_buffer")]
		public abstract void GetBuffer(uint index, ref byte[] buf);
		[IdlName("release_buffers")]
		public abstract void ReleaseBuffers();
	}

}
