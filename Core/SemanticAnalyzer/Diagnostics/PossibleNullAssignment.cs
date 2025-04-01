namespace Core.SemanticAnalyzer.Diagnostics;

public class PossibleNullAssignment(string variable) : Diagnostic(Severity.Warning)
{
    public override string GetMessage()
    {
        var message = $"{Warning}Assigning possible nil value to non-nullable '{variable}'{GetLocation()}";
        
        if (Code != string.Empty)
            message += $"\n{GetCodeLocation()}";
        
        return message;
    }
}