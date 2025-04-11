namespace Core.SemanticAnalyzer.Diagnostics;

public class VariableShadowing: Diagnostic
{
    public VariableShadowing(string variable) : base(Severity.Warning)
    {
        Message = $"Variable '{variable}' shadows existing binding";
    }
}