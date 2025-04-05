using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer.DataTypes;

public class NumberType : DataType
{
    private static readonly Dictionary<string, bool> _validBinaryOperations = new()
    {
        { "__add", true },
        { "__sub", true },
        { "__mul", true },
        { "__div", true },
        { "__mod", true },
        { "__pow", true },
        { "__exp", true }
    };

    private static readonly Dictionary<string, bool> _validUnaryOperations = new()
    {
        { "__unm", true }
    };
    
    public override string GetName() => "number";

    public override bool TryBinaryOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
        if (!_validBinaryOperations.ContainsKey(op))
        {
            result = null;
            return false;
        }

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
    
    public override bool TryUnaryOperation(Symbol self, string op, [NotNullWhen(true)] out Symbol? result)
    {
        if (!_validUnaryOperations.ContainsKey(op))
        {
            result = null;
            return false;
        }
        
        if (self.Value.Kind == ValueKind.Number)
        {
            var number = op switch
            {
                "__unm" => -self.Value.GetNumber(),
                _ => throw new NotImplementedException($"Invalid operator {op}")
            };
            
            result = new Symbol(new Value(ValueKind.Number, number), this, false);
        }

        if (self.DataType == NumberType)
        {
            result = new Symbol(new Value(ValueKind.Unknown, null), this, false);
            return true;
        }
        
        result = null;
        return false;
    }

    public override bool TryToString(Symbol self, [NotNullWhen(true)] out Symbol? result)
    {
        if (self.Value.Kind == ValueKind.Number)
        {
            var stringRepresentation = self.Value.GetNumber().ToString();
            result = new Symbol(new Value(ValueKind.String, stringRepresentation), StringType, false);
            return true;
        }
        
        result = new Symbol(new Value(ValueKind.Unknown, null), StringType, false);
        return true;
    }
}