/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:26
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace CORBA
{

	public class PollableSetLocalTie: _PollableSetLocalBase
	{
		public IPollableSetOperations _OperationsDelegate { get; set; }

		public PollableSetLocalTie(IPollableSetOperations d)
		{
			_OperationsDelegate = d;
		}

		[IdlName("create_dii_pollable")]
		public override CORBA.IDIIPollable CreateDiiPollable()
		{
			return _OperationsDelegate.CreateDiiPollable();
		}
		[IdlName("add_pollable")]
		public override void AddPollable(CORBA.IPollable potential)
		{
			_OperationsDelegate.AddPollable(potential);
		}
		[IdlName("get_ready_pollable")]
		[ThrowsIdlException(typeof(CORBA.PollableSet.NoPossiblePollable))]
		public override CORBA.IPollable GetReadyPollable(uint timeout)
		{
			return _OperationsDelegate.GetReadyPollable(timeout);
		}
		[IdlName("remove")]
		[ThrowsIdlException(typeof(CORBA.PollableSet.UnknownPollable))]
		public override void Remove(CORBA.IPollable potential)
		{
			_OperationsDelegate.Remove(potential);
		}
		[IdlName("number_left")]
		public override ushort NumberLeft()
		{
			return _OperationsDelegate.NumberLeft();
		}
	}
}
