// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;

namespace DotNetOrb.Core.POA
{
    public class SharedRPPoolManager : RPPoolManager
    {
        public SharedRPPoolManager(Current current, int min, int max, int pt, ILogger logger, IConfiguration configuration) : base(current, min, max, pt, logger, configuration)
        {
        }

        /// <summary>
        /// Invoked by clients to indicate that they won't use this poolManager anymore.
        /// </summary>
        public override void Destroy()
        {
            // ignore request as this is a shared pool.
            // the pool will be destroyed with the enclosing factory.
        }
    }
}
