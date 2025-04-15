using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer;

public class Symbol(Value value, DataType dataType, bool nullable)
{
    public static readonly Symbol InvalidSymbol = new(Value.InvalidValue, DataType.InvalidType, false);
    public static readonly Symbol InvalidDataType = new(Value.InvalidType, DataType.MetaType, false);
    
    public Value Value = value;
    public DataType DataType = dataType;
    public bool Nullable = nullable;

    public Result ArithmeticOperation(Symbol other, string op)
    {
        return DataType.TryArithmeticOperation(this, other, op);
    }

    public Result LogicOperation(Symbol other, string op)
    {
        return DataType.TryLogicOperation(this, other, op);
    }
    
    public Result UnaryOperation(string op)
    {
        return DataType.TryUnaryOperation(this, op);
    }

    public new Result ToString()
    {
        return DataType.TryToString(this);
    }
    
    public void Assign(Symbol symbol)
    {
        Value = symbol.Value;
        DataType = symbol.DataType;
        Nullable = symbol.Nullable;
    }

    public Result Index(Symbol indexingSymbol)
    {
        return DataType.TryIndex(this, indexingSymbol);
    }

    public Result Call(List<PossibleSymbols> arguments)
    {
        return new Result(OperationResult.NotImplemented);
    }
    
    public static implicit operator PossibleSymbols(Symbol symbol) => [symbol];
}
