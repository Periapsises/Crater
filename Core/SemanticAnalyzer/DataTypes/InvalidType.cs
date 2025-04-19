namespace Core.SemanticAnalyzer.DataTypes;

public class InvalidType() : DataType(BaseType)
{
    public override string GetName() => "INVALID_TYPE";
    
    public override Result TryArithmeticOperation(Value left, Value right, string op)
    {
        return new Result(OperationResult.Success, Value.InvalidValue);
    }

    public override Result TryLogicOperation(Value left, Value right, string op)
    {
        return new Result(OperationResult.Success, Value.InvalidValue);
    }

    public override Result TryUnaryOperation(Value self, string op)
    {
        return new Result(OperationResult.Success, Value.InvalidValue);
    }

    public override Result TryToString(Value self)
    {
        return new Result(OperationResult.Success, Value.InvalidValue);
    }

    public override Result TryIndex(Value self, Value index)
    {
        return new Result(OperationResult.Success, Value.InvalidValue);
    }

    public override Result TryCall(Value self, List<Value> arguments)
    {
        return new Result(OperationResult.Success, Value.InvalidValue);
    }

    public override bool IsCompatible(DataType target)
    {
        return true;
    }

    public override DataType FindCommonType(DataType other)
    {
        return other;
    }
}