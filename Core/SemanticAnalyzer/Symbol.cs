namespace Core.SemanticAnalyzer;

public class Symbol(Value value, DataType dataType, bool nullable)
{
    public static readonly Symbol InvalidSymbol = new(Value.InvalidValue, DataType.InvalidType, false);
    public static readonly Symbol InvalidDataType = new(Value.InvalidType, DataType.MetaType, false);
    
    public Value Value = value;
    public readonly DataType DataType = dataType;
    public readonly bool Nullable = nullable;

    public Result ArithmeticOperation(Value other, string op)
    {
        return DataType.TryArithmeticOperation(Value, other, op);
    }

    public Result LogicOperation(Value other, string op)
    {
        return DataType.TryLogicOperation(Value, other, op);
    }
    
    public Result UnaryOperation(string op)
    {
        return DataType.TryUnaryOperation(Value, op);
    }

    public new Result ToString()
    {
        return DataType.TryToString(Value);
    }
    
    public void Assign(Value value)
    {
        Value = value;
    }

    public Result Index(Value indexingValue)
    {
        return DataType.TryIndex(Value, indexingValue);
    }

    public Result Call(List<Value> arguments)
    {
        return DataType.TryCall(Value, arguments);
    }
}
