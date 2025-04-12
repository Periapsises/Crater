using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidType : ErrorDiagnostic
{
    public InvalidType(VariableReference reference)
    {
        Message = Format("The type name '{0}' is not a valid type", reference);
    }
}