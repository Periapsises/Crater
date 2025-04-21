namespace Core.Diagnostics.TypeErrors;

public class UndefinedType(string name)
    : Diagnostic("CRA201", "Undefined type '{0}'", Severity.Error)
{
    public override string[] args => [name];
}