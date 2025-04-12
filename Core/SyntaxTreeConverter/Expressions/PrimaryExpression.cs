using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class PrimaryExpression(Expression prefixExpression, List<Expression> postfixExpressions, CraterParser.PrimaryExpressionContext context) : Expression
{
    public readonly Expression PrefixExpression = prefixExpression;
    public readonly List<Expression> PostfixExpressions = postfixExpressions;

    public readonly CraterParser.PrimaryExpressionContext Context = context;
}