using System.Diagnostics.CodeAnalysis;
using Core.SemanticAnalyzer.DataTypes;
using InvalidType = Core.SemanticAnalyzer.DataTypes.InvalidType;

namespace Core.SemanticAnalyzer;

public abstract class DataType
{
    // MetaType is the type of a 'Type'
    public static readonly DataType MetaType = new MetaType();
    
    // To prevent throwing more errors for unknown variables
    public static readonly DataType InvalidType = new InvalidType();
    
    public static readonly DataType VoidType = new VoidType();
    
    public static readonly DataType NumberType = new NumberType();
    public static readonly DataType StringType = new StringType();
    public static readonly DataType BooleanType = new BooleanType();

    public abstract string GetName();
    
    public abstract bool TryOperation(Symbol left, Symbol right, string op, [NotNullWhen(true)] out Symbol? result);
    
    public virtual bool IsCompatible(DataType target)
    {
        return target == this;
    }
}