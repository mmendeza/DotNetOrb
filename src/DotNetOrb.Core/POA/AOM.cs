// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using PortableServer;
using static PortableServer.POA;
using System.Collections.Concurrent;
using CORBA;
using DotNetOrb.Core.POA.Util;
using DotNetOrb.Core.POA.Exceptions;
using System.Collections.Generic;
using System.Threading;

namespace DotNetOrb.Core.POA
{
    public class AOM
    {
        class RemovalStruct
        {
            public ByteArrayKey Oidbak;
            public RequestController RequestController;
            public IServantActivator ServantActivator;
            public POA Poa;
            public bool CleanupInProgress;

            public RemovalStruct() { }

            public RemovalStruct(ByteArrayKey oidbak, RequestController requestController, IServantActivator servantActivator, POA poa, bool cleanupInProgress)
            {
                Oidbak = oidbak;
                RequestController = requestController;
                ServantActivator = servantActivator;
                Poa = poa;
                CleanupInProgress = cleanupInProgress;
            }
        }

        private object syncObject = new object();
        private IAOMListener aomListener;
        private bool unique;
        private ILogger logger;

        // an ObjectID can appear only once, but an servant can have multiple ObjectId's
        // if MULTIPLE_ID is set
        private Dictionary<ByteArrayKey, Servant> objectMap = new Dictionary<ByteArrayKey, Servant>(); // oid -> servant

        // only meaningful if UNIQUE_ID is set
        // only for performance improvements
        private Dictionary<Servant, ByteArrayKey> servantMap = new Dictionary<Servant, ByteArrayKey>(); // servant -> oid

        // for synchronisation of servant activator calls
        private HashSet<ByteArrayKey> etherealisationList = new HashSet<ByteArrayKey>();
        private HashSet<ByteArrayKey> incarnationList = new HashSet<ByteArrayKey>();
        private HashSet<ByteArrayKey> deactivationList = new HashSet<ByteArrayKey>();

        private BlockingCollection<RemovalStruct> removalQueue = new BlockingCollection<RemovalStruct>();

        /// <summary>
        /// AOMRemoval thread
        /// </summary>
        private Thread aomRemoval;
        internal bool aomRemovalActive;

        /// <summary>
        /// Counter for AOMRemoval threads
        /// </summary>
        private static int count = 0;

        /// <summary>
        /// Marker object used to signal the end of the removalQueue.
        /// </summary>
        private static RemovalStruct End = new RemovalStruct();


        internal AOM(bool unique, ILogger logger)
        {
            this.unique = unique;
            this.logger = logger;
            aomRemoval = new Thread(new ThreadStart(AOMRemoval));
            aomRemoval.Name = "AOMRemoval-" + (++count);
            aomRemovalActive = true;
            aomRemoval.Start();
        }


        /**
         * <code>add</code> is called by the POA when activating an object
         * to add a Servant into the Active Object Map.
         *
         * @param oid a <code>byte[]</code>, the id to use.
         * @param servant a <code>Servant</code>, the servant to store.
         * @exception ObjectAlreadyActive if an error occurs
         * @exception ServantAlreadyActive if an error occurs
         */
        internal void Add(byte[] oid, Servant servant)
        {
            ByteArrayKey oidbak = new ByteArrayKey(oid);
            Add(oidbak, servant);
        }


        internal void Add(ByteArrayKey oidbak, Servant servant)
        {
            lock (syncObject)
            {
                byte[] oid = oidbak.GetBytes();
                /* an incarnation and activation with the same oid has
                   priority, a reactivation for the same oid blocks until
                   etherealization is complete */

                while (incarnationList.Contains(oidbak) ||
                       etherealisationList.Contains(oidbak) ||
                       // This is to check whether we are currently deactivating an
                       // object with the same id - if so, wait for the deactivate
                       // thread to complete.
                       deactivationList.Contains(oidbak) ||
                           // This is to check whether we are attempting to reactivate
                           // a servant that is currently being deactivated with another ID.

                           servantMap.ContainsKey(servant) &&
                           deactivationList.Contains(servantMap[servant])
                       )
                {
                    try
                    {
                        Monitor.Wait(syncObject);
                    }
                    catch (ThreadInterruptedException e)
                    {
                    }
                }

                if (objectMap.ContainsKey(oidbak))
                {
                    throw new ObjectAlreadyActive();
                }

                if (unique && servantMap.ContainsKey(servant))
                {
                    throw new ServantAlreadyActive();
                }

                /* this is the actual object activation: */

                objectMap.Add(oidbak, servant);
                if (unique)
                {
                    servantMap.Add(servant, oidbak);
                }
                if (logger.IsInfoEnabled)
                {
                    logger.Info("oid: " + POAUtil.Convert(oid) + "object is activated");
                }

                // notify an aom listener
                if (aomListener != null)
                {
                    aomListener.ObjectActivated(oid, servant, objectMap.Count);
                }
            }

        }
        internal void AddAOMListener(IAOMListener listener)
        {
            lock (syncObject)
            {
                aomListener = (IAOMListener)EventMulticaster.Add(aomListener, listener);
            }
        }


        internal bool IsDeactivating(ByteArrayKey oid)
        {
            lock (syncObject)
            {
                return deactivationList.Contains(oid);
            }
        }
        internal bool Contains(Servant servant)
        {
            lock (syncObject)
            {
                return ContainsInternal(servant);
            }
        }

        private bool ContainsInternal(Servant servant)
        {
            if (unique)
            {
                return servantMap.ContainsKey(servant);
            }
            else
            {
                return objectMap.ContainsValue(servant);
            }
        }

        internal StringPair[] DeliverContent()
        {
            lock (syncObject)
            {
                List<StringPair> result = new List<StringPair>(objectMap.Count);
                foreach (var item in objectMap)
                {
                    result.Add(new StringPair(item.Key.ToString(), item.Value.GetType().Name));
                }
                return result.ToArray();
            }
        }

        internal byte[] GetObjectId(Servant servant)
        {
            lock (syncObject)
            {
                if (!unique)
                {
                    throw new POAInternalException("error: not UNIQUE_ID policy (getObjectId)");
                }

                if (servantMap.ContainsKey(servant))
                {
                    var oidbak = servantMap[servant];
                    return oidbak.GetBytes();
                }

                return null;
            }
        }

        internal Servant GetServant(byte[] oid)
        {
            return GetServant(new ByteArrayKey(oid));
        }

        internal Servant GetServant(ByteArrayKey oid)
        {
            lock (syncObject)
            {
                return objectMap[oid];
            }
        }

        internal Servant Incarnate(ByteArrayKey oidbak, IServantActivator servantActivator, POA poa)
        {
            lock (syncObject)
            {
                byte[] oid = oidbak.GetBytes();
                Servant servant = null;

                if (logger.IsInfoEnabled)
                {
                    logger.Info("oid: " + POAUtil.Convert(oid) + "incarnate");
                }

                /* all invocations of incarnate on the servant manager are serialized */
                /* all invocations of etherealize on the servant manager are serialized */
                /* invocations of incarnate and etherialize are mutually exclusive */

                while (incarnationList.Count > 0 || etherealisationList.Count > 0)
                {
                    try
                    {
                        Monitor.Wait(syncObject);
                    }
                    catch (ThreadInterruptedException e)
                    {
                    }
                }

                /* another thread was faster, the incarnation is unnecessary now */
                if (objectMap.ContainsKey(oidbak))
                {
                    return objectMap[oidbak];
                }

                /* servant incarnation */
                if (servantActivator == null)
                {
                    // This might be thrown if they failed to set implname
                    throw new ObjAdapter("Servant Activator for " + POAUtil.Convert(oid) + " was null.");
                }

                incarnationList.Add(oidbak);
                try
                {
                    servant = servantActivator.Incarnate(oid, poa);
                }
                finally
                {
                    incarnationList.Remove(oidbak);
                    Monitor.PulseAll(syncObject);
                }

                if (servant == null)
                {
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info("oid: " + POAUtil.Convert(oid) + "servant is not incarnated (incarnate returns null)");
                    }
                }
                else
                {
                    if (unique && servantMap.ContainsKey(servant))
                    {
                        if (logger.IsInfoEnabled)
                        {
                            logger.Info("oid: " + POAUtil.Convert(oid) + "servant is not incarnated (unique_id policy is violated)");
                        }
                        servant = null;
                    }
                    else
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("oid: " + POAUtil.Convert(oid) + "servant is incarnated");
                        }

                        // notify an aom listener
                        if (aomListener != null)
                        {
                            aomListener.ServantIncarnated(oid, servant);
                        }

                        /* object activation */

                        try
                        {
                            Add(oidbak, servant);
                        }
                        catch (ObjectAlreadyActive e)
                        {
                            throw new POAInternalException("error: object already active (AOM.incarnate)");
                        }
                        catch (ServantAlreadyActive e)
                        {
                            throw new POAInternalException("error: servant already active (AOM.incarnate)");
                        }
                    }
                }
                poa._GetOrb().SetDelegate(servant);
                return servant;
            }
        }

        internal void Remove(ByteArrayKey oidbak, RequestController requestController, IServantActivator servantActivator, POA poa, bool cleanupInProgress)
        {
            lock (syncObject)
            {
                if (!objectMap.ContainsKey(oidbak) || deactivationList.Contains(oidbak))
                {
                    throw new ObjectNotActive();
                }

                deactivationList.Add(oidbak);

                RemovalStruct rs = new RemovalStruct(oidbak, requestController, servantActivator, poa, cleanupInProgress);

                try
                {
                    removalQueue.Add(rs);
                }
                catch (ThreadInterruptedException ie)
                {
                }
            }
        }


        /// <summary>
        /// spawned by remove to allow deactivate_object to return immediately.
        /// </summary>
        /// <param name="oidbak">the id to use</param>
        /// <param name="requestController"></param>
        /// <param name="servantActivator"></param>
        /// <param name="poa"></param>
        /// <param name="cleanupInProgress"></param>
        private void RemoveInternal(ByteArrayKey oidbak, RequestController requestController, IServantActivator servantActivator, POA poa, bool cleanupInProgress)
        {
            byte[] oid = oidbak.GetBytes();

            if (!objectMap.ContainsKey(oidbak))
            {
                // should not happen but ...
                deactivationList.Remove(oidbak);
                return;
            }

            // wait for request completion on this object (see freeObject below)
            if (requestController != null)
            {
                requestController.WaitForObjectCompletion(oidbak);
            }

            try
            {
                ActualRemove(oidbak, servantActivator, poa, cleanupInProgress, oid);
            }
            finally
            {
                if (requestController != null)
                {
                    requestController.FreeObject(oidbak);
                }
            }
        }


        private void ActualRemove(ByteArrayKey oidbak, IServantActivator servantActivator, POA poa, bool cleanupInProgress, byte[] oid)
        {
            lock (syncObject)
            {
                Servant servant;

                if ((servant = objectMap[oidbak]) == null)
                {
                    deactivationList.Remove(oidbak);
                    return;
                }

                /* object deactivation */

                objectMap.Remove(oidbak);

                if (unique)
                {
                    servantMap.Remove(servant);
                }

                // Wait to remove the oid from the deactivationList here so that the
                // object map can be cleared out first. This ensures we don't
                // reactivate an object we're currently deactivating.
                deactivationList.Remove(oidbak);

                if (logger.IsInfoEnabled)
                {
                    logger.Info("oid: " + POAUtil.Convert(oid) + "object is deactivated");
                }

                // notify an aom listener
                if (aomListener != null)
                {
                    aomListener.ObjectDeactivated(oid, servant, objectMap.Count);
                }

                if (servantActivator == null)
                {
                    // Tell anyone waiting we're done now.
                    Monitor.PulseAll(syncObject);
                    return;
                }

                /* servant etherealization */

                /* all invocations of incarnate on the servant manager are
                   serialized,  all  invocations   of  etherealize  on  the
                   servant manager are serialized, invocations of incarnate
                   and etherialize are mutually exclusive */

                while (incarnationList.Count > 0 || etherealisationList.Count > 0)
                {
                    try
                    {
                        Monitor.Wait(syncObject);
                    }
                    catch (ThreadInterruptedException e)
                    {
                    }
                }
                etherealisationList.Add(oidbak);

                try
                {
                    servantActivator.Etherealize(oid, poa, servant, cleanupInProgress, ContainsInternal(servant));

                    if (logger.IsInfoEnabled)
                    {
                        logger.Info("oid: " + POAUtil.Convert(oid) + "servant is etherealized");
                    }

                    // notify an aom listener

                    if (aomListener != null)
                    {
                        aomListener.ServantEtherialized(oid, servant);
                    }
                }
                catch (CORBA.SystemException e)
                {
                    if (logger.IsWarnEnabled)
                    {
                        logger.Info("oid: " + POAUtil.Convert(oid) + "exception occurred during servant etherialisation: " + e.Message);
                    }
                }
                finally
                {
                    etherealisationList.Remove(oidbak);
                    Monitor.PulseAll(this);
                }
            }

        }

        internal void RemoveAll(IServantActivator servantActivator, POA poa, bool cleanupInProgress)
        {
            lock (syncObject)
            {
                foreach (var oid in objectMap.Keys)
                {
                    RemoveInternal(oid, null, servantActivator, poa, cleanupInProgress);
                }
            }
        }

        internal void RemoveAOMListener(IAOMListener listener)
        {
            lock (syncObject)
            {
                aomListener = (IAOMListener)EventMulticaster.Remove(aomListener, listener);
            }

        }

        internal int Size()
        {
            lock (syncObject)
            {
                return objectMap.Count;
            }

        }

        private void AOMRemoval()
        {
            while (aomRemovalActive)
            {
                try
                {
                    RemovalStruct rs = removalQueue.Take();
                    if (rs != End)
                    {
                        RemoveInternal(rs.Oidbak, rs.RequestController, rs.ServantActivator, rs.Poa, rs.CleanupInProgress);
                    }
                }
                catch (ThreadInterruptedException ie)
                {
                }
            }
        }

        public void EndAOMRemoval()
        {
            aomRemovalActive = false;
            try
            {
                removalQueue.Add(End);
            }
            catch (ThreadInterruptedException ex)
            {
            }
        }

    }
}
