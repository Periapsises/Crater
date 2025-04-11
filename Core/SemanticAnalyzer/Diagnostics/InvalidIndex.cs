namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidIndex : Diagnostic
{
    public InvalidIndex(DataType indexedType, DataType indexingType) : base(Severity.Error)
    {
        Message = Format("Type %s does not support indexing of type %s", indexedType, indexingType);
    }

    public InvalidIndex(DataType indexedType, string indexName) : base(Severity.Error)
    {
        Message = $"Type {indexedType.GetName()} does not contain an index named {indexName}";
    }
}