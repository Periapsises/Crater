namespace Core.Diagnostics.TypeErrors;

public class FailedTypeInference(string identifier)
    : Diagnostic("CRA209", "Failed to infer type for '{0}'", Severity.Error)
{
    public override string[] args => [identifier];
}