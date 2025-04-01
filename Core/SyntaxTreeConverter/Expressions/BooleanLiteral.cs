namespace Core.SyntaxTreeConverter.Expressions;

public class BooleanLiteral(string literal, object context) : Expression(context)
{
    public readonly bool Value = Convert.ToBoolean(literal);
}