namespace Core.SyntaxTreeConverter.Expressions;

public class StringLiteral(string literal)
{
    public readonly string Value = literal.Trim('"');
}