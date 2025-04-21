namespace Core.SyntaxTreeConverter.Statements;

public class IfStatement(
    Expression condition,
    Block block,
    List<ElseIfStatement> elseIfStatements,
    ElseStatement? elseStatement,
    IfStatementCtx context) : Statement
{
    public readonly Block Block = block;
    public readonly Expression Condition = condition;

    public readonly IfStatementCtx Context = context;

    public readonly List<ElseIfStatement> ElseIfStatements = elseIfStatements;
    public readonly ElseStatement? ElseStatement = elseStatement;
}