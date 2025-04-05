using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer.DataTypes;

public class CustomType : DataType
{
    public override string GetName() => "class";
    
    public override bool TryBinaryOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
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
        result = null;
        return false;
    }
}