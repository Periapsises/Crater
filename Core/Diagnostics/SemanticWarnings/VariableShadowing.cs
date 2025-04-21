namespace Core.Diagnostics.SemanticWarnings;

public class VariableShadowing(string identifier)
    : Diagnostic("CRA402", "Variable '{0}' shadows existing binding", Severity.Warning)
{
    public override string[] args => [identifier];
}