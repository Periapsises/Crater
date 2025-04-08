using Antlr4.Runtime;
using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class BinaryOperationContext(object context)
{
    public CraterParser.ExpressionContext Left => GetExpression(0);
    public CraterParser.ExpressionContext Right => GetExpression(1);
    
    private CraterParser.ExpressionContext GetExpression(int id)
    {
        switch (context)
        {
            case CraterParser.ExponentOperationContext exponentOperation:
                return exponentOperation.expression()[id];
            case CraterParser.MultiplicativeOperationContext multiplicativeOperation:
                return multiplicativeOperation.expression()[id];
            case CraterParser.AdditiveOperationContext additiveOperation:
                return additiveOperation.expression()[id];
            case CraterParser.ConcatenationOperationContext concatenationoperation:
                return concatenationoperation.expression()[id];
            case CraterParser.LogicalOperationContext logicalOperation:
                return logicalOperation.expression()[id];
            default:
                throw new NotImplementedException($"Context {context} is not implemented");
        }
    }

    public IToken Op => GetOperator();
    
    private IToken GetOperator()
    {
        switch (context)
        {
            case CraterParser.ExponentOperationContext exponentOperation:
                return exponentOperation.EXP().Symbol;
            case CraterParser.MultiplicativeOperationContext multiplicativeOperation:
                return multiplicativeOperation.op;
            case CraterParser.AdditiveOperationContext additiveOperation:
                return additiveOperation.op;
            case CraterParser.ConcatenationOperationContext concatenationoperation:
                return concatenationoperation.CONCAT().Symbol;
            case CraterParser.LogicalOperationContext logicalOperation:
                return logicalOperation.op;
            default:
                throw new NotImplementedException($"Context {context} is not implemented");
        }
    }
}

public class BinaryOperation(Expression left, Expression right, string op, BinaryOperationContext context) : Expression
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
    public readonly BinaryOperationContext Context = context;
}

public class LogicalOperation(Expression left, Expression right, string op, CraterParser.LogicalOperationContext context) : Expression
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
    public readonly CraterParser.LogicalOperationContext Context = context;
}

public class AndOperation(Expression left, Expression right, string op, CraterParser.AndOperationContext context) : Expression
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
    public readonly CraterParser.AndOperationContext Context = context;
}

public class OrOperation(Expression left, Expression right, string op, CraterParser.OrOperationContext context) : Expression
{
    public readonly Expression Left = left;
    public readonly Expression Right = right;
    public readonly string Operator = op;
    public readonly CraterParser.OrOperationContext Context = context;
}