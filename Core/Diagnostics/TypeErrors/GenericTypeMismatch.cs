using Core.SemanticAnalyzer;

namespace Core.Diagnostics.TypeErrors;

public class GenericTypeMismatch(DataType source, DataType target)
    : Diagnostic("CRA210", "Generic type '{0}' does not satisfy constraint '{1}'", Severity.Error)
{
    public override string[] args => [source.GetName(), target.GetName()];
}