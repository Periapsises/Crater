using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidType : ErrorDiagnostic
{
    public InvalidType(VariableReference reference)
    {
        Message = $"The type name '\u001b[96m{reference.FullString}\u001b[0m' is not a valid type";
    }
}