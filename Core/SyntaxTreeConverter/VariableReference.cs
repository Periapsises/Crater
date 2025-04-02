namespace Core.SyntaxTreeConverter;

public class VariableReference(string name, string? fullString, object context) : Expression(context)
{
    public readonly string Name = name;
    public readonly string FullString = fullString ?? name;
}