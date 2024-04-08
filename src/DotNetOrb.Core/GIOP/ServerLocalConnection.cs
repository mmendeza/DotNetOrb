// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.Config;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Local;
using System;
using System.Threading.Tasks;

namespace DotNetOrb.Core.GIOP
{
    public class ServerLocalConnection : ServerConnection
    {
        private MultithreadEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
        private MultithreadEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
        private LocalAddress localAddress;

        private ServerLocalChannelHandler channelHandler;

        private IChannel? channel;

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

        public ServerLocalConnection(ORB orb) : base(orb)
        {
            channelHandler = new ServerLocalChannelHandler(orb, this);
            localAddress = orb.LocalAddress;
            Configure(orb.Configuration);
            Connect();
        }

        public override void Configure(IConfiguration config)
        {
            base.Configure(config);

            logger = config.GetLogger(GetType().FullName);

            bootstrap
                   .Group(bossGroup, workerGroup)
                   .Channel<LocalServerChannel>()
                   //.Handler(new LoggingHandler("LSTN"))
                   .ChildHandler(new ActionChannelInitializer<LocalChannel>(channel =>
                   {
                       IChannelPipeline pipeline = channel.Pipeline;
                       //pipeline.AddLast(new LoggingHandler("CONN"));
                       pipeline.AddLast(new GIOPCodec(orb), channelHandler);
                   }));
        }

        public override void Connect()
        {
            if (!IsConnected)
            {
                try
                {
                    var task = bootstrap.BindAsync(localAddress);
                    channel = task.Result;
                    logger.Info("Opened server-side local channel");
                }
                catch (AggregateException ae)
                {
                    logger.Error("Unable to open server-side local channel", ae.InnerException);
                }
            }
        }

        public override void Close()
        {
            if (IsConnected)
            {
                try
                {
                    var task = channel.CloseAsync();
                    task.Wait();
                    IsClosed = true;
                    logger.Info("Closed server-side local channel");
                }
                catch (AggregateException ae)
                {
                    logger.Error("Unable to close server-side local channel", ae.InnerException);
                }
                finally
                {
                    Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
                }
            }
        }
    }

}
