/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:38
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace ImplementationRepository
{
	public interface IServerInformationIteratorOperations
	{
		[IdlName("next_n")]
		public bool NextN(uint howMany, out ImplementationRepository.ServerInformation[] servers);
		[IdlName("destroy")]
		public void Destroy();
	}
}
