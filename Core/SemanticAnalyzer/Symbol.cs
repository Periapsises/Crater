﻿using System.Diagnostics.CodeAnalysis;

namespace Core.SemanticAnalyzer;

public class Symbol(Value value, DataType dataType, bool nullable)
{
    public static readonly Symbol InvalidSymbol = new(Value.InvalidValue, DataType.InvalidType, false);
    public static readonly Symbol InvalidDataType = new(Value.InvalidType, DataType.MetaType, false);
    
    public Value Value = value;
    public DataType DataType = dataType;
    public bool Nullable = nullable;

    public bool ArithmeticOperation(Symbol other, string op, [NotNullWhen(true)] out Symbol? result)
    {
        return DataType.TryArithmeticOperation(this, other, op, out result);
    }

    public bool LogicOperation(Symbol other, string op, [NotNullWhen(true)] out Symbol? result)
    {
        return DataType.TryLogicOperation(this, other, op, out result);
    }
    
    public bool UnaryOperation(string op, [NotNullWhen(true)] out Symbol? result)
    {
        return DataType.TryUnaryOperation(this, op, out result);
    }

    public bool ToString([NotNullWhen(true)] out Symbol? result)
    {
        return DataType.TryToString(this, out result);
    }
    
    public void Assign(Symbol symbol)
    {
        Value = symbol.Value;
        DataType = symbol.DataType;
        Nullable = symbol.Nullable;
    }

    public bool Index(Symbol indexingSymbol, [NotNullWhen(true)] out Symbol? result)
    {
        return DataType.TryIndex(this, indexingSymbol, out result);
    }

    public static implicit operator PossibleSymbols(Symbol symbol) => [symbol];
}
