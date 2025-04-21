using Core.SemanticAnalyzer;

namespace Core.Diagnostics.TypeErrors;

public class UnsupportedComparisonOperation(string op, DataType left, DataType right)
    : Diagnostic("CRA205", "Operator '{0}' is not defined for comparing types '{1}' and '{2}'", Severity.Error)
{
    public override string[] args => [op, left.GetName(), right.GetName()];
}