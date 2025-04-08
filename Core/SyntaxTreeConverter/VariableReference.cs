using Core.Antlr;

namespace Core.SyntaxTreeConverter;

public class VariableReference(string name, string? fullString, CraterParser.VariableReferenceContext context) : Expression()
{
    public readonly string Name = name;
    public readonly string FullString = fullString ?? name;
    public readonly CraterParser.VariableReferenceContext Context = context;
}