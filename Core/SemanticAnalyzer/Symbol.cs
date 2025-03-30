namespace Core.SemanticAnalyzer;

public class Symbol(Value value, DataType dataType, bool nullable)
{
    public Value Value = value;
    public DataType DataType = dataType;
    public bool Nullable = nullable;

    public void Assign(Symbol other)
    {
        if (other.Nullable && !Nullable)
        {
            // TODO: Check for a possibly null value and display a warning
            throw new NotImplementedException();
        }
        
        dataType.Assign(this, other);
    }
}
