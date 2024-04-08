// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using PortableServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrb.NameService
{
    public class NameServantActivator : _ServantActivatorLocalBase, IConfigurable
    {        
        private ORB orb;
        private Core.Config.IConfiguration configuration;
        private Core.ILogger logger;
        
        public NameServantActivator(ORB orb)
        {
            this.orb = orb;            
        }

        public void Configure(Core.Config.IConfiguration config)
        {
            this.configuration = config;
            this.logger = config.GetLogger(GetType());
        }

        public override void Etherealize(byte[] oid, IPOA adapter, Servant serv, bool cleanupInProgress, bool remainingActivations)
        {
            var oidStr = Encoding.Default.GetString(oid);
            try
            {
                if (serv is NamingContextImpl nc)
                {
                    var f = new FileInfo(NameService.filePrefix + oidStr);
                    var fs = f.OpenWrite();
                    var sw = new StreamWriter(fs);
                    nc.Write(sw);
                    sw.Close();
                    fs.Close();
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("Saved state for servant " + oidStr);
                    }
                }
                else
                {
                    logger.Error("Invalid servant type. Expected NamingContext");
                }
                
            }
            catch (IOException io)
            {
                logger.Error("Error opening output file " + NameService.filePrefix + oidStr, io);
            }
        }

        public override Servant Incarnate(byte[] oid, IPOA adapter)
        {
            string oidStr = Encoding.Default.GetString(oid);

            NamingContextImpl n = null;
            try
            {
                var f = new FileInfo(NameService.filePrefix + oidStr);
                if (f.Exists)
                {
                    if (logger.IsDebugEnabled)
                        logger.Debug("Reading in context state from file");


                    var fs = f.OpenRead();                                        

                    if (fs.Length > 0)
                    {
                        var sr = new StreamReader(fs);
                        n = new NamingContextImpl(adapter);
                        n.Read(sr);
                        sr.Close();
                    }
                    fs.Close();
                }
                else
                {
                    if (logger.IsDebugEnabled)
                        logger.Debug("No naming context state, starting empty");
                }

            }
            catch (IOException io)
            {
                if (logger.IsDebugEnabled)
                    logger.Debug("File seems corrupt, starting empty");
            }
            catch (Exception ex)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error("Could not read object from file: " + ex.Message);
                }
                throw new RuntimeException("Could not read object from file: " + ex.Message);
            }

            if (n == null)
            {
                n = new NamingContextImpl(adapter);
            }            
            try
            {
                n.Configure(configuration);
            }
            catch (ConfigException ce)
            {
                if (logger.IsErrorEnabled)
                    logger.Error("ConfigurationException: " + ce.Message);
            }
            return n;
        }
    }
}
