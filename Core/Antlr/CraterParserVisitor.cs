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
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="CraterParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.2")]
[System.CLSCompliant(false)]
public interface ICraterParserVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProgram([NotNull] CraterParser.ProgramContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBlock([NotNull] CraterParser.BlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] CraterParser.StatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.variableDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableDeclaration([NotNull] CraterParser.VariableDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.functionDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionDeclaration([NotNull] CraterParser.FunctionDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.functionParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionParameters([NotNull] CraterParser.FunctionParametersContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.functionParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionParameter([NotNull] CraterParser.FunctionParameterContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfStatement([NotNull] CraterParser.IfStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.elseIfStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElseIfStatement([NotNull] CraterParser.ElseIfStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.elseStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElseStatement([NotNull] CraterParser.ElseStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ParenthesizedExpression</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParenthesizedExpression([NotNull] CraterParser.ParenthesizedExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>LogicalOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLogicalOperation([NotNull] CraterParser.LogicalOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>OrOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOrOperation([NotNull] CraterParser.OrOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ConcatenationOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConcatenationOperation([NotNull] CraterParser.ConcatenationOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>UnaryOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnaryOperation([NotNull] CraterParser.UnaryOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>VariableReference</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableReference([NotNull] CraterParser.VariableReferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ExponentOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExponentOperation([NotNull] CraterParser.ExponentOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>LiteralExpression</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteralExpression([NotNull] CraterParser.LiteralExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>MultiplicativeOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultiplicativeOperation([NotNull] CraterParser.MultiplicativeOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>AndOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAndOperation([NotNull] CraterParser.AndOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>AdditiveOperation</c>
	/// labeled alternative in <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAdditiveOperation([NotNull] CraterParser.AdditiveOperationContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="CraterParser.literal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLiteral([NotNull] CraterParser.LiteralContext context);
}
} // namespace Core.Antlr
