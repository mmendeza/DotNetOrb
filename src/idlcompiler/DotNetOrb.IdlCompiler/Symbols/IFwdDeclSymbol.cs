// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.IdlCompiler.Symbols
{
    public interface IFwdDeclSymbol: IScopeSymbol
    {        
        public bool IsForwardDeclaration { get; }
        public void Define(IFwdDeclSymbol definition);
    }
}
