// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.IdlCompiler
{
    public class IdlCompilerException : Exception
    {
        public String FileName { get; set; }
        public int LineNumber { get; set; }
        public int ColumnNumber { get; set; }        

        public IdlCompilerException(String message, String fileName, int lineNumber, int columnNumber) : base(message)
        {
            FileName = fileName;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }

        public IdlCompilerException(String message) : base(message)
        {
        }
    }
}
