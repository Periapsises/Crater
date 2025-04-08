using Core.Antlr;

namespace Core.SyntaxTreeConverter.Statements;

public class ParameterDeclartion(string name, VariableReference reference, bool nullable, CraterParser.FunctionParameterContext context)
{
    public readonly string Name = name;
    public readonly VariableReference DataTypeReference = reference;
    public readonly bool Nullable = nullable;
    public readonly CraterParser.FunctionParameterContext Context = context;
}

public class FunctionDeclaration(bool isLocal, string identifier, List<ParameterDeclartion> parameters, VariableReference returnTypeReference, bool returnNullable, Block block, CraterParser.FunctionDeclarationContext context): Statement()
{
    public readonly bool Local = isLocal;
    public readonly string Identifier = identifier;
    public readonly List<ParameterDeclartion> Parameters = parameters;
    public readonly VariableReference ReturnTypeReference = returnTypeReference;
    public readonly bool ReturnNullable = returnNullable;
    public readonly Block Block = block;
    public readonly CraterParser.FunctionDeclarationContext Context = context;
}