using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer.DataTypes;

public class BooleanType : DataType
{
    public override string GetName() => "bool";

    public override bool TryArithmeticOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
        result = null;
        return false;
    }

    public override bool TryLogicOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
        if (op != "__eq")
        {
            result = null;
            return false;
        }
        
        if (left.Value.Kind == ValueKind.Boolean && right.Value.Kind == ValueKind.Boolean)
        {
            result = new Symbol(Value.From(left.Value.GetBoolean() == right.Value.GetBoolean()), BooleanType, false);
            return true;
        }
        
        result = new Symbol(Value.Unknown, BooleanType, false);
        return true;
    }

    public override bool TryUnaryOperation(Symbol self, string op, [NotNullWhen(true)] out Symbol? result)
    {
        result = null;
        return false;
    }

    public override bool TryToString(Symbol self, [NotNullWhen(true)] out Symbol? result)
    {
        if (self.Value.Kind == ValueKind.Boolean)
        {
            result = new Symbol(Value.From(self.Value.GetBoolean().ToString().ToLower()), StringType, false);
            return true;
        }
        
        result = new Symbol(Value.Unknown, StringType, false);
        return true;
    }
}