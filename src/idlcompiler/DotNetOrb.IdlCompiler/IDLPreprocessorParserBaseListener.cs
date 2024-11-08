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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IIDLPreprocessorParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class IDLPreprocessorParserBaseListener : IIDLPreprocessorParserListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="IDLPreprocessorParser.idlDocument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIdlDocument([NotNull] IDLPreprocessorParser.IdlDocumentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="IDLPreprocessorParser.idlDocument"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIdlDocument([NotNull] IDLPreprocessorParser.IdlDocumentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="IDLPreprocessorParser.text"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterText([NotNull] IDLPreprocessorParser.TextContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="IDLPreprocessorParser.text"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitText([NotNull] IDLPreprocessorParser.TextContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="IDLPreprocessorParser.code"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCode([NotNull] IDLPreprocessorParser.CodeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="IDLPreprocessorParser.code"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCode([NotNull] IDLPreprocessorParser.CodeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorInclude</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.include"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorInclude([NotNull] IDLPreprocessorParser.PreprocessorIncludeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorInclude</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.include"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorInclude([NotNull] IDLPreprocessorParser.PreprocessorIncludeContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorPragmaPrefix</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.pragma"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorPragmaPrefix([NotNull] IDLPreprocessorParser.PreprocessorPragmaPrefixContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorPragmaPrefix</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.pragma"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorPragmaPrefix([NotNull] IDLPreprocessorParser.PreprocessorPragmaPrefixContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorPragma</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.pragma"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorPragma([NotNull] IDLPreprocessorParser.PreprocessorPragmaContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorPragma</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.pragma"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorPragma([NotNull] IDLPreprocessorParser.PreprocessorPragmaContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorConditional</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorConditional([NotNull] IDLPreprocessorParser.PreprocessorConditionalContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorConditional</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorConditional([NotNull] IDLPreprocessorParser.PreprocessorConditionalContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorDef</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorDef([NotNull] IDLPreprocessorParser.PreprocessorDefContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorDef</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorDef([NotNull] IDLPreprocessorParser.PreprocessorDefContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorError</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorError([NotNull] IDLPreprocessorParser.PreprocessorErrorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorError</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorError([NotNull] IDLPreprocessorParser.PreprocessorErrorContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorDefine</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorDefine([NotNull] IDLPreprocessorParser.PreprocessorDefineContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorDefine</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.directive"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorDefine([NotNull] IDLPreprocessorParser.PreprocessorDefineContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="IDLPreprocessorParser.directive_text"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDirective_text([NotNull] IDLPreprocessorParser.Directive_textContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="IDLPreprocessorParser.directive_text"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDirective_text([NotNull] IDLPreprocessorParser.Directive_textContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorParenthesis</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorParenthesis([NotNull] IDLPreprocessorParser.PreprocessorParenthesisContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorParenthesis</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorParenthesis([NotNull] IDLPreprocessorParser.PreprocessorParenthesisContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorNot</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorNot([NotNull] IDLPreprocessorParser.PreprocessorNotContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorNot</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorNot([NotNull] IDLPreprocessorParser.PreprocessorNotContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorBinary</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorBinary([NotNull] IDLPreprocessorParser.PreprocessorBinaryContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorBinary</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorBinary([NotNull] IDLPreprocessorParser.PreprocessorBinaryContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorConstant</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorConstant([NotNull] IDLPreprocessorParser.PreprocessorConstantContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorConstant</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorConstant([NotNull] IDLPreprocessorParser.PreprocessorConstantContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorConditionalSymbol</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorConditionalSymbol([NotNull] IDLPreprocessorParser.PreprocessorConditionalSymbolContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorConditionalSymbol</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorConditionalSymbol([NotNull] IDLPreprocessorParser.PreprocessorConditionalSymbolContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>preprocessorDefined</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPreprocessorDefined([NotNull] IDLPreprocessorParser.PreprocessorDefinedContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>preprocessorDefined</c>
	/// labeled alternative in <see cref="IDLPreprocessorParser.preprocessor_expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPreprocessorDefined([NotNull] IDLPreprocessorParser.PreprocessorDefinedContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace DotNetOrb.IdlCompiler
