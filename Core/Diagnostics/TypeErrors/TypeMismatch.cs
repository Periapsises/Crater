using Core.SemanticAnalyzer;

namespace Core.Diagnostics.TypeErrors;

public class TypeMismatch(DataType expected, DataType actual)
    : Diagnostic("CRA200", "Type mismatch: expected '{0}', but got '{1}'", Severity.Error)
{
    public override string[] args => [expected.GetName(), actual.GetName()];
}