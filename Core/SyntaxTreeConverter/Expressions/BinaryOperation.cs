using Antlr4.Runtime;

namespace Core.SyntaxTreeConverter.Expressions;

public class BinaryOperation(Expression left, Expression right, string op, ParserRuleContext context) : Expression(context.GetText())
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
    public readonly ParserRuleContext Context = context;
}

public class LogicalOperation(Expression left, Expression right, string op, LogicalOperationCtx context) : Expression(context.GetText())
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
    public readonly LogicalOperationCtx Context = context;
}

public class AndOperation(Expression left, Expression right, string op, AndOperationCtx context) : Expression(context.GetText())
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
    public readonly AndOperationCtx Context = context;
}

public class OrOperation(Expression left, Expression right, string op, OrOperationCtx context) : Expression(context.GetText())
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
    public readonly OrOperationCtx Context = context;
}