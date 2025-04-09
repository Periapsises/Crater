using Core.Antlr;

namespace Core.SyntaxTreeConverter.Statements;

public class ElseIfStatement(Expression condition, Block block, CraterParser.ElseIfStatementContext context) : Statement
{
    public readonly Expression Condition = condition;
    public readonly Block Block = block;
    
    public readonly CraterParser.ElseIfStatementContext Context = context;
}