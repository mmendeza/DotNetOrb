// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Annotation: IDLSymbol
    {
        public string MappedType
        {
            get
            {
                if (String.IsNullOrEmpty(Namespace))
                {
                    return MappedName;
                }
                return Namespace + "." + MappedName;
            }
        }

        public Literal ConstantExpr { get; set; }
        private IDictionary<string, Literal> parameters = new Dictionary<string, Literal>();
        public IDictionary<string, Literal> Parameters { get => parameters; }

        public Annotation(string name, bool dotNetNaming): base(name, dotNetNaming) { }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (ConstantExpr != null)
            {
                sb.AppendLine($"[{MappedType}({ConstantExpr.ToString()})]");
            }
            else if (parameters.Count > 0)
            {
                sb.Append($"[{MappedType}(");
                int index = 0;
                foreach (var kvp in parameters)
                {
                    if (index > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append($"{GetAttributeName(kvp.Key)} = {kvp.Value.ToString()}");
                }
                sb.AppendLine($")]");
            }
            else
            {
                sb.AppendLine($"[{MappedType}]");
            }
            return sb.ToString();
        }

        private string GetAttributeName(string name)
        {
            if (dotNetNaming)
            {
                name = Utils.ToPascalCase(Name);
            }
            if (Utils.CSharpKeywords.Contains(name))
            {
                return "@" + name;
            }
            return name;
        }
    }
}
