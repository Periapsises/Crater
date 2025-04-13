using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class UnaryOperation(Expression expression, string op, UnaryOperationCtx context): Expression(context.GetText())
{
    public readonly Expression Expression = expression;
    public readonly string Operator = op;
    public readonly UnaryOperationCtx Context = context;
}