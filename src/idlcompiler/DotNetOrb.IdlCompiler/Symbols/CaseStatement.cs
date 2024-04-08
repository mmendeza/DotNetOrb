// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class CaseStatement: IDLSymbol
    {
        public bool IsDefault { get; set; }
        public ITypeSymbol DataType { get; set; }
        public List<object> CaseLabels { get; set; }

        public CaseStatement(string name, Scope parentScope, bool dotNetNaming, List<Annotation> annotations = null) : base(name, dotNetNaming, annotations)
        {
            CaseLabels = new List<object>();
        }

    }
}
