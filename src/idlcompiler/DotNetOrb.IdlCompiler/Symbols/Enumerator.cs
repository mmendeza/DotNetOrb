// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class Enumerator: IDLSymbol
    {        
        public Enumeration Enumeration { get; set; }
        public bool HasValue { get; set; }
        public long Value { get; set; }

        public string MappedType
        {
            get
            {
                return Enumeration.MappedType + "." + MappedName;
            }
        }

        public Enumerator(Enumeration enumeration, string name, List<Annotation> annotations = null) : base(name, annotations)        
        {
            Enumeration = enumeration;            
        }
    }
}
