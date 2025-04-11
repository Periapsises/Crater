namespace Core.SemanticAnalyzer.Diagnostics;

public class PossibleNullAssignment : Diagnostic
{
    public PossibleNullAssignment(string variable) : base(Severity.Warning)
    {
        Message = "Assigning possible nil value to non-nullable '" + variable + "'";
    }
}