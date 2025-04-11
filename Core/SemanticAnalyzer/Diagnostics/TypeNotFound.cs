using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class TypeNotFound : Diagnostic
{
    public TypeNotFound(VariableReference reference) : base(Severity.Error)
    {
        Message = $"Cannot resolve type name '\u001b[96m{reference.FullString}\u001b[0m'";
    }
}