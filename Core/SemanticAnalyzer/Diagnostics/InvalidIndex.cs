namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidIndex : ErrorDiagnostic
{
    public InvalidIndex(DataType indexedType, DataType indexingType)
    {
        Message = Format("Type '{0}' does not support indexing of type '{0}'", indexedType, indexingType);
    }

    public InvalidIndex(DataType indexedType, string indexName)
    {
        Message = $"Type {indexedType.GetName()} does not contain an index named {indexName}";
    }
}