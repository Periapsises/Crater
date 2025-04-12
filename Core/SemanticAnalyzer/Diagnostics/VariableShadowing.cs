namespace Core.SemanticAnalyzer.Diagnostics;

public class VariableShadowing: WarningDiagnostic
{
    public VariableShadowing(string variable)
    {
        Message = $"Variable '{variable}' shadows existing binding";
    }
}