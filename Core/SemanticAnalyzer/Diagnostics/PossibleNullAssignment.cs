namespace Core.SemanticAnalyzer.Diagnostics;

public class PossibleNullAssignment : WarningDiagnostic
{
    public PossibleNullAssignment(string variable)
    {
        Message = "Assigning possible nil value to non-nullable '" + variable + "'";
    }
}