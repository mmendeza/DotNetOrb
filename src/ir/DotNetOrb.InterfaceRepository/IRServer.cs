// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.InterfaceRepository
{
    public class IRServer : BackgroundService
    {
        private readonly ILogger<IRServer> _logger;

        public IRServer(ILogger<IRServer> logger, ILoggerFactory loggerFactory)
        {
            _logger = logger;
            Logger.LoggerFactory = loggerFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                StringTokenizer strtok =
                    new StringTokenizer(args[0], java.io.File.pathSeparator);

                URL[] urls = new URL[strtok.countTokens()];
                for (int i = 0; strtok.hasMoreTokens(); i++)
                {
                    urls[i] = new java.io.File(strtok.nextToken()).toURL();
                }

                URLClassLoader classLoader = new URLClassLoader(urls);

                Class repositoryClass = classLoader.loadClass("org.jacorb.ir.RepositoryImpl");


                Properties props = new Properties();
                props.setProperty("jacorb.orb.objectKeyMap.InterfaceRepository",
                                  "InterfaceRepository/InterfaceRepositoryPOA/IfR");
                props.setProperty("jacorb.implname", "InterfaceRepository");

                org.omg.CORBA.ORB orb = org.omg.CORBA.ORB.init(args, props);

                Object repository =
                    repositoryClass.getConstructors()[0].newInstance(
                            new Object[] { args[0], args[1], classLoader, orb });

                repositoryClass.getDeclaredMethod("loadContents", (Class[])null).invoke(repository, (Object[])null);

                orb.run();
            }
            catch (Exception e)
            {
                e.printStackTrace();
                System.exit(1);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}