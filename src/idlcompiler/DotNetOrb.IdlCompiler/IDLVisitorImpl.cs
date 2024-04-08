// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DotNetOrb.IdlCompiler.Symbols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNetOrb.IdlCompiler
{
    internal class IDLVisitorImpl : IDLParserBaseVisitor<bool>
    {
        int LineDelta { get; set; }
        FileInfo CurrentFile { get; set; }        

        private Scope currentScope;

        private StringBuilder output = new StringBuilder();

        private bool dotNetNaming;
        private bool ami;

        private string prefix;

        private List<DirectoryInfo> includeDirs;

        private Dictionary<string, HashSet<string>> importNamespaces;

        private Dictionary<string, string> symbolTable;

        private SequenceType defaultSeqType;

        public IDLVisitorImpl(FileInfo file, Scope scope, List<DirectoryInfo> includeDirs, Dictionary<string, HashSet<string>> importNamespaces, Dictionary<string, string> symbolTable, SequenceType defaultSeqType, bool ami, bool dotNetNaming)
        {
            this.defaultSeqType = defaultSeqType;
            this.dotNetNaming = dotNetNaming;
            this.ami = ami;
            this.includeDirs = includeDirs;
            this.importNamespaces = importNamespaces;
            this.symbolTable = symbolTable;
            currentScope = scope;
            CurrentFile = file;
        }

        private void ThrowIdlCompilerException(String msg, ParserRuleContext context)
        {
            var line = context.Start.Line - LineDelta;
            var charPositionInLine = context.Start.Column;
            throw new IdlCompilerException(msg, CurrentFile.Name, line, charPositionInLine);
        }

        private void ThrowIdlCompilerException(Exception innerException, ParserRuleContext context)
        {
            if (innerException is IdlCompilerException ex && ex.LineNumber > 0)
            {
                throw ex;
            }
            else
            {
                var line = context.Start.Line - LineDelta;
                var charPositionInLine = context.Start.Column;
                throw new IdlCompilerException(innerException.Message, CurrentFile.Name, line, charPositionInLine);
            }
        }

        //public override bool VisitLine([NotNull] IDLParser.LineContext context)
        //{
        //    try
        //    {
        //        var currentLine = context.Start.Line;
        //        var temp = context.INTEGER_LITERAL();
        //        var line = Convert.ToInt32(temp.Symbol.Text);
        //        var lineDelta = currentLine - line;
        //        var file = context.STRING_LITERAL().Symbol.Text;
        //        LineDelta = lineDelta;
        //        CurrentFile = file;
        //    }
        //    catch (FormatException)
        //    {
        //        ThrowIdlCompilerException("Incorrect line number format", context);
        //    }
        //    return true;
        //}

        public override bool VisitPreprocessorInclude([NotNull] IDLParser.PreprocessorIncludeContext context)
        {
            var fileName = context.directive_text().GetSourceText().Trim();
            //var fileName = tokensStream.GetText(context.directive_text());
            FileInfo fileToInclude = null;
            char ch0 = fileName[0];
            char ch1 = fileName[fileName.Length - 1];
            fileName = fileName.Substring(1, fileName.Length - 2);

            if ((ch0 == '"') && (ch1 == '"'))
            {
                fileToInclude = new FileInfo(Path.Combine(CurrentFile.Directory.FullName, fileName)); // search in the directory, the file containing the include is in
            }
            else if ((ch0 != '<') || (ch1 != '>'))
            {
                throw new IdlCompilerException("include argument error", CurrentFile.Name, context.Start.Line, context.directive_text().Start.Column);
            }
            if ((fileToInclude == null) || (!fileToInclude.Exists))
            {
                fileToInclude = GetFile(fileName);
                if (fileToInclude == null)
                {
                    throw new IdlCompilerException("File not found: " + fileName, CurrentFile.Name, context.Start.Line, context.directive_text().Start.Column);
                }
            }
            Console.WriteLine("Including " + fileToInclude.FullName + "...");
            var errorListener = new ErrorListener();
            errorListener.CurrentFile = fileToInclude.Name;
            var preLexer = new IDLPreprocessorLexer(new AntlrFileStream(fileToInclude.FullName));
            var preTokens = new CommonTokenStream(preLexer);
            var preParser = new IDLPreprocessorParser(preTokens);
            preParser.RemoveErrorListeners();
            preParser.AddErrorListener(errorListener);
            preParser.AddParseListener(new ParseTreeListener(errorListener));
            IParseTree preTree = preParser.idlDocument();
            if (preParser.NumberOfSyntaxErrors == 0)
            {
                var preVisitor = new IDLPreprocessorVisitorImpl(fileToInclude, includeDirs, symbolTable);
                var result = preVisitor.Visit(preTree);
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
                    var visitor = new IDLVisitorImpl(fileToInclude, currentScope, includeDirs, importNamespaces, symbolTable, defaultSeqType, ami, dotNetNaming);
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
            return true;
        }

        public override bool VisitImport_decl([NotNull] IDLParser.Import_declContext context)
        {
            var ns = context.imported_scope().GetSourceText().Trim();
            if (ns.StartsWith("::"))
            {
                ns = ns.Substring(2);
            }
            if (importNamespaces.ContainsKey(ns))
            {
                Console.WriteLine("importing namespace " + ns + "...");
                foreach (var fileName in importNamespaces[ns])
                {
                    Console.WriteLine("importing " + fileName + "...");
                    var fileToInclude = new FileInfo(fileName);
                    var errorListener = new ErrorListener();
                    errorListener.CurrentFile = fileToInclude.Name;
                    var preLexer = new IDLPreprocessorLexer(new AntlrFileStream(fileToInclude.FullName));
                    var preTokens = new CommonTokenStream(preLexer);
                    var preParser = new IDLPreprocessorParser(preTokens);
                    preParser.RemoveErrorListeners();
                    preParser.AddErrorListener(errorListener);
                    preParser.AddParseListener(new ParseTreeListener(errorListener));
                    IParseTree preTree = preParser.idlDocument();
                    if (preParser.NumberOfSyntaxErrors == 0)
                    {
                        var preVisitor = new IDLPreprocessorVisitorImpl(fileToInclude, includeDirs, symbolTable);
                        var result = preVisitor.Visit(preTree);
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
                            var visitor = new IDLVisitorImpl(fileToInclude, currentScope, includeDirs, importNamespaces, symbolTable, defaultSeqType, ami, dotNetNaming);
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
            }
            else
            {
                throw new IdlCompilerException("import " + ns + " not found", CurrentFile.Name, context.Start.Line, context.Start.Column);
            }
            return true;
        }

        public override bool VisitSpecification([NotNull] IDLParser.SpecificationContext context)
        {
            return base.VisitSpecification(context);
        }

        public override bool VisitModule([NotNull] IDLParser.ModuleContext context)
        {
            try
            {                
                var annotations = new List<Annotation>();
                if (context.Parent.GetChild(0) is IDLParser.AnnappsContext annaps)
                {
                    annotations = GetAnnotations(annaps, currentScope);
                }
                var name = context.ID().GetText();
                if (currentScope.ContainsChildScope(name))
                {
                    currentScope = currentScope.GetChildScope(name);
                }
                else
                {
                    var module = new Module(name, dotNetNaming, annotations);
                    currentScope.AddSymbolDefinition(module);
                    SetTypeInfo(module);
                    currentScope = module.NamingScope;
                }
                VisitChildren(context);
                currentScope = currentScope.ParentScope;
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            return true;
        }

        public override bool VisitConst_decl([NotNull] IDLParser.Const_declContext context)
        {
            try
            {
                GetConstant(context, currentScope);
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            return true;
        }

        public override bool VisitType_decl([NotNull] IDLParser.Type_declContext context)
        {
            try
            {
                var typeDefCtx = context.type_def();
                if (typeDefCtx != null)
                {
                    GetTypeDefinition(typeDefCtx, currentScope);
                }
                var structType = context.struct_type();
                if (structType != null)
                {
                    GetStructTypeSymbol(structType, currentScope);
                }
                var unionType = context.union_type();
                if (unionType != null)
                {
                    GetUnionTypeSymbol(unionType, currentScope);
                }
                var enumType = context.enum_type();
                if (enumType != null)
                {
                    GetEnumerationTypeSymbol(enumType, currentScope);
                }
                var bitSetType = context.bitset_type();
                if (bitSetType != null)
                {
                    GetBitSetTypeSymbol(bitSetType, currentScope);
                }
                var bitMaskType = context.bitmask_type();
                if (bitMaskType != null)
                {
                    GetBitMaskTypeSymbol(bitMaskType, currentScope);
                }
                var forwardDecl = context.constr_forward_decl();
                if (forwardDecl != null)
                {
                    if (forwardDecl.KW_STRUCT() != null)
                    {
                        var structure = new Struct(forwardDecl.ID().GetText(), dotNetNaming);
                        structure.IsForwardDeclaration = true;
                        currentScope.AddSymbolDefinition(structure);
                        SetTypeInfo(structure);

                    }
                    else if (forwardDecl.KW_UNION() != null)
                    {
                        var union = new Union(forwardDecl.ID().GetText(), dotNetNaming);
                        union.IsForwardDeclaration = true;
                        currentScope.AddSymbolDefinition(union);
                        SetTypeInfo(union);
                    }
                }
                var nativeType = context.native_type();
                if (nativeType != null)
                {
                    var annaps = GetAnnotations(nativeType.annapps(), currentScope);
                    var declarators = nativeType.simple_declarators().ID();
                    foreach (var decl in declarators)
                    {
                        var native = new NativeType(decl.GetText(), dotNetNaming, annaps);
                        currentScope.AddSymbolDefinition(native);
                        SetTypeInfo(native);
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            return true;
        }

        public override bool VisitExcept_decl([NotNull] IDLParser.Except_declContext context)
        {
            try
            {
                var ex = new ExceptionSymbol(context.ID().GetText(), dotNetNaming, GetAnnotations(context.annapps(), currentScope));
                currentScope.AddSymbolDefinition(ex);
                SetTypeInfo(ex);
                AddMembers(context.member(), ex.NamingScope);
            }
            catch (Exception e)
            {
                ThrowIdlCompilerException(e, context);
            }
            return true;
        }

        public override bool VisitInterface_or_forward_decl([NotNull] IDLParser.Interface_or_forward_declContext context)
        {
            try
            {
                var annotations = GetAnnotations(context.annapps(), currentScope);
                var forwardDecl = context.forward_decl();
                if (forwardDecl != null)
                {
                    var ifz = new Interface(forwardDecl.ID().GetText(), dotNetNaming, annotations);
                    ifz.IsAbstract = forwardDecl.KW_ABSTRACT() != null;
                    ifz.IsLocal = forwardDecl.KW_LOCAL() != null;
                    ifz.IsForwardDeclaration = true;
                    currentScope.AddSymbolDefinition(ifz);
                    SetTypeInfo(ifz);
                }
                var interfaceDecl = context.interface_decl();
                if (interfaceDecl != null)
                {
                    var ifzHeader = interfaceDecl.interface_header();
                    var ifz = new Interface(ifzHeader.ID().GetText(), dotNetNaming, annotations);
                    ifz.IsAbstract = ifzHeader.KW_ABSTRACT() != null;
                    ifz.IsLocal = ifzHeader.KW_LOCAL() != null;
                    var ifzInheritance = ifzHeader.interface_inheritance_spec();
                    if (ifzInheritance != null)
                    {
                        var inherits = ifzInheritance.interface_name();
                        foreach (var i in inherits)
                        {
                            var type = currentScope.ResolveType(i.scoped_name().GetText());
                            if (type is Interface parent)
                            {
                                if (ifz.IsAbstract && !parent.IsAbstract)
                                {
                                    ThrowIdlCompilerException(parent.Name + " is not abstract", context);
                                }
                                ifz.Inherits.Add(parent);
                            }
                            else
                            {
                                ThrowIdlCompilerException(type.Name + " is not an interface", context);
                            }
                        }
                    }
                    currentScope.AddSymbolDefinition(ifz);
                    SetTypeInfo(ifz);
                    currentScope = ifz.NamingScope;
                    VisitChildren(context);
                    currentScope = currentScope.ParentScope;
                }
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            return true;
        }

        public override bool VisitAttr_decl([NotNull] IDLParser.Attr_declContext context)
        {
            try
            {
                var annapps = GetAnnotations(context.annapps(), currentScope);
                var readOnly = context.readonly_attr_spec();
                if (readOnly != null)
                {
                    ITypeSymbol dataType = GetParamTypeSymbol(readOnly.param_type_spec(), currentScope);
                    var declarators = readOnly.readonly_attr_declarator().simple_declarator();
                    var raises = readOnly.readonly_attr_declarator().raises_expr();
                    List<ExceptionSymbol> exceptions = GetExceptions(raises, currentScope);
                    foreach (var simpleDecl in declarators)
                    {
                        var name = simpleDecl.ID().GetText();
                        var attribute = new AttributeType(name, dotNetNaming, true, annapps);
                        attribute.GetRaises = exceptions;
                        attribute.DataType = dataType;
                        currentScope.AddSymbolDefinition(attribute);
                    }
                }
                var attributeDecl = context.attr_spec();
                if (attributeDecl != null)
                {
                    ITypeSymbol dataType = GetParamTypeSymbol(attributeDecl.param_type_spec(), currentScope);
                    var declarators = attributeDecl.attr_declarator().simple_declarator();
                    List<ExceptionSymbol> getRaises = new List<ExceptionSymbol>();
                    List<ExceptionSymbol> setRaises = new List<ExceptionSymbol>();
                    var raises = attributeDecl.attr_declarator().attr_raises_expr();
                    if (raises != null)
                    {
                        var getExceptions = raises.get_excep_expr();
                        if (getExceptions != null)
                        {
                            getRaises = GetExceptions(getExceptions.exception_list(), currentScope);
                        }
                        var setExceptions = raises.set_excep_expr();
                        if (setExceptions != null)
                        {
                            setRaises = GetExceptions(setExceptions.exception_list(), currentScope);
                        }
                    }
                    foreach (var simpleDecl in declarators)
                    {
                        var name = simpleDecl.ID().GetText();
                        var attribute = new AttributeType(name, dotNetNaming, false, annapps);
                        attribute.DataType = dataType;
                        attribute.GetRaises = getRaises;
                        attribute.SetRaises = setRaises;
                        currentScope.AddSymbolDefinition(attribute);
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            return true;
        }

        public override bool VisitOp_decl([NotNull] IDLParser.Op_declContext context)
        {
            try
            {
                var annotations = GetAnnotations(context.annapps(), currentScope);
                var operation = new Operation(context.ID().GetText(), dotNetNaming, ami, annotations);
                var opAttribute = context.op_attribute();
                if (opAttribute != null)
                {
                    operation.IsOneWay = true;
                }
                var paramTypeSpec = context.op_type_spec().param_type_spec();
                if (paramTypeSpec != null)
                {
                    operation.ReturnType = GetParamTypeSymbol(context.op_type_spec().param_type_spec(), currentScope);
                }
                operation.Raises = GetExceptions(context.raises_expr(), currentScope);
                var contextDecl = context.context_expr();
                if (contextDecl != null)
                {
                    var properties = contextDecl.STRING_LITERAL();
                    foreach (var property in properties)
                    {
                        operation.Context.Add(property.GetText());
                    }
                }
                currentScope.AddSymbolDefinition(operation);
                var parameters = context.parameter_decls().param_decl();
                foreach (var paramDecl in parameters)
                {
                    var annapps = GetAnnotations(paramDecl.annapps(), currentScope);
                    var parameter = new OperationParameter(paramDecl.simple_declarator().ID().GetText(), dotNetNaming, annapps);
                    var paramAttribute = paramDecl.param_attribute();
                    switch (paramAttribute.GetText())
                    {
                        case "in":
                            parameter.Direction = ParameterDirection.IN;
                            break;
                        case "out":
                            parameter.Direction = ParameterDirection.OUT;
                            break;
                        case "inout":
                            parameter.Direction = ParameterDirection.INOUT;
                            break;
                    }
                    parameter.DataType = GetParamTypeSymbol(paramDecl.param_type_spec(), operation.NamingScope);
                    operation.NamingScope.AddSymbolDefinition(parameter);
                }
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            return true;
        }

        public override bool VisitValue([NotNull] IDLParser.ValueContext context)
        {
            try
            {
                var annotations = new List<Annotation>();
                if (context.Parent.GetChild(0) is IDLParser.AnnappsContext annaps)
                {
                    annotations = GetAnnotations(annaps, currentScope);
                }
                var valueBox = context.value_box_decl();
                if (valueBox != null)
                {
                    var valueType = new Symbols.ValueType(valueBox.ID().GetText(), dotNetNaming, annotations);
                    currentScope.AddSymbolDefinition(valueType);
                    SetTypeInfo(valueType);
                    var typeSpec = GetTypeSymbol(valueBox.type_spec(), currentScope);
                    //valueType.NamingScope.AddSymbolDefinition(typeSpec);
                    valueType.IsBoxed = true;
                    valueType.BoxedValue = typeSpec;
                }
                var forwardDecl = context.value_forward_decl();
                if (forwardDecl != null)
                {
                    var valueType = new Symbols.ValueType(forwardDecl.ID().GetText(), dotNetNaming, annotations);
                    valueType.IsForwardDeclaration = true;
                    if (forwardDecl.KW_ABSTRACT() != null)
                    {
                        valueType.IsAbstract = true;
                    }
                    currentScope.AddSymbolDefinition(valueType);
                    SetTypeInfo(valueType);
                }
                var valueAbs = context.value_abs_decl();
                if (valueAbs != null)
                {
                    var valueType = new Symbols.ValueType(valueAbs.ID().GetText(), dotNetNaming, annotations);
                    valueType.IsAbstract = true;
                    GetInheritance(valueType, valueAbs.value_inheritance_spec());
                    currentScope.AddSymbolDefinition(valueType);
                    SetTypeInfo(valueType);
                    currentScope = valueType.NamingScope;
                    VisitChildren(valueAbs);
                    currentScope = currentScope.ParentScope;
                }
                var valueDecl = context.value_decl();
                if (valueDecl != null)
                {
                    var valueHeader = valueDecl.value_header();
                    var valueType = new Symbols.ValueType(valueHeader.ID().GetText(), dotNetNaming, annotations);
                    if (valueHeader.KW_CUSTOM() != null)
                    {
                        valueType.IsCustom = true;
                    }
                    GetInheritance(valueType, valueHeader.value_inheritance_spec());
                    currentScope.AddSymbolDefinition(valueType);
                    SetTypeInfo(valueType);
                    currentScope = valueType.NamingScope;
                    VisitChildren(valueDecl);
                    currentScope = currentScope.ParentScope;
                }
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            return true;
        }

        public override bool VisitAnnotation_decl([NotNull] IDLParser.Annotation_declContext context)
        {
            try
            {
                var definition = context.annotation_def();
                if (definition != null)
                {
                    var header = definition.annotation_header();
                    var annotationType = new AnnotationType(header.ID().GetText(), dotNetNaming);
                    var inheritance = header.annotation_inheritance_spec();
                    if (inheritance != null)
                    {
                        var type = currentScope.ResolveType(inheritance.scoped_name().GetText());
                        if (type is AnnotationType at)
                        {
                            annotationType.Inherits.Add(at);
                        }
                        else
                        {
                            ThrowIdlCompilerException(type.Name + " is not an annotation", context);
                        }

                    }
                    annotationType = (AnnotationType)currentScope.AddSymbolDefinition(annotationType);
                    SetTypeInfo(annotationType);
                    var exports = definition.annotation_body().annotation_export();
                    foreach (var export in exports)
                    {
                        var member = export.annotation_member();
                        if (member != null)
                        {
                            ITypeSymbol memberType = null;
                            var constType = member.annotation_member_type().const_type();
                            if (constType != null)
                            {
                                var scopedName = constType.scoped_name();
                                if (scopedName != null)
                                {
                                    memberType = annotationType.NamingScope.ResolveType(scopedName.GetText());
                                }
                                else
                                {
                                    memberType = BaseType.CreateBaseType(constType.GetSourceText(), dotNetNaming);
                                }
                            }
                            var anyType = member.annotation_member_type().any_type();
                            if (anyType != null)
                            {
                                memberType = BaseType.CreateBaseType(anyType.GetSourceText(), dotNetNaming);
                            }
                            var annotationMember = new AnnotationMember(member.simple_declarator().ID().GetText(), dotNetNaming);
                            annotationMember.DataType = memberType;
                            var constExp = member.const_exp();
                            if (constExp != null)
                            {
                                annotationMember.DefaultValue = GetConstantExpValue(constExp, annotationType.NamingScope);
                            }
                            annotationType.NamingScope.AddSymbolDefinition(annotationMember);
                        }
                        var enumType = export.enum_type();
                        if (enumType != null)
                        {
                            GetEnumerationTypeSymbol(enumType, annotationType.NamingScope);
                        }
                        var constDecl = export.const_decl();
                        if (constDecl != null)
                        {
                            GetConstant(constDecl, annotationType.NamingScope);
                        }
                        var typeDef = export.type_def();
                        if (typeDef != null)
                        {
                            GetTypeDefinition(typeDef, annotationType.NamingScope);
                        }
                    }
                }
                var fwdDecl = context.annotation_forward_dcl();
                if (fwdDecl != null)
                {
                    var annotationType = new AnnotationType(fwdDecl.ID().GetText(), dotNetNaming);
                    annotationType.IsForwardDeclaration = true;
                    currentScope.AddSymbolDefinition(annotationType);
                    SetTypeInfo(annotationType);
                }
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            return true;
        }

        public override bool VisitState_member([NotNull] IDLParser.State_memberContext context)
        {
            try
            {
                var annotations = GetAnnotations(context.annapps(), currentScope);
                ITypeSymbol memberSymbolType = GetTypeSymbol(context.type_spec(), currentScope);
                var isPublic = false;
                if (context.KW_PUBLIC() != null)
                {
                    isPublic = true;
                }
                foreach (var declaratorCtx in context.declarators().declarator())
                {
                    var simpleDecl = declaratorCtx.simple_declarator();
                    if (simpleDecl != null)
                    {
                        var memberId = simpleDecl.GetText();
                        var member = new StateMember(memberId, dotNetNaming, annotations);
                        member.IsPublic = isPublic;
                        member.DataType = memberSymbolType;
                        currentScope.AddSymbolDefinition(member);
                    }
                    else
                    {
                        var complexDecl = declaratorCtx.complex_declarator();
                        if (complexDecl != null)
                        {
                            var arrayDecl = complexDecl.array_declarator();
                            var array = GetArrayType(complexDecl.array_declarator(), currentScope);
                            var memberId = arrayDecl.ID().GetText();
                            var member = new StateMember(memberId, dotNetNaming, annotations);
                            member.IsPublic = isPublic;
                            array.DataType = memberSymbolType;
                            member.DataType = array;
                            currentScope.AddSymbolDefinition(member);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            return true;
        }

        public override bool VisitInit_decl([NotNull] IDLParser.Init_declContext context)
        {
            try
            {
                var initializer = new Initializer(context.ID().GetText(), dotNetNaming, GetAnnotations(context.annapps(), currentScope));
                currentScope.AddSymbolDefinition(initializer);
                var parameters = context.init_param_decls().init_param_decl();
                foreach (var paramDecl in parameters)
                {
                    var annotations = GetAnnotations(paramDecl.annapps(), currentScope);
                    var parameter = new OperationParameter(paramDecl.simple_declarator().ID().GetText(), dotNetNaming, annotations);
                    parameter.Direction = ParameterDirection.IN;
                    parameter.DataType = GetParamTypeSymbol(paramDecl.param_type_spec(), initializer.NamingScope);
                    initializer.NamingScope.AddSymbolDefinition(parameter);
                }
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            return true;
        }

        public override bool VisitPragma_prefix([NotNull] IDLParser.Pragma_prefixContext context)
        {
            prefix = context.STRING_LITERAL().GetText().Trim('"');
            return true;
        }

        public override bool VisitPragma_id([NotNull] IDLParser.Pragma_idContext context)
        {
            var name = context.scoped_name().GetText();
            var id = context.STRING_LITERAL().GetText();
            currentScope.RepositoryIds[name] = id;
            return true;
        }

        public override bool VisitPragma_version([NotNull] IDLParser.Pragma_versionContext context)
        {
            var name = context.SCOPED_NAME().GetText();
            var version = context.VERSION_NUM().GetText();
            currentScope.Versions[name] = version;
            return true;
        }

        private void GetInheritance(Symbols.ValueType valueType, IDLParser.Value_inheritance_specContext context)
        {
            var inherits = context.value_name();
            foreach (var item in inherits)
            {
                var type = currentScope.ResolveType(item.scoped_name().GetText());
                if (type is Symbols.ValueType vt)
                {
                    if (!vt.IsAbstract)
                    {
                        if (valueType.IsAbstract)
                        {
                            ThrowIdlCompilerException(vt.Name + " is abstract and can only inherit abstract value types", context);
                        }
                        else if (valueType.Base != null)
                        {
                            ThrowIdlCompilerException(valueType.Name + " already inherits from concrete value type " + valueType.Base.Name, context);
                        }
                        if (context.KW_TRUNCATABLE() != null)
                        {
                            if (valueType.IsCustom)
                            {
                                ThrowIdlCompilerException("Custom value type " + valueType.Name + " cannot be truncated ", context);
                            }
                            valueType.Truncatable = vt;
                        }
                        valueType.Base = vt;
                    }
                    else
                    {
                        valueType.Inherits.Add(vt);
                    }
                }
                else
                {
                    ThrowIdlCompilerException(type.Name + " is not a value type", context);
                }
            }
            var supports = context.interface_name();
            var inheritanceSupports = valueType.GetInheritanceSupports();
            foreach (var item in supports)
            {
                var type = currentScope.ResolveType(item.scoped_name().GetText());
                if (type is Interface ifz)
                {
                    if (!ifz.IsAbstract)
                    {
                        var concretIfz = valueType.Supports.Find(i => i.IsAbstract == false);
                        if (concretIfz != null)
                        {
                            ThrowIdlCompilerException(valueType.Name + " already inherits from non abstract interface" + concretIfz.Name, context);
                        }
                        foreach (var sup in inheritanceSupports)
                        {
                            if (!ifz.DerivesFrom(sup))
                            {
                                ThrowIdlCompilerException(ifz.Name + " must be derived from " + sup.Name, context);
                            }
                        }
                    }
                    valueType.Supports.Add(ifz);
                }
                else
                {
                    ThrowIdlCompilerException(type.Name + " is not an interface", context);
                }
            }
        }

        public override bool VisitType_id_decl([NotNull] IDLParser.Type_id_declContext context)
        {
            var name = context.scoped_name().GetText();
            var id = context.STRING_LITERAL().GetText().Trim('"');
            currentScope.RepositoryIds[name] = id;
            return true;
        }

        public override bool VisitType_prefix_decl([NotNull] IDLParser.Type_prefix_declContext context)
        {
            var name = context.scoped_name().GetText();
            var prefix = context.STRING_LITERAL().GetText().Trim('"');
            currentScope.Prefixes[name] = prefix;
            return true;
        }

        private List<ExceptionSymbol> GetExceptions(IDLParser.Raises_exprContext context, Scope scope)
        {
            List<ExceptionSymbol> exceptions = new List<ExceptionSymbol>();
            if (context != null)
            {
                var scopedNames = context.scoped_name();
                foreach (var sn in scopedNames)
                {
                    var ex = currentScope.ResolveType(sn.GetText());
                    if (ex is ExceptionSymbol es)
                    {
                        exceptions.Add(es);
                    }
                    else
                    {
                        ThrowIdlCompilerException(ex.Name + " is not an exception type", context);
                    }
                }
            }
            return exceptions;
        }

        private List<ExceptionSymbol> GetExceptions(IDLParser.Exception_listContext context, Scope scope)
        {
            List<ExceptionSymbol> exceptions = new List<ExceptionSymbol>();
            var scopedNames = context.scoped_name();
            foreach (var sn in scopedNames)
            {
                var ex = currentScope.ResolveType(sn.GetText());
                if (ex is ExceptionSymbol es)
                {
                    exceptions.Add(es);
                }
                else
                {
                    ThrowIdlCompilerException(ex.Name + " is not an exception type", context);
                }
            }
            return exceptions;
        }

        private ITypeSymbol GetParamTypeSymbol(IDLParser.Param_type_specContext context, Scope scope)
        {
            try
            {
                var baseType = context.base_type_spec();
                if (baseType != null)
                {
                    return BaseType.CreateBaseType(baseType.GetSourceText(), dotNetNaming);
                }
                var stringType = context.string_type();
                if (stringType != null)
                {
                    var length = stringType.positive_int_const();
                    if (length != null)
                    {
                        var constExp = GetConstantExpValue(length.const_exp(), scope);
                        return new StringType(constExp.GetIntValue(), dotNetNaming);
                    }
                    return new StringType(dotNetNaming);
                }
                var wStringType = context.wide_string_type();
                if (wStringType != null)
                {
                    var length = wStringType.positive_int_const();
                    if (length != null)
                    {
                        var constExp = GetConstantExpValue(length.const_exp(), scope);
                        return new WStringType(constExp.GetIntValue(), dotNetNaming);
                    }
                    return new WStringType(dotNetNaming);
                }
                var scopedName = context.scoped_name();
                if (scopedName != null)
                {
                    return currentScope.ResolveType(scopedName.GetText());
                }
                ThrowIdlCompilerException(context.GetText() + " not found", context);
            }
            catch (Exception ex)
            {
                ThrowIdlCompilerException(ex, context);
            }
            
            return null;
        }

        private ITypeSymbol GetTypeSymbol(IDLParser.Type_specContext context, Scope scope)
        {
            var simpleType = context.simple_type_spec();
            if (simpleType != null)
            {
                return GetSimpleTypeSymbol(simpleType, scope);
            }
            else
            {
                var constrType = context.constr_type_spec();
                var symbolType = GetConstructedTypeSymbol(constrType, scope);
                //scope.AddSymbolDefinition(symbolType);
                return symbolType;
            }
        }

        private ITypeSymbol GetSimpleTypeSymbol(IDLParser.Simple_type_specContext context, Scope scope)
        {
            var baseType = context.base_type_spec();
            if (baseType != null)
            {
                return BaseType.CreateBaseType(baseType.GetSourceText(), dotNetNaming);
            }
            var scopedName = context.scoped_name();
            if (scopedName != null)
            {
                var ns = scopedName.GetText();
                var resolvedType = scope.ResolveType(ns);
                if (scope.IsRecursive(resolvedType))
                {
                    if (resolvedType is Sequence seq)
                    {
                        var recursiveType = new RecursiveType(seq.DataType, seq.DataType.Name, scope, dotNetNaming, new List<Annotation>());
                        return new Sequence(recursiveType, seq.SequenceType, dotNetNaming, seq.Annotations);
                    }
                    else
                    {
                        return new RecursiveType(resolvedType, resolvedType.Name, scope, dotNetNaming, new List<Annotation>());
                    }                    
                }
                else
                {
                    return resolvedType;
                }
            }
            var templateType = context.template_type_spec();
            if (templateType != null)
            {
                var sequenceType = templateType.sequence_type();
                if (sequenceType != null)
                {
                    var simpleType = sequenceType.simple_type_spec();
                    var typeSymbol = GetSimpleTypeSymbol(simpleType, scope);
                    var length = sequenceType.positive_int_const();
                    var sequence = new Sequence(typeSymbol, defaultSeqType, dotNetNaming);
                    if (length != null)
                    {
                        var constExp = GetConstantExpValue(length.const_exp(), scope);
                        if (constExp.GetIntValue() > 0)
                        {
                            sequence.Length = constExp.GetIntValue();
                        }
                        else
                        {
                            ThrowIdlCompilerException("Length must be a positive integer", context);
                        }
                    }
                    return sequence;
                }
                var mapType = templateType.map_type();
                if (mapType != null)
                {

                    var keyType = mapType.simple_type_spec(0);
                    var keyTypeSymbol = GetSimpleTypeSymbol(keyType, scope);
                    var dataType = mapType.simple_type_spec(1);
                    var dataTypeSymbol = GetSimpleTypeSymbol(dataType, scope);
                    var length = sequenceType.positive_int_const();
                    var map = new Map(keyTypeSymbol, dataTypeSymbol, dotNetNaming);
                    if (length != null)
                    {
                        var constExp = GetConstantExpValue(length.const_exp(), scope);
                        if (constExp.GetIntValue() > 0)
                        {
                            map.Length = constExp.GetIntValue();
                        }
                        else
                        {
                            ThrowIdlCompilerException("Length must be a positive integer", context);
                        }
                    }
                    return map;
                }
                var stringType = templateType.string_type();
                if (stringType != null)
                {
                    var length = stringType.positive_int_const();
                    if (length != null)
                    {
                        var constExp = GetConstantExpValue(length.const_exp(), scope);
                        if (constExp.GetIntValue() > 0)
                        {
                            return new StringType(constExp.GetIntValue(), dotNetNaming);
                        }
                        else
                        {
                            ThrowIdlCompilerException("Length must be a positive integer", context);
                        }
                    }
                    return new StringType(dotNetNaming);
                }
                var wStringType = templateType.wide_string_type();
                if (wStringType != null)
                {
                    var length = wStringType.positive_int_const();
                    if (length != null)
                    {
                        var constExp = GetConstantExpValue(length.const_exp(), scope);
                        if (constExp.GetIntValue() > 0)
                        {
                            return new WStringType(constExp.GetIntValue(), dotNetNaming);
                        }
                        else
                        {
                            ThrowIdlCompilerException("Length must be a positive integer", context);
                        }
                    }
                    return new WStringType(dotNetNaming);
                }
                var fixedType = templateType.fixed_pt_type();
                if (fixedType != null)
                {
                    var digits = GetConstantExpValue(fixedType.positive_int_const()[0].const_exp(), scope);
                    if (digits.GetIntValue() <= 0)
                    {
                        ThrowIdlCompilerException("Digits must be a positive integer", context);
                    }
                    var fractionalDigits = GetConstantExpValue(fixedType.positive_int_const()[1].const_exp(), scope);
                    if (fractionalDigits.GetIntValue() <= 0)
                    {
                        ThrowIdlCompilerException("Fractional digits must be a positive integer", context);
                    }
                    return new FixedPointType(digits.GetIntValue(), fractionalDigits.GetIntValue(), dotNetNaming);

                }
            }
            ThrowIdlCompilerException("Type " + context.GetText() + " not found", context);
            return null;
        }

        private ITypeSymbol GetConstructedTypeSymbol(IDLParser.Constr_type_specContext context, Scope scope)
        {
            var structType = context.struct_type();
            if (structType != null)
            {
                return GetStructTypeSymbol(structType, scope);
            }
            var unionType = context.union_type();
            if (unionType != null)
            {
                return GetUnionTypeSymbol(unionType, scope);
            }
            var enumType = context.enum_type();
            if (enumType != null)
            {
                return GetEnumerationTypeSymbol(enumType, scope);
            }
            var bitSet = context.bitset_type();
            if (bitSet != null)
            {
                return GetBitSetTypeSymbol(bitSet, scope);
            }
            var bitMask = context.bitmask_type();
            if (bitMask != null)
            {
                return GetBitMaskTypeSymbol(bitMask, scope);
            }
            ThrowIdlCompilerException("Type " + context.GetText() + " not found", context);
            return null;
        }

        private ICollection<ITypeSymbol> GetTypeDefinition(IDLParser.Type_defContext context, Scope scope)
        {
            var types = new List<ITypeSymbol>();
            var typeDeclarator = context.type_declarator();
            if (typeDeclarator != null)
            {
                var annaps = GetAnnotations(context.annapps(), scope);
                var typeSymbol = GetTypeSymbol(typeDeclarator.type_spec(), scope);
                var declarators = typeDeclarator.declarators().declarator();
                foreach (var declarator in declarators)
                {
                    var simpleDecl = declarator.simple_declarator();
                    if (simpleDecl != null)
                    {
                        var name = simpleDecl.GetText();
                        var typeDef = new TypeDefinition(name, dotNetNaming, annaps);
                        typeDef.DataType = typeSymbol;
                        scope.AddSymbolDefinition(typeDef);
                        SetTypeInfo(typeDef);
                        types.Add(typeDef);
                    }
                    else
                    {
                        var complexDecl = declarator.complex_declarator();
                        if (complexDecl != null)
                        {
                            var arrayDecl = complexDecl.array_declarator();
                            if (arrayDecl != null)
                            {
                                var array = GetArrayType(arrayDecl, scope);
                                var name = arrayDecl.ID().GetText();
                                var typeDef = new TypeDefinition(name, dotNetNaming, annaps);
                                array.DataType = typeSymbol;
                                typeDef.DataType = array;
                                scope.AddSymbolDefinition(typeDef);
                                SetTypeInfo(typeDef);
                                types.Add(typeDef);
                            }
                        }
                    }
                }
            }
            return types;
        }

        private Constant GetConstant(IDLParser.Const_declContext context, Scope scope)
        {
            var constType = context.const_type();
            var annotations = GetAnnotations(context.annapps(), scope);
            var constant = new Constant(context.ID().GetText(), dotNetNaming, annotations);
            var scopedName = constType.scoped_name();
            ITypeSymbol type;
            if (scopedName != null)
            {
                type = scope.ResolveType(scopedName.GetText());
            }
            else
            {
                type = BaseType.CreateBaseType(constType.GetSourceText(), dotNetNaming);
            }
            constant.DataType = type;
            constant.Value = GetConstantExpValue(context.const_exp(), scope);
            scope.AddSymbolDefinition(constant);
            return constant;
        }

        private Struct GetStructTypeSymbol(IDLParser.Struct_typeContext context, Scope scope)
        {
            var annapps = GetAnnotations(context.annapps(), scope);
            var structure = new Struct(context.ID().GetText(), dotNetNaming, annapps);
            structure = (Struct)scope.AddSymbolDefinition(structure);
            SetTypeInfo(structure);
            var inherits = context.scoped_name();
            if (inherits != null)
            {
                var type = scope.ResolveType(inherits.GetText());
                if (type is Struct parent)
                {
                    structure.Base = parent;
                }
                else
                {
                    ThrowIdlCompilerException(type.Name + " is not a struct", context);
                }
            }
            var memberList = context.member_list();
            if (memberList != null)
            {
                AddMembers(memberList.member(), structure.NamingScope);
            }
            return structure;
        }

        private void AddMembers(IDLParser.MemberContext[] members, Scope scope)
        {
            foreach (var memberCtx in members)
            {
                ITypeSymbol memberSymbolType = GetTypeSymbol(memberCtx.type_spec(), scope);
                foreach (var declaratorCtx in memberCtx.declarators().declarator())
                {
                    var simpleDecl = declaratorCtx.simple_declarator();
                    if (simpleDecl != null)
                    {
                        var memberId = simpleDecl.GetText();
                        var member = new Member(memberId, scope, dotNetNaming, GetAnnotations(memberCtx.annapps(), scope));
                        member.DataType = memberSymbolType;
                        scope.AddSymbolDefinition(member);
                    }
                    else
                    {
                        var complexDecl = declaratorCtx.complex_declarator();
                        if (complexDecl != null)
                        {
                            var arrayDecl = complexDecl.array_declarator();
                            if (arrayDecl != null)
                            {
                                var array = new ArrayType(dotNetNaming);
                                var memberId = arrayDecl.ID().GetText();
                                var fixedArraySize = arrayDecl.fixed_array_size();
                                foreach (var dim in fixedArraySize)
                                {
                                    var constExp = GetConstantExpValue(dim.positive_int_const().const_exp(), scope);
                                    if (constExp.GetIntValue() <= 0)
                                    {
                                        ThrowIdlCompilerException("Length must be a positive integer", memberCtx);
                                    }
                                    array.Dimensions.Add(constExp.GetIntValue());
                                }
                                var member = new Member(memberId, scope, dotNetNaming, GetAnnotations(memberCtx.annapps(), scope));
                                array.DataType = memberSymbolType;
                                member.DataType = array;
                                scope.AddSymbolDefinition(member);
                            }
                        }
                    }
                }
            }
        }

        private Union GetUnionTypeSymbol(IDLParser.Union_typeContext context, Scope scope)
        {
            var annapps = GetAnnotations(context.annapps(), scope);
            var union = new Union(context.ID().GetText(), dotNetNaming, annapps);
            union = (Union)scope.AddSymbolDefinition(union);
            SetTypeInfo(union);
            var switchType = context.switch_type_spec();
            var enumType = switchType.enum_type();
            var scopedName = switchType.scoped_name();
            if (enumType != null)
            {
                union.Discriminator = GetEnumerationTypeSymbol(enumType, union.NamingScope);
            }
            else if (scopedName != null)
            {
                union.Discriminator = union.NamingScope.ResolveType(scopedName.GetText());
            }
            else
            {
                union.Discriminator = BaseType.CreateBaseType(switchType.GetSourceText(), dotNetNaming);
            }
            var switchBody = context.switch_body();
            var stmts = switchBody.case_stmt();
            foreach (var stmt in stmts)
            {
                var elementSpec = stmt.element_spec();
                var annotationsStmt = GetAnnotations(elementSpec.annapps(), union.NamingScope);
                var stmtType = GetTypeSymbol(elementSpec.type_spec(), union.NamingScope);
                var declarator = elementSpec.declarator();
                CaseStatement caseStmt = null;
                var simpleDecl = declarator.simple_declarator();
                if (simpleDecl != null)
                {
                    var memberId = simpleDecl.GetText();
                    caseStmt = new CaseStatement(memberId, union.NamingScope, dotNetNaming, annotationsStmt);
                    caseStmt.DataType = stmtType;
                    union.NamingScope.AddSymbolDefinition(caseStmt);
                }
                else
                {
                    var complexDecl = declarator.complex_declarator();
                    if (complexDecl != null)
                    {
                        var arrayDecl = complexDecl.array_declarator();
                        if (arrayDecl != null)
                        {
                            var array = GetArrayType(arrayDecl, union.NamingScope);
                            var memberId = arrayDecl.ID().GetText();
                            caseStmt = new CaseStatement(memberId, union.NamingScope, dotNetNaming, annotationsStmt);
                            array.DataType = stmtType;
                            caseStmt.DataType = array;
                            union.NamingScope.AddSymbolDefinition(caseStmt);
                        }
                    }
                }
                var caseLabels = stmt.case_label();
                foreach (var caseLabel in caseLabels)
                {
                    if (caseLabel.KW_DEFAULT() != null)
                    {
                        if (caseStmt != null)
                        {
                            caseStmt.IsDefault = true;
                        }
                    }
                    else
                    {
                        if (union.Discriminator is Enumeration enumeration)
                        {
                            bool found = false;
                            foreach (var enumerator in enumeration.Enumerators)
                            {
                                if (enumerator.Name.Equals(caseLabel.const_exp().GetSourceText()))
                                {
                                    caseStmt.CaseLabels.Add(enumerator);
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                ThrowIdlCompilerException("Enumerator " + caseLabel.const_exp().GetSourceText() + " not found", context);
                            }
                        }
                        else
                        {
                            var constExpr = GetConstantExpValue(caseLabel.const_exp(), union.NamingScope);
                            if (caseStmt != null)
                            {
                                caseStmt.CaseLabels.Add(constExpr);
                            }
                        }                        
                    }
                }
            }
            return union;
        }

        private ArrayType GetArrayType(IDLParser.Array_declaratorContext context, Scope scope)
        {
            var array = new ArrayType(dotNetNaming);
            var fixedArraySize = context.fixed_array_size();
            foreach (var dim in fixedArraySize)
            {
                var constExp = GetConstantExpValue(dim.positive_int_const().const_exp(), scope);
                if (constExp.GetIntValue() > 0)
                {
                    array.Dimensions.Add(constExp.GetIntValue());
                }
                else
                {
                    ThrowIdlCompilerException("Length must be a positive integer", context);
                }
            }
            return array;
        }

        private Enumeration GetEnumerationTypeSymbol(IDLParser.Enum_typeContext context, Scope scope)
        {
            var annapps = GetAnnotations(context.annapps(), scope);
            var enumeration = new Enumeration(context.ID().GetText(), dotNetNaming, annapps);
            var bitBound = GetAnnotation("bit_bound", annapps);
            if (bitBound != null)
            {
                enumeration.Length = (byte)bitBound.ConstantExpr.GetIntValue();
                enumeration.Annotations.Remove(bitBound);
            }
            SetTypeInfo(enumeration);
            scope.AddSymbolDefinition(enumeration);
            var enumerators = context.enumerator();
            foreach (var e in enumerators)
            {
                var enumerator = new Enumerator(enumeration, e.ID().GetText(), dotNetNaming, GetAnnotations(e.annapps(), scope));
                var value = GetAnnotation("value", enumerator.Annotations);
                if (value != null)
                {
                    enumerator.HasValue = true;
                    enumerator.Value = value.ConstantExpr.GetIntValue();
                    enumerator.Annotations.Remove(value);
                }
                enumeration.Enumerators.Add(enumerator);
                scope.AddSymbolDefinition(enumerator);
            }
            return enumeration;
        }

        private BitSet GetBitSetTypeSymbol(IDLParser.Bitset_typeContext context, Scope scope)
        {
            var annapps = GetAnnotations(context.annapps(), scope);
            var bitSet = new BitSet(context.ID().GetText(), dotNetNaming, annapps);
            scope.AddSymbolDefinition(bitSet);
            SetTypeInfo(bitSet);
            var scopedName = context.scoped_name();
            if (scopedName != null)
            {
                var inherits = scope.ResolveType(scopedName.GetText());
                if (inherits == null)
                {
                    ThrowIdlCompilerException("bitset " + scopedName.GetText() + " not found", context);
                }
                else if (inherits is BitSet bs)
                {
                    bitSet.Base = bs;
                }
                else
                {
                    ThrowIdlCompilerException(scopedName.GetText() + " is not a bitset", context);
                }
            }
            var bitfields = context.bitfield();
            foreach (var bitfield in bitfields)
            {
                var bitFieldSpec = bitfield.bitfield_spec();
                var ann = GetAnnotations(bitFieldSpec.annapps(), bitSet.NamingScope);
                var constExpr = GetConstantExpValue(bitFieldSpec.positive_int_const().const_exp(), scope);
                var length = constExpr.GetIntValue();
                if (length <= 0)
                {
                    ThrowIdlCompilerException("Length must be a positive integer", context);
                }
                else if (length > 64)
                {
                    ThrowIdlCompilerException("Max bitfield length is 64 bits", context);
                }
                ITypeSymbol bfType = null;
                var typeSpec = bitFieldSpec.bitfield_type_spec();
                if (typeSpec != null)
                {
                    bfType = BaseType.CreateBaseType(typeSpec.GetSourceText(), dotNetNaming);
                }
                else
                {
                    if (length == 1)
                    {
                        bfType = BaseType.CreateBaseType("boolean", dotNetNaming);
                    }
                    else if (length <= 8)
                    {
                        bfType = BaseType.CreateBaseType("octet", dotNetNaming);
                    }
                    else if (length <= 16)
                    {
                        bfType = BaseType.CreateBaseType("unsigned short", dotNetNaming);
                    }
                    else if (length <= 32)
                    {
                        bfType = BaseType.CreateBaseType("unsigned long", dotNetNaming);
                    }
                    else if (length <= 64)
                    {
                        bfType = BaseType.CreateBaseType("unsigned long long", dotNetNaming);
                    }
                    else
                    {
                        bfType = BaseType.CreateBaseType("unsigned long long", dotNetNaming);
                    }
                }
                var simpleDecl = bitfield.simple_declarators();
                if (simpleDecl != null)
                {
                    var ids = simpleDecl.ID();
                    foreach (var id in ids)
                    {
                        var bf = new BitField(id.GetText(), dotNetNaming, annapps);
                        bf.Length = (int)length;
                        bf.DataType = bfType;
                        bitSet.NamingScope.AddSymbolDefinition(bf);
                    }
                }
            }
            return bitSet;
        }

        private BitMask GetBitMaskTypeSymbol(IDLParser.Bitmask_typeContext context, Scope scope)
        {
            var annapps = GetAnnotations(context.annapps(), scope);
            var bitMask = new BitMask(context.ID().GetText(), dotNetNaming, annapps);
            var bitBound = GetAnnotation("bit_bound", annapps);
            if (bitBound != null)
            {
                bitMask.Length = (byte)bitBound.ConstantExpr.GetIntValue();
                bitMask.Annotations.Remove(bitBound);
            }
            scope.AddSymbolDefinition(bitMask);
            SetTypeInfo(bitMask);
            var bitValues = context.bit_value();
            foreach (var bv in bitValues)
            {
                var value = new BitValue(bv.ID().GetText(), dotNetNaming, GetAnnotations(bv.annapps(), scope));
                var position = GetAnnotation("position", value.Annotations);
                if (position != null)
                {
                    value.Position = (byte)position.ConstantExpr.GetIntValue();
                    value.Annotations.Remove(position);
                }
                else
                {
                    var last = bitMask.Values.LastOrDefault();
                    if (last != null)
                    {
                        value.Position = last.Position;
                        value.Position++;
                    }
                }
                var duplicate = bitMask.Values.Find(bv => bv.Position == value.Position);
                if (duplicate != null)
                {
                    ThrowIdlCompilerException(value.Name + " duplicates " + duplicate.Name, context);
                }
                bitMask.Values.Add(value);
                scope.AddSymbolDefinition(value);
            }
            return bitMask;
        }

        private Literal GetConstantExpValue(IDLParser.Const_expContext context, Scope scope)
        {
            var orExpr = context.or_expr();
            var xorExpr = orExpr.xor_expr();
            var literals = new List<Literal>();
            foreach (var exp in xorExpr)
            {
                literals.Add(GetXorExpValue(exp, scope));
            }
            var result = literals.Aggregate((acc, literal) =>
            {
                if (acc == null)
                {
                    return literal;
                }
                else
                {
                    return acc.Or(literal);
                }
            });
            return result;
        }

        private Literal GetXorExpValue(IDLParser.Xor_exprContext context, Scope scope)
        {
            var andExpr = context.and_expr();
            var literals = new List<Literal>();
            foreach (var exp in andExpr)
            {
                literals.Add(GetAndExpValue(exp, scope));
            }
            var result = literals.Aggregate((acc, literal) =>
            {
                if (acc == null)
                {
                    return literal;
                }
                else
                {
                    return acc.Xor(literal);
                }
            });
            return result;
        }

        private Literal GetAndExpValue(IDLParser.And_exprContext context, Scope scope)
        {
            var shiftExpr = context.shift_expr();
            var literals = new List<Literal>();
            foreach (var exp in shiftExpr)
            {
                literals.Add(GetShiftExpValue(exp, scope));
            }
            var result = literals.Aggregate((acc, literal) =>
            {
                if (acc == null)
                {
                    return literal;
                }
                else
                {
                    return acc.And(literal);
                }
            });
            return result;
        }

        private Literal GetShiftExpValue(IDLParser.Shift_exprContext context, Scope scope)
        {
            Literal result = null;
            ITerminalNode operation = null;
            foreach (IParseTree child in context.children)
            {
                if (child is IDLParser.Add_exprContext addExp)
                {
                    if (result == null)
                    {
                        result = GetAddExpValue(addExp, scope);
                    }
                    else if (operation != null)
                    {
                        var literal = GetAddExpValue(addExp, scope);
                        switch (operation.Symbol.Type)
                        {
                            case IDLParser.RIGHT_SHIFT:
                                result = result.ShiftRightBy(literal);
                                break;
                            case IDLParser.LEFT_SHIFT:
                                result = result.ShiftLeftBy(literal);
                                break;
                        }
                        operation = null;
                    }
                }
                else if (child is ITerminalNode)
                {
                    operation = (ITerminalNode)child;
                }
            }
            return result;
        }

        private Literal GetAddExpValue(IDLParser.Add_exprContext context, Scope scope)
        {
            Literal result = null;
            ITerminalNode operation = null;
            foreach (IParseTree child in context.children)
            {
                if (child is IDLParser.Mult_exprContext multExp)
                {
                    if (result == null)
                    {
                        result = GetMultExpValue(multExp, scope);
                    }
                    else if (operation != null)
                    {
                        var literal = GetMultExpValue(multExp, scope);
                        switch (operation.Symbol.Type)
                        {
                            case IDLParser.PLUS:
                                result = result.Add(literal);
                                break;
                            case IDLParser.MINUS:
                                result = result.Sub(literal);
                                break;
                        }
                        operation = null;
                    }
                }
                else if (child is ITerminalNode)
                {
                    operation = (ITerminalNode)child;
                }
            }
            return result;
        }

        private Literal GetMultExpValue(IDLParser.Mult_exprContext context, Scope scope)
        {
            Literal result = null;
            ITerminalNode operation = null;
            foreach (IParseTree child in context.children)
            {
                if (child is IDLParser.Unary_exprContext unaryExp)
                {
                    if (result == null)
                    {
                        result = GetUnaryExpValue(unaryExp, scope);
                    }
                    else if (operation != null)
                    {
                        var literal = GetUnaryExpValue(unaryExp, scope);
                        switch (operation.Symbol.Type)
                        {
                            case IDLParser.STAR:
                                result = result.MultBy(literal);
                                break;
                            case IDLParser.SLASH:
                                result = result.DivBy(literal);
                                break;
                            case IDLParser.PERCENT:
                                result = result.ModBy(literal);
                                break;
                        }
                        operation = null;
                    }
                }
                else if (child is ITerminalNode)
                {
                    operation = (ITerminalNode)child;
                }
            }
            return result;
        }

        private Literal GetUnaryExpValue(IDLParser.Unary_exprContext context, Scope scope)
        {
            var unaryOp = context.unary_operator();
            var exp = context.primary_expr();
            var literal = GetPrimaryExpValue(exp, scope);
            if (unaryOp != null)
            {
                var op = unaryOp.children[0] as ITerminalNode;
                switch (op.Symbol.Type)
                {
                    case IDLParser.MINUS:
                        literal.InvertSign();
                        break;
                    case IDLParser.TILDE:
                        literal.Negate();
                        break;
                }
            }
            return literal;
        }

        private Literal GetPrimaryExpValue(IDLParser.Primary_exprContext context, Scope scope)
        {
            var scopedName = context.scoped_name();
            if (scopedName != null)
            {
                var symbol = scope.ResolveSymbol(scopedName.GetText());
                switch (symbol)
                {
                    case Constant constant:
                        return constant.Value;
                    case Enumerator enumValue:
                        return new EnumValLiteral(enumValue);
                    default:
                        throw new IdlCompilerException(scopedName.GetText() + " is not a constant");
                }
            }
            var literal = context.literal();
            if (literal != null)
            {
                return GetLiteralValue(literal, scope);
            }
            var constExp = context.const_exp();
            if (constExp != null)
            {
                return GetConstantExpValue(constExp, scope);
            }
            throw new IdlCompilerException("Cannot evaluate primary expression: " + context.GetText());
        }

        private Literal GetLiteralValue(IDLParser.LiteralContext context, Scope scope)
        {
            if (context.ChildCount > 0 && context.children[0] is ITerminalNode node)
            {
                switch (node.Symbol.Type)
                {
                    case IDLParser.OCTAL_LITERAL:
                    case IDLParser.HEX_LITERAL:
                    case IDLParser.INTEGER_LITERAL:
                        return new IntegerLiteral(node.Symbol.Text);
                    case IDLParser.STRING_LITERAL:
                        return new StringLiteral(node.Symbol.Text.Trim('"'), false);
                    case IDLParser.WIDE_STRING_LITERAL:
                        return new StringLiteral(node.Symbol.Text.Substring(1).Trim('"'), true);
                    case IDLParser.CHARACTER_LITERAL:
                        return new CharLiteral(node.Symbol.Text.Trim('\''), false);
                    case IDLParser.WIDE_CHARACTER_LITERAL:
                        return new CharLiteral(node.Symbol.Text.Substring(1).Trim('\''), true);
                    case IDLParser.FIXED_PT_LITERAL:
                    case IDLParser.FLOATING_PT_LITERAL:
                        return new FloatLiteral(node.Symbol.Text);
                    case IDLParser.BOOLEAN_LITERAL:
                        return new BooleanLiteral(node.Symbol.Text);
                }
            }
            throw new IdlCompilerException("Literal type not supported");
        }

        private List<Annotation> GetAnnotations(IDLParser.AnnappsContext annapps, Scope scope)
        {
            List<Annotation> result = new List<Annotation>();
            if (annapps != null && !annapps.IsEmpty)
            {
                var annotations = annapps.annotation_appl();
                foreach (var ann in annotations)
                {
                    var name = ann.scoped_name();
                    var a = new Annotation(name.GetText(), dotNetNaming);
                    var parameters = ann.annotation_appl_params();
                    if (parameters != null)
                    {
                        var constExp = parameters.const_exp();
                        if (constExp != null)
                        {
                            a.ConstantExpr = GetConstantExpValue(constExp, scope);
                        }
                        var paramList = parameters.annotation_appl_param();
                        if (paramList != null)
                        {
                            foreach (var param in paramList)
                            {
                                var id = param.ID().GetText();
                                var literal = GetConstantExpValue(param.const_exp(), scope);
                                a.Parameters.Add(id, literal);
                            }
                        }                        
                    }
                    result.Add(a);
                }
            };
            return result;
        }

        private Annotation GetAnnotation(string annotationName, List<Annotation> annotations)
        {
            return annotations.FirstOrDefault(a => a.Name.Equals(annotationName));
        }

        private void SetTypeInfo(IIDLSymbol symbol)
        {
            //if (repositoryIds.ContainsKey(symbol.FullName))
            //{
            //    symbol.TypeId = repositoryIds[symbol.FullName];
            //}
            //if (versions.ContainsKey(symbol.FullName))
            //{
            //    symbol.TypeVersion = versions[symbol.FullName];
            //}
            //if (prefixes.ContainsKey(symbol.FullName))
            //{
            //    symbol.TypePrefix = prefixes[symbol.FullName];
            //}
            if (prefix != null && String.IsNullOrEmpty(symbol.TypePrefix))
            {
                symbol.TypePrefix = prefix;
            }
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
            if (File.Exists(Path.Combine(CurrentFile.Directory.FullName, name)))
            {
                // search also in the same directory, then the file containing the include directive
                return new FileInfo(Path.Combine(CurrentFile.Directory.FullName, name));
            }
            return null;
        }
    }
}
