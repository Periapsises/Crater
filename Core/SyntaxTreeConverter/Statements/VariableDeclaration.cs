namespace Core.SyntaxTreeConverter.Statements;

public class VariableDeclaration(
    bool local,
    string identifier,
    TypeReference dataTypeReference,
    Expression? initializer,
    VariableDeclarationCtx context) : Statement
{
    public readonly VariableDeclarationCtx Context = context;
    public readonly TypeReference DataTypeReference = dataTypeReference;
    public readonly string Identifier = identifier;
    public readonly Expression? Initializer = initializer;
    public readonly bool Local = local;
}