namespace Core.SyntaxTreeConverter.Expressions;

public class NumberLiteral(string literal)
{
    public readonly double Value = double.Parse(literal);
}