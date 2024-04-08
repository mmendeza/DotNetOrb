// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core;
using PortableServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrb.InterfaceRepository
{
    public abstract class Contained : IRObject, IContainedOperations
    {
        protected string id;
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        protected string version = "1.0";
        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                version = value;
            }
        }

        protected IContainer definedIn;
        public IContainer DefinedIn
        {
            get
            {
                return definedIn;
            }
        }

        protected IRepository containingRepository;
        public IRepository ContainingRepository
        {
            get
            {
                return containingRepository;
            }
        }

        protected string absoluteName;
        public string AbsoluteName
        {
            get
            {
                return absoluteName;
            }
        }

        internal string FullName { get; set; }

        private static Type intfType = typeof(CORBA.IObject);
        private static Type idlType = typeof(CORBA.IIDLEntity);
        private static Type stubType = typeof(CORBA.Object);
        private static Type exceptType = typeof(CORBA.UserException);        

        public Contained()
        {
        }

        public Contained(string id, string name, string version, IContainer definedIn, string absoluteName, IRepository containingRepository)
        {
            this.id = id;
            this.name = name;
            this.version = version;
            this.definedIn = definedIn;
            this.absoluteName = absoluteName;
            this.containingRepository = containingRepository;
        }

        public static Contained CreateContained(Type type, string path, IContainer definedIn, IRepository ir, Core.ILogger logger, IPOA poa)
        {           
            if (stubType.IsAssignableFrom(type))
            {
                return null; // don't care for stubs
            }
            else if (type.IsInterface)
            {
                if (intfType.IsAssignableFrom(type))
                {
                    try
                    {
                        var helperClass = RepositoryId.GetHelperType(type);                            
                        if (helperClass == null)
                        {
                            return null;
                        }
                        InterfaceDef idef = new InterfaceDef(type, helperClass, path, definedIn, ir, poa, logger);
                        return idef;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }                                
            }
            else if (exceptType.IsAssignableFrom(type))
            {
                return new ExceptionDef(type, definedIn, ir, poa, logger);
            }
            else if (idlType.IsAssignableFrom(type))
            {
                try
                {
                    var helperClass = RepositoryId.GetHelperType(type);
                    if (helperClass == null)
                    {
                        return null;
                    }

                    var typeMethod = helperClass.GetMethod("Type");
                    if (typeMethod == null)
                    {
                        return null;
                    }
                    CORBA.TypeCode tc = (CORBA.TypeCode)typeMethod.Invoke(null, null);
                    
                    switch (tc.Kind)
                    {
                        case TCKind.TkStruct:
                            return new StructDef(type, path, definedIn, ir, logger, poa);
                        case TCKind.TkEnum:
                            return new EnumDef(type, definedIn, ir);
                        case TCKind.TkUnion:
                            return new UnionDef(type, path, definedIn, ir, logger, poa);
                        default:
                            return null;
                    }
                }                
                catch (Exception e)
                {
                    logger.Error("Caught Exception", e);
                }
                return null;
            }
            else if (type.Name.EndsWith("Helper"))
            {
                try
                {
                    var typeMethod = type.GetMethod("Type");
                    if (typeMethod == null)
                    {
                        return null;
                    }
                    CORBA.TypeCode tc = (CORBA.TypeCode)typeMethod.Invoke(null, null);
                    
                    if (tc.Kind == TCKind.TkAlias)
                    {
                        return new AliasDef(tc, definedIn, ir, logger, poa);
                    }
                }
                catch (Exception)
                {
                }
                return null;
            }
            else if (type.IsSealed)
            {
                var idlNameAtt = (IdlNameAttribute)type.GetCustomAttribute(typeof(IdlNameAttribute));
                if (idlNameAtt != null)
                {
                    try
                    {
                        var f = type.GetField("Value");
                        if (f != null)
                        {
                            return new ConstantDef(type, definedIn, ir, logger, poa);
                        }                        
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }               
            }
            else
            {
                return null;
            }
        }

        public static CORBA.IContained CreateContainedReference(Contained containedObject, Core.ILogger logger, IPOA poa)
        {
            if (containedObject == null)
            {
                throw new IntfRepos("Precondition violated in Contained createContainedReference");
            }

            PortableServer.Servant servant = null;

            switch (containedObject.DefKind)
            {
                case CORBA.DefinitionKind.DkInterface:
                    servant = new CORBA.InterfaceDefPOATie((CORBA.IInterfaceDefOperations)containedObject);
                    break;
                case CORBA.DefinitionKind.DkException:
                    servant = new CORBA.ExceptionDefPOATie((CORBA.IExceptionDefOperations)containedObject);
                    break;
                case CORBA.DefinitionKind.DkStruct:
                    servant = new CORBA.StructDefPOATie((CORBA.IStructDefOperations)containedObject);
                    break;
                case CORBA.DefinitionKind.DkEnum:
                    servant = new CORBA.EnumDefPOATie((CORBA.IEnumDefOperations)containedObject);
                    break;
                case CORBA.DefinitionKind.DkUnion:
                    servant = new CORBA.UnionDefPOATie((CORBA.IUnionDefOperations)containedObject);
                    break;
                case CORBA.DefinitionKind.DkModule:
                    servant = new CORBA.ModuleDefPOATie((CORBA.IModuleDefOperations)containedObject);
                    break;
                case CORBA.DefinitionKind.DkAlias:
                    servant = new CORBA.AliasDefPOATie((CORBA.IAliasDefOperations)containedObject);
                    break;
                case CORBA.DefinitionKind.DkConstant:
                    servant = new CORBA.ConstantDefPOATie((CORBA.IConstantDefOperations)containedObject);
                    break;
                default:
                    logger.Warn("WARNING, createContainedReference returns null for dk " + containedObject.DefKind);
                    return null;
            }

            try
            {
                var containedRef = CORBA.ContainedHelper.Narrow(poa.ServantToReference(servant));
                containedObject.Reference = containedRef;
                return containedRef;
            }
            catch (Exception e)
            {
                logger.Error("unexpected exception", e);
                return null;
            }
        }

        public abstract CORBA.Contained.Description Describe();        

        public void Move(IContainer newContainer, [WideChar(false)] string newName, [WideChar(false)] string newVersion)
        {
            definedIn = newContainer;
            version = newVersion;
            name = newName;
        }
    }
}
