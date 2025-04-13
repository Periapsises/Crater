namespace Core.SyntaxTreeConverter;

public abstract class Expression(string fullString)
{
    public readonly string FullString = fullString;
}