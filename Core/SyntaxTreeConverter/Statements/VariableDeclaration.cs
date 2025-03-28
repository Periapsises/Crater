namespace Core.SyntaxTreeConverter.Statements;

public class VariableDeclaration(bool local, string identifier, DataTypeReference dataTypeReference, Expression? initializer = null) : Statement
{
    
}