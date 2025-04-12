using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class DotIndex(string index, CraterParser.DotIndexingContext context) : Expression
{
    public readonly string Index = index;
    public readonly CraterParser.DotIndexingContext Context = context;
}

public class BracketIndex(Expression index, CraterParser.BracketIndexingContext context) : Expression
{
    public readonly Expression Index = index;
    public readonly CraterParser.BracketIndexingContext Context = context;
}