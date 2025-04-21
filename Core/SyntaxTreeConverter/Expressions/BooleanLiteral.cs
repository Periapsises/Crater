namespace Core.SyntaxTreeConverter.Expressions;

public class BooleanLiteral(string literal, LiteralCtx context) : Expression(context.GetText())
{
    public readonly LiteralCtx Context = context;
    public readonly bool Value = Convert.ToBoolean(literal);
}