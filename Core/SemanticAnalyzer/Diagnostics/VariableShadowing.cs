namespace Core.SemanticAnalyzer.Diagnostics;

public class VariableShadowing(string variable): Diagnostic(Severity.Warning)
{
    public override string GetMessage()
    {
        var message = $"{Warning}Variable '{variable}' shadows existing binding{GetLocation()}";
        
        if (Code != string.Empty)
            message += $"\n{GetCodeLocation()}";
        
        return message;
    }
}