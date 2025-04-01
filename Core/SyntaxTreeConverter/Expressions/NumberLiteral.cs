namespace Core.SyntaxTreeConverter.Expressions;

public class NumberLiteral(string literal, object context) : Expression(context)
{
    public readonly double Value = double.Parse(literal);
}