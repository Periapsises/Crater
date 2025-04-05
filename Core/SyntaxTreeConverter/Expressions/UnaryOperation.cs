namespace Core.SyntaxTreeConverter.Expressions;

public class UnaryOperation(Expression expression, string op, object context): Expression(context)
{
    public Expression Expression = expression;
    public string Operator = op;
}