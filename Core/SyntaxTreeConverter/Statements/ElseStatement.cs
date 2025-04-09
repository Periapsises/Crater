using Core.Antlr;

namespace Core.SyntaxTreeConverter.Statements;

public class ElseStatement(Block block, CraterParser.ElseStatementContext context) : Statement
{
    public readonly Block Block = block;
    
    public readonly CraterParser.ElseStatementContext Context = context;
}