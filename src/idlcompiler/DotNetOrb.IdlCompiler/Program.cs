// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DotNetOrb.IdlCompiler.Symbols;
using System;
using System.Collections.Generic;
using System.IO;

[assembly: CLSCompliant(false)]
namespace DotNetOrb.IdlCompiler
{
    class Program
    {
        private static Dictionary<string, string> predefinedSymbols = new Dictionary<string, string>();
        private static List<DirectoryInfo> includeDirs = new List<DirectoryInfo>();
        private static List<FileInfo> inputFiles = new List<FileInfo>();
        private static DirectoryInfo outputDir = new DirectoryInfo(".");
        private static bool isHelpRequested = false;
        private static string error;
        private static SequenceType sequenceType = SequenceType.Array;
        private static bool ami = false;
        private static bool dotNetNaming = true;

        public static void PrintHelp()
        {
            Console.WriteLine("IdlCompiler usage:");
            Console.WriteLine("  DotNetOrb.IdlCompiler [options] [files]");
            Console.WriteLine();
            Console.WriteLine("Creates C# source code for the OMG IDL definition files.");
            Console.WriteLine("files: idl files containg OMG IDL definitions.");
            Console.WriteLine();
            Console.WriteLine("options are:");
            Console.WriteLine("-h or -help                              help");
            Console.WriteLine("-i directory                             additional directories for idl file includes (multiple -i allowed)");
            Console.WriteLine("-o directory                             output directory. Default is .\\");
            Console.WriteLine("-d define                                defines a preprocessor symbol");
            Console.WriteLine("-sequence_type [array | list]            type of sequence fields: array (default) or List");
            Console.WriteLine("-ami                                     generate async methods");
            Console.WriteLine("-naming_scheme [dotnet | idl]            respect original idl names or convert to dotnet naming (default)");
        }

        static void Main(string[] args)
        {
            ParseArgs(args);
            if (isHelpRequested)
            {
                PrintHelp();
                Environment.Exit(0);
            }
            if (!String.IsNullOrEmpty(error))
            {
                Console.WriteLine(error);
                Console.WriteLine();
                PrintHelp();
                Environment.Exit(1);
            }
            if (!Directory.Exists(outputDir.FullName))
            {
                Directory.CreateDirectory(outputDir.FullName);
            }

            var importNamespaces = new Dictionary<string, HashSet<string>>();

            //Extract Namespaces from idl files in current dir and input dirs
            var symbolTable = new Dictionary<string, string>();
            symbolTable.Add("_DOTNET_ORB_", "");
            string currentDir = Directory.GetCurrentDirectory();
            string[] filenames = Directory.GetFiles(currentDir, "*.idl");
            foreach (var fileName in filenames)
            {
                var file = new FileInfo(fileName);
                ExtractNamespacesFromFile(file, symbolTable, importNamespaces);
            }

            foreach (var iDir in includeDirs)
            {
                var files = iDir.GetFiles("*.idl");
                foreach (var file in files)
                {
                    ExtractNamespacesFromFile(file, symbolTable, importNamespaces);
                }
            }

            foreach (FileInfo file in inputFiles)
            {
                ExtractNamespacesFromFile(file, symbolTable, importNamespaces);
            }

            symbolTable = new Dictionary<string, string>();
            symbolTable.Add("_DOTNET_ORB_", "");
            var globalScope = new Scope();
            //Process input files
            foreach (FileInfo file in inputFiles)
            {
                ProcessFile(file, globalScope, symbolTable, importNamespaces);
            }
            globalScope.UpdateTypeInfo();
            var codeGenerator = new CodeGenerator();
            codeGenerator.Run(globalScope, outputDir);
        }

        static void ExtractNamespacesFromFile(FileInfo file, Dictionary<string, string> symbolTable, Dictionary<string, HashSet<string>> namespaces)
        {
            try
            {
                Console.WriteLine("Preprocessing file " + file.Name + " ...");
                var errorListener = new ErrorListener();
                errorListener.CurrentFile = file.Name;
                var preLexer = new IDLPreprocessorLexer(new AntlrFileStream(file.FullName));
                var preTokens = new CommonTokenStream(preLexer);
                var preParser = new IDLPreprocessorParser(preTokens);                
                preParser.RemoveErrorListeners();
                preParser.AddErrorListener(errorListener);
                preParser.AddParseListener(new ParseTreeListener(errorListener));
                IParseTree preTree = preParser.idlDocument();
                if (preParser.NumberOfSyntaxErrors == 0)
                {
                    var preVisitor = new IDLPreprocessorVisitorImpl(file, includeDirs, symbolTable);
                    var result = preVisitor.Visit(preTree);
                    Console.WriteLine("Extracting namespaces from file " + file.Name + ". Symbol count: " + symbolTable.Count + "...");
                    var input = new AntlrInputStream(result);
                    var lexer = new IDLLexer(input);
                    var tokens = new CommonTokenStream(lexer);
                    var parser = new IDLParser(tokens);
                    parser.RemoveErrorListeners();
                    parser.AddErrorListener(errorListener);
                    parser.AddParseListener(new ParseTreeListener(errorListener));
                    IParseTree tree = parser.specification(); // begin parsing at init rule
                    if (parser.NumberOfSyntaxErrors == 0)
                    {                        
                        ParseTreeWalker walker = new ParseTreeWalker();
                        var namespaceListener = new IDLNamespaceListenerImpl();
                        walker.Walk(namespaceListener, tree);
                        foreach (var ns in namespaceListener.Namespaces)
                        {                            
                            if (!namespaces.ContainsKey(ns))
                            {
                                namespaces.Add(ns, new HashSet<string>());
                            }
                            namespaces[ns].Add(file.FullName);
                        }
                    }
                    else
                    {
                        Console.WriteLine(parser.NumberOfSyntaxErrors + " syntax errors");
                    }
                }
                else
                {
                    Console.WriteLine(preParser.NumberOfSyntaxErrors + " syntax errors");
                }
            }
            catch (IdlCompilerException ex)
            {
                if (!String.IsNullOrEmpty(ex.FileName))
                {
                    Console.WriteLine(ex.FileName + " line " + ex.LineNumber + ":" + ex.ColumnNumber + " " + ex.Message);
                }
                else
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing file: " + file.Name + ": " + ex.Message);
            }
        }


        static void ProcessFile(FileInfo file, Scope scope, Dictionary<string, string> symbolTable, Dictionary<string, HashSet<string>> importNamespaces)
        {
            try
            {
                Console.WriteLine("Preprocessing file " + file.Name + " ...");
                var errorListener = new ErrorListener();
                errorListener.CurrentFile = file.Name;
                var preLexer = new IDLPreprocessorLexer(new AntlrFileStream(file.FullName));
                var preTokens = new CommonTokenStream(preLexer);
                var preParser = new IDLPreprocessorParser(preTokens);
                preParser.RemoveErrorListeners();
                preParser.AddErrorListener(errorListener);
                preParser.AddParseListener(new ParseTreeListener(errorListener));
                IParseTree preTree = preParser.idlDocument();
                if (preParser.NumberOfSyntaxErrors == 0)
                {
                    var preVisitor = new IDLPreprocessorVisitorImpl(file, includeDirs, symbolTable);
                    var result = preVisitor.Visit(preTree);
                    Console.WriteLine("Processing file " + file.Name + " ...");
                    var stream = new AntlrInputStream(result);
                    var lexer = new IDLLexer(stream);
                    var tokens = new CommonTokenStream(lexer);
                    var parser = new IDLParser(tokens);
                    parser.RemoveErrorListeners();
                    parser.AddErrorListener(errorListener);
                    parser.AddParseListener(new ParseTreeListener(errorListener));
                    IParseTree tree = parser.specification();
                    if (parser.NumberOfSyntaxErrors == 0)
                    {
                        var visitor = new IDLVisitorImpl(file, scope, includeDirs, importNamespaces, symbolTable, sequenceType, ami, dotNetNaming);
                        visitor.Visit(tree);
                    }
                    else
                    {
                        Console.WriteLine(parser.NumberOfSyntaxErrors + " syntax errors");
                    }
                }
                else
                {
                    Console.WriteLine(preParser.NumberOfSyntaxErrors + " syntax errors");
                }
            }
            catch (IdlCompilerException ex)
            {
                if (!String.IsNullOrEmpty(ex.FileName))
                {
                    Console.WriteLine(ex.FileName + " line " + ex.LineNumber + ":" + ex.ColumnNumber + " " + ex.Message);
                    Environment.Exit(1);
                }
                else
                {
                    Console.WriteLine(ex.Message);
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing file: " + file.Name + ": " + ex.Message);
                Environment.Exit(1);
            }
        }
        static void ParseArgs(string[] args)
        {
            int i = 0;
            while ((i < args.Length) && (args[i].StartsWith("-")))
            {
                if (args[i].Equals("-h") || args[i].Equals("-help"))
                {
                    isHelpRequested = true;
                    return;
                }
                else if (args[i].Equals("-o"))
                {
                    i++;
                    outputDir = new DirectoryInfo(args[i++]);
                }
                else if (args[i].Equals("-i"))
                {
                    i++;
                    includeDirs.Add(new DirectoryInfo(args[i++]));
                }
                else if (args[i].Equals("-d"))
                {
                    i++;
                    predefinedSymbols.Add(args[i++].Trim(), "");
                }
                else if (args[i].Equals("-sequence_type"))
                {
                    i++;
                    var type = args[i++].Trim();
                    switch (type.ToLower())
                    {
                        case "array":
                            break;
                        case "list":
                            sequenceType = SequenceType.List; break;
                        default:
                            error = String.Format("Error: invalid option {0}", type);
                            break;
                    }                    
                }
                else if (args[i].Equals("-naming_scheme"))
                {
                    i++;
                    var type = args[i++].Trim();
                    switch (type.ToLower())
                    {
                        case "idl":
                            dotNetNaming = false;
                            break;
                        case "dotnet":
                            dotNetNaming = true;
                            break;
                        default:
                            error = String.Format("Error: invalid option {0}", type);
                            break;
                    }
                }
                else if (args[i].Equals("-ami"))
                {
                    i++;
                    ami = true;
                }
                else
                {
                    error = String.Format("Error: invalid option {0}", args[i]);
                    return;
                }
            }            

            for (int j = i; j < args.Length; j++)
            {                
                if (args[j].Contains("*"))
                {
                    // file pattern
                    string dir = Path.GetDirectoryName(args[j]);
                    string pattern = Path.GetFileName(args[j]);

                    if (string.IsNullOrEmpty(dir))
                    {
                        dir = Directory.GetCurrentDirectory();
                    }

                    string[] files = Directory.GetFiles(dir, pattern);
                    foreach (var file in files)
                    {
                        inputFiles.Add(new FileInfo(file));
                    }
                }
                else if (File.Exists(args[j]))
                {
                    inputFiles.Add(new FileInfo(args[j]));
                }
            }            

            if (inputFiles.Count == 0)
            {
                error = "No input files found";
            }
        }
    }
}
