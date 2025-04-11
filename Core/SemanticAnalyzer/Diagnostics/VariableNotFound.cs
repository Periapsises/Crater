using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class VariableNotFound : Diagnostic
{
    public VariableNotFound(VariableReference reference) : base(Severity.Error)
    {
        Message = $"Cannot resolve variable name '\u001b[96m{reference.FullString}\u001b[0m'";
    }
}