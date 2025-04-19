using Core.Antlr;

namespace Core.SyntaxTreeConverter.Statements;

public class VariableDeclaration(bool local, string identifier, TypeReference dataTypeReference, Expression? initializer, VariableDeclarationCtx context) : Statement
{
    public readonly bool Local = local;
    public readonly string Identifier =  identifier;
    public readonly TypeReference DataTypeReference = dataTypeReference;
    public readonly Expression? Initializer = initializer;
    public readonly VariableDeclarationCtx Context = context;
}