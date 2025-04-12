namespace Core.SemanticAnalyzer.Diagnostics;

public class UninitializedNonNullable: WarningDiagnostic
{
    public UninitializedNonNullable(string variable)
    {
        Message = $"Variable '{variable}' is declared as non-nullable but is uninitialized";
    }
}