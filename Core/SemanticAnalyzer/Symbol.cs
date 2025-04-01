namespace Core.SemanticAnalyzer;

public class Symbol(Value value, DataType dataType, bool nullable)
{
    public Value Value = value;
    public DataType DataType = dataType;
    public bool Nullable = nullable;

    public void Assign(Symbol symbol)
    {
        Value = symbol.Value;
        DataType = symbol.DataType;
        Nullable = symbol.Nullable;
    }
}
