// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using System;
using System.Linq;
using System.Reflection;

namespace DotNetOrb.Core
{
    public class RepositoryId
    {
        public static Type GetTypeFromRepositoryId(string repId)
        {
            if (repId.StartsWith("IDL:"))
            {
                if (repId.Equals(WStringValueHelper.Id))
                {
                    return typeof(string);
                }

                if (repId.Equals(StringValueHelper.Id))
                {
                    return typeof(string);
                }
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var types = asm.GetTypes();
                    foreach (var type in types)
                    {
                        var att = (RepositoryIDAttribute)type.GetCustomAttributes(typeof(RepositoryIDAttribute)).FirstOrDefault();
                        if (att != null && att.Id.Equals(repId))
                        {
                            return type;
                        }
                    }
                }
            }
            else
            {
                throw new IntfRepos("Unrecognized RepositoryID: " + repId);
            }
            return null;
        }

        public static Type GetHelperType(string repId)
        {
            if (repId.StartsWith("IDL:"))
            {
                if (repId.Equals(WStringValueHelper.Id))
                {
                    return typeof(WStringValueHelper);
                }

                if (repId.Equals(StringValueHelper.Id))
                {
                    return typeof(StringValueHelper);
                }
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var types = asm.GetTypes();
                    foreach (var type in types)
                    {
                        var att = (RepositoryIDAttribute)type.GetCustomAttributes(typeof(RepositoryIDAttribute)).FirstOrDefault();
                        if (att != null && att.Id.Equals(repId))
                        {
                            var helper = GetHelperType(type);
                            if (helper != null)
                            {
                                return helper;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new IntfRepos("Unrecognized RepositoryID: " + repId);
            }
            return null;
        }

        public static Type GetHelperType(Type objectType)
        {
            var helperAtt = (HelperAttribute)objectType.GetCustomAttribute(typeof(HelperAttribute));
            if (helperAtt != null)
            {
                return helperAtt.HelperType;
            }
            return null;
        }

        public static Type GetStubType(string repId)
        {
            if (repId.StartsWith("IDL:"))
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var types = asm.GetTypes();
                    foreach (var type in types)
                    {
                        var att = (RepositoryIDAttribute)type.GetCustomAttributes(typeof(RepositoryIDAttribute)).FirstOrDefault();
                        if (att != null && att.Id.Equals(repId))
                        {
                            var stubAtt = (StubAttribute)type.GetCustomAttribute(typeof(StubAttribute));
                            if (stubAtt != null)
                            {
                                return stubAtt.StubType;
                            }
                        }
                    }
                }
            }
            else
            {
                throw new IntfRepos("Unrecognized RepositoryID: " + repId);
            }
            return null;
        }

        public static Type GetValueFactoryType(string repId)
        {
            var valueType = GetTypeFromRepositoryId(repId);
            if (valueType != null)
            {
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var types = asm.GetTypes();
                    foreach (var type in types)
                    {
                        var att = (ValueFactoryAttribute)type.GetCustomAttribute(typeof(ValueFactoryAttribute));
                        if (att != null && att.ValueType.Equals(valueType))
                        {
                            return type;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///  Returns the fully qualified name of the Java class to which the given Repository ID is mapped, 
        ///  with a given suffix appended to the class name. For example, the string "Helper" can be used as
        ///  the suffix to find the helper class for a given Repository ID.
        /// </summary>
        public static string GetClassName(string repId, string suffix)
        {
            if (repId.StartsWith("IDL:"))
            {
                if (repId.Equals(WStringValueHelper.Id))
                {
                    return "System.String";
                }

                if (repId.Equals(StringValueHelper.Id))
                {
                    return "System.String";
                }
                //"IDL:XXX:1.0":
                string id = repId.Substring(4, repId.LastIndexOf(':') - 4) + (suffix != null ? suffix : "");

                //"IDL:omg.org/CosNaming/NamingContext/NotFound:1.0":
                int firstSlash = id.IndexOf("/");
                var className = id.Substring(firstSlash + 1).Replace('/', '.');

                var type = GetType(className);
                if (type != null)
                {
                    return className;
                }
                //try with I prefix for interfaces
                className = "I" + className;
                type = GetType(className);
                if (type != null)
                {
                    return className;
                }
            }
            throw new IntfRepos("Unrecognized RepositoryID: " + repId);
        }

        public static string GetRepositoryId(Type type)
        {
            var idAtt = type.GetCustomAttributes(typeof(RepositoryIDAttribute)).FirstOrDefault() as RepositoryIDAttribute;
            if (idAtt != null)
            {
                return idAtt.Id;
            }
            throw new IntfRepos("Repository id not found for type: " + type.FullName);
        }

        public static IBoxedValueHelper CreateBoxedValueHelper(string repId)
        {
            var helperType = GetHelperType(repId);
            if (helperType != null)
            {
                try
                {
                    return (IBoxedValueHelper)Activator.CreateInstance(helperType);
                }
                catch (Exception e)
                {
                    throw new RuntimeException("Unable to create boxed value helper", e);
                }
            }
            return null;
        }

        public static Type GetType(string className)
        {
            var type = Type.GetType(className);
            if (type != null)
            {
                return type;
            }
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(className);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}
