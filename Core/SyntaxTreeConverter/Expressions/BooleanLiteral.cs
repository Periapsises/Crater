namespace Core.SyntaxTreeConverter.Expressions;

public class BooleanLiteral(string literal)
{
    public readonly bool Value = Convert.ToBoolean(literal);
}