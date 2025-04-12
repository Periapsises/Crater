namespace Core.SemanticAnalyzer.Diagnostics;

public class PossibleNullAssignment : WarningDiagnostic
{
    public PossibleNullAssignment(string variableName)
    {
        Message = Format("Assigning possible nil value to non-nullable '{0}'", variableName);
    }
}