using Core.SemanticAnalyzer.DataTypes;
using InvalidType = Core.SemanticAnalyzer.DataTypes.InvalidType;

namespace Core.SemanticAnalyzer;

public abstract class DataType
{
    // MetaType is the type of a 'Type'
    public static readonly DataType MetaType = new MetaType();
    
    // To prevent throwing more errors for unknown variables
    public static readonly DataType InvalidType = new InvalidType();
    
    public static readonly DataType NumberType = new NumberType();
    public static readonly DataType StringType = new StringType();
    public static readonly DataType BooleanType = new BooleanType();

    public abstract string GetName();
    
    public abstract Result TryArithmeticOperation(Symbol left, Symbol right, string op);
    
    public abstract Result TryLogicOperation(Symbol left, Symbol right, string op);
    
    public abstract Result TryUnaryOperation(Symbol self, string op);
    
    public abstract Result TryToString(Symbol self);
    
    public abstract Result TryIndex(Symbol self, Symbol index);
    
    public virtual bool IsCompatible(DataType target)
    {
        return target == this;
    }
}

public enum OperationResult
{
    Success,
    NotImplemented,
    InvalidArgument,
}

public struct Result(OperationResult operationResult, Symbol? symbol = null, int argumentIndex = 0)
{
    public readonly OperationResult OperationResult = operationResult;
    public readonly Symbol? Symbol = symbol;
    public readonly int ArgumentIndex = argumentIndex;
}
