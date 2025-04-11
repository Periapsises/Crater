namespace Core.SemanticAnalyzer.Diagnostics;

public class TypeMismatch : Diagnostic
{
    public TypeMismatch(DataType source, DataType target) : base(Severity.Error)
    {
        Message = Format("Type mismatch: Cannot convert from '%s' to '%s'", source, target);
    }
}