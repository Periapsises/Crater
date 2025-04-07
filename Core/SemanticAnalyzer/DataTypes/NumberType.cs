using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer.DataTypes;

public class NumberType : DataType
{
    public override string GetName() => "number";

    public override bool TryArithmeticOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
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

    public override bool TryLogicOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
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
            
            result = new Symbol(new Value(ValueKind.Boolean, value), BooleanType, false);
            return true;
        }

        if (right.DataType == NumberType)
        {
            result = new Symbol(new Value(ValueKind.Unknown, null), BooleanType, false);
            return true;
        }

        result = null;
        return false;
    }

    public override bool TryUnaryOperation(Symbol self, string op, [NotNullWhen(true)] out Symbol? result)
    {
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

    public Symbol Equals(Symbol left, Symbol right)
    {
        if (left.Value.Kind == ValueKind.Number && right.Value.Kind == ValueKind.Number)
        {
            // TODO: Determine if double precision could mess with the result and become a problem
            var isEqual = left.Value.GetNumber() == right.Value.GetNumber();
            return new Symbol(new Value(ValueKind.Boolean, isEqual), BooleanType, false);
        }

        if (right.DataType == NumberType)
            return new Symbol(new Value(ValueKind.Unknown, null), BooleanType, false);
        
        return new Symbol(new Value(ValueKind.Boolean, false), BooleanType, false);
    }
}