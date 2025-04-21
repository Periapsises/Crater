namespace Core.SyntaxTreeConverter.Statements;

public class ElseStatement(Block block, ElseStatementCtx context) : Statement
{
    public readonly Block Block = block;

    public readonly ElseStatementCtx Context = context;
}