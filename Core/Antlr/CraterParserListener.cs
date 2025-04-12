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
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="CraterParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface ICraterParserListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProgram([NotNull] CraterParser.ProgramContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProgram([NotNull] CraterParser.ProgramContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlock([NotNull] CraterParser.BlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlock([NotNull] CraterParser.BlockContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] CraterParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] CraterParser.StatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.variableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableDeclaration([NotNull] CraterParser.VariableDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.variableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableDeclaration([NotNull] CraterParser.VariableDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.functionDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionDeclaration([NotNull] CraterParser.FunctionDeclarationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.functionDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionDeclaration([NotNull] CraterParser.FunctionDeclarationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.functionParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionParameters([NotNull] CraterParser.FunctionParametersContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.functionParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionParameters([NotNull] CraterParser.FunctionParametersContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.functionParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionParameter([NotNull] CraterParser.FunctionParameterContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.functionParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionParameter([NotNull] CraterParser.FunctionParameterContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfStatement([NotNull] CraterParser.IfStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfStatement([NotNull] CraterParser.IfStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.elseIfStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterElseIfStatement([NotNull] CraterParser.ElseIfStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.elseIfStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitElseIfStatement([NotNull] CraterParser.ElseIfStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.elseStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterElseStatement([NotNull] CraterParser.ElseStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.elseStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitElseStatement([NotNull] CraterParser.ElseStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.functionCallStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionCallStatement([NotNull] CraterParser.FunctionCallStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.functionCallStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionCallStatement([NotNull] CraterParser.FunctionCallStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.functionArguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionArguments([NotNull] CraterParser.FunctionArgumentsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.functionArguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionArguments([NotNull] CraterParser.FunctionArgumentsContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>LogicalOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLogicalOperation([NotNull] CraterParser.LogicalOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>LogicalOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLogicalOperation([NotNull] CraterParser.LogicalOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>OrOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOrOperation([NotNull] CraterParser.OrOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>OrOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOrOperation([NotNull] CraterParser.OrOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>BaseExpression</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBaseExpression([NotNull] CraterParser.BaseExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>BaseExpression</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBaseExpression([NotNull] CraterParser.BaseExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ConcatenationOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConcatenationOperation([NotNull] CraterParser.ConcatenationOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ConcatenationOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConcatenationOperation([NotNull] CraterParser.ConcatenationOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>UnaryOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUnaryOperation([NotNull] CraterParser.UnaryOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>UnaryOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUnaryOperation([NotNull] CraterParser.UnaryOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ExponentOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExponentOperation([NotNull] CraterParser.ExponentOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ExponentOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExponentOperation([NotNull] CraterParser.ExponentOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>LiteralExpression</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteralExpression([NotNull] CraterParser.LiteralExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>LiteralExpression</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteralExpression([NotNull] CraterParser.LiteralExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>MultiplicativeOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMultiplicativeOperation([NotNull] CraterParser.MultiplicativeOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>MultiplicativeOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMultiplicativeOperation([NotNull] CraterParser.MultiplicativeOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>AndOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAndOperation([NotNull] CraterParser.AndOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>AndOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAndOperation([NotNull] CraterParser.AndOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>AdditiveOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAdditiveOperation([NotNull] CraterParser.AdditiveOperationContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>AdditiveOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAdditiveOperation([NotNull] CraterParser.AdditiveOperationContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.primaryExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrimaryExpression([NotNull] CraterParser.PrimaryExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.primaryExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrimaryExpression([NotNull] CraterParser.PrimaryExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ParenthesizedExpression</c>
	/// labeled alternative in <see cref="CraterParser.prefixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenthesizedExpression([NotNull] CraterParser.ParenthesizedExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ParenthesizedExpression</c>
	/// labeled alternative in <see cref="CraterParser.prefixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenthesizedExpression([NotNull] CraterParser.ParenthesizedExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>VariableReference</c>
	/// labeled alternative in <see cref="CraterParser.prefixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableReference([NotNull] CraterParser.VariableReferenceContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>VariableReference</c>
	/// labeled alternative in <see cref="CraterParser.prefixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableReference([NotNull] CraterParser.VariableReferenceContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>DotIndexing</c>
	/// labeled alternative in <see cref="CraterParser.postfixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDotIndexing([NotNull] CraterParser.DotIndexingContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>DotIndexing</c>
	/// labeled alternative in <see cref="CraterParser.postfixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDotIndexing([NotNull] CraterParser.DotIndexingContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>BracketIndexing</c>
	/// labeled alternative in <see cref="CraterParser.postfixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBracketIndexing([NotNull] CraterParser.BracketIndexingContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>BracketIndexing</c>
	/// labeled alternative in <see cref="CraterParser.postfixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBracketIndexing([NotNull] CraterParser.BracketIndexingContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>FunctionCall</c>
	/// labeled alternative in <see cref="CraterParser.postfixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionCall([NotNull] CraterParser.FunctionCallContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>FunctionCall</c>
	/// labeled alternative in <see cref="CraterParser.postfixExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionCall([NotNull] CraterParser.FunctionCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLiteral([NotNull] CraterParser.LiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLiteral([NotNull] CraterParser.LiteralContext context);
}
} // namespace Core.Antlr
