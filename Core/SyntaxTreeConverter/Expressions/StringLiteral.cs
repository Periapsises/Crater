using Antlr4.Runtime;
using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class StringLiteral(string literal, CraterParser.LiteralContext context) : Expression()
{
    public readonly string Value = literal.Trim('"');
    public readonly CraterParser.LiteralContext Context = context;
}