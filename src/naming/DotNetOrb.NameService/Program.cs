// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.NameService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .UseWindowsService()  
                .ConfigureServices(services =>
                {
                    services.AddHostedService<NameService>();
                })
                .Build();

            host.Run();
        }
    }
}