using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class VariableReference(string name, VariableReferenceCtx context) : Expression(context.GetText())
{
    public readonly string Name = name;
    public readonly VariableReferenceCtx Context = context;
}