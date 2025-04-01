namespace Core.SyntaxTreeConverter.Expressions;

public class ParenthesizedExpression(Expression expression, object context) : Expression(context)
{
    public readonly Expression Expression = expression;
}