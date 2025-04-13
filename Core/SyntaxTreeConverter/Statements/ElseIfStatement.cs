using Core.Antlr;

namespace Core.SyntaxTreeConverter.Statements;

public class ElseIfStatement(Expression condition, Block block, ElseIfStatementCtx context) : Statement
{
    public readonly Expression Condition = condition;
    public readonly Block Block = block;
    
    public readonly ElseIfStatementCtx Context = context;
}