namespace Core.SemanticAnalyzer.DataTypes;

public class InvalidType : DataType
{
    public override string GetName() => "INVALID_TYPE";
    
    public override Result TryArithmeticOperation(Symbol left, Symbol right, string op)
    {
        return new Result(OperationResult.Success, Symbol.InvalidSymbol);
    }

    public override Result TryLogicOperation(Symbol left, Symbol right, string op)
    {
        return new Result(OperationResult.Success, Symbol.InvalidSymbol);
    }

    public override Result TryUnaryOperation(Symbol self, string op)
    {
        return new Result(OperationResult.Success, Symbol.InvalidSymbol);
    }

    public override Result TryToString(Symbol self)
    {
        return new Result(OperationResult.Success, Symbol.InvalidSymbol);
    }

    public override Result TryIndex(Symbol self, Symbol index)
    {
        return new Result(OperationResult.Success, Symbol.InvalidSymbol);
    }
}