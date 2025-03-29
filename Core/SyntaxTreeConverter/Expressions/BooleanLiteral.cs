namespace Core.SyntaxTreeConverter.Expressions;

public class BooleanLiteral(string literal) : Expression
{
    public readonly bool Value = Convert.ToBoolean(literal);
}