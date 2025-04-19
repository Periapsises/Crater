namespace Core.SemanticAnalyzer;

public class PossibleValues : List<Value>
{
    public bool CanBeNull()
    {
        return this.Any(value => value.Kind == ValueKind.Null);
    }

    public bool ValueCanBe(ValueKind kind, object value)
    {
        return this.Any(possibleValue => possibleValue.Kind == kind && possibleValue.GetValue() == value);
    }

    public bool AlwaysTrue()
    {
        foreach (var value in this)
        {
            switch (value.Kind)
            {
                case ValueKind.Boolean when !value.GetBoolean():
                    return false;
                case ValueKind.Boolean:
                    continue;
                case ValueKind.Null:
                    return false;
            }

            if (value.DataType == DataType.BooleanType)
                return false;
        }
        
        return true;
    }

    public bool AlwaysFalse()
    {
        foreach (var value in this)
        {
            if (value.Kind == ValueKind.Boolean)
            {
                if (value.GetBoolean()) return false;
                continue;
            }

            if (value.Kind != ValueKind.Null)
                return false;
            
            if (value.DataType == DataType.BooleanType)
                return false;
        }
        
        return true;
    }

    public Value? Resolve()
    {
        if (Count == 0)
            return null;
        
        if (Count == 1)
            return this[0];

        var commonDataType = this[0].DataType;
        for (var i = 1; i < Count; i++)
        {
            var dataType =  this[i].DataType;
            var newCommonDataType = dataType.FindCommonType(commonDataType);
            
            if (newCommonDataType == null)
                return null;
            
            commonDataType = newCommonDataType;
        }

        return Value.Unknown(commonDataType);
    }
}
