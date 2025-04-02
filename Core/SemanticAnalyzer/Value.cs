namespace Core.SemanticAnalyzer;

public enum ValueKind
{
    Boolean,
    Number,
    String,
    Null,
    Function,
    DataType,
    Unknown,
    Invalid,
}

public class Value(ValueKind kind, object? value)
{
    public static readonly Value InvalidValue = new(ValueKind.Invalid, null);
    public static readonly Value InvalidType = new(ValueKind.DataType, DataType.InvalidType);
    
    public static readonly Value NullValue = new(ValueKind.Null, null);
    
    public static readonly Value FalseValue = new(ValueKind.Boolean, false);
    public static readonly Value TrueValue = new(ValueKind.Boolean, true);
    
    public readonly ValueKind Kind = kind;
    
    public bool GetBoolean()
    {
        if (Kind != ValueKind.Boolean)
            throw new InvalidOperationException();

        return (bool)value!;
    }

    public double GetNumber()
    {
        if (Kind != ValueKind.Number)
            throw new InvalidOperationException();
        
        return (double)value!;
    }

    public string GetString()
    {
        if (Kind != ValueKind.String)
            throw new InvalidOperationException();
        
        return (string)value!;
    }

    public DataType GetDataType()
    {
        if (Kind != ValueKind.DataType)
            throw new InvalidOperationException();
        
        return (DataType)value!;
    }
}