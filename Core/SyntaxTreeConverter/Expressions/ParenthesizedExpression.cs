namespace Core.SyntaxTreeConverter.Expressions;

public class ParenthesizedExpression(Expression expression, ParenthesizedExpressionCtx context)
    : Expression(context.GetText())
{
    public readonly ParenthesizedExpressionCtx Context;
    public readonly Expression Expression = expression;
}