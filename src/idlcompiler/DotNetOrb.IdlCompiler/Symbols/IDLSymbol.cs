// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public abstract class IDLSymbol: IIDLSymbol
    {
        private string typePrefix;
        public string TypePrefix 
        {
            get
            {
                return typePrefix;
            }
            set
            {
                if (string.IsNullOrEmpty(typePrefix))
                {
                    typePrefix = value;
                }
                else if (value != typePrefix)
                {
                    throw new IdlCompilerException("Trying to set a different prefix to idl symbol " + Name);
                }
            }
        }
        private string typeId;
        public string TypeId
        {
            get
            {
                return typeId;
            }
            set
            {
                if (string.IsNullOrEmpty(typeId))
                {
                    typeId = value;
                }
                else if (value != typeId)
                {
                    throw new IdlCompilerException("Trying to set a different repository id to idl symbol " + Name);
                }
            }
        }
        private string typeVersion;
        public string TypeVersion
        {
            get
            {
                return typeVersion;
            }
            set
            {
                if (string.IsNullOrEmpty(typeVersion))
                {
                    typeVersion = value;
                }
                else if (value != typeVersion)
                {
                    throw new IdlCompilerException("Trying to set a different version to idl symbol " + Name);
                }
            }
        }

        protected readonly string name;
        public string Name { get => name; }
        public Scope ParentScope { get; set; }

        protected List<Annotation> annotations;
        public List<Annotation> Annotations 
        { 
            get
            {
                return annotations;
            }
            set
            {
                annotations = value;
            }
        }

        public bool IsIncluded { get; set; }

        public IDLSymbol(string name, List<Annotation> annotations = null)
        {
            this.name = name;           
            this.annotations = annotations ?? new List<Annotation>();
        }

        public Annotation GetAnnotation(string name)
        {
            if (Annotations != null)
            {
                var ann = Annotations.FirstOrDefault(a => a.Name.Equals(name));
                return ann;
            }
            return null;            
        }

        public virtual List<string> MappedAttributes
        {
            get
            {
                var ret = new List<string>();
                foreach (var a in annotations)
                {
                    ret.Add(a.ToString());
                }
                return ret;
            }
        }
        public string FullName
        {
            get
            {
                var ns = new List<string>();
                var parent = ParentScope;
                while (parent != null && parent.Symbol != null)
                {
                    //var prefix = parent.Symbol.TypePrefix != null ? parent.Symbol.TypePrefix : "";
                    //if (String.IsNullOrEmpty(prefix))
                    //{
                    //    ns.Add(parent.Symbol.Name);
                    //}
                    //else
                    //{
                    //    ns.Add(prefix + "." + parent.Symbol.Name);
                    //}
                    ns.Add(parent.Symbol.Name);
                    parent = parent.ParentScope;
                }
                ns.Reverse();
                if (ns.Count > 0)
                {
                    return String.Join("::", ns) + "::" + Name;
                }
                return Name;
            }
        }

        public virtual string GetMappedName(string prefix = "", string suffix = "")
        {
            var name = Name;            
            if (Compiler.DotNetNaming)
            {
                name = Utils.ToPascalCase(Name);
            }
            if (!String.IsNullOrEmpty(prefix) || !String.IsNullOrEmpty(suffix))
            {
                name = prefix + name + suffix;
                if (ParentScope.ContainsSymbol(name))
                {
                    name = "_" + name;
                }
            }
            if (ParentScope != null && ParentScope.Symbol != null && ParentScope.Symbol.GetMappedName().Equals(name))
            {
                name = "_" + name;
            }
            if (Utils.CSharpKeywords.Contains(name))
            {
                return "@" + name;
            }
            return name;
        }

        public string Namespace
        {
            get
            {
                var ns = new List<string>();
                var parent = ParentScope;
                while (parent != null && parent.Symbol != null)
                {
                    ns.Add(parent.Symbol.GetMappedName());
                    parent = parent.ParentScope;
                }
                ns.Reverse();
                return String.Join(".", ns);
            }
        }

        public virtual string MappedName
        {
            get
            {
                return GetMappedName();
            }
        }

        public virtual string RepositoryId
        {
            get
            {
                if (String.IsNullOrEmpty(TypeId))
                {
                    var id = "IDL:";
                    if (!String.IsNullOrEmpty(TypePrefix))
                    {
                        id += TypePrefix + "/";
                    }
                    var path = ParentScope.Namespace.Replace("::", "/");
                    if (String.IsNullOrEmpty(path))
                    {
                        id += Name;
                    }
                    else
                    {
                        id += path + "/" + Name;
                    }                                                            
                    if (String.IsNullOrEmpty(TypeVersion))
                    {
                        id += ":1.0";
                    }
                    else
                    {
                        id += ":" + TypeVersion;
                    }
                    return id;
                }
                return TypeId;
            }
        }

        public virtual void Include(DirectoryInfo currentDir, string indent = "", StreamWriter stream = null)
        {
            if (!IsIncluded)
            {
                IsIncluded = true;
            }
        }
        public void PrintComment(StreamWriter sw)
        {
            sw.WriteLine("/**");
            sw.WriteLine(" * Generated by DotNetORb.IdlCompiler V " + Assembly.GetEntryAssembly().GetName().Version);
            sw.WriteLine(" * Timestamp: " + DateTime.Now);
            sw.WriteLine(" *");
            sw.WriteLine(" */");
            sw.WriteLine("");

        }
        public void PrintUsings(StreamWriter sw)
        {
            sw.WriteLine("using System;");
            sw.WriteLine("using System.Collections.Generic;");
            sw.WriteLine("using System.Linq;");
            sw.WriteLine("using CORBA;");
            sw.WriteLine();
            //sw.WriteLine("using DotNetOrb.Core;");
        }

    }
}
