// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Module: IDLSymbol, IScopeSymbol, IFwdDeclSymbol
    {
        public Scope NamingScope { get; set; }
        public bool IsForwardDeclaration { get => true; }

        public Module(string name,  List<Annotation> annotations = null) : base(name, annotations)
        {
        }

        public void Define(IFwdDeclSymbol definition)
        {            
        }

        public override string GetMappedName(string prefix = "", string suffix = "")
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
            if (Utils.CSharpKeywords.Contains(name))
            {
                return "@" + name;
            }
            return name;
        }

        public override void Include(DirectoryInfo currentDir, string indent = "", StreamWriter stream = null)
        {
            if (!IsIncluded)
            {
                IsIncluded = true;
                var dir = new DirectoryInfo($"{currentDir.FullName}\\{MappedName}");
                if (!dir.Exists)
                {
                    dir.Create();
                }
                foreach (IDLSymbol symbol in NamingScope.Symbols.Values)
                {
                    symbol.Include(dir);
                }
            }
        }
    }
}
