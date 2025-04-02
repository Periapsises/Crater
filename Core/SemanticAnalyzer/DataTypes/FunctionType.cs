namespace Core.SemanticAnalyzer.DataTypes;

public class FunctionType(List<DataType> argTypes, List<DataType> returnTypes) : MetaType
{
    private readonly List<DataType> _argTypes = argTypes;
    private readonly List<DataType> _returnTypes = returnTypes;
    
    public static readonly MetaType FunctionBase = new FunctionType([], [DataType.VoidType]);

    public override string GetName()
    {
        return $"func({string.Join(", ", _argTypes.Select(x => x.GetName()))}): {_returnTypes.Select(x => x.GetName())}";
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