// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using ETF;

namespace DotNetOrb.Core.ETF
{
    internal abstract class Listener : IConfigurable
    {
        protected ORB orb;
        public ListenEndpoint ListenEndpoint { get; private set; }

        protected IProfile profile;
        public IProfile Profile { get => profile; }

        protected IConfiguration configuration;

        protected ILogger logger;
        public abstract void Configure(IConfiguration config);

        /// <summary>
        /// It is possible that connection requests arrive after the initial creation of the Listener instance but before the
        /// conclusion of the configuration of the specific endpoint in this plugin. In order to provide a clear end of this configuration state,
        /// we added the Listen() method. It is called by the ORB when it ready for incoming connection and thus signals the Listener instance to
        /// start processing the incoming connection requests.Therefore, a Listener instance shall not deliver incoming connections 
        /// to the ORB before this method was called.
        /// </summary>
        public abstract void Listen();


        /// <summary>
        /// The Listener is instructed to close its endpoint. 
        /// It shall no longer accept any connection requests and shall close all connections opened by it.
        /// </summary>
        public abstract void Destroy();

    }
}
