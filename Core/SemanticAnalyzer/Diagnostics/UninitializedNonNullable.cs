namespace Core.SemanticAnalyzer.Diagnostics;

public class UninitializedNonNullable(string variable): Diagnostic(Severity.Warning)
{
    public override string GetMessage()
    {
        var message = $"{Warning}Variable '{variable}' is declared as non-nullable but is uninitialized{GetLocation()}";
        
        if (Code != string.Empty)
            message += $"\n{GetCodeLocation()}";
        
        return message;
    }
}