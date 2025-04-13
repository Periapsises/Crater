using Core.Antlr;

namespace Core.SyntaxTreeConverter.Statements;

public class ParameterDeclartion(string name, Expression dataTypeReference, bool nullable, FunctionParameterCtx context)
{
    public readonly string Name = name;
    public readonly Expression DataTypeReference = dataTypeReference;
    public readonly bool Nullable = nullable;
    public readonly FunctionParameterCtx Context = context;
}

public class FunctionDeclaration(bool isLocal, string identifier, List<ParameterDeclartion> parameters, Expression returnTypeReference, bool returnNullable, Block block, FunctionDeclarationCtx context): Statement()
{
    public readonly bool Local = isLocal;
    public readonly string Identifier = identifier;
    public readonly List<ParameterDeclartion> Parameters = parameters;
    public readonly Expression ReturnTypeReference = returnTypeReference;
    public readonly bool ReturnNullable = returnNullable;
    public readonly Block Block = block;
    public readonly FunctionDeclarationCtx Context = context;
}