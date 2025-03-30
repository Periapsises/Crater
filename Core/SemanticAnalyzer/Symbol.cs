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
            DiagnosticsReporter.CurrentDiagnostics?.PushWarning("Assigning nullable to a non-nullable symbol");
        }
        
        DataType.Assign(this, other);
    }
}
