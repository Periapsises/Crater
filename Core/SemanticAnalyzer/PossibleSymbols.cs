namespace Core.SemanticAnalyzer;

public class PossibleSymbols : List<Symbol>
{
    public bool CanBeNull()
    {
        foreach (var symbol in this)
            if (symbol.Nullable) return true;
        
        return false;
    }

    public bool ValueCanBe(ValueKind kind, object value)
    {
        foreach (var symbol in this)
            if (symbol.Value.Kind == kind && symbol.Value.GetValue() == value)
                return true;
        
        return false;
    }

    public bool AlwaysTrue()
    {
        foreach (var symbol in this)
        {
            if (symbol.Value.Kind == ValueKind.Boolean)
            {
                if (!symbol.Value.GetBoolean()) return false;
                continue;
            }
            
            if (symbol.Nullable)
                return false;
            else if (symbol.DataType == DataType.BooleanType)
                return false;
        }
        
        return true;
    }

    public bool AlwaysFalse()
    {
        foreach (var symbol in this)
        {
            if (symbol.Value.Kind == ValueKind.Boolean)
            {
                if (symbol.Value.GetBoolean()) return false;
                continue;
            }

            if (!symbol.Nullable)
                return false;
            else if (symbol.DataType == DataType.BooleanType)
                return false;
        }
        
        return true;
    }
}