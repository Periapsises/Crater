using Core.Antlr;

namespace Core.SyntaxTreeConverter.Statements;

public class ParameterDeclaration(string name, TypeReference dataTypeReference, FunctionParameterCtx context)
{
    public readonly string Name = name;
    public readonly TypeReference DataTypeReference = dataTypeReference;
    public readonly FunctionParameterCtx Context = context;
}

public class FunctionDeclaration(bool isLocal, string identifier, List<ParameterDeclaration> parameters, List<TypeReference> returns, Block block, FunctionDeclarationCtx context): Statement()
{
    public readonly bool Local = isLocal;
    public readonly string Identifier = identifier;
    public readonly List<ParameterDeclaration> Parameters = parameters;
    public readonly List<TypeReference> Returns = returns;
    public readonly Block Block = block;
    public readonly FunctionDeclarationCtx Context = context;
}