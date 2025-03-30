namespace Core.SemanticAnalyzer;

public class DataType
{
    // MetaType is the type of a 'Type'
    public static readonly DataType MetaType = new();
    
    public static readonly DataType NumberType = new();
    public static readonly DataType StringType = new();
    public static readonly DataType BooleanType = new();
}