using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer.DataTypes;

public class MetaType : DataType
{
    public override string GetName() => "type";
    
    public override bool TryBinaryOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
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
}