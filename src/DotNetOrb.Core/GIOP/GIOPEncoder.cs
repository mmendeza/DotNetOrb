// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using DotNetOrb.Core.Util;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;

namespace DotNetOrb.Core.GIOP
{
    public class GIOPEncoder : MessageToByteEncoder<GIOPMessage>, IConfigurable
    {
        private ORB orb;

        ILogger logger;

        public GIOPEncoder(ORB orb)
        {
            Configure(orb.Configuration);

        }
        public void Configure(IConfiguration config)
        {
            try
            {
                logger = config.GetLogger(GetType().FullName);
            }
            catch (ConfigException)
            {
                throw new CORBA.Internal("Unable to configure GIOPEncoder");
            }
        }
        protected override void Encode(IChannelHandlerContext context, GIOPMessage message, IByteBuffer output)
        {
            message.Write(output);
            //var bytes = new byte[output.ReadableBytes];
            //output.GetBytes(output.ReaderIndex, bytes);
            //var str = ObjectUtil.BufToString(bytes, 0, bytes.Length);
        }
    }
}
