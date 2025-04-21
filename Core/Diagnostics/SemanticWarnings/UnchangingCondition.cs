namespace Core.Diagnostics.SemanticWarnings;

public class UnchangingCondition(bool evaluated)
    : Diagnostic("CRA401", "The condition always evaluates to a '{0}' value.", Severity.Warning)
{
    public override string[] args => evaluated ? ["truthful"] : ["false"];
}