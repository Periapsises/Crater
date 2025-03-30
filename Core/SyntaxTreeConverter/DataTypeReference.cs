namespace Core.SyntaxTreeConverter;

public class DataTypeReference(string name, string? fullString = null)
{
    public readonly string Name = name;
    public readonly string FullString = fullString ?? name;
}