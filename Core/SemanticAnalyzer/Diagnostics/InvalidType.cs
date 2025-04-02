using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidType(VariableReference reference): Diagnostic(Severity.Error)
{
    public override string GetMessage()
    {
        var message = $"{Error}The type name '\u001b[96m{reference.FullString}\u001b[0m' is not a valid type{GetLocation()}";
        
        if (Code != string.Empty)
            message += $"\n{GetCodeLocation()}";
        
        return message;
    }
}