// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Antlr4.Runtime;
using System;
using System.IO;

namespace DotNetOrb.IdlCompiler
{
    internal class ErrorListener : BaseErrorListener
    {
        public int LineDelta {  get; set; }

        public string CurrentFile { get; set; }

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            //base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
            
            Console.WriteLine(CurrentFile + " line " + (line - LineDelta) + ":" + charPositionInLine + " " + msg);
        }
    }
}
