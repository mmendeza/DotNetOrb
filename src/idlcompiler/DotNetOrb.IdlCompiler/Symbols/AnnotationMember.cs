// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class AnnotationMember: IDLSymbol
    {        
        public ITypeSymbol DataType { get; set; }
        public Literal DefaultValue { get; set; }

        public AnnotationMember(string name, List<Annotation> annotations = null) : base(name, annotations)
        {
        }

    }
}
