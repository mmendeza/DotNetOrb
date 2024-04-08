// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;

namespace DotNetOrb.IdlCompiler
{
    internal class ParseTreeListener : IParseTreeListener
    {
        private ErrorListener errorListener;

        public ParseTreeListener(ErrorListener errorListener)
        {
            this.errorListener = errorListener;
        }

        public void EnterEveryRule(ParserRuleContext ctx)
        {

        }

        public void ExitEveryRule(ParserRuleContext ctx)
        {
            switch (ctx.RuleIndex)
            {
                case IDLParser.RULE_line:
                    {
                        var currentLine = ctx.Start.Line;
                        var context = (IDLParser.LineContext)ctx;
                        var temp = context.INTEGER_LITERAL();
                        var line = Convert.ToInt32(temp.Symbol.Text);
                        var lineDelta = currentLine - line;
                        var file = context.STRING_LITERAL().Symbol.Text;
                        errorListener.LineDelta = lineDelta;
                        errorListener.CurrentFile = file;
                    }
                    break;
            }
        }

        public void VisitErrorNode(IErrorNode node)
        {
            
        }

        public void VisitTerminal(ITerminalNode node)
        {
            
        }
    }
}
