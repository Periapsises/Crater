﻿namespace Core.SemanticAnalyzer.DataTypes;

public class MetaType : DataType
{
    public override string GetName() => "type";
    
    public override Result TryArithmeticOperation(Symbol left, Symbol right, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryLogicOperation(Symbol left, Symbol right, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryUnaryOperation(Symbol self, string op)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryToString(Symbol self)
    {
        return new Result(OperationResult.NotImplemented);
    }

    public override Result TryIndex(Symbol self, Symbol index)
    {
        return new Result(OperationResult.NotImplemented);
    }
}