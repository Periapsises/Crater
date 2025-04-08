using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class UnaryOperation(Expression expression, string op, CraterParser.UnaryOperationContext context): Expression()
{
    public Expression Expression = expression;
    public string Operator = op;
    public CraterParser.UnaryOperationContext Context = context;
}