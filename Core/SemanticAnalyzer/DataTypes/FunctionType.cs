using Core.SemanticAnalyzer.Diagnostics;

namespace Core.SemanticAnalyzer.DataTypes;

public class FunctionType(List<TypeUsage> parameterTypes, List<TypeUsage> returnTypes) : DataType(BaseType)
{
    private readonly List<TypeUsage> _parameterTypes = parameterTypes;
    private readonly List<TypeUsage> _returnTypes = returnTypes;
    
    public static readonly DataType FunctionBase = new FunctionType([], []);

    public override string GetName()
    {
        var args = string.Join(", ", _parameterTypes.Select(x => x.DataType.GetName()));
        var returns = string.Join(", ", _returnTypes.Select(x => x.DataType.GetName()));

        if (_returnTypes.Count == 0)
            returns = "void";
        
        return $"func({args}): {returns}";
    }
    
    public override Result TryArithmeticOperation(Value left, Value right, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryLogicOperation(Value left, Value right, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryUnaryOperation(Value self, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryToString(Value self)
    {
        return new Result(OperationResult.Success, Value.Unknown(StringType));
    }

    public override Result TryIndex(Value self, Value index)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryCall(Value self, List<Value> arguments)
    {
        if (arguments.Count != _parameterTypes.Count)
        {
            DiagnosticReporter.Report(new InvalidArgumentCount(_parameterTypes.Count, arguments.Count));
            return new Result(OperationResult.Failure);
        }

        var hasErroringArguments = false;
        for (var i = 0; i < _parameterTypes.Count; i++)
        {
            if (!arguments[i].DataType.IsCompatible(_parameterTypes[i].DataType))
            {
                DiagnosticReporter.Report(new InvalidArgumentType(arguments[i].DataType, _parameterTypes[i].DataType, i));
                hasErroringArguments = true;
            }
        }
        
        if (hasErroringArguments)
            return new Result(OperationResult.Failure);
        
        return new Result(OperationResult.Success);
    }

    public override bool IsCompatible(DataType other)
    {
        if (other == FunctionBase) return true;
        
        if (other is FunctionType function)
        {
            if (function._parameterTypes.Count != _parameterTypes.Count) return false;
            if (function._returnTypes.Count != _returnTypes.Count) return false;
            
            for (var i = 0; i < _parameterTypes.Count; i++)
                if (!function._parameterTypes[i].DataType.IsCompatible(_parameterTypes[i].DataType))
                    return false;
            
            for (var i = 0; i < _returnTypes.Count; i++)
                if (!_returnTypes[i].DataType.IsCompatible(function._returnTypes[i].DataType))
                    return false;
            
            return true;
        }

        return false;
    }
    
    private bool SameSignatureAs(FunctionType function)
    {
        if (function._parameterTypes.Count != _parameterTypes.Count) return false;

        for (var i = 0; i < _parameterTypes.Count; i++)
        {
            if (function._parameterTypes[i].Nullable != _parameterTypes[i].Nullable) return false;
            if (function._parameterTypes[i].DataType != _parameterTypes[i].DataType) return false;
        }
        
        return true;
    }

    private bool SameReturnTypeAs(FunctionType function)
    {
        if (function._returnTypes.Count != _returnTypes.Count) return false;

        for (var i = 0; i < _returnTypes.Count; i++)
        {
            if (function._returnTypes[i].Nullable != _returnTypes[i].Nullable) return false;
            if (function._returnTypes[i].DataType != _returnTypes[i].DataType) return false;
        }
        
        return true;
    }

    public bool IdenticalTo(FunctionType function)
    {
        return SameSignatureAs(function) && SameReturnTypeAs(function);
    }

    public static FunctionType GenerateType(Function function)
    {
        return new FunctionType(function.ParameterTypes, function.ReturnTypes);
    }
}