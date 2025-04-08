using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class BooleanLiteral(string literal, CraterParser.LiteralContext context) : Expression
{
    public readonly bool Value = Convert.ToBoolean(literal);
    public readonly CraterParser.LiteralContext Context = context;
}