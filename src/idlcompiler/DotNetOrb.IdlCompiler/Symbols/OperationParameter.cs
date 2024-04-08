// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public enum ParameterDirection
    {
        IN,
        OUT,
        INOUT
    }

    public class OperationParameter: IDLSymbol
    {        
        public ITypeSymbol DataType { get; set; }
        public ParameterDirection Direction { get; set; }

        public OperationParameter(string name, bool dotNetNaming, List<Annotation> annotations = null) : base(name, dotNetNaming, annotations)
        {
        }

        public override string GetMappedName(string prefix = "", string suffix = "")
        {
            var name = Name;
            if (dotNetNaming)
            {
                name = Utils.ToCamelCase(Name);
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
    }

}
