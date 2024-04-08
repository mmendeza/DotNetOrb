// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public interface Literal
    {
        Literal Or(Literal arg);
        Literal Xor(Literal arg);
        Literal And(Literal arg);
        Literal ShiftLeftBy(Literal arg);
        Literal ShiftRightBy(Literal arg);
        Literal MultBy(Literal arg);
        Literal DivBy(Literal arg);
        Literal ModBy(Literal arg);
        Literal Add(Literal arg);
        Literal Sub(Literal arg);
        void Negate();
        void InvertSign();

        object GetValue();
        
        Double GetFloatValue();
        Int64 GetIntValue();
        Char GetCharValue();
        String GetStringValue();
        Boolean GetBooleanValue();

        String ToString();
    }
}
