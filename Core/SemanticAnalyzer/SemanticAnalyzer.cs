using Core.SyntaxTreeConverter;

namespace Core.SemanticAnalyzer;

public class SemanticAnalyzer
{
    public void AnalyzeModule(Module module)
    {
    }
    
    public PossibleSymbols AnalyzeAndOperation(PossibleSymbols leftSymbols, PossibleSymbols rightSymbols)
    {
        var leftCanBeTruthy = false;
        var leftCanBeFalsy = false;
        
        var resultingSymbols = new PossibleSymbols();

        foreach (var symbol in leftSymbols)
        {
            // If we know the actual value of the symbol, and it is a boolean then we can determine if we need to add the symbol.
            if (symbol.Value.Kind == ValueKind.Boolean)
            {
                var isTruthy = symbol.Value.GetBoolean();
                leftCanBeTruthy |= isTruthy;
                leftCanBeFalsy |= !isTruthy;

                if (!isTruthy)
                    resultingSymbols.Add(symbol);
            }
            else if (symbol.Value.Kind == ValueKind.Null)
            {
                leftCanBeFalsy = true;
                resultingSymbols.Add(symbol);
            }
            // If the symbol is an unknown boolean, an and operation causes only "falsy" values to pass further.
            // This means if we have a non-nil boolean, we can determine it will be added only if it is `false`.
            else if (symbol.DataType == DataType.BooleanType)
            {
                leftCanBeTruthy = true;
                leftCanBeFalsy = true;
                resultingSymbols.Add(new Symbol(Value.FalseValue, symbol.DataType, false));
            }
            else
            {
                leftCanBeTruthy = true;
            }
            
            // If the symbol is nullable, then one of the possibilities is a `nil` value.
            if (symbol.Nullable && symbol.Value.Kind != ValueKind.Null)
            {
                leftCanBeTruthy = true;
                leftCanBeFalsy = true;
                resultingSymbols.Add(new Symbol(Value.NullValue, symbol.DataType, true));
            }
        }

        // If an 'and' operation's left symbols are only "falsy" then they are the only symbols passed further.
        if (!leftCanBeTruthy)
        {
            // TODO: Show a warning for an always "falsy" condition.
            Console.WriteLine("And condition: Left operand is always evaluated as falsy");
            return resultingSymbols;
        }
        
        // If an 'and' operation's left symbols are only "truthy" then only the right symbols are passed further.
        if (!leftCanBeFalsy)
        {
            // TODO: Show a warning for an always "truthy" condition.
            Console.WriteLine("And condition: Left operand is always evaluated as truthy");
            return rightSymbols;
        }

        resultingSymbols.AddRange(rightSymbols);
        
        return resultingSymbols;
    }

    public PossibleSymbols AnalyzeOrOperation( PossibleSymbols leftSymbols, PossibleSymbols rightSymbols )
    {
        var leftCanBeFalsy = false;

        var resultingSymbols = new PossibleSymbols();

        foreach (var symbol in leftSymbols)
        {
            if (symbol.Value.Kind == ValueKind.Boolean)
            {
                var isTruthy = symbol.Value.GetBoolean();
                leftCanBeFalsy |= !isTruthy;
                
                if (isTruthy)
                    resultingSymbols.Add(symbol);
            }
            else if (symbol.Value.Kind == ValueKind.Null)
            {
                leftCanBeFalsy = true;
            }
            else
            {
                // If the symbol is an unknown boolean, an or operation causes only "truthy" values to pass further.
                // This means if we have a non-nil boolean, we can determine it will be added only if it is `true`.
                if (symbol.DataType == DataType.BooleanType)
                {
                    leftCanBeFalsy = true;
                    resultingSymbols.Add(new Symbol(Value.TrueValue, symbol.DataType, false));
                }
                // If the symbol is nullable, add a non-nullable version because 'or' will filter out `nil` from the left operand.
                else if (symbol.Nullable)
                {
                    leftCanBeFalsy = true;
                    resultingSymbols.Add(new Symbol(symbol.Value, symbol.DataType, false));
                }
                else
                {
                    resultingSymbols.Add(symbol);
                }
            }
        }

        if (!leftCanBeFalsy)
        {
            // TODO: Show a warning for an always "truthy" condition.
            Console.WriteLine("Or condition: Left operand is always evaluated as truthy");
            return resultingSymbols;
        }

        resultingSymbols.AddRange(rightSymbols);
        
        return resultingSymbols;
    }
}