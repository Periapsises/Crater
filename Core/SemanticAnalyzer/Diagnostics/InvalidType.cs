using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidType : Diagnostic
{
    public InvalidType(VariableReference reference) : base(Severity.Error)
    {
        Message = $"The type name '\u001b[96m{reference.FullString}\u001b[0m' is not a valid type";
    }
}