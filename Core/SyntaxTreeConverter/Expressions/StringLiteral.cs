namespace Core.SyntaxTreeConverter.Expressions;

public class StringLiteral(string literal) : Expression
{
    public readonly string Value = literal.Trim('"');
}