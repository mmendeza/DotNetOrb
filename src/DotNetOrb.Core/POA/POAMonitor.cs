// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using DotNetOrb.Core.Util;
using System;

namespace DotNetOrb.Core.POA
{
    internal class POAMonitor : IPOAMonitor
    {
        private POA poa;
        private AOM aom;
        private RequestQueue requestQueue;
        private RPPoolManager rpPoolManager;

        /** the configuration object for this POA instance */
        private IConfiguration configuration = null;
        private ILogger logger;
        private bool doMonitor;

        private string prefix;

        public void Init(POA poa, AOM aom, RequestQueue requestQueue, RPPoolManager pm, string prefix)
        {
            this.poa = poa;
            this.aom = aom;
            this.requestQueue = requestQueue;
            rpPoolManager = pm;
            this.prefix = prefix;
        }


        public void ChangeState(string state)
        {
        }

        public void CloseMonitor()
        {
        }

        public void Configure(IConfiguration config)
        {
            configuration = config;
            logger = config.GetLogger(GetType());
            doMonitor = config.GetAsBoolean("DotNetOrb.POA.Monitoring", false);
        }

        public void OpenMonitor()
        {
            if (doMonitor)
            {
                try
                {
                    POAMonitor newMonitor = (POAMonitor)ObjectUtil.GetInstance("DotNetOrb.POA.POAMonitorImpl");
                    newMonitor.Init(poa, aom, requestQueue, rpPoolManager, prefix);
                    newMonitor.Configure(configuration);
                    poa.POAMonitor = newMonitor;
                    newMonitor.OpenMonitor();
                }
                catch (Exception exception)
                {
                    if (logger.IsWarnEnabled)
                    {
                        logger.Warn("Exception during OpenMonitor() of POAMonitor" + exception.ToString());
                    }
                }
            }
        }
    }
}
