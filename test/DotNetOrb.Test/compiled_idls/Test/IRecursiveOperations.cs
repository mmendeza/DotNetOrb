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
	public interface IRecursiveOperations
	{
		[IdlName("SendFoo")]
		public Test.Recursive.Foo SendFoo(Test.Recursive.Foo f);
		[IdlName("SendBar")]
		public Test.Recursive.Bar SendBar(Test.Recursive.Bar b);
		[IdlName("SendAny")]
		public CORBA.Any SendAny(CORBA.Any a);
		[IdlName("SendAny")]
		public Task<CORBA.Any> SendAnyAsync(CORBA.Any a);
	}
}
