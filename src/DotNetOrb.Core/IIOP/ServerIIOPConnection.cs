// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using CSIIOP;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.GIOP;
using DotNetty.Handlers.Timeout;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace DotNetOrb.Core.IIOP
{
    public class ServerIIOPConnection : ServerConnection
    {
        /// <summary>
        /// The maximum set of security options supported by the SSL mechanism 
        /// </summary>
        private static int MAX_SSL_OPTIONS = Integrity.Value |
                                                   Confidentiality.Value |
                                                   DetectReplay.Value |
                                                   DetectMisordering.Value |
                                                   EstablishTrustInTarget.Value |
                                                   EstablishTrustInClient.Value;

        /// <summary>
        /// The minimum set of security options supported by the SSL mechanism which cannot be turned off, so they are always supported
        /// </summary>
        private static int MIN_SSL_OPTIONS = Integrity.Value | DetectReplay.Value | DetectMisordering.Value;

        private MultithreadEventLoopGroup bossGroup = new MultithreadEventLoopGroup(1);
        private MultithreadEventLoopGroup workerGroup = new MultithreadEventLoopGroup();
        private X509Certificate2 tlsCertificate = null;
        private bool supportSSL;
        private bool isSSL;
        private int serverSupports = 0;
        private int serverRequires = 0;
        private bool requireSSL;
        protected int noOfRetries = 5;
        protected int retryInterval = 0;
        private int idleTimeout = 0;
        private IIOPAddress address;
        private IPEndPoint endpoint;

        private IChannel? channel;
        public IChannel? Channel => channel;

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

        private bool NegotiateClientCertificate
        {
            get
            {
                return (serverRequires & EstablishTrustInClient.Value) != 0;
            }
        }

        public ServerIIOPConnection(ORB orb, IIOPAddress address, bool isSSL) : base(orb)
        {
            this.address = address;
            this.isSSL = isSSL;
            Configure(orb.Configuration);
            Connect();
        }

        public override void Configure(IConfiguration config)
        {
            base.Configure(config);

            noOfRetries = config.GetAsInteger("DotNetOrb.Retries", 5);
            retryInterval = config.GetAsInteger("DotNetOrb.RetryInterval", 500);
            idleTimeout = config.GetAsInteger("DotNetOrb.Connection.Server.IdleTimeout", 0);

            if (address != null)
            {
                address.Configure(config);
                endpoint = new IPEndPoint(address.Address, address.Port);
            }

            supportSSL = config.GetAsBoolean("DotNetOrb.Security.SSL.IsSupported", false);

            if (supportSSL)
            {
                serverRequires = config.GetAsInteger("DotNetOrb.Security.SSL.Server.RequiredOptions", 0x20, 16);
                serverSupports = (ushort)config.GetAsInteger("DotNetOrb.Security.SSL.Server.SupportedOptions", 0, 16);
                // make sure that the minimum options are always in the set of supported options
                serverSupports |= MIN_SSL_OPTIONS;
                //if we require EstablishTrustInTarget or EstablishTrustInClient, SSL must be used.
                requireSSL = (serverRequires & MAX_SSL_OPTIONS) != 0;
            }

            if (isSSL)
            {
                //TODO Retrieve certificate from X509Store ??
                var certPath = config.GetValue("DotNetOrb.IIOP.SSL.Certificate");
                var pwd = config.GetValue("DotNetOrb.IIOP.SSL.CertificatePassword");
                tlsCertificate = new X509Certificate2(certPath, pwd);
            }

            logger = config.GetLogger(GetType());

            bootstrap
                    .Group(bossGroup, workerGroup)
                    .Channel<TcpServerSocketChannel>()
                    //.Option(ChannelOption.SoBacklog, 100)
                    //.Handler(new LoggingHandler("LSTN"))
                    .ChildHandler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;
                        if (idleTimeout > 0)
                        {
                            pipeline.AddLast(new IdleStateHandler(0, 0, idleTimeout));
                        }
                        if (tlsCertificate != null)
                        {
                            pipeline.AddLast(new TlsHandler(new ServerTlsSettings(tlsCertificate, NegotiateClientCertificate)));
                        }
                        //pipeline.AddLast(new LoggingHandler("CONN"));
                        pipeline.AddLast(new GIOPCodec(orb), new GIOPCodeSetServerHandler(orb, this));
                        if (orb.IsBidir)
                        {
                            pipeline.AddLast(new GIOPBiDirServerHandler(orb));
                        }
                        pipeline.AddLast(new ServerIIOPChannelHandler(orb, this));
                    }));
        }


        public override void Connect()
        {
            var retries = 0;
            while (!IsConnected && retries < noOfRetries)
            {
                retries++;
                try
                {
                    var task = bootstrap.BindAsync(endpoint);
                    channel = task.Result;
                    logger.Info("Opened server-side TCP/IP transport: " + endpoint.ToString());
                }
                catch (AggregateException ae)
                {
                    logger.Error("Unable to open server-side TCP/IP transport: " + endpoint.ToString(), ae.InnerException);
                    Task.Delay(retryInterval).Wait();
                }
            }
            if (!IsConnected)
            {
                throw new Transient("Unable to open server-side TCP/IP transport: " + endpoint.ToString());
            }
        }

        public override void Close()
        {
            if (channel != null && IsConnected)
            {
                try
                {
                    var task = channel.CloseAsync();
                    task.Wait();
                    IsClosed = true;
                    logger.Info("Closed server-side TCP/IP transport: " + endpoint.ToString());
                }
                catch (AggregateException ex)
                {
                    logger.Error("Unable to close server-side TCP/IP transport: " + endpoint.ToString(), ex.InnerException);
                }
                finally
                {
                    Task.WaitAll(bossGroup.ShutdownGracefullyAsync(), workerGroup.ShutdownGracefullyAsync());
                }
            }
        }
    }

}
