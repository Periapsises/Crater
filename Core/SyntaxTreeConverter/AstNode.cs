namespace Core.SyntaxTreeConverter;

public abstract class AstNode(object context)
{
    public readonly object Context = context;
}