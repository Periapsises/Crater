using Core.SemanticAnalyzer;

namespace Core.Diagnostics.TypeErrors;

public class NullableTypeMismatch(DataType target)
    : Diagnostic("CRA202", "Cannot assign potential null value to non-nullable type '{0}'", Severity.Error)
{
    public override string[] args => [target.GetName()];
}