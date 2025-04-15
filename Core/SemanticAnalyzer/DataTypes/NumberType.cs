namespace Core.SemanticAnalyzer.DataTypes;

public class NumberType : DataType
{
    public override string GetName() => "number";

    public override Result TryArithmeticOperation(Symbol left, Symbol right, string op)
    {
        if (right.DataType == StringType)
        {
            // TODO: Show a warning for potential conversion of invalid number formats
            if (right.Value.Kind == ValueKind.String)
            {
                try
                {
                    var numberValue = double.Parse(right.Value.GetString());
                    right = new Symbol(new Value(ValueKind.Number, numberValue), this, false);
                }
                catch (FormatException) { }
            }
            else
            {
                right = new Symbol(new Value(ValueKind.Unknown, null), this, false);
            }
        }
        
        if (left.Value.Kind == ValueKind.Number && right.Value.Kind == ValueKind.Number)
        {
            var number = op switch
            {
                "__add" => left.Value.GetNumber() + right.Value.GetNumber(),
                "__sub" => left.Value.GetNumber() - right.Value.GetNumber(),
                "__mul" => left.Value.GetNumber() * right.Value.GetNumber(),
                "__div" => left.Value.GetNumber() / right.Value.GetNumber(),
                "__mod" => left.Value.GetNumber() % right.Value.GetNumber(),
                "__exp" => Math.Pow(left.Value.GetNumber(), right.Value.GetNumber()),
                _ => throw new NotImplementedException($"Invalid arithmetic operator {op}")
            };
            
            return new Result(OperationResult.Success, new Symbol(new Value(ValueKind.Number, number), this, false));
        }

        if (right.DataType == NumberType)
        {
            return new Result(OperationResult.Success, new Symbol(new Value(ValueKind.Unknown, null), this, false));
        }

        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryLogicOperation(Symbol left, Symbol right, string op)
    {
        if (left.Value.Kind == ValueKind.Number && right.Value.Kind == ValueKind.Number)
        {
            var value = op switch
            {
                "__eq" => left.Value.GetNumber() == right.Value.GetNumber(),
                "__lt" => left.Value.GetNumber() < right.Value.GetNumber(),
                "__le" => left.Value.GetNumber() <= right.Value.GetNumber(),
                _ => throw new NotImplementedException($"Invalid logic operator {op}")
            };
            
            return new Result(OperationResult.Success, new Symbol(new Value(ValueKind.Boolean, value), BooleanType, false));
        }

        if (right.DataType == NumberType)
        {
            return new Result(OperationResult.Success,
                new Symbol(new Value(ValueKind.Unknown, null), BooleanType, false));
        }

        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryUnaryOperation(Symbol self, string op)
    {
        if (self.Value.Kind == ValueKind.Number)
        {
            var number = op switch
            {
                "__unm" => -self.Value.GetNumber(),
                _ => throw new NotImplementedException($"Invalid operator {op}")
            };

            return new Result(OperationResult.Success, new Symbol(new Value(ValueKind.Number, number), this, false));
        }

        if (self.DataType == NumberType)
        {
            return new Result(OperationResult.Success, new Symbol(new Value(ValueKind.Unknown, null), this, false));
        }
        
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryToString(Symbol self)
    {
        if (self.Value.Kind == ValueKind.Number)
        {
            var stringRepresentation = self.Value.GetNumber().ToString();
            return new Result(OperationResult.Success, new Symbol(new Value(ValueKind.String, stringRepresentation), StringType, false));
        }
        
        return new Result(OperationResult.Success, new Symbol(new Value(ValueKind.Unknown, null), StringType, false));
    }

    public override Result TryIndex(Symbol self, Symbol index)
    {
        return new Result(OperationResult.NotImplemented);
    }
}