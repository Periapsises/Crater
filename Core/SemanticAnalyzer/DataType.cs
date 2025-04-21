using Core.SemanticAnalyzer.DataTypes;
using InvalidType = Core.SemanticAnalyzer.DataTypes.InvalidType;

namespace Core.SemanticAnalyzer;

public abstract class DataType(DataType? parentType)
{
    // MetaType is the type of a 'Type'
    public static readonly DataType MetaType = new MetaType();

    // The base type everything inherits from (Equivalent to 'object' in C#, might name it 'any' in the future.)
    public static readonly DataType BaseType = new BaseType();

    // To prevent throwing more errors for unknown variables
    public static readonly DataType InvalidType = new InvalidType();

    public static readonly DataType NumberType = new NumberType();
    public static readonly DataType StringType = new StringType();
    public static readonly DataType BooleanType = new BooleanType();

    public abstract string GetName();

    public abstract Result TryArithmeticOperation(Value left, Value right, string op);

    public abstract Result TryLogicOperation(Value left, Value right, string op);

    public abstract Result TryUnaryOperation(Value self, string op);

    public abstract Result TryToString(Value self);

    public abstract Result TryIndex(Value self, Value index);

    public abstract Result TryCall(Value self, List<Value> arguments);

    public virtual bool IsCompatible(DataType target)
    {
        if (target == this) return true;
        return parentType != null && parentType.IsCompatible(target);
    }

    public virtual DataType? FindCommonType(DataType other)
    {
        return other == this ? this : parentType?.FindCommonType(other);
    }
}

public enum OperationResult
{
    Success,
    Failure,
    NotImplemented,
    InvalidArgument
}

public struct Result(OperationResult operationResult, Value? value = null, int argumentIndex = 0)
{
    public readonly OperationResult OperationResult = operationResult;
    public readonly Value? Value = value;
    public readonly int ArgumentIndex = argumentIndex;
}

public struct TypeUsage(DataType dataType, bool nullable)
{
    public readonly DataType DataType = dataType;
    public readonly bool Nullable = nullable;
}