// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;

namespace DotNetOrb.Core.POA
{
    public class SingleThreadedRPPoolManager : RPPoolManager
    {
        public SingleThreadedRPPoolManager(Current current, int pt, ILogger logger, IConfiguration configuration) : base(current, 1, 1, pt, logger, configuration)
        {
        }

        /// <summary>
        /// Invoked by clients to indicate that they won't use this poolManager anymore.
        /// </summary>
        public override void Destroy()
        {
            // allow destruction by clients
            _Destroy();
        }

        internal override void WarnPoolIsEmpty()
        {
            // disable the warning as this Pool is single threaded by definition there's no point
            // in issueing a warning that you should increase the pool size
        }
    }
}
