namespace Core.SyntaxTreeConverter.Expressions;

public class StringLiteral(string literal, object context) : Expression(context)
{
    public readonly string Value = literal.Trim('"');
}