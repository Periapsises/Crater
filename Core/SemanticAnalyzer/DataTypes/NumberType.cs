namespace Core.SemanticAnalyzer.DataTypes;

public class NumberType : DataType
{
    public override string GetName() => "number";

    public override bool TryOperation(Symbol left, Symbol right, string op, out Symbol? result)
    {
        if (left.Value.Kind == ValueKind.Number && right.Value.Kind == ValueKind.Number)
        {
            double number = op switch
            {
                "__add" => left.Value.GetNumber() + right.Value.GetNumber(),
                "__sub" => left.Value.GetNumber() - right.Value.GetNumber(),
                _ => throw new NotImplementedException($"Invalid operator {op}")
            };
            
            result = new Symbol(new Value(ValueKind.Number, number), this, false);
            return true;
        }

        if (right.DataType == NumberType)
        {
            result = new Symbol(new Value(ValueKind.Unknown, null), this, false);
            return true;
        }

        result = null;
        return false;
    }
}