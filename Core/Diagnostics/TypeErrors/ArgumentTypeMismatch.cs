using Core.SemanticAnalyzer;

namespace Core.Diagnostics.TypeErrors;

public class ArgumentTypeMismatch(string identifier, DataType source, DataType target)
    : Diagnostic("CRA211", "Argument '{0}' has type '{1}', but expected '{2}'", Severity.Error)
{
    public override string[] args => [identifier, source.GetName(), target.GetName()];
}