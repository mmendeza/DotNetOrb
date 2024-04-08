/**
 * Generated by DotNetORb.IdlCompiler V 1.0.0.0
 * Timestamp: 25/02/2024 9:49:28
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CORBA;


namespace RTCORBA
{
	public interface IRTORBOperations
	{
		[IdlName("create_mutex")]
		public RTCORBA.IMutex CreateMutex();
		[IdlName("destroy_mutex")]
		public void DestroyMutex(RTCORBA.IMutex theMutex);
		[IdlName("create_threadpool")]
		public uint CreateThreadpool(uint stacksize, uint staticThreads, uint dynamicThreads, short defaultPriority, bool allowRequestBuffering, uint maxBufferedRequests, uint maxRequestBufferSize);
		[IdlName("create_threadpool_with_lanes")]
		public uint CreateThreadpoolWithLanes(uint stacksize, RTCORBA.ThreadpoolLane[] lanes, bool allowBorrowing, bool allowRequestBuffering, uint maxBufferedRequests, uint maxRequestBufferSize);
		[IdlName("destroy_threadpool")]
		[ThrowsIdlException(typeof(RTCORBA.RTORB.InvalidThreadpool))]
		public void DestroyThreadpool(uint threadpool);
		[IdlName("create_priority_model_policy")]
		public RTCORBA.IPriorityModelPolicy CreatePriorityModelPolicy(RTCORBA.PriorityModel priorityModel, short serverPriority);
		[IdlName("create_threadpool_policy")]
		public RTCORBA.IThreadpoolPolicy CreateThreadpoolPolicy(uint threadpool);
		[IdlName("create_priority_banded_connection_policy")]
		public RTCORBA.IPriorityBandedConnectionPolicy CreatePriorityBandedConnectionPolicy(RTCORBA.PriorityBand[] priorityBands);
		[IdlName("create_server_protocol_policy")]
		public RTCORBA.IServerProtocolPolicy CreateServerProtocolPolicy(RTCORBA.Protocol[] protocols);
		[IdlName("create_client_protocol_policy")]
		public RTCORBA.IClientProtocolPolicy CreateClientProtocolPolicy(RTCORBA.Protocol[] protocols);
		[IdlName("create_private_connection_policy")]
		public RTCORBA.IPrivateConnectionPolicy CreatePrivateConnectionPolicy();
		[IdlName("create_tcp_protocol_properties")]
		public RTCORBA.ITCPProtocolProperties CreateTcpProtocolProperties(int sendBufferSize, int recvBufferSize, bool keepAlive, bool dontRoute, bool noDelay);
	}
}

