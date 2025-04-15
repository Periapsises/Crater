namespace Core.SemanticAnalyzer.DataTypes;

public class StringType : DataType
{
    public override string GetName() => "string";
    
    public override Result TryArithmeticOperation(Symbol left, Symbol right, string op)
    {
        if (op != "__concat")
        {
            return new Result(OperationResult.NotImplemented);
        }

        if (right.DataType != StringType)
        {
            var conversionResult = right.ToString();
            if (conversionResult.OperationResult == OperationResult.Success)
                right = conversionResult.Symbol!;
        }

        if (left.Value.Kind == ValueKind.String && right.Value.Kind == ValueKind.String)
        {
            var value = left.Value.GetString() + right.Value.GetString();
            return new Result(OperationResult.Success, new Symbol(new Value(ValueKind.String, value), this, false));
        }

        if (right.DataType == StringType)
        {
            return new Result(OperationResult.Success, new Symbol(new Value(ValueKind.Unknown, null), this, false));
        }
        
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryLogicOperation(Symbol left, Symbol right, string op)
    {
        if (left.Value.Kind == ValueKind.String && right.Value.Kind == ValueKind.String)
        {
            var value = op switch
            {
                "__eq" => left.Value.GetString() == right.Value.GetString(),
                "__lt" => string.Compare(left.Value.GetString(), right.Value.GetString(), StringComparison.CurrentCultureIgnoreCase) < 0,
                "__le" => string.Compare(left.Value.GetString(), right.Value.GetString(), StringComparison.CurrentCultureIgnoreCase) <= 0,
                _ => throw new NotImplementedException($"Invalid logic operator: {op}")
            };
            
            return new Result(OperationResult.Success, new Symbol(Value.From(value), BooleanType, false));
        }

        if (right.DataType == StringType)
        {
            return new Result(OperationResult.Success, new Symbol(Value.Unknown, BooleanType, false));
        }
        
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryUnaryOperation(Symbol self, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryToString(Symbol self)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryIndex(Symbol self, Symbol index)
    {
        return new Result(OperationResult.NotImplemented);
    }
}