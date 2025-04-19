namespace Core.SemanticAnalyzer.DataTypes;

public class BooleanType() : DataType(BaseType)
{
    public override string GetName() => "bool";

    public override Result TryArithmeticOperation(Value left, Value right, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryLogicOperation(Value left, Value right, string op)
    {
        if (op != "__eq")
            return new Result(OperationResult.NotImplemented);
        
        if (left.Kind == ValueKind.Boolean && right.Kind == ValueKind.Boolean)
            return new Result(OperationResult.Success, Value.From(left.GetBoolean() == right.GetBoolean()));

        return new Result(OperationResult.Success, Value.Unknown(BooleanType));
    }

    public override Result TryUnaryOperation(Value self, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryToString(Value self)
    {
        if (self.Kind == ValueKind.Boolean)
            return new Result(OperationResult.Success, Value.From(self.GetBoolean().ToString().ToLower()));
        
        return new Result(OperationResult.Success, Value.Unknown(StringType));
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