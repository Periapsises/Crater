using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class ParenthesizedExpression(Expression expression, ParenthesizedExpressionCtx context) : Expression(context.GetText())
{
    public readonly Expression Expression = expression;
    public readonly ParenthesizedExpressionCtx Context;
}