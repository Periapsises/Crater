namespace Core.Diagnostics.SemanticWarnings;

public class UnreachableCode()
    : Diagnostic("CRA400", "The code is never reached due to a previous return statement.", Severity.Warning)
{
    public override string[] args => [];
}