// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.DSI;
using PortableServer;
using static DotNetOrb.Core.POA.POAConstants;

namespace DotNetOrb.Core.POA
{
    public class EventMulticaster : IAOMListener, IRequestQueueListener, IRPPoolManagerListener, IPOAListener, IEventListener
    {

        protected IEventListener one, two;
        protected EventMulticaster(IEventListener one, IEventListener two)
        {
            this.one = one;
            this.two = two;
        }
        public static IEventListener Add(IEventListener one, IEventListener two)
        {
            if (one == null) return two;
            if (two == null) return one;
            return new EventMulticaster(one, two);
        }
        public void ObjectActivated(byte[] oid, Servant servant, int size)
        {
            ((IAOMListener)one).ObjectActivated(oid, servant, size);
            ((IAOMListener)two).ObjectActivated(oid, servant, size);
        }
        public void ObjectDeactivated(byte[] oid, Servant servant, int size)
        {
            ((IAOMListener)one).ObjectDeactivated(oid, servant, size);
            ((IAOMListener)two).ObjectDeactivated(oid, servant, size);
        }
        public void PoaCreated(POA poa)
        {
            ((IPOAListener)one).PoaCreated(poa);
            ((IPOAListener)two).PoaCreated(poa);
        }
        public void PoaStateChanged(POA poa, POAState newState)
        {
            ((IPOAListener)one).PoaStateChanged(poa, newState);
            ((IPOAListener)two).PoaStateChanged(poa, newState);
        }
        public void ProcessorAddedToPool(RequestProcessor processor, int pool_count, int pool_size)
        {
            ((IRPPoolManagerListener)one).ProcessorAddedToPool(processor, pool_count, pool_size);
            ((IRPPoolManagerListener)two).ProcessorAddedToPool(processor, pool_count, pool_size);
        }
        public void ProcessorRemovedFromPool(RequestProcessor processor, int pool_count, int pool_size)
        {
            ((IRPPoolManagerListener)one).ProcessorRemovedFromPool(processor, pool_count, pool_size);
            ((IRPPoolManagerListener)two).ProcessorRemovedFromPool(processor, pool_count, pool_size);
        }
        public void ReferenceCreated(CORBA.Object obj)
        {
            ((IPOAListener)one).ReferenceCreated(obj);
            ((IPOAListener)two).ReferenceCreated(obj);
        }
        private IEventListener Remove(IEventListener l)
        {
            if (l == one) return two;
            if (l == two) return one;
            IEventListener a = Remove(one, l);
            IEventListener b = Remove(two, l);
            if (a == one && b == two)
            {
                return this;
            }
            return Add(a, b);
        }

        public static IEventListener Remove(IEventListener l, IEventListener old)
        {
            if (l == old || l == null)
            {
                return null;
            }
            else if (l is EventMulticaster)
            {
                return ((EventMulticaster)l).Remove(old);
            }
            else
            {
                return l;
            }
        }
        public void RequestAddedToQueue(ServerRequest request, int queueSize)
        {
            ((IRequestQueueListener)one).RequestAddedToQueue(request, queueSize);
            ((IRequestQueueListener)two).RequestAddedToQueue(request, queueSize);
        }
        public void RequestRemovedFromQueue(ServerRequest request, int queueSize)
        {
            ((IRequestQueueListener)one).RequestRemovedFromQueue(request, queueSize);
            ((IRequestQueueListener)two).RequestRemovedFromQueue(request, queueSize);
        }
        public void ServantEtherialized(byte[] oid, Servant servant)
        {
            ((IAOMListener)one).ServantEtherialized(oid, servant);
            ((IAOMListener)two).ServantEtherialized(oid, servant);
        }
        public void ServantIncarnated(byte[] oid, Servant servant)
        {
            ((IAOMListener)one).ServantIncarnated(oid, servant);
            ((IAOMListener)two).ServantIncarnated(oid, servant);
        }
    }
}
