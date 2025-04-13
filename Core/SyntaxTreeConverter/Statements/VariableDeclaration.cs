using Core.Antlr;

namespace Core.SyntaxTreeConverter.Statements;

public class VariableDeclaration(bool local, string identifier, Expression dataTypeReference, bool nullable, Expression? initializer, VariableDeclarationCtx context) : Statement
{
    public readonly bool Local = local;
    public readonly string Identifier =  identifier;
    public readonly Expression DataTypeReference = dataTypeReference;
    public readonly bool Nullable = nullable;
    public readonly Expression? Initializer = initializer;
    public readonly VariableDeclarationCtx Context = context;
}