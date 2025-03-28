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
	/// Enter a parse tree produced by <see cref="CraterParser.typeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeName([NotNull] CraterParser.TypeNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.typeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeName([NotNull] CraterParser.TypeNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] CraterParser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="CraterParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] CraterParser.ExpressionContext context);
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
