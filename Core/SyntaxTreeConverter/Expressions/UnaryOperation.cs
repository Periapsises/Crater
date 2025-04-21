namespace Core.SyntaxTreeConverter.Expressions;

public class UnaryOperation(Expression expression, string op, UnaryOperationCtx context) : Expression(context.GetText())
{
    public readonly UnaryOperationCtx Context = context;
    public readonly Expression Expression = expression;
    public readonly string Operator = op;
}