using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class TypeNotFound : ErrorDiagnostic
{
    public TypeNotFound(VariableReference reference)
    {
        Message = $"Cannot resolve type name '\u001b[96m{reference.FullString}\u001b[0m'";
    }
}