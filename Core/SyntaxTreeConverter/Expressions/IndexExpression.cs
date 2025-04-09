using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class DotIndex(Expression expression, string index, CraterParser.DotIndexingContext context) : Expression
{
    public readonly Expression Expression = expression;
    public readonly string Index = index;

    public readonly CraterParser.DotIndexingContext Context = context;
}

public class BracketIndex(Expression expression, Expression index, CraterParser.BracketIndexingContext context) : Expression
{
    public readonly Expression Expression = expression;
    public readonly Expression Index = index;
    
    public readonly CraterParser.BracketIndexingContext Context = context;
}