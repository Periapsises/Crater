namespace Core.SyntaxTreeConverter.Statements;

public class VariableDeclaration(bool local, string identifier, VariableReference variableReference, bool nullable, Expression? initializer, object context) : Statement(context)
{
    public readonly bool Local = local;
    public readonly string Identifier =  identifier;
    public readonly VariableReference VariableReference = variableReference;
    public readonly bool Nullable = nullable;
    public readonly Expression? Initializer = initializer;
}