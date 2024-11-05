// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class BitValue: IDLSymbol
    {
        public byte Position { get; set; }

        public BitValue(string name, List<Annotation> annotations = null) : base(name, annotations)
        {
        }
    }
}
