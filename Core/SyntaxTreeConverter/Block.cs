using Core.Antlr;

namespace Core.SyntaxTreeConverter;

public class Block(List<Statement> statements, BlockCtx context)
{
    public readonly List<Statement> Statements = statements;
    public readonly BlockCtx Context = context;
}