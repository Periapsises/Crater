namespace Core.SemanticAnalyzer.DataTypes;

public class NumberType() : DataType(BaseType)
{
    public override string GetName()
    {
        return "number";
    }

    public override Result TryArithmeticOperation(Value left, Value right, string op)
    {
        if (right.DataType == StringType)
        {
            // TODO: Show a warning for potential conversion of invalid number formats
            if (right.Kind == ValueKind.String)
                try
                {
                    var numberValue = double.Parse(right.GetString());
                    right = Value.From(numberValue);
                }
                catch (FormatException)
                {
                }
            else
                right = Value.Unknown(NumberType);
        }

        if (left.Kind == ValueKind.Number && right.Kind == ValueKind.Number)
        {
            var number = op switch
            {
                "__add" => left.GetNumber() + right.GetNumber(),
                "__sub" => left.GetNumber() - right.GetNumber(),
                "__mul" => left.GetNumber() * right.GetNumber(),
                "__div" => left.GetNumber() / right.GetNumber(),
                "__mod" => left.GetNumber() % right.GetNumber(),
                "__exp" => Math.Pow(left.GetNumber(), right.GetNumber()),
                _ => throw new NotImplementedException($"Invalid arithmetic operator {op}")
            };

            return new Result(OperationResult.Success, Value.From(number));
        }

        if (right.DataType == NumberType)
            return new Result(OperationResult.Success, Value.Unknown(NumberType));

        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryLogicOperation(Value left, Value right, string op)
    {
        if (left.Kind == ValueKind.Number && right.Kind == ValueKind.Number)
        {
            var value = op switch
            {
                "__eq" => left.GetNumber() == right.GetNumber(),
                "__lt" => left.GetNumber() < right.GetNumber(),
                "__le" => left.GetNumber() <= right.GetNumber(),
                _ => throw new NotImplementedException($"Invalid logic operator {op}")
            };

            return new Result(OperationResult.Success, Value.From(value));
        }

        if (right.DataType == NumberType) return new Result(OperationResult.Success, Value.Unknown(BooleanType));

        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryUnaryOperation(Value self, string op)
    {
        if (self.Kind == ValueKind.Number)
        {
            var number = op switch
            {
                "__unm" => -self.GetNumber(),
                _ => throw new NotImplementedException($"Invalid operator {op}")
            };

            return new Result(OperationResult.Success, Value.From(number));
        }

        if (self.DataType == NumberType) return new Result(OperationResult.Success, Value.Unknown(NumberType));

        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryToString(Value self)
    {
        if (self.Kind == ValueKind.Number)
        {
            var stringRepresentation = self.GetNumber().ToString();
            return new Result(OperationResult.Success, Value.From(stringRepresentation));
        }

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