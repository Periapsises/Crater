using Antlr4.Runtime;

namespace Core.SyntaxTreeConverter.Expressions;

public class BinaryOperation(Expression left, Expression right, string op, ParserRuleContext context)
    : Expression(context.GetText())
{
    public readonly ParserRuleContext Context = context;
    public readonly Expression Left = left;
    public readonly string Operator = op;
    public readonly Expression Right = right;
}

public class LogicalOperation(Expression left, Expression right, string op, LogicalOperationCtx context)
    : Expression(context.GetText())
{
    public readonly LogicalOperationCtx Context = context;
    public readonly Expression Left = left;
    public readonly string Operator = op;
    public readonly Expression Right = right;
}

public class AndOperation(Expression left, Expression right, string op, AndOperationCtx context)
    : Expression(context.GetText())
{
    public readonly AndOperationCtx Context = context;
    public readonly Expression Left = left;
    public readonly string Operator = op;
    public readonly Expression Right = right;
}

public class OrOperation(Expression left, Expression right, string op, OrOperationCtx context)
    : Expression(context.GetText())
{
    public readonly OrOperationCtx Context = context;
    public readonly Expression Left = left;
    public readonly string Operator = op;
    public readonly Expression Right = right;
}