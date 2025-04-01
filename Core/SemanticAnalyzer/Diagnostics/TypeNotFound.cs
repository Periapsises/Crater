using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer.Diagnostics;

public class TypeNotFound(DataTypeReference reference) : Diagnostic(Severity.Error)
{
    public override string GetMessage()
    {
        var message = $"{Error}Cannot resolve type name '\u001b[96m{reference.FullString}\u001b[0m'{GetLocation()}";
        
        if (Code != string.Empty)
            message += $"\n{GetCodeLocation()}";
        
        return message;
    }
}