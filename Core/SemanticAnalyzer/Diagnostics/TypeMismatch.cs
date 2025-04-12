namespace Core.SemanticAnalyzer.Diagnostics;

public class TypeMismatch : ErrorDiagnostic
{
    public TypeMismatch(DataType source, DataType target)
    {
        Message = Format("Type mismatch: Cannot convert from '{0}' to '{1}'", source, target);
    }
}