// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;

namespace DotNetOrb.Core.POA
{
    /// <summary>
    ///  Factory class to create instances of RPPoolManager depending on the configuration of the ORB 
    ///  this factory will return a new instance for every call or return a shared instance.
    /// </summary>
    public class RPPoolManagerFactory
    {
        private ORB orb;
        private ILogger logger;
        private IConfiguration configuration;
        private int threadPoolMin;
        private int threadPoolMax;
        private int poolThreadTimeout;
        private RPPoolManager sharedInstance;
        private bool poolsShouldBeShared;


        public RPPoolManagerFactory(ORB orb)
        {
            configuration = orb.Configuration;
            this.orb = orb;
            logger = configuration.GetLogger(GetType());
            threadPoolMin = configuration.GetAsInteger("DotNetOrb.POA.ThreadPoolMin", 5);

            if (threadPoolMin < 1)
            {
                throw new ConfigException("DotNetOrb.POA.ThreadPoolMin must be >= 1");
            }

            threadPoolMax = configuration.GetAsInteger("DotNetOrb.POA.ThreadPoolMax", 20);

            if (threadPoolMax > 0 && threadPoolMax < threadPoolMin)
            {
                throw new ConfigException("DotNetOrb.POA.ThreadPoolMax must be >= " + threadPoolMin + "(DotNetOrb.POA.ThreadPoolMin)");
            }

            poolsShouldBeShared = configuration.GetAsBoolean("DotNetOrb.POA.ThreadPoolShared", false);

            poolThreadTimeout = configuration.GetAsInteger("DotNetOrb.POA.ThreadTimeout", 0);

            if (logger.IsDebugEnabled)
            {
                logger.Debug("RequestProcessorPoolFactory settings: ThreadPoolMin=" + threadPoolMin + " ThreadPoolMax=" + threadPoolMax + " ThreadPoolShared=" + poolsShouldBeShared);
            }

            if (poolsShouldBeShared)
            {
                sharedInstance = new SharedRPPoolManager(orb.GetPOACurrent(), threadPoolMin, threadPoolMax, poolThreadTimeout, logger, configuration);
            }
        }

        public RPPoolManager NewRPPoolManager(bool isSingleThreaded)
        {
            if (isSingleThreaded)
            {
                return new SingleThreadedRPPoolManager(orb.GetPOACurrent(), poolThreadTimeout, logger, configuration);

            }
            else if (poolsShouldBeShared)
            {
                return sharedInstance;
            }
            else
            {
                return new DefaultRPPoolManager(orb.GetPOACurrent(), threadPoolMin, threadPoolMax, poolThreadTimeout, logger, configuration);
            }
        }

        public void Destroy()
        {
            if (sharedInstance != null)
            {
                sharedInstance._Destroy();
            }
        }

    }
}
