using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class PrimaryExpression(Expression prefixExpression, List<Expression> postfixExpressions, PrimaryExpressionCtx context) : Expression(context.GetText())
{
    public readonly Expression PrefixExpression = prefixExpression;
    public readonly List<Expression> PostfixExpressions = postfixExpressions;

    public readonly PrimaryExpressionCtx Context = context;
}