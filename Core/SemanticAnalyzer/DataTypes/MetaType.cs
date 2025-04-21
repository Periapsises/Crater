namespace Core.SemanticAnalyzer.DataTypes;

public class MetaType() : DataType(BaseType)
{
    public override string GetName()
    {
        return "type";
    }

    public override Result TryArithmeticOperation(Value left, Value right, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryLogicOperation(Value left, Value right, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryUnaryOperation(Value self, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryToString(Value self)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryIndex(Value self, Value index)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryCall(Value self, List<Value> arguments)
    {
        return new Result(OperationResult.NotImplemented);
    }
}