using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer.DataTypes;

public class CustomType : DataType
{
    public override string GetName() => "class";
    
    public override bool TryArithmeticOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
        result = null;
        return false;
    }

    public override bool TryLogicOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
        throw new NotImplementedException();
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

    public override bool TryIndex(Symbol self, Symbol index, [NotNullWhen(true)] out Symbol? result)
    {
        throw new NotImplementedException();
    }
}