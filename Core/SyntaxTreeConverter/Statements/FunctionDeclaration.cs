namespace Core.SyntaxTreeConverter.Statements;

public class ParameterDeclaration(string name, TypeReference dataTypeReference, FunctionParameterCtx context)
{
    public readonly FunctionParameterCtx Context = context;
    public readonly TypeReference DataTypeReference = dataTypeReference;
    public readonly string Name = name;
}

public class FunctionDeclaration(
    bool isLocal,
    string identifier,
    List<ParameterDeclaration> parameters,
    List<TypeReference> returns,
    Block block,
    FunctionDeclarationCtx context) : Statement
{
    public readonly Block Block = block;
    public readonly FunctionDeclarationCtx Context = context;
    public readonly string Identifier = identifier;
    public readonly bool Local = isLocal;
    public readonly List<ParameterDeclaration> Parameters = parameters;
    public readonly List<TypeReference> Returns = returns;
}