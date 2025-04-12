namespace Core.SemanticAnalyzer.Diagnostics;

public class UninitializedNonNullable: WarningDiagnostic
{
    public UninitializedNonNullable(string variableName)
    {
        Message = Format("Variable '{0}' is declared as non-nullable but is uninitialized", variableName);
    }
}