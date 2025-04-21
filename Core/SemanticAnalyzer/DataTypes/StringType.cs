namespace Core.SemanticAnalyzer.DataTypes;

public class StringType() : DataType(BaseType)
{
    public override string GetName()
    {
        return "string";
    }

    public override Result TryArithmeticOperation(Value left, Value right, string op)
    {
        if (op != "__concat")
            return new Result(OperationResult.NotImplemented);

        if (right.DataType != StringType)
        {
            var conversionResult = right.DataType.TryToString(right);
            if (conversionResult.OperationResult == OperationResult.Success)
                right = conversionResult.Value!;
        }

        if (left.Kind == ValueKind.String && right.Kind == ValueKind.String)
        {
            var value = left.GetString() + right.GetString();
            return new Result(OperationResult.Success, Value.From(value));
        }

        if (right.DataType == StringType)
            return new Result(OperationResult.Success, Value.Unknown(StringType));

        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryLogicOperation(Value left, Value right, string op)
    {
        if (left.Kind == ValueKind.String && right.Kind == ValueKind.String)
        {
            var value = op switch
            {
                "__eq" => left.GetString() == right.GetString(),
                "__lt" => string.Compare(left.GetString(), right.GetString(),
                    StringComparison.CurrentCultureIgnoreCase) < 0,
                "__le" => string.Compare(left.GetString(), right.GetString(),
                    StringComparison.CurrentCultureIgnoreCase) <= 0,
                _ => throw new NotImplementedException($"Invalid logic operator: {op}")
            };

            return new Result(OperationResult.Success, Value.From(value));
        }

        if (right.DataType == StringType)
            return new Result(OperationResult.Success, Value.Unknown(BooleanType));

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