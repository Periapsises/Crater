using Core.SemanticAnalyzer;

namespace Core.Diagnostics.TypeErrors;

public class InvalidCast(DataType source, DataType target)
    : Diagnostic("CRA207", "Cannot cast '{0}' to '{1}'", Severity.Error)
{
    public override string[] args => [source.GetName(), target.GetName()];
}