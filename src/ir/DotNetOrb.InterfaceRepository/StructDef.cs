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
    public class StructDef : ITypedefDef, IStructDefOperations, IContainerType
    {
        private CORBA.TypeCode type;
        private Type myClass;
        private Type helperClass;
        private CORBA.StructMember[] members;

        // local references to contained objects
        private Dictionary<string, IContained> containedLocals = new Dictionary<string, IContained>();
        // CORBA references to contained objects 
        private Dictionary<string, IContained> contained = new Dictionary<string, IContained> ();

        private DirectoryInfo myDir;
        private string path;

        private bool defined = false;

        private Core.ILogger logger;
        private IPOA poa;

        public DefinitionKind DefKind
        {
            get
            {
                return DefinitionKind.DkStruct;
            }
        }

        private IRepository containingRepository;
        public IRepository ContainingRepository
        {
            get
            {
                return containingRepository;
            }
        }

        private IContainer definedIn;
        public CORBA.IContainer DefinedIn
        {
            get
            {
                return definedIn;
            }
        }

        public string Version
        {
            get;

            set;
        }
        public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string AbsoluteName => throw new NotImplementedException();

        public CORBA.TypeCode Type => throw new NotImplementedException();

        public StructMember[] Members { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public StructDef(Type type, string path, CORBA.IContainer definedIn, CORBA.IRepository ir, Core.ILogger logger, IPOA poa)
        {
            this.logger = logger;
            this.poa = poa;            
            this.containingRepository = ir;
            this.definedIn = definedIn;
            this.path = path;
            if (definedIn == null)
            {
                throw new IntfRepos("definedIn = null");
            }
            if (containingRepository == null)
            {
                throw new IntfRepos("containingRepository = null");
            }

            try
            {
                string classId = type.Name;
                myClass = type;
                Version = "1.0";
                full_name = classId;

                if (classId.indexOf('.') > 0)
                {
                    name(classId.substring(classId.lastIndexOf('.') + 1));
                    absolute_name =
                        org.omg.CORBA.ContainedHelper.narrow(defined_in).absolute_name() +
                        "::" + name;
                }
                else
                {
                    name(classId);
                    absolute_name = "::" + name;
                }

                helperClass = this.loader.loadClass(classId + "Helper");
                id((String)helperClass.getDeclaredMethod("id", (Class[])null).invoke(null, (Object[])null));

                type = (org.omg.CORBA.TypeCode)helperClass.getDeclaredMethod(
                                                       "type",
                                                       (Class[])null).invoke(null, (Object[])null);

                members = new org.omg.CORBA.StructMember[type.member_count()];
                for (int i = 0; i < members.length; i++)
                {
                    org.omg.CORBA.TypeCode type_code = type.member_type(i);
                    String member_name = type.member_name(i);

                    if (this.logger.isDebugEnabled())
                    {
                        this.logger.debug("StructDef " + absolute_name +
                                          " member " + member_name);
                    }

                    members[i] = new org.omg.CORBA.StructMember(member_name,
                                                                 type_code,
                                                                 null);
                }
                /* get directory for nested definitions' classes */
                File f = new File(path + fileSeparator +
                                   classId.replace('.', fileSeparator) + "Package");

                if (f.exists() && f.isDirectory())
                    my_dir = f;

                if (this.logger.isDebugEnabled())
                {
                    this.logger.debug("StructDef: " + absolute_name);
                }
            }
            catch (Exception e)
            {
                logger.error("Caught Exception", e);
                throw new INTF_REPOS(ErrorMsg.IR_Not_Implemented,
                                      org.omg.CORBA.CompletionStatus.COMPLETED_NO);
            }
        }

        public void LoadContents()
        {
            // read from the  class (operations and atributes)
            if (getReference() == null)
            {
                throw new INTF_REPOS("getReference returns null");
            }

            org.omg.CORBA.StructDef myReference =
                org.omg.CORBA.StructDefHelper.narrow(getReference());

            if (myReference == null)
            {
                throw new INTF_REPOS("narrow failed for " + getReference());
            }

            /* load nested definitions from interfacePackage directory */

            String[] classes = null;
            if (my_dir != null)
            {
                classes = my_dir.list(new IRFilenameFilter(".class"));

                // load class files in this interface's Package directory
                if (classes != null)
                {
                    for (int j = 0; j < classes.length; j++)
                    {
                        try
                        {
                            if (this.logger.isDebugEnabled())
                            {
                                this.logger.debug("Struct " + name + " tries " +
                                                  full_name +
                                                  "Package." +
                                                  classes[j].substring(0, classes[j].indexOf(".class")));
                            }

                            ClassLoader loader = getClass().getClassLoader();
                            if (loader == null)
                            {
                                loader = this.loader;
                            }

                            Class cl =
                                loader.loadClass(
                                                 (full_name +
                                                   "Package." +
                                                   classes[j].substring(0, classes[j].indexOf(".class"))
                                                   ));


                            Contained containedObject =
                                Contained.createContained(cl,
                                                           path,
                                                           myReference,
                                                           containing_repository,
                                                           this.logger,
                                                           this.loader,
                                                           this.poa);
                            if (containedObject == null)
                                continue;

                            org.omg.CORBA.Contained containedRef =
                                Contained.createContainedReference(containedObject,
                                                                   this.logger,
                                                                   this.poa);

                            if (containedObject instanceof ContainerType )
                            ((ContainerType)containedObject).loadContents();

                    containedRef.move(myReference, containedRef.name(), containedRef.version());

                    if (this.logger.isDebugEnabled())
                    {
                        this.logger.debug("Struct " + full_name +
                                          " loads " + containedRef.name());
                    }

                    contained.put(containedRef.name(), containedRef);
                    containedLocals.put(containedRef.name(), containedObject);
                }
                    catch (Exception e )
                    {
                    logger.error("Caught Exception", e);
                }
            }
        }

        public Contained.Description Describe()
        {
            throw new NotImplementedException();
        }

        public void Move(IContainer newContainer, [WideChar(false)] string newName, [WideChar(false)] string newVersion)
        {
            throw new NotImplementedException();
        }

        public IRequest _CreateRequest(IContext ctx, string operation, NVList argList, NamedValue result)
        {
            throw new NotImplementedException();
        }

        public IRequest _CreateRequest(IContext ctx, string operation, NVList argList, NamedValue result, ExceptionList excList, ContextList ctxList)
        {
            throw new NotImplementedException();
        }

        public IObject _Duplicate()
        {
            throw new NotImplementedException();
        }

        public IPolicy _GetClientPolicy(uint type)
        {
            throw new NotImplementedException();
        }

        public IObject _GetComponent()
        {
            throw new NotImplementedException();
        }

        public IDomainManager[] _GetDomainManagers()
        {
            throw new NotImplementedException();
        }

        public IInterfaceDef _GetInterface()
        {
            throw new NotImplementedException();
        }

        public IObject _GetInterfaceDef()
        {
            throw new NotImplementedException();
        }

        public ORB _GetOrb()
        {
            throw new NotImplementedException();
        }

        public IPolicy _GetPolicy(uint policyType)
        {
            throw new NotImplementedException();
        }

        public IPolicy[] _GetPolicyOverrides(uint[] types)
        {
            throw new NotImplementedException();
        }

        public int _Hash(int maximum)
        {
            throw new NotImplementedException();
        }

        public string[] _Ids()
        {
            throw new NotImplementedException();
        }

        public IInputStream _Invoke(IOutputStream output)
        {
            throw new NotImplementedException();
        }

        public Task<IInputStream> _InvokeAsync(IOutputStream output)
        {
            throw new NotImplementedException();
        }

        public bool _IsA(string repositoryId)
        {
            throw new NotImplementedException();
        }

        public bool _IsEquivalent(IObject other)
        {
            throw new NotImplementedException();
        }

        public bool _IsLocal()
        {
            throw new NotImplementedException();
        }

        public bool _IsNil()
        {
            throw new NotImplementedException();
        }

        public bool _NonExistent()
        {
            throw new NotImplementedException();
        }

        public void _Release()
        {
            throw new NotImplementedException();
        }

        public void _ReleaseReply(IInputStream input)
        {
            throw new NotImplementedException();
        }

        public string _RepositoryId()
        {
            throw new NotImplementedException();
        }

        public IRequest _Request(string operation)
        {
            throw new NotImplementedException();
        }

        public IOutputStream _Request(string operation, bool responseExpected)
        {
            throw new NotImplementedException();
        }

        public IObject _SetPolicyOverrides(List<IPolicy> policies, SetOverrideType setAdd)
        {
            throw new NotImplementedException();
        }

        public bool _ValidateConnection(out List<IPolicy> inconsistentPolicies)
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            throw new NotImplementedException();
        }

        public IContained Lookup([WideChar(false)] string searchName)
        {
            throw new NotImplementedException();
        }

        public IContained[] Contents(DefinitionKind limitType, bool excludeInherited)
        {
            throw new NotImplementedException();
        }

        public IContained[] LookupName([WideChar(false)] string searchName, int levelsToSearch, DefinitionKind limitType, bool excludeInherited)
        {
            throw new NotImplementedException();
        }

        public Container.Description[] DescribeContents(DefinitionKind limitType, bool excludeInherited, int maxReturnedObjs)
        {
            throw new NotImplementedException();
        }

        public IModuleDef CreateModule([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version)
        {
            throw new NotImplementedException();
        }

        public IConstantDef CreateConstant([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IIDLType type, Any value)
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

        public IEnumDef CreateEnum([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, string[] members)
        {
            throw new NotImplementedException();
        }

        public IAliasDef CreateAlias([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IIDLType originalType)
        {
            throw new NotImplementedException();
        }

        public IInterfaceDef CreateInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IInterfaceDef[] baseInterfaces, bool isAbstract)
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

        public IExceptionDef CreateException([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, StructMember[] members)
        {
            throw new NotImplementedException();
        }

        public INativeDef CreateNative([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version)
        {
            throw new NotImplementedException();
        }

        public IAbstractInterfaceDef CreateAbstractInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IAbstractInterfaceDef[] baseInterfaces)
        {
            throw new NotImplementedException();
        }

        public ILocalInterfaceDef CreateLocalInterface([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, IInterfaceDef[] baseInterfaces)
        {
            throw new NotImplementedException();
        }

        public IExtValueDef CreateExtValue([WideChar(false)] string id, [WideChar(false)] string name, [WideChar(false)] string version, bool isCustom, bool isAbstract, IValueDef baseValue, bool isTruncatable, IValueDef[] abstractBaseValues, IInterfaceDef[] supportedInterfaces, ExtInitializer[] initializers)
        {
            throw new NotImplementedException();
        }
    }
}

    }
}
