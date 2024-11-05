//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Z:/DotNetOrb/src/idlcompiler/IDL.Grammar/IDLPreprocessorParser.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace DotNetOrb.IdlCompiler {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IIDLPreprocessorParserVisitor{Result}"/>,
/// which can be extended to create a visitor which only needs to handle a subset
/// of the available methods.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class IDLPreprocessorParserBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, IIDLPreprocessorParserVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="IDLPreprocessorParser.idlDocument"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitIdlDocument([NotNull] IDLPreprocessorParser.IdlDocumentContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="IDLPreprocessorParser.text"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitText([NotNull] IDLPreprocessorParser.TextContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="IDLPreprocessorParser.code"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitCode([NotNull] IDLPreprocessorParser.CodeContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorInclude</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.include"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorInclude([NotNull] IDLPreprocessorParser.PreprocessorIncludeContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorPragmaPrefix</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.pragma"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorPragmaPrefix([NotNull] IDLPreprocessorParser.PreprocessorPragmaPrefixContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorPragma</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.pragma"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorPragma([NotNull] IDLPreprocessorParser.PreprocessorPragmaContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorConditional</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorConditional([NotNull] IDLPreprocessorParser.PreprocessorConditionalContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorDef</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorDef([NotNull] IDLPreprocessorParser.PreprocessorDefContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorError</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorError([NotNull] IDLPreprocessorParser.PreprocessorErrorContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorDefine</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorDefine([NotNull] IDLPreprocessorParser.PreprocessorDefineContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by <see cref="IDLPreprocessorParser.directive_text"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitDirective_text([NotNull] IDLPreprocessorParser.Directive_textContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorParenthesis</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorParenthesis([NotNull] IDLPreprocessorParser.PreprocessorParenthesisContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorNot</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorNot([NotNull] IDLPreprocessorParser.PreprocessorNotContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorBinary</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorBinary([NotNull] IDLPreprocessorParser.PreprocessorBinaryContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorConstant</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorConstant([NotNull] IDLPreprocessorParser.PreprocessorConstantContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorConditionalSymbol</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorConditionalSymbol([NotNull] IDLPreprocessorParser.PreprocessorConditionalSymbolContext context) { return VisitChildren(context); }
	/// <summary>
	/// Visit a parse tree produced by the <c>preprocessorDefined</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	public virtual Result VisitPreprocessorDefined([NotNull] IDLPreprocessorParser.PreprocessorDefinedContext context) { return VisitChildren(context); }
}
} // namespace DotNetOrb.IdlCompiler
