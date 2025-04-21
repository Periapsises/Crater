namespace Core.SyntaxTreeConverter.Expressions;

public class StringLiteral(string literal, LiteralCtx context) : Expression(context.GetText())
{
    public readonly LiteralCtx Context = context;
    public readonly string Value = literal.Trim('"');
}