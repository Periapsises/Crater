using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer.DataTypes;

public class StringType : DataType
{
    public override string GetName() => "string";
    
    public override bool TryArithmeticOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
        if (op != "__concat")
        {
            result = null;
            return false;
        }

        if (right.DataType != StringType && right.ToString(out var asString))
            right = asString;
        
        if (left.Value.Kind == ValueKind.String && right.Value.Kind == ValueKind.String)
        {
            var value = left.Value.GetString() + right.Value.GetString();
            result = new Symbol(new Value(ValueKind.String, value), this, false);
            return true;
        }

        if (right.DataType == StringType)
        {
            result = new Symbol(new Value(ValueKind.Unknown, null), this, false);
            return true;
        }
        
        result = null;
        return false;
    }

    public override bool TryLogicOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
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
            
            result = new Symbol(Value.From(value), BooleanType, false);
            return true;
        }

        if (right.DataType == StringType)
        {
            result = new Symbol(Value.Unknown, BooleanType, false);
            return true;
        }
        
        result = null;
        return false;
    }

    public override bool TryUnaryOperation(Symbol self, string op, [NotNullWhen(true)] out Symbol? result)
    {
        result = null;
        return false;
    }

    public override bool TryToString(Symbol self, [NotNullWhen(true)] out Symbol? result)
    {
        result = self;
        return true;
    }
}