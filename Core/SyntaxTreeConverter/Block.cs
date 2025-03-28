namespace Core.SyntaxTreeConverter;

public class Block(List<Statement> statements)
{
    public readonly List<Statement> Statements = statements;
}