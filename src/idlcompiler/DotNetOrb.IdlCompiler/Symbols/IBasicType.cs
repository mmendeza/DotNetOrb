// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public interface IBasicType : ITypeSymbol
    {        
        public abstract void PrintReadArray(string indent, TextWriter sw, string streamName, string varName, string length);

        public abstract void PrintWriteArray(string indent, TextWriter sw, string streamName, string varName, string length);
        
    }
}
