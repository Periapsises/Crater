using Antlr4.Runtime;
using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class BinaryOperationContext(object context)
{
    public ExpressionCtx Left => GetExpression(0);
    public ExpressionCtx Right => GetExpression(1);
    
    private ExpressionCtx GetExpression(int id)
    {
        switch (context)
        {
            case ExponentOperationCtx exponentOperation:
                return exponentOperation.expression()[id];
            case MultiplicativeOperationCtx multiplicativeOperation:
                return multiplicativeOperation.expression()[id];
            case AdditiveOperationCtx additiveOperation:
                return additiveOperation.expression()[id];
            case ConcatenationOperationCtx concatenationoperation:
                return concatenationoperation.expression()[id];
            case LogicalOperationCtx logicalOperation:
                return logicalOperation.expression()[id];
            default:
                throw new NotImplementedException($"Context {context} is not implemented");
        }
    }

    public string GetFullString()
    {
        switch (context)
        {
            case ExponentOperationCtx exponentOperation:
                return exponentOperation.GetText();
            case MultiplicativeOperationCtx multiplicativeOperation:
                return multiplicativeOperation.GetText();
            case AdditiveOperationCtx additiveOperation:
                return additiveOperation.GetText();
            case ConcatenationOperationCtx concatenationoperation:
                return concatenationoperation.GetText();
            case LogicalOperationCtx logicalOperation:
                return logicalOperation.GetText();
            default:
                throw new NotImplementedException($"Context {context} is not implemented");
        }
    }
    
    public IToken Op => GetOperator();
    
    private IToken GetOperator()
    {
        switch (context)
        {
            case ExponentOperationCtx exponentOperation:
                return exponentOperation.EXP().Symbol;
            case MultiplicativeOperationCtx multiplicativeOperation:
                return multiplicativeOperation.op;
            case AdditiveOperationCtx additiveOperation:
                return additiveOperation.op;
            case ConcatenationOperationCtx concatenationoperation:
                return concatenationoperation.CONCAT().Symbol;
            case LogicalOperationCtx logicalOperation:
                return logicalOperation.op;
            default:
                throw new NotImplementedException($"Context {context} is not implemented");
        }
    }
}

public class BinaryOperation(Expression left, Expression right, string op, BinaryOperationContext context) : Expression(context.GetFullString())
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
    public readonly BinaryOperationContext Context = context;
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