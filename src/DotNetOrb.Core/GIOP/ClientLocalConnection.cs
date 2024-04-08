// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Local;
using System;

namespace DotNetOrb.Core.GIOP
{
    public class ClientLocalConnection : GIOPConnection
    {
        protected MultithreadEventLoopGroup group = new MultithreadEventLoopGroup(1);
        protected Bootstrap bootstrap = new Bootstrap();
        private LocalAddress localAddress;

        private ClientLocalChannelHandler channelHandler;
        public GIOPChannelHandler ChannelHandler { get => channelHandler; }

        IChannel? channel;

        public bool IsConnected
        {
            get
            {
                if (channel != null)
                {
                    return channel.Active;
                }
                return false;
            }
        }

        public ClientLocalConnection(ORB orb) : base(orb)
        {
            localAddress = orb.LocalAddress;
            channelHandler = new ClientLocalChannelHandler(orb, this);
            Configure(orb.Configuration);
            Connect();
        }

        public override void Configure(IConfiguration config)
        {
            base.Configure(config);

            logger = config.GetLogger(GetType());

            bootstrap
                   .Group(group)
                   .Channel<LocalChannel>()
                   .Handler(new ActionChannelInitializer<LocalChannel>(channel =>
                   {
                       IChannelPipeline pipeline = channel.Pipeline;
                       pipeline.AddLast(new GIOPCodec(orb), channelHandler);
                   }));
        }



        public override void Connect()
        {
            if (!IsConnected)
            {
                try
                {
                    var task = bootstrap.ConnectAsync(localAddress);
                    channel = task.Result;
                    logger.Info("Opened client-side local transport");
                }
                catch (Exception ex)
                {
                    logger.Error("Unable to open client-side local transport", ex);
                }
            }
        }

        public override void Close()
        {
            if (channel != null && channel.Active)
            {
                try
                {
                    var task = channel.CloseAsync();
                    task.Wait();
                    logger.Info("Closed client-side local transport");
                }
                catch (AggregateException ex)
                {
                    logger.Error("Unable to close client-side local transport", ex.InnerException);
                }
                finally
                {
                    group.ShutdownGracefullyAsync().Wait(1000);
                }
            }

        }
    }

}
