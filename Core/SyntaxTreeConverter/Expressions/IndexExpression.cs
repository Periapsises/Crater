namespace Core.SyntaxTreeConverter.Expressions;

public class DotIndex(string index, DotIndexingCtx context) : Expression(context.GetText())
{
    public readonly DotIndexingCtx Context = context;
    public readonly string Index = index;
}

public class BracketIndex(Expression index, BracketIndexingCtx context) : Expression(context.GetText())
{
    public readonly BracketIndexingCtx Context = context;
    public readonly Expression Index = index;
}