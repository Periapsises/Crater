//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/yannk/Desktop/Crater/Core/CraterParser.g4 by ANTLR 4.13.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Core.Antlr {

using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ICraterParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class CraterParserBaseListener : ICraterParserListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterProgram([NotNull] CraterParser.ProgramContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitProgram([NotNull] CraterParser.ProgramContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlock([NotNull] CraterParser.BlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlock([NotNull] CraterParser.BlockContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] CraterParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] CraterParser.StatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.variableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableDeclaration([NotNull] CraterParser.VariableDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.variableDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableDeclaration([NotNull] CraterParser.VariableDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.functionDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionDeclaration([NotNull] CraterParser.FunctionDeclarationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.functionDeclaration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionDeclaration([NotNull] CraterParser.FunctionDeclarationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.functionParameters"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionParameters([NotNull] CraterParser.FunctionParametersContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.functionParameters"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionParameters([NotNull] CraterParser.FunctionParametersContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.functionParameter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionParameter([NotNull] CraterParser.FunctionParameterContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.functionParameter"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionParameter([NotNull] CraterParser.FunctionParameterContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfStatement([NotNull] CraterParser.IfStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfStatement([NotNull] CraterParser.IfStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.elseIfStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterElseIfStatement([NotNull] CraterParser.ElseIfStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.elseIfStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitElseIfStatement([NotNull] CraterParser.ElseIfStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.elseStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterElseStatement([NotNull] CraterParser.ElseStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.elseStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitElseStatement([NotNull] CraterParser.ElseStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>LogicalOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLogicalOperation([NotNull] CraterParser.LogicalOperationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>LogicalOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLogicalOperation([NotNull] CraterParser.LogicalOperationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>OrOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOrOperation([NotNull] CraterParser.OrOperationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>OrOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOrOperation([NotNull] CraterParser.OrOperationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ConcatenationOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterConcatenationOperation([NotNull] CraterParser.ConcatenationOperationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ConcatenationOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitConcatenationOperation([NotNull] CraterParser.ConcatenationOperationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>UnaryOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterUnaryOperation([NotNull] CraterParser.UnaryOperationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>UnaryOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitUnaryOperation([NotNull] CraterParser.UnaryOperationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ExponentOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExponentOperation([NotNull] CraterParser.ExponentOperationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ExponentOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExponentOperation([NotNull] CraterParser.ExponentOperationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>PrefixExpression</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrefixExpression([NotNull] CraterParser.PrefixExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>PrefixExpression</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrefixExpression([NotNull] CraterParser.PrefixExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>LiteralExpression</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLiteralExpression([NotNull] CraterParser.LiteralExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>LiteralExpression</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLiteralExpression([NotNull] CraterParser.LiteralExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>MultiplicativeOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterMultiplicativeOperation([NotNull] CraterParser.MultiplicativeOperationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>MultiplicativeOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitMultiplicativeOperation([NotNull] CraterParser.MultiplicativeOperationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>AndOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAndOperation([NotNull] CraterParser.AndOperationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>AndOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAndOperation([NotNull] CraterParser.AndOperationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>AdditiveOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAdditiveOperation([NotNull] CraterParser.AdditiveOperationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>AdditiveOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAdditiveOperation([NotNull] CraterParser.AdditiveOperationContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>ParenthesizedExpression</c>
	/// labeled alternative in <see cref="CraterParser.primaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParenthesizedExpression([NotNull] CraterParser.ParenthesizedExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>ParenthesizedExpression</c>
	/// labeled alternative in <see cref="CraterParser.primaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParenthesizedExpression([NotNull] CraterParser.ParenthesizedExpressionContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>DotIndexing</c>
	/// labeled alternative in <see cref="CraterParser.primaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterDotIndexing([NotNull] CraterParser.DotIndexingContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>DotIndexing</c>
	/// labeled alternative in <see cref="CraterParser.primaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitDotIndexing([NotNull] CraterParser.DotIndexingContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>VariableReference</c>
	/// labeled alternative in <see cref="CraterParser.primaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableReference([NotNull] CraterParser.VariableReferenceContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>VariableReference</c>
	/// labeled alternative in <see cref="CraterParser.primaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableReference([NotNull] CraterParser.VariableReferenceContext context) { }
	/// <summary>
	/// Enter a parse tree produced by the <c>BracketIndexing</c>
	/// labeled alternative in <see cref="CraterParser.primaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBracketIndexing([NotNull] CraterParser.BracketIndexingContext context) { }
	/// <summary>
	/// Exit a parse tree produced by the <c>BracketIndexing</c>
	/// labeled alternative in <see cref="CraterParser.primaryExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBracketIndexing([NotNull] CraterParser.BracketIndexingContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLiteral([NotNull] CraterParser.LiteralContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.literal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLiteral([NotNull] CraterParser.LiteralContext context) { }

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
} // namespace Core.Antlr
