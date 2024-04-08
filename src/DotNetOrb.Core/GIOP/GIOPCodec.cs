// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using DotNetty.Transport.Channels;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPCodec : CombinedChannelDuplexHandler<GIOPDecoder, GIOPEncoder>, IConfigurable
    {
        private ORB orb;

        GIOPDecoder decoder;

        GIOPEncoder encoder;

        public GIOPCodec(ORB orb)
        {
            this.orb = orb;
            encoder = new GIOPEncoder(orb);
            decoder = new GIOPDecoder(orb);
            Init(decoder, encoder);
        }

        public void Configure(IConfiguration config)
        {
            try
            {
                decoder.Configure(config);
                encoder.Configure(config);
            }
            catch (ConfigException)
            {
                throw new CORBA.Internal("Unable to configure GIOPEncoder");
            }
        }


    }
}
