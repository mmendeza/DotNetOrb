// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetOrb.ImplementationRepository
{
    /// <summary>
    /// This class provides shared or exclusive access to a ressource. It preferes the exclusive access, 
    /// i.e. if threads are waiting for exclusive access, shared locks can't be gained.
    /// </summary>
    [Serializable]
    public class ResourceLock
    {
        private int shared;
        private int exclusive;
        private bool exclusivesWaiting = false;
        private object syncObject = new object();

        public ResourceLock()
        {
            shared = 0;
            exclusive = 0;
        }

        /// <summary>
        /// This method tries to aquire a shared lock. It blocks until the exclusive lock is released.
        /// </summary>
        public void GainSharedLock()
        {
            lock (syncObject)
            {
                while (exclusive > 0 && exclusivesWaiting)
                {
                    try
                    {
                        Monitor.Wait(syncObject);
                    }
                    catch (Exception)
                    {
                    }
                }
                shared++;
            }
        }

        /// <summary>
        /// Release the shared lock. Unblocks threads waiting for access.
        /// </summary>
        public void ReleaseSharedLock()
        {
            lock (syncObject)
            {
                if (--shared == 0)
                {
                    Monitor.PulseAll(syncObject);
                }

            }
        }

        /// <summary>
        /// This method tries to aquire an exclusive lock. It blocks until all shared locks have been released.
        /// </summary>
        public void GainExclusiveLock()
        {
            lock (syncObject)
            {
                while (shared > 0 || exclusive > 0)
                {
                    try
                    {
                        exclusivesWaiting = true;
                        Monitor.Wait(syncObject);
                    }
                    catch (Exception)
                    {
                    }
                }
                exclusive++;
                exclusivesWaiting = false;
            }
        }

        /// <summary>
        /// Releases the exclusive lock. Unblocks all threads waiting for access.
        /// </summary>
        public void ReleaseExclusiveLock()
        {
            lock (syncObject)
            {
                if (--exclusive == 0)
                {
                    Monitor.PulseAll(syncObject);
                }

            }
        }

    }
}
