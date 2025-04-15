namespace Core.SemanticAnalyzer.DataTypes;

public class BooleanType : DataType
{
    public override string GetName() => "bool";

    public override Result TryArithmeticOperation(Symbol left, Symbol right, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryLogicOperation(Symbol left, Symbol right, string op)
    {
        if (op != "__eq")
            return new Result(OperationResult.NotImplemented);
        
        if (left.Value.Kind == ValueKind.Boolean && right.Value.Kind == ValueKind.Boolean)
        {
            return new Result(OperationResult.Success,
                new Symbol(Value.From(left.Value.GetBoolean() == right.Value.GetBoolean()), BooleanType, false));
        }

        return new Result(OperationResult.Success, new Symbol(Value.Unknown, BooleanType, false));
    }

    public override Result TryUnaryOperation(Symbol self, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryToString(Symbol self)
    {
        if (self.Value.Kind == ValueKind.Boolean)
            return new Result(OperationResult.Success, new Symbol(Value.From(self.Value.GetBoolean().ToString().ToLower()), StringType, false));
        
        return new Result(OperationResult.Success, new Symbol(Value.Unknown, StringType, false));
    }

    public override Result TryIndex(Symbol self, Symbol index)
    {
        return new Result(OperationResult.NotImplemented);
    }
}