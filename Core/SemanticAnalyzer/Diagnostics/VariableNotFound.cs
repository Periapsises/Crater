using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class VariableNotFound : ErrorDiagnostic
{
    public VariableNotFound(VariableReference reference)
    {
        Message = $"Cannot resolve variable name '\u001b[96m{reference.FullString}\u001b[0m'";
    }
}