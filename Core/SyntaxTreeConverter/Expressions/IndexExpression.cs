using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class DotIndex(string index, DotIndexingCtx context) : Expression(context.GetText())
{
    public readonly string Index = index;
    public readonly DotIndexingCtx Context = context;
}

public class BracketIndex(Expression index, BracketIndexingCtx context) : Expression(context.GetText())
{
    public readonly Expression Index = index;
    public readonly BracketIndexingCtx Context = context;
}