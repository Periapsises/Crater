using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer.DataTypes;

public class InvalidType : DataType
{
    public override string GetName() => "INVALID_TYPE";
    
    public override bool TryArithmeticOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
        throw new NotImplementedException();
    }

    public override bool TryLogicOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
        throw new NotImplementedException();
    }

    public override bool TryUnaryOperation(Symbol self, string op, [NotNullWhen(true)] out Symbol? result)
    {
        throw new NotImplementedException();
    }

    public override bool TryToString(Symbol self, [NotNullWhen(true)] out Symbol? result)
    {
        throw new NotImplementedException();
    }

    public override bool TryIndex(Symbol self, Symbol index, [NotNullWhen(true)] out Symbol? result)
    {
        throw new NotImplementedException();
    }
}