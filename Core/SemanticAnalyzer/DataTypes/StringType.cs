using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer.DataTypes;

public class StringType : DataType
{
    public override string GetName() => "string";
    
    public override bool TryBinaryOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
        if (op != "__concat")
        {
            result = null;
            return false;
        }
        
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
    
    public override bool TryUnaryOperation(Symbol self, string op, [NotNullWhen(true)] out Symbol? result)
    {
        result = null;
        return false;
    }
}