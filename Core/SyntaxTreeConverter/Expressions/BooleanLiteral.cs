using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class BooleanLiteral(string literal, LiteralCtx context) : Expression(context.GetText())
{
    public readonly bool Value = Convert.ToBoolean(literal);
    public readonly LiteralCtx Context = context;
}