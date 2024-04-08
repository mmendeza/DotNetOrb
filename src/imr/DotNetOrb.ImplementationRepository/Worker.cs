// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.POA;
using Microsoft.Extensions.Logging;
using PortableServer;

namespace DotNetOrb.ImplementationRepository
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ImplementationRepository imr;

        public Worker(ILogger<Worker> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            Logger.LoggerFactory = loggerFactory;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var argProps = new Dictionary<string, string>();
            argProps.Add("DotNetOrb.ImplName", "the_ImR");
            argProps.Add("DotNetOrb.UseImR", "false");
            argProps.Add("DotNetOrb.UseTaoImR", "false");

            //Write IOR to file
            Core.ORB orb = (Core.ORB)CORBA.ORB.Init(argProps);

            imr = new ImplementationRepository(orb);
            imr.Configure(orb.Configuration);

            IPOA rootPOA = POAHelper.Narrow(orb.ResolveInitialReferences("RootPOA"));
            rootPOA.ThePOAManager.Activate();

            var policies = new CORBA.IPolicy[2];

            policies[0] = rootPOA.CreateLifespanPolicy(LifespanPolicyValue.PERSISTENT);
            policies[1] = rootPOA.CreateIdAssignmentPolicy(IdAssignmentPolicyValue.USER_ID);

            IPOA imrPoa = rootPOA.CreatePoa("ImRPOA", rootPOA.ThePOAManager, policies);

            for (int i = 0; i < policies.Length; i++)
            {
                policies[i].Destroy();
            }

            byte[] id = System.Text.Encoding.Default.GetBytes("ImR");

            imrPoa.ActivateObjectWithId(id, imr);

            var iorFile = imr.IorFile;

            var imrReference = imrPoa.ServantToReference(imr);

            var ior = orb.ObjectToString(imrReference);
            _logger.LogInformation(ior);
            try
            {
                File.WriteAllText(iorFile, ior);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to write ior to " + iorFile);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            imr.Shutdown(true);
            return base.StopAsync(cancellationToken);
        }

        private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            _logger.LogDebug("ImR: Shutting down");
            imr.Shutdown(true);
        }
    }
}