namespace Core.SyntaxTreeConverter.Expressions;

public class BinaryOperation(Expression left, Expression right, string op, object context) : Expression(context)
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
}