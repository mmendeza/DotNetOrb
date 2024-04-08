// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetOrb.Core.Config;
using DotNetty.Common.Utilities;
using ETF;
using IOP;

namespace DotNetOrb.Core.GIOP
{
    public abstract class GIOPConnection : IConfigurable
    {
        public static readonly AttributeKey<CodeSet> TCSAttrKey = AttributeKey<CodeSet>.ValueOf("TCS");
        public static readonly AttributeKey<CodeSet> TCSWAttrKey = AttributeKey<CodeSet>.ValueOf("TCSW");
        public static readonly AttributeKey<ServiceContext> BiDirAttrKey = AttributeKey<ServiceContext>.ValueOf("BiDir");

        protected IProfile profile;
        public IProfile Profile => profile;
        public CodeSet CodeSetChar { get; protected set; }
        public CodeSet CodeSetWChar { get; protected set; }

        protected ILogger logger;
        protected ORB orb;

        public bool IsClosed { get; protected set; }

        protected IConfiguration configuration;

        protected GIOPConnection(ORB orb)
        {
            this.orb = orb;
            configuration = orb.Configuration;
            CodeSetChar = orb.DefaultCodeSetChar;
            CodeSetWChar = orb.DefaultCodeSetWChar;
        }

        public virtual void Configure(IConfiguration config)
        {
        }

        public abstract void Connect();
        public abstract void Close();
    }

}
