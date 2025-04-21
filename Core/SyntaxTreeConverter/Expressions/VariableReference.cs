namespace Core.SyntaxTreeConverter.Expressions;

public class VariableReference(string name, VariableReferenceCtx context) : Expression(context.GetText())
{
    public readonly VariableReferenceCtx Context = context;
    public readonly string Name = name;
}