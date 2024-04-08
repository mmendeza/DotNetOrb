// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CONV_FRAME;
using DotNetOrb.Core.CDR;
using DotNetOrb.Core.Config;
using DotNetty.Transport.Bootstrapping;
using ETF;

namespace DotNetOrb.Core.GIOP
{
    public abstract class ClientConnection : GIOPConnection
    {
        protected Bootstrap bootstrap = new Bootstrap();
        protected bool IsTCSNegotiated = false;
        protected bool ignoreComponentInfo = false;
        protected uint clientCount;
        internal uint ClientCount => clientCount;

        private object syncObject = new object();

        public abstract GIOPChannelHandler ChannelHandler { get; }

        protected ClientConnection(ORB orb, IProfile profile) : base(orb)
        {
            this.profile = profile;
        }

        public override void Configure(IConfiguration config)
        {
            base.Configure(config);
            //ignoreComponentInfo = !config.GetAsBoolean("DotNetOrb.Codeset", true);
        }

        public void SetCodeSet(ParsedIOR pior)
        {
            if (IsTCSNegotiated)
            {
                //if already negotiated, do nothing
                return;
            }

            //if the other side only talks GIOP 1.0, don't send a codeset
            //context and don't try again
            if (pior.GetEffectiveProfile().Version.Minor == 0)
            {
                IsTCSNegotiated = true;
                return;
            }

            CodeSetComponentInfo info = pior.CodeSetComponentInfo;
            if (info != null && !ignoreComponentInfo)
            {
                IsTCSNegotiated = true;
                CodeSetChar = CodeSet.GetNegotiatedCodeSet(orb, info, /* wide */ false);
                CodeSetWChar = CodeSet.GetNegotiatedCodeSet(orb, info, /* wide */ true);

                logger.Info("Negotiated char codeset of " + CodeSetChar + " and wchar of " + CodeSetWChar);
            }
            else
            {
                logger.Debug("No CodeSetComponentInfo in IOR. Will use default CodeSets");

                IsTCSNegotiated = true;
            }

        }

        public void IncClients()
        {
            lock (syncObject)
            {
                clientCount++;
            }
        }

        public bool DecClients()
        {
            lock (syncObject)
            {
                clientCount--;
                if (clientCount == 0)
                {
                    return true;
                }
                return false;
            }
        }

    }

}
