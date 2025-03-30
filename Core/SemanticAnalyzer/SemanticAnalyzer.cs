using Core.SyntaxTreeConverter;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;
using Core.Utils;

namespace Core.SemanticAnalyzer;

public class SemanticAnalyzer
{
    private readonly Scope _globalScope;
    private Scope _localScope;

    public SemanticAnalyzer()
    {
        _globalScope = new Scope();
        _localScope = new Scope(_globalScope);
        
        _globalScope.Declare("number", new Symbol(new Value(ValueKind.DataType, DataType.NumberType), DataType.MetaType, false));
        _globalScope.Declare("string", new Symbol(new Value(ValueKind.DataType, DataType.StringType), DataType.MetaType, false));
        _globalScope.Declare("bool", new Symbol(new Value(ValueKind.DataType, DataType.BooleanType), DataType.MetaType, false));
    }
    
    public void AnalyzeModule(Module module)
    {
        AnalyzeBlock(module.Block);
    }

    public void AnalyzeBlock(Block block)
    {
        foreach (var statement in block.Statements)
        {
            switch (statement)
            {
                case VariableDeclaration variableDeclaration:
                    AnalyzeVariableDeclaration(variableDeclaration);
                    break;
                default:
                    throw new NotImplementedException($"Unknown statement type {statement.GetType()}");
            }
        }
    }

    public void AnalyzeVariableDeclaration(VariableDeclaration variableDeclaration)
    {
        var scope = variableDeclaration.Local ? _localScope : _globalScope;
        
        var dataTypeSymbol = _localScope.Find(variableDeclaration.DataTypeReference);
        if (dataTypeSymbol == null)
        {
            // TODO: Display and handle errors like these
            throw new Exception($"Type not found: {variableDeclaration.DataTypeReference.Name}");
        }

        if (dataTypeSymbol.DataType != DataType.MetaType)
        {
            // TODO: Display and handle errors like these
            throw new Exception($"{variableDeclaration.DataTypeReference.Name} is not a valid type");
        }
        
        var dataType = dataTypeSymbol.Value.GetDataType();
        
        var defaultSymbol = new Symbol(Value.NullValue, dataType, variableDeclaration.Nullable);

        if (variableDeclaration.Initializer != null)
        {
            var assignedSymbol = AnalyzeExpression(variableDeclaration.Initializer);
            defaultSymbol.Assign(assignedSymbol);
        }

        scope.Declare(variableDeclaration.Identifier, defaultSymbol);
    }

    public Symbol AnalyzeExpression(Expression expression)
    {
        switch (expression)
        {
            case NumberLiteral numberLiteral:
                return new Symbol(new Value(ValueKind.Number, numberLiteral.Value), DataType.NumberType, false);
            case StringLiteral stringLiteral:
                return new Symbol(new Value(ValueKind.String, stringLiteral.Value), DataType.StringType, false);
            case BooleanLiteral booleanLiteral:
                return new Symbol(new Value(ValueKind.Boolean, booleanLiteral.Value), DataType.BooleanType, false);
            case ParenthesizedExpression parenthesizedExpression:
                return AnalyzeExpression(parenthesizedExpression.Expression);
            default:
                throw new NotImplementedException($"Unknown expression type {expression.GetType()}");
        }
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
            DebugMessage.Write("TODO: Show a warning for an always \"falsy\" condition.");
            return resultingSymbols;
        }
        
        // If an 'and' operation's left symbols are only "truthy" then only the right symbols are passed further.
        if (!leftCanBeFalsy)
        {
            // TODO: Show a warning for an always "truthy" condition.
            DebugMessage.Write("TODO: Show a warning for an always \"truthy\" condition.");
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
            DebugMessage.Write("TODO: Show a warning for an always \"truthy\" condition.");
            return resultingSymbols;
        }

        resultingSymbols.AddRange(rightSymbols);
        
        return resultingSymbols;
    }
}