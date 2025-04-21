namespace Core.SyntaxTreeConverter.Expressions;

public class PrimaryExpression(
    Expression prefixExpression,
    List<Expression> postfixExpressions,
    PrimaryExpressionCtx context) : Expression(context.GetText())
{
    public readonly PrimaryExpressionCtx Context = context;
    public readonly List<Expression> PostfixExpressions = postfixExpressions;
    public readonly Expression PrefixExpression = prefixExpression;
}