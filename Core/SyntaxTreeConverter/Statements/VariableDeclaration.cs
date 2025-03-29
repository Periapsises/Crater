namespace Core.SyntaxTreeConverter.Statements;

public class VariableDeclaration(bool local, string identifier, DataTypeReference dataTypeReference, Expression? initializer = null) : Statement
{
    public readonly bool Local = local;
    public readonly string Identifier =  identifier;
    public readonly DataTypeReference DataTypeReference = dataTypeReference;
    public readonly Expression? Initializer = initializer;
}