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

public class Value
{
    public static readonly Value InvalidValue = new(ValueKind.Invalid, null);
    public static readonly Value InvalidType = new(ValueKind.DataType, DataType.InvalidType);
    
    public static readonly Value NullValue = new(ValueKind.Null, null);
    
    public static readonly Value FalseValue = new(ValueKind.Boolean, false);
    public static readonly Value TrueValue = new(ValueKind.Boolean, true);
    
    public readonly ValueKind Kind;
    private readonly object? _value;

    public static Value From(bool value) => new(ValueKind.Boolean, value);
    public static Value From(double value) => new(ValueKind.Number, value);
    public static Value From(string value) => new(ValueKind.String, value);
    public static Value From(DataType value) => new(ValueKind.DataType, value);
    public static Value Unknown => new(ValueKind.Unknown, null);
    
    public Value(ValueKind kind, object? value)
    {
        Kind = kind;
        _value = value;
    }
    
    public bool GetBoolean()
    {
        if (Kind != ValueKind.Boolean)
            throw new InvalidOperationException();

        return (bool)_value!;
    }

    public double GetNumber()
    {
        if (Kind != ValueKind.Number)
            throw new InvalidOperationException();
        
        return (double)_value!;
    }

    public string GetString()
    {
        if (Kind != ValueKind.String)
            throw new InvalidOperationException();
        
        return (string)_value!;
    }

    public DataType GetDataType()
    {
        if (Kind != ValueKind.DataType)
            throw new InvalidOperationException();
        
        return (DataType)_value!;
    }
}