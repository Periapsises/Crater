using Core.Antlr;

namespace Core.SyntaxTreeConverter.Statements;

public class FunctionCallStatement(Expression primaryExpression, List<Expression> arguments, CraterParser.FunctionCallStatementContext context) : Statement
{
    public readonly Expression PrimaryExpression = primaryExpression;
    public readonly List<Expression> Arguments = arguments;
    
    public readonly CraterParser.FunctionCallStatementContext Context = context;
}