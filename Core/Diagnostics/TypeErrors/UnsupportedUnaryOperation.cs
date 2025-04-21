using Core.SemanticAnalyzer;

namespace Core.Diagnostics.TypeErrors;

public class UnsupportedUnaryOperation(string op, DataType type)
    : Diagnostic("CRA204", "Operator '{0}' is not defined for type '{1}'", Severity.Error)
{
    public override string[] args => [op, type.GetName()];
}