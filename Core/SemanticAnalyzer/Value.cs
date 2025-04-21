using Core.SemanticAnalyzer.DataTypes;

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
    Invalid
}

public class Value(DataType dataType, bool nullable, ValueKind kind, object? value)
{
    public static readonly Value InvalidValue = new(DataType.InvalidType, false, ValueKind.Invalid, null);

    public static readonly Value InvalidType =
        new(DataType.InvalidType, false, ValueKind.DataType, DataType.InvalidType);

    public static readonly Value NullValue = new(DataType.InvalidType, true, ValueKind.Null, null);

    public readonly DataType DataType = dataType;
    public readonly ValueKind Kind = kind;
    public readonly bool Nullable = nullable;

    public static Value From(bool value, bool nullable = false)
    {
        return new Value(DataType.BooleanType, nullable, ValueKind.Boolean, value);
    }

    public static Value From(double value, bool nullable = false)
    {
        return new Value(DataType.NumberType, nullable, ValueKind.Number, value);
    }

    public static Value From(string value, bool nullable = false)
    {
        return new Value(DataType.StringType, nullable, ValueKind.String, value);
    }

    public static Value From(Function value, bool nullable = false)
    {
        return new Value(FunctionType.GenerateType(value), nullable, ValueKind.DataType, value);
    }

    public static Value From(DataType value, bool nullable = false)
    {
        return new Value(DataType.MetaType, nullable, ValueKind.DataType, value);
    }

    public static Value Unknown(DataType dataType, bool nullable = false)
    {
        return new Value(dataType, nullable, ValueKind.Unknown, null);
    }

    public Result ArithmeticOperation(Value other, string op)
    {
        return DataType.TryArithmeticOperation(this, other, op);
    }

    public Result LogicOperation(Value other, string op)
    {
        return DataType.TryLogicOperation(this, other, op);
    }

    public Result UnaryOperation(string op)
    {
        return DataType.TryUnaryOperation(this, op);
    }

    public new Result ToString()
    {
        return DataType.TryToString(this);
    }

    public Result Index(Value index)
    {
        return DataType.TryIndex(this, index);
    }

    public Result Call(List<Value> arguments)
    {
        return DataType.TryCall(this, arguments);
    }

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

    public object? GetValue()
    {
        return value;
    }

    public static implicit operator PossibleValues(Value value)
    {
        return [value];
    }
}