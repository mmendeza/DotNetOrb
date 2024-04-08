// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;

namespace DotNetOrb.ServerStartupDaemon
{
    public class SSDService : BackgroundService
    {
        private readonly ILogger<SSDService> _logger;

        private Core.ORB orb;

        public SSDService(ILogger<SSDService> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            Logger.LoggerFactory = loggerFactory;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                orb = (Core.ORB)ORB.Init();
                ServerStartupDaemon ssd = new ServerStartupDaemon(orb);
                ssd.Configure(orb.Configuration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString(), ex);
                System.Environment.Exit(1);
            }
            _logger.LogInformation("ServerStartupDaemon started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            orb.Shutdown(true);
            _logger.LogInformation("ServerStartupDaemon stopped.");
            return base.StopAsync(cancellationToken);
        }

        private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            orb.Shutdown(true);
        }
    }
}
