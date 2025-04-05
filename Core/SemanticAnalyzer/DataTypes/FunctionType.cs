using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer.DataTypes;

public class FunctionType(List<DataType> argTypes, List<DataType> returnTypes) : DataType
{
    private readonly List<DataType> _argTypes = argTypes;
    private readonly List<DataType> _returnTypes = returnTypes;
    
    public static readonly DataType FunctionBase = new FunctionType([], [DataType.VoidType]);

    public override string GetName()
    {
        return $"func({string.Join(", ", _argTypes.Select(x => x.GetName()))}): {_returnTypes.Select(x => x.GetName())}";
    }
    
    public override bool TryBinaryOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result)
    {
        throw new NotImplementedException();
    }
    
    public override bool TryUnaryOperation(Symbol self, string op, [NotNullWhen(true)] out Symbol? result)
    {
        throw new NotImplementedException();
    }

    public override bool IsCompatible(DataType other)
    {
        if (other == FunctionBase) return true;
        
        if (other is FunctionType function)
        {
            if (function._argTypes.Count != _argTypes.Count) return false;
            if (function._returnTypes.Count != _returnTypes.Count) return false;
            
            for (var i = 0; i < _argTypes.Count; i++)
                if (!function._argTypes[i].IsCompatible(_argTypes[i]))
                    return false;
            
            for (var i = 0; i < _returnTypes.Count; i++)
                if (!_returnTypes[i].IsCompatible(function._returnTypes[i]))
                    return false;
            
            return true;
        }

        return false;
    }
}