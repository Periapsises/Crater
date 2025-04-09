using Core.Antlr;

namespace Core.SyntaxTreeConverter.Statements;

public class IfStatement(Expression condition, Block block, List<ElseIfStatement> elseIfStatements, ElseStatement? elseStatement, CraterParser.IfStatementContext context) : Statement
{
    public readonly Expression Condition = condition;
    public readonly Block Block = block;
    
    public readonly List<ElseIfStatement> ElseIfStatements = elseIfStatements;
    public readonly ElseStatement? ElseStatement = elseStatement;
    
    public readonly CraterParser.IfStatementContext Context = context;
}