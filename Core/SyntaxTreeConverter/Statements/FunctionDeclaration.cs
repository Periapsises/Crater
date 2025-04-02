namespace Core.SyntaxTreeConverter.Statements;

public class ParameterDeclartion(string name, VariableReference reference, bool nullable, object context) : AstNode(context)
{
    public readonly string Name = name;
    public readonly VariableReference DataTypeReference = reference;
    public readonly bool Nullable = nullable;
}

public class FunctionDeclaration(bool isLocal, string identifier, List<ParameterDeclartion> parameters, VariableReference returnTypeReference, bool returnNullable, Block block, object context): Statement(context)
{
    public readonly bool Local = isLocal;
    public readonly string Identifier = identifier;
    public readonly List<ParameterDeclartion> Parameters = parameters;
    public readonly VariableReference ReturnTypeReference = returnTypeReference;
    public readonly bool ReturnNullable = returnNullable;
    public readonly Block Block = block;
}