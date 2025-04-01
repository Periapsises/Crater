namespace Core.SemanticAnalyzer.Diagnostics;

public class TypeMismatch(DataType source, DataType target) : Diagnostic(Severity.Error)
{
    public override string GetMessage()
    {
        var message = $"{Error}Type mismatch: Cannot convert from '\u001b[96m{source.GetName()}\u001b[0m' to '\u001b[96m{target.GetName()}\u001b[0m'{GetLocation()}";
        
        if (Code != string.Empty)
            message += $"\n{GetCodeLocation()}";
        
        return message;
    }
}