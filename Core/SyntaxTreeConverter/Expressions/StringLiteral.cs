using Antlr4.Runtime;
using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class StringLiteral(string literal, LiteralCtx context) : Expression(context.GetText())
{
    public readonly string Value = literal.Trim('"');
    public readonly LiteralCtx Context = context;
}