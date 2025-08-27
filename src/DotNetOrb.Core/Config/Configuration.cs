// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;

namespace DotNetOrb.Core.Config
{

    public class Configuration : IConfiguration
    {
        private IDictionary<string, object> settings = new Dictionary<string, object>();

        private readonly ORB orb;
        public ORB ORB => orb;

        public Configuration(IDictionary<string, string> props, ORB orb)
        {
            this.orb = orb;
            string id = "DotNetOrb";

            // Check ORB properties
            if (props != null && props.ContainsKey("ORBid"))
            {
                id = props["ORBid"];
            }
            //Parse command line parameters
            var args = System.Environment.GetCommandLineArgs();
            Dictionary<string, string> argsDict = new Dictionary<string, string>();
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == null)
                    {
                        continue;
                    }
                    if (string.Equals(args[i], "-ORBID", StringComparison.OrdinalIgnoreCase))
                    {
                        if (i + 1 < args.Length)
                        {
                            id = args[++i];
                            if (id == null)
                            {
                                throw new Initialize("ORBID cannot be null");
                            }
                            id = id.Trim();
                            i++;
                            break;
                        }
                        else
                        {
                            throw new Initialize("Invalid number of arguments to ORB.init");
                        }
                    }
                    if (string.Equals(args[i], "-ORBInitRef", StringComparison.OrdinalIgnoreCase))
                    {
                        if (i + 1 < args.Length)
                        {
                            var value = args[++i];
                            if (value != null)
                            {
                                var parts = value.Split('=');
                                if (parts.Length > 1)
                                {
                                    argsDict.Add("ORBInitRef." + parts[0], parts[1]);
                                }
                                throw new Initialize("Invalid ORBInitRef format -ORBInitRef " + parts[0] + ". it should be -ORBInitRef name=value.");
                            }
                            i++;
                            break;
                        }
                        else
                        {
                            throw new Initialize("Invalid ORBInitRef format. It should be -ORBInitRef name=value.");
                        }
                    }
                    if (args[i].StartsWith("-"))
                    {
                        // Strip the leading '-'.
                        var arg = args[i].Substring(1);
                        if (i + 1 < args.Length)
                        {
                            var value = args[++i];
                            if (value != null)
                            {
                                value = value.Trim();
                            }
                            argsDict.Add(arg, value);
                            i++;
                            break;
                        }
                        else
                        {
                            throw new Initialize("Invalid number of arguments to ORB.init");
                        }
                    }

                }
            }

            settings["ORBid"] = id;

            //Read environment variables
            IDictionary env = System.Environment.GetEnvironmentVariables();
            foreach (string key in env.Keys)
            {                
                var value = env[key];
                if (value != null)
                {
                    settings.Add(key, value);
                }                
            }

            //Read custom app settings section
            var section = (Hashtable)System.Configuration.ConfigurationManager.GetSection(id);
            if (section != null)
            {
                foreach (string key in section.Keys)
                {
                    var value = section[key];
                    if (value != null)
                    {
                        settings.Add(key, value);
                    }
                }
            }

            //Read appSettings.json
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder().AddJsonFile("appsettings.json");
            try
            {
                var configuration = builder.Build();
                var configSection = configuration.GetSection(id);
                if (configSection != null)
                {
                    foreach (var kvp in configSection.AsEnumerable())
                    {
                        if (kvp.Value != null)
                        {
                            //remove section name
                            var index = kvp.Key.IndexOf(':');
                            var name = kvp.Key.Substring(index + 1);
                            settings.Add(name, kvp.Value);
                        }
                    }
                }
            }            
            catch (FileNotFoundException ex)
            {
                //No configuration file found
            }

            //Add custom props
            if (props != null)
            {
                foreach (string key in props.Keys)
                {
                    var value = props[key];
                    if (value != null)
                    {
                        settings.Add(key, value);
                    }
                }
            }
            //Read command line parameters            
            foreach (string key in argsDict.Keys)
            {
                var value = argsDict[key];
                if (value != null)
                {
                    settings.Add(key, value);
                }
            }
        }

        public bool ContainsKey(string key)
        {
            return settings.ContainsKey(key);
        }

        public bool GetAsBoolean(string key)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return Convert.ToBoolean(settings[key]);
                }
                else
                {
                    throw new ConfigException("Key " + key + " not found");
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to bool");
            }
        }

        public bool GetAsBoolean(string key, bool defaultValue)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return Convert.ToBoolean(settings[key]);
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to bool");
            }
        }

        public string GetValue(string key)
        {
            if (settings.ContainsKey(key))
            {
                return settings[key]?.ToString();
            }
            else
            {
                throw new ConfigException("Key " + key + " not found");
            }
        }

        public string GetValue(string key, string defaultValue)
        {
            if (settings.ContainsKey(key))
            {
                return settings[key]?.ToString();
            }
            else
            {
                return defaultValue;
            }
        }

        public double GetAsFloat(string key)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return Convert.ToDouble(settings[key]);
                }
                else
                {
                    throw new ConfigException("Key " + key + " not found");
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to double");
            }
        }

        public double GetAsFloat(string key, double defaultValue)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return Convert.ToDouble(settings[key]);
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to double");
            }
        }

        public int GetAsInteger(string key)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return Convert.ToInt32(settings[key]);
                }
                else
                {
                    throw new ConfigException("Key " + key + " not found");
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to int");
            }
        }

        public int GetAsInteger(string key, int defaultValue)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return Convert.ToInt32(settings[key]);
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to int");
            }
        }

        public int GetAsInteger(string key, int defaultValue, int radix)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return Convert.ToInt32((string)settings[key], radix);
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to int");
            }
        }

        public long GetAsLong(string key)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return Convert.ToInt64(settings[key]);
                }
                else
                {
                    throw new ConfigException("Key " + key + " not found");
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to int");
            }
        }

        public long GetAsLong(string key, long defaultValue)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    return Convert.ToInt64(settings[key]);
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to int");
            }
        }

        public object? GetAsObject(string key, params object[] paramArray)
        {
            try
            {
                if (settings.ContainsKey(key))
                {
                    var strFullyQualifiedName = settings[key].ToString();
                    if (strFullyQualifiedName != null)
                    {
                        var type = Type.GetType(strFullyQualifiedName);
                        if (type != null)
                            return Activator.CreateInstance(type, paramArray);
                        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            type = asm.GetType(strFullyQualifiedName);
                            if (type != null)
                                return Activator.CreateInstance(type, paramArray);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to object");
            }
            return null;
        }

        public object? GetAsObject(string key, string defaultClass, params object[] paramArray)
        {
            try
            {
                var strFullyQualifiedName = defaultClass;
                if (settings.ContainsKey(key))
                {
                    strFullyQualifiedName = settings[key].ToString();
                }
                if (strFullyQualifiedName != null)
                {
                    var type = Type.GetType(strFullyQualifiedName);
                    if (type != null)
                        return Activator.CreateInstance(type, paramArray);
                    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        type = asm.GetType(strFullyQualifiedName);
                        if (type != null)
                            return Activator.CreateInstance(type, paramArray);
                    }
                }
            }
            catch (Exception)
            {
                throw new ConfigException("Unable to convert value " + settings[key] + " to object");
            }
            return null;
        }

        public ILogger GetLogger(string name)
        {
            try
            {
                if (settings.ContainsKey("DotNetOrb.Logger"))
                {
                    var strFullyQualifiedName = settings["DotNetOrb.Logger"].ToString();
                    if (strFullyQualifiedName != null)
                    {
                        var type = Type.GetType(strFullyQualifiedName);
                        if (type != null)
                            return Activator.CreateInstance(type, name) as ILogger;
                        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            type = asm.GetType(strFullyQualifiedName);
                            if (type != null)
                                return Activator.CreateInstance(type, name) as ILogger;
                        }
                    }
                }
                return new EmptyLogger(name);
            }
            catch (Exception e)
            {
                throw new ConfigException("Unable to create logger: " + e.Message);
            }
        }

        public ILogger GetLogger(Type type)
        {
            try
            {
                if (settings.ContainsKey("DotNetOrb.Logger"))
                {
                    var strFullyQualifiedName = settings["DotNetOrb.Logger"].ToString();
                    if (strFullyQualifiedName != null)
                    {
                        var loggerType = Type.GetType(strFullyQualifiedName);
                        if (loggerType != null)
                            return Activator.CreateInstance(loggerType, type) as ILogger;
                        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            type = asm.GetType(strFullyQualifiedName);
                            if (type != null)
                                return Activator.CreateInstance(type, type) as ILogger;
                        }
                    }
                }
                return new EmptyLogger(type);
            }
            catch (Exception e)
            {
                throw new ConfigException("Unable to create logger: " + e.Message);
            }
        }

        public string[] GetAsStringArray(string key)
        {
            if (settings.ContainsKey(key))
            {
                return settings[key]?.ToString().Split(',');
            }
            else
            {
                throw new ConfigException("Key " + key + " not found");
            }
        }

        public string[] GetAsStringArray(string key, string defaultValue)
        {
            if (settings.ContainsKey(key))
            {
                return settings[key]?.ToString().Split(',');
            }
            else
            {
                return defaultValue.Split(',');
            }
        }

        public List<string> GetAsList(string key)
        {
            if (settings.ContainsKey(key))
            {
                var list = new List<string>();
                var value = settings[key]?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    var items = value.Split(',');
                    foreach (var item in items)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
            else
            {
                return new List<string>();
            }
        }

        public List<string> GetAsList(string key, string defaultValue)
        {
            if (settings.ContainsKey(key))
            {
                var list = new List<string>();
                var value = settings[key]?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    var items = value.Split(',');
                    foreach (var item in items)
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
            else
            {
                return new List<string>(defaultValue.Split(','));
            }
        }

        public List<string> GetKeysWithPrefix(string prefix)
        {
            var list = new List<string>();
            foreach (var key in settings.Keys)
            {
                if (key.StartsWith(prefix))
                {
                    list.Add(key);
                }
            }
            return list;
        }

        public void SetValue(string key, int value)
        {
            settings[key] = value;
        }

        public void SetValue(string key, string value)
        {
            settings[key] = value;
        }

        public void SetValues(IDictionary<string, object> properties)
        {
            foreach (var item in properties)
            {
                settings[item.Key] = item.Value;
            }
        }
    }
}
