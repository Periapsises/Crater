using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer;

public class Symbol(Value value, DataType dataType, bool nullable)
{
    public static readonly Symbol InvalidSymbol = new(Value.InvalidValue, DataType.InvalidType, false);
    public static readonly Symbol InvalidDataType = new(Value.InvalidType, DataType.MetaType, false);
    
    public Value Value = value;
    public DataType DataType = dataType;
    public bool Nullable = nullable;

    public bool BinaryOperation(Symbol other, string op, [NotNullWhen(true)] out Symbol? result)
    {
        return DataType.TryOperation(this, other, op, out result);
    }
    
    public void Assign(Symbol symbol)
    {
        Value = symbol.Value;
        DataType = symbol.DataType;
        Nullable = symbol.Nullable;
    }

    public static implicit operator PossibleSymbols(Symbol symbol) => [symbol];
}
