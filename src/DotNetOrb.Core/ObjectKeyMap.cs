// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.Util;
using System;
using System.Collections.Generic;

namespace DotNetOrb.Core
{
    public class ObjectKeyMap
    {
        private ORB orb;
        private Dictionary<string, object> objectKeyMap = new Dictionary<string, object>();
        private object syncObject = new object();

        public ObjectKeyMap(ORB orb)
        {
            this.orb = orb;
        }

        /// <summary>
        /// Map an object key to another, as defined by the value of a corresponding configuration property 
        /// e.g.map "NameService" to "StandardNS/NameServer-POA/_root"
        /// </summary>
        /// <param name="originalKey">originalKey a byte[] value containing the original key</param>
        /// <returns>a byte[] value containing the mapped key, if a  mapping is defined, originalKey otherwise</returns>
        /// <exception cref="BadParam"></exception>
        public byte[] MapObjectKey(byte[] originalKey)
        {
            lock (syncObject)
            {
                byte[] result = originalKey;

                if (objectKeyMap.Count > 0)
                {
                    var origKey = BitConverter.ToString(originalKey);

                    if (objectKeyMap.ContainsKey(origKey))
                    {
                        object found = objectKeyMap[origKey];
                        if (found is string keyStr)
                        {
                            if (ParsedIOR.IsParsableProtocol(keyStr))
                            {
                                // We have found a file reference. Use ParsedIOR to get the byte key.
                                try
                                {
                                    ParsedIOR ior = new ParsedIOR(orb, keyStr);

                                    result = ior.GetObjectKey();
                                }
                                catch (ArgumentException e)
                                {
                                    throw new BadParam("Could not extract object key from IOR: " + e);
                                }
                            }
                            else
                            {
                                result = CorbaLoc.ParseKey(keyStr);
                            }
                            // This 'hack' does the following - we cannot parse the key in configuration
                            // as the service may not have been started yet so the files may not exist. So we have
                            // to do it on demand - as an optimisation we then overwrite the original value with the
                            // parsed form to save speed on future lookups.
                            objectKeyMap[origKey] = result;
                        }
                        else
                        {
                            // Must be a byte - already parsed it
                            result = (byte[])found;
                        }
                    }
                }
                return result;
            }
        }

        public void ConfigureObjectKeyMap(IConfiguration config)
        {
            lock (syncObject)
            {
                string prefix = "DotNetOrb.Orb.ObjectKeyMap.";
                List<string> names = config.GetKeysWithPrefix(prefix);
                try
                {
                    foreach (var name in names)
                    {
                        var keyName = name.Substring(prefix.Length);
                        var fullPath = config.GetValue(name);
                        AddObjectKey(keyName, fullPath);
                    }
                }
                catch (ConfigException e)
                {
                    throw new RuntimeException("should never happen", e);
                }
            }
        }

        /// <summary>
        /// Allows the internal objectKeyMap to be altered programmatically. The objectKeyMap 
        /// allows more readable corbaloc URLs by mapping the actual object key to an arbitary string. 
        /// </summary>
        /// <param name="keyName">a string value e.g. "NameService"</param>
        /// <param name="fullPath">a string value e.g. "file:/home/rnc/NameSingleton.ior"</param>
        public void AddObjectKey(string keyName, string fullPath)
        {
            lock (syncObject)
            {
                objectKeyMap[keyName] = fullPath;
            }

        }

        public void AddObjectKey(string keyName, CORBA.Object @object)
        {
            AddObjectKey(keyName, orb.ObjectToString(@object));
        }
    }
}
