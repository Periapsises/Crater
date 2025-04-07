using Antlr4.Runtime;

namespace Core.SyntaxTreeConverter.Expressions;

public class LogicalOperation(Expression left, Expression right, string op, object context, IToken opToken) : Expression(context)
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
    public readonly IToken OpToken = opToken;
}