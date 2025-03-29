namespace Core.SyntaxTreeConverter.Expressions;

public class NumberLiteral(string literal) : Expression
{
    public readonly double Value = double.Parse(literal);
}