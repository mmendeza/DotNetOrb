// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using DotNetOrb.Core.Util;
using System;

namespace DotNetOrb.Core.POA
{
    public class POAManagerMonitor : IPOAManagerMonitor
    {
        private POAManager model = null;

        private IConfiguration configuration = null;
        private ILogger logger;
        private bool doMonitor;

        public void Configure(IConfiguration config)
        {
            configuration = config;
            logger = configuration.GetLogger(GetType());
            doMonitor = configuration.GetAsBoolean("DotNetOrb.POA.Monitoring", false);
        }

        public void AddPOA(string name)
        {
        }

        public void CloseMonitor()
        {
        }

        public void Init(POAManager poaManager)
        {
            model = poaManager;
        }

        public void OpenMonitor()
        {
            if (doMonitor)
            {
                try
                {
                    IPOAManagerMonitor newMonitor = (IPOAManagerMonitor)ObjectUtil.GetInstance("DotNetOrb.POA.POAManagerMonitorImpl");
                    if (newMonitor != null)
                    {
                        newMonitor.Init(model);
                        newMonitor.Configure(configuration);
                        model.Monitor = newMonitor;
                        newMonitor.OpenMonitor();
                    }

                }
                catch (Exception exception)
                {
                    if (logger.IsErrorEnabled)
                        logger.Error("Exception in OpenMonitor(): " + exception.ToString());
                }
            }
        }

        public void PrintMessage(string str)
        {
        }

        public void RemovePOA(string name)
        {
        }

        public void SetToActive()
        {
        }

        public void SetToDiscarding(bool wait)
        {
        }

        public void SetToHolding(bool wait)
        {
        }

        public void SetToInactive(bool wait, bool etherialize)
        {
        }
    }
}
