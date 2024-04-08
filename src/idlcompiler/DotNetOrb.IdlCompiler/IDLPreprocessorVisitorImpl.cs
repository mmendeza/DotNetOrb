// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DotNetOrb.IdlCompiler
{
    internal class IDLPreprocessorVisitorImpl: IDLPreprocessorParserBaseVisitor<String>
    {

        private List<DirectoryInfo> includeDirs { get; set; }
        private Dictionary<string, string> symbolTable;
        private FileInfo file;
        private String prefix = @"""""";

        private Stack<bool> conditions = new Stack<bool>();
        private bool Compile
        {
            get
            {
                return !conditions.Contains(false);
            }
        }        
        
        public IDLPreprocessorVisitorImpl(FileInfo file, List<DirectoryInfo> includeDirs, Dictionary<string, string> symbolTable)
        {
            this.includeDirs = includeDirs;
            this.file = file;            
            this.symbolTable = symbolTable;
            conditions.Push(true);
        }

        public override string VisitIdlDocument([NotNull] IDLPreprocessorParser.IdlDocumentContext context)
        {
            var result = new StringBuilder();
            //result.AppendLine(@"#pragma prefix """"");            
            //result.AppendLine("#line 0 \"" + file.Name + "\"");
            foreach (var text in context.text())
            {
                var tmp = Visit(text);
                result.Append(tmp);
            }
            return result.ToString();
        }

        public override string VisitText([NotNull] IDLPreprocessorParser.TextContext context)
        {
            //String result = tokensStream.GetText(context);
            var result = context.GetSourceText();
            bool directive = false;
            if (context.directive() != null)
            {
                Visit(context.directive());
                directive = true;
            }
            if (Compile && context.pragma() != null)
            {
                Visit(context.pragma());
            }
            if (!Compile || directive)
            {
                StringBuilder sb = new StringBuilder(result.Length);
                char[] charArray = result.ToCharArray();
                foreach (char c in charArray)
                {
                    sb.Append((c == '\r' || c == '\n') ? c : ' ');
                }
                result = sb.ToString();
            }
            else if (context.include() == null)
            {                
                foreach (var s in symbolTable.Keys)
                {
                    if (result.Contains(s))
                    {                        
                        result = result.Replace(s, symbolTable[s]);
                    }
                    
                }
            }
            return result;
        }     

        public override string VisitPreprocessorPragmaPrefix([NotNull] IDLPreprocessorParser.PreprocessorPragmaPrefixContext context)
        {
            prefix = context.directive_text().GetText();
            return Compile.ToString();
        }
        
        public override string VisitPreprocessorConditional([NotNull] IDLPreprocessorParser.PreprocessorConditionalContext context)
        {
            if (context.IF() != null)
            {
                bool exprResult = Visit(context.preprocessor_expression()) == Boolean.TrueString;
                conditions.Push(exprResult);
                return exprResult.ToString();
            }
            else if (context.ELIF() != null)
            {
                conditions.Pop();
                bool exprResult = Visit(context.preprocessor_expression()) == Boolean.TrueString;
                conditions.Push(exprResult);
                return exprResult.ToString();
            }
            else if (context.ELSE() != null)
            {
                bool val = conditions.Pop();
                conditions.Push(!val);
                return (!val).ToString();
            }
            else
            {
                conditions.Pop();
                if (conditions.Count > 0)
                {
                    return conditions.Peek().ToString();
                }
                else
                {
                    return Boolean.TrueString;
                }
            }
        }

        public override string VisitPreprocessorDef([NotNull] IDLPreprocessorParser.PreprocessorDefContext context)
        {            
            String conditionalSymbolText = context.CONDITIONAL_SYMBOL().GetText();
            if (context.IFDEF() != null || context.IFNDEF() != null)
            {
                bool condition = symbolTable.ContainsKey(conditionalSymbolText);
                if (context.IFNDEF() != null)
                {
                    condition = !condition;
                }
                conditions.Push(condition);
                return condition.ToString();
            }
            else
            {
                if (Compile)
                {
                    symbolTable.Remove(conditionalSymbolText);
                }
                return Compile.ToString();
            }
        }

        public override string VisitPreprocessorDefine([NotNull] IDLPreprocessorParser.PreprocessorDefineContext context)
        {
            if (Compile)
            {
                StringBuilder str = new StringBuilder();
                if (context.directive_text()  != null)
                {
                    foreach (var d in context.directive_text().TEXT())
                    {
                        str.Append(d.GetText() != null ? d.GetText() : "\r\n");
                    }
                }
                String directiveText = str.ToString().Trim();
                symbolTable[context.CONDITIONAL_SYMBOL().GetText().Replace(" ", "")] = directiveText;
            }
            return Compile.ToString();
        }

        public override string VisitPreprocessorConstant([NotNull] IDLPreprocessorParser.PreprocessorConstantContext context)
        {
            if (context.TRUE() != null)
            {
                return Boolean.TrueString;
            }
            else if (context.FALSE() != null)
            {
                return Boolean.FalseString;
            }
            else
            {
                return context.GetText();
            }
        }

        public override string VisitPreprocessorConditionalSymbol([NotNull] IDLPreprocessorParser.PreprocessorConditionalSymbolContext context)
        {
            var name = context.CONDITIONAL_SYMBOL().GetText();
            if (symbolTable.ContainsKey(name))
            {
                return symbolTable[name];
            }
            else
            {
                return Boolean.FalseString;
            }
        }

        public override string VisitPreprocessorParenthesis([NotNull] IDLPreprocessorParser.PreprocessorParenthesisContext context)
        {
            return Visit(context.preprocessor_expression());
        }

        public override string VisitPreprocessorNot([NotNull] IDLPreprocessorParser.PreprocessorNotContext context)
        {
            return Boolean.Parse(Visit(context.preprocessor_expression())).ToString();
        }

        public override string VisitPreprocessorBinary([NotNull] IDLPreprocessorParser.PreprocessorBinaryContext context)
        {
            String expr1Result = Visit(context.preprocessor_expression(0));
            String expr2Result = Visit(context.preprocessor_expression(1));
            String op = context.op.Text;
            bool result = false;
            switch (op)
            {
                case "&&":
                    result = expr1Result == Boolean.TrueString && expr2Result == Boolean.TrueString;
                    break;
                case "||":
                    result = expr1Result == Boolean.TrueString || expr2Result == Boolean.TrueString;
                    break;
                case "==":
                    result = expr1Result == expr2Result;
                    break;
                case "!=":
                    result = expr1Result != expr2Result;
                    break;
                case "<":
                case ">":
                case "<=":
                case ">=":
                    long x1, x2;
                    if (Int64.TryParse(expr1Result, out x1) && Int64.TryParse(expr2Result, out x2))
                    {
                        switch (op)
                        {
                            case "<":
                                result = x1 < x2;
                                break;
                            case ">":
                                result = x1 > x2;
                                break;
                            case "<=":
                                result = x1 <= x2;
                                break;
                            case ">=":
                                result = x1 >= x2;
                                break;
                        }
                    }
                    else
                    {
                        result = false;
                    }                    
                    break;
                default:
                    result = true;
                    break;
            }
            return result.ToString();
        }

        public override string VisitPreprocessorDefined([NotNull] IDLPreprocessorParser.PreprocessorDefinedContext context)
        {
            return symbolTable.ContainsKey(context.CONDITIONAL_SYMBOL().GetText()).ToString();
        }

        private FileInfo GetFile(String name)
        {
            FileInfo fi = null;

            foreach (DirectoryInfo di in includeDirs)
            {
                fi = new FileInfo(Path.Combine(di.FullName, name));
                if (fi.Exists)
                {
                    return fi;
                }
            }
            if (File.Exists(Path.Combine(file.Directory.FullName, name)))
            {
                // search also in the same directory, then the file containing the include directive
                return new FileInfo(Path.Combine(file.Directory.FullName, name));
            }
            return null;
        }
    }
}
