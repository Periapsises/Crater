namespace Core.SemanticAnalyzer.Diagnostics;

public class UninitializedNonNullable: Diagnostic
{
    public UninitializedNonNullable(string variable) : base(Severity.Warning)
    {
        Message = $"Variable '{variable}' is declared as non-nullable but is uninitialized";
    }
}