﻿using System.Globalization;
using Core.Antlr;

namespace Core.SyntaxTreeConverter.Expressions;

public class NumberLiteral: Expression
{
    public readonly string StringRepresentation;
    public readonly double Value;

    public NumberLiteral(CraterParser.LiteralContext context): base(context)
    {
        StringRepresentation = context.number.Text;
        
        if (context.NUMBER() != null)
            Value = double.Parse(context.NUMBER().GetText()!);
        if (context.EXPONENTIAL() != null)
            Value = double.Parse(context.EXPONENTIAL().GetText()!, NumberStyles.AllowExponent);
        if (context.HEXADECIMAL() != null)
            Value = long.Parse(context.HEXADECIMAL().GetText()!.Substring(2), NumberStyles.HexNumber);
        if (context.BINARY() != null)
        {
            Value = long.Parse(context.BINARY().GetText()!.Substring(2), NumberStyles.BinaryNumber);
            StringRepresentation = Value.ToString();
        }
    }
}