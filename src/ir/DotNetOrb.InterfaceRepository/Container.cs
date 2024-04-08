// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrb.InterfaceRepository
{
    public class Container : IRObject, CORBA.IContainerOperations
    {
        protected IRObject delegator;

        // CORBA references to contained objects
        protected Dictionary<string, IContained> contained = new Dictionary<string, IContained>();

        // local references to contained objects
        protected Dictionary<string, IContained> containedLocals = new Dictionary<string, IContained>();
            
        protected DirectoryInfo myDir = null;
        protected string path = null;
        protected string fullName = null;

        // CORBA reference to this container
        protected CORBA.IContainer thisContainer;

        // outer container
        protected CORBA.IContainer definedIn;
        protected CORBA.IRepository containingRepository;

        protected bool defined = false;
        
        private IPOA poa;
        private Core.ILogger logger;

        public Container(IRObject delegator, string path, string fullName, IPOA poa, Core.ILogger logger)
        {            
            this.poa = poa;
            this.logger = logger;
            this.delegator = delegator;
            this.path = path;
            this.fullName = fullName;

            myDir = new DirectoryInfo(path + Path.DirectorySeparatorChar + 
                (fullName != null ? fullName : "").Replace('.', Path.DirectorySeparatorChar));

            if (!myDir.Exists)
            {
                throw new IntfRepos("no directory : " + path + Path.DirectorySeparatorChar + fullName);
            }

            this.name = delegator.Name;

            if (this.logger.IsDebugEnabled)
            {
                this.logger.Debug("New Container full_name " + fullName + " name : " + name + " path: " + path);
                this.logger.Debug("New Container full_name " + fullName + " name : " + name + " path: " + path);
            }
            // else: get reference from delegator, but must be postponed until later
        }

        void LoadContents()
        {
            thisContainer = CORBA.ContainerHelper.Narrow(delegator.Reference);
            if (thisContainer == null)
            {
                throw new IntfRepos("no container !");
            }

            if (delegator is Contained contained)
            {
                containingRepository = contained.ContainingRepository;
                definedIn = contained.DefinedIn;
            }
            else
            {
                containingRepository = CORBA.RepositoryHelper.Narrow(delegator.Reference);
                definedIn = containingRepository;
            }

            if (containingRepository == null)
            {
                throw new IntfRepos("no containing repository");
            }

            FileInfo[] assemblyFiles;
            DirectoryInfo[] dirs;

            // get all files in this directory which either end in ".class" or
            // do not contain a "." at all

            assemblyFiles = myDir.GetFiles("*.dll");
            dirs = myDir.GetDirectories();

            // load class files in this module/package
            if (classes != null)
            {
                String prefix =
                    (full_name != null ? full_name + '.' : "");

                for (int j = 0; j < classes.length; j++)
                {
                    try
                    {
                        if (this.logger.isDebugEnabled())
                        {
                            this.logger.debug("Container " + name + " tries " +
                                              prefix +
                                              classes[j].substring(0, classes[j].indexOf(".class")));
                        }

                        Class cl =
                            this.loader.loadClass(
                                             (prefix +
                                               classes[j].substring(0, classes[j].indexOf(".class"))
                                               ));

                        Contained containedObject =
                            Contained.createContained(cl,
                                                       path,
                                                       this_container,
                                                       containing_repository,
                                                       this.logger,
                                                       this.loader,
                                                       this.poa);
                        if (containedObject == null)
                        {
                            if (this.logger.isDebugEnabled())
                            {
                                this.logger.debug("Container: nothing created for "
                                                  + cl.getClass().getName());
                            }
                            continue;
                        }

                        org.omg.CORBA.Contained containedRef =
                            Contained.createContainedReference(containedObject,
                                                               this.logger,
                                                               this.poa);

                        containedRef.move(this_container,
                                           containedRef.name(),
                                           containedRef.version());

                        if (this.logger.isDebugEnabled())
                        {
                            this.logger.debug("Container " + prefix +
                                              " loads " + containedRef.name());
                        }

                        contained.put(containedRef.name(), containedRef);
                        containedLocals.put(containedRef.name(), containedObject);
                        if (containedObject instanceof ContainerType )
                        ((ContainerType)containedObject).loadContents();

            }
                catch (java.lang.Throwable e )
                {
                this.logger.error("Caught exception", e);
            }
        }
    }

        if(dirs != null)
        {
            for(int k = 0; k<dirs.length; k++ )
            {
                if( !dirs[k].endsWith("Package"))
                {
                    File f = new File(my_dir.getAbsolutePath() +
                                       fileSeparator +
                                       dirs[k]);
                    try
                    {
                        String[] classList = f.list();
                        if(classList != null && classList.length > 0)
                        {
                            ModuleDef m =
                                new ModuleDef(path,
                                               ((full_name != null ?
                                                 full_name + fileSeparator :
                                                 ""
                                                ) + dirs[k]).replace(fileSeparator, '.'),
                                               this_container,
                                               containing_repository,
                                               this.loader,
                                               this.poa,
                                               this.logger);

                            org.omg.CORBA.ModuleDef moduleRef =
                                org.omg.CORBA.ModuleDefHelper.narrow(
                                    this.poa.servant_to_reference(
                                        new org.omg.CORBA.ModuleDefPOATie(m)));

                            m.setReference(moduleRef );
                                                    m.loadContents();

                                                    if (this.logger.isDebugEnabled())
                                                    {
                                this.logger.debug("Container " +
                                                  full_name +
                                                  " puts module " + dirs[k]);
                            }

                            m.move( this_container, m.name(), m.version() );
                            contained.put( m.name() , moduleRef );
                            containedLocals.put( m.name(), m );
                            }
                        }
                    catch (Exception e )
                    {
                        this.logger.error("Caught Exception", e);
                    }
                }
            }
        }
    }


        public IContained[] Contents(DefinitionKind limitType, bool excludeInherited)
        {
            throw new NotImplementedException();
        }

        public IAbstractInterfaceDef CreateAbstractInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IAbstractInterfaceDef[] baseInterfaces)
        {
            throw new NotImplementedException();
        }

        public IAliasDef CreateAlias([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IIDLType originalType)
        {
            throw new NotImplementedException();
        }

        public IConstantDef CreateConstant([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IIDLType type, Any value)
        {
            throw new NotImplementedException();
        }

        public IEnumDef CreateEnum([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, string[] members)
        {
            throw new NotImplementedException();
        }

        public IExceptionDef CreateException([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, StructMember[] members)
        {
            throw new NotImplementedException();
        }

        public IExtValueDef CreateExtValue([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, bool isCustom, bool isAbstract, IValueDef baseValue, bool isTruncatable, IValueDef[] abstractBaseValues, IInterfaceDef[] supportedInterfaces, ExtInitializer[] initializers)
        {
            throw new NotImplementedException();
        }

        public IInterfaceDef CreateInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IInterfaceDef[] baseInterfaces, bool isAbstract)
        {
            throw new NotImplementedException();
        }

        public ILocalInterfaceDef CreateLocalInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IInterfaceDef[] baseInterfaces)
        {
            throw new NotImplementedException();
        }

        public IModuleDef CreateModule([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version)
        {
            throw new NotImplementedException();
        }

        public INativeDef CreateNative([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version)
        {
            throw new NotImplementedException();
        }

        public IStructDef CreateStruct([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, StructMember[] members)
        {
            throw new NotImplementedException();
        }

        public IUnionDef CreateUnion([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IIDLType discriminatorType, UnionMember[] members)
        {
            throw new NotImplementedException();
        }

        public IValueDef CreateValue([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, bool isCustom, bool isAbstract, IValueDef baseValue, bool isTruncatable, IValueDef[] abstractBaseValues, IInterfaceDef[] supportedInterfaces, Initializer[] initializers)
        {
            throw new NotImplementedException();
        }

        public IValueBoxDef CreateValueBox([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IIDLType originalTypeDef)
        {
            throw new NotImplementedException();
        }

        public CORBA.Container.Description[] DescribeContents(DefinitionKind limitType, bool excludeInherited, int maxReturnedObjs)
        {
            throw new NotImplementedException();
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }

        public IContained Lookup([WideChar(false)] string searchName)
        {
            throw new NotImplementedException();
        }

        public IContained[] LookupName([WideChar(false)] string searchName, int levelsToSearch, DefinitionKind limitType, bool excludeInherited)
        {
            throw new NotImplementedException();
        }

        protected override void Define()
        {
            throw new NotImplementedException();
        }
    }
}
