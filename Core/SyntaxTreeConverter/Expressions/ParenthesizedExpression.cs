namespace Core.SyntaxTreeConverter.Expressions;

public class ParenthesizedExpression(Expression expression) : Expression
{
    public readonly Expression Expression = expression;
}