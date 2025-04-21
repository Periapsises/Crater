using Core.SemanticAnalyzer;

namespace Core.Diagnostics.TypeErrors;

public class UnsupportedBinaryOperation(string op, DataType left, DataType right)
    : Diagnostic("CRA203", "Operator '{0}' is not defined for types '{1}' and '{2}'", Severity.Error)
{
    public override string[] args => [op, left.GetName(), right.GetName()];
}