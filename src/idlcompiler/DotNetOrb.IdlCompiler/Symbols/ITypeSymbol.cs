// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public interface ITypeSymbol: IIDLSymbol
    {    

        int TCKind { get; }

        string IDLType { get; }

        string MappedType { get; }

        string HelperName { get; }

        string FullHelperName { get; }

        string TypeCodeExp { get; }

        void PrintRead(string indent, TextWriter sw, String streamName, String varName, int iteration);
        void PrintWrite(string indent, TextWriter sw, String streamName, String varName, int iteration);
        void PrintInsert(string indent, TextWriter sw, String anyName, String varName);
        void PrintExtract(string indent, TextWriter sw, String anyName, String varName, String type = null);

    }
}
