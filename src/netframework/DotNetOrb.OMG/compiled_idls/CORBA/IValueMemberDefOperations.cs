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
	public interface IValueMemberDefOperations : CORBA.IContainedOperations
	{
		[IdlName("type")]
		public CORBA.TypeCode Type 
		{
			get;
		}
		[IdlName("type_def")]
		public CORBA.IIDLType TypeDef 
		{
			get;

			set;
		}
		[IdlName("access")]
		public short Access 
		{
			get;

			set;
		}
	}
}
