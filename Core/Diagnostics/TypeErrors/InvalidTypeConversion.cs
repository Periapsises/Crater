using Core.SemanticAnalyzer;

namespace Core.Diagnostics.TypeErrors;

public class InvalidTypeConversion(DataType source, DataType target)
    : Diagnostic("CRA206", "Cannot implicitly convert from '{0}' to '{1}'", Severity.Error)
{
    public override string[] args => [source.GetName(), target.GetName()];
}