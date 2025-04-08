using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class ParenthesizedExpression(Expression expression, CraterParser.ParenthesizedExpressionContext context) : Expression()
{
    public readonly Expression Expression = expression;
    public readonly CraterParser.ParenthesizedExpressionContext Context;
}