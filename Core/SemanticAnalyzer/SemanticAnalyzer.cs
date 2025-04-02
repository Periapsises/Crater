using Antlr4.Runtime;
using Core.Antlr;
using Core.SemanticAnalyzer.Diagnostics;
using Core.SyntaxTreeConverter;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;
using Expression = Core.SyntaxTreeConverter.Expression;

namespace Core.SemanticAnalyzer;

public class SemanticAnalyzer
{
    public readonly DiagnosticReporter Reporter;
    
    private readonly Scope _globalScope;
    private Scope _localScope;

    public SemanticAnalyzer()
    {
        DiagnosticReporter.CurrentReporter = new DiagnosticReporter();
        Reporter = DiagnosticReporter.CurrentReporter;
        
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
        
        var dataTypeSymbol = _localScope.Find(variableDeclaration.VariableReference);
        if (dataTypeSymbol == null)
        {
            Reporter.Report(new TypeNotFound(variableDeclaration.VariableReference)
                .WithContext(((CraterParser.VariableDeclarationContext)variableDeclaration.Context).typeName()));
            return;
        }

        if (dataTypeSymbol.DataType != DataType.MetaType)
        {
            Reporter.Report(new InvalidType(variableDeclaration.VariableReference)
                .WithContext(((CraterParser.VariableDeclarationContext)variableDeclaration.Context).typeName()));
            return;
        }
        
        var dataType = dataTypeSymbol.Value.GetDataType();
        
        var defaultSymbol = new Symbol(Value.NullValue, dataType, variableDeclaration.Nullable);

        if (variableDeclaration.Initializer != null)
        {
            var assignedSymbols = AnalyzeExpression(variableDeclaration.Initializer);

            var context = ((CraterParser.VariableDeclarationContext)variableDeclaration.Context).ASSIGN().Symbol;
            defaultSymbol.Assign(ResolveSymbols(assignedSymbols, defaultSymbol, variableDeclaration.Identifier, context));
        }
        else if (!variableDeclaration.Nullable)
        {
            Reporter.Report(new UninitializedNonNullable(variableDeclaration.Identifier)
                .WithContext(((CraterParser.VariableDeclarationContext)variableDeclaration.Context).IDENTIFIER()));
        }

        if (scope.HasVariable(variableDeclaration.Identifier))
        {
            Reporter.Report(new VariableShadowing(variableDeclaration.Identifier)
                .WithContext(((CraterParser.VariableDeclarationContext)variableDeclaration.Context).IDENTIFIER()));
        }
        
        scope.Declare(variableDeclaration.Identifier, defaultSymbol);
    }

    public PossibleSymbols AnalyzeExpression(Expression expression)
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
            case BinaryOperation binaryOperation:
                return AnalyzeBinaryOperation(binaryOperation);
            case VariableReference variableReference:
                return AnalyzeVariableReference(variableReference);
            default:
                throw new NotImplementedException($"Unknown expression type {expression.GetType()}");
        }
    }

    public PossibleSymbols AnalyzeBinaryOperation(BinaryOperation binaryOperation)
    {
        var left = AnalyzeExpression(binaryOperation.Left);
        var right = AnalyzeExpression(binaryOperation.Right);
        
        switch (binaryOperation.Operator)
        {
            case "and":
                return AnalyzeAndOperation(left, right, binaryOperation);
            case "or":
                return AnalyzeOrOperation(left, right, binaryOperation);
            default:
                throw new NotImplementedException($"Unknown binary operator {binaryOperation.Operator}");
        }
    }
    
    public PossibleSymbols AnalyzeAndOperation(PossibleSymbols leftSymbols, PossibleSymbols rightSymbols, BinaryOperation binaryOperation)
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
#if !TESTING
            Reporter.Report(new AndAlwaysFalse().WithContext(((CraterParser.AndOperationContext)binaryOperation.Context).expression()[0]));
#endif
            return resultingSymbols;
        }
        
        // If an 'and' operation's left symbols are only "truthy" then only the right symbols are passed further.
        if (!leftCanBeFalsy)
        {
#if !TESTING
            Reporter.Report(new AndAlwaysTrue().WithContext(((CraterParser.AndOperationContext)binaryOperation.Context).expression()[0]));
#endif
            return rightSymbols;
        }

        resultingSymbols.AddRange(rightSymbols);
        
        return resultingSymbols;
    }

    public PossibleSymbols AnalyzeOrOperation(PossibleSymbols leftSymbols, PossibleSymbols rightSymbols, BinaryOperation binaryOperation)
    {
        var leftCanBeTruthy = false;
        var leftCanBeFalsy = false;

        var resultingSymbols = new PossibleSymbols();

        foreach (var symbol in leftSymbols)
        {
            if (symbol.Value.Kind == ValueKind.Boolean)
            {
                var isTruthy = symbol.Value.GetBoolean();
                leftCanBeTruthy |= isTruthy;
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
                    leftCanBeTruthy = true;
                    leftCanBeFalsy = true;
                    resultingSymbols.Add(new Symbol(Value.TrueValue, symbol.DataType, false));
                }
                // If the symbol is nullable, add a non-nullable version because 'or' will filter out `nil` from the left operand.
                else if (symbol.Nullable)
                {
                    leftCanBeTruthy = true;
                    leftCanBeFalsy = true;
                    resultingSymbols.Add(new Symbol(symbol.Value, symbol.DataType, false));
                }
                else
                {
                    resultingSymbols.Add(symbol);
                }
            }
        }

        if (!leftCanBeTruthy)
        {
#if !TESTING
            Reporter.Report(new OrAlwaysFalse().WithContext(((CraterParser.OrOperationContext)binaryOperation.Context).expression()[0]));
#endif
        }
        
        if (!leftCanBeFalsy)
        {
#if !TESTING
            Reporter.Report(new OrAlwaysTrue().WithContext(((CraterParser.OrOperationContext)binaryOperation.Context).expression()[0]));
#endif
            return resultingSymbols;
        }

        resultingSymbols.AddRange(rightSymbols);
        
        return resultingSymbols;
    }

    public Symbol AnalyzeVariableReference(VariableReference variableReference)
    {
        var symbol = _localScope.Find(variableReference);
        if (symbol == null)
        {
            Reporter.Report(new VariableNotFound(variableReference).WithContext(((CraterParser.VariableReferenceContext)variableReference.Context).IDENTIFIER()));
            return new Symbol(new Value(ValueKind.Unknown, null), DataType.InvalidType, false);
        }
        
        return symbol;
    }
    
    private Symbol ResolveSymbols(PossibleSymbols possibleSymbols, Symbol target, string variable, IToken context)
    {
        var resultingSymbols = new PossibleSymbols();
        
        var hasErroredForType = new HashSet<DataType>();
        var hasErroredForNullable = false;
        
        foreach (var symbol in possibleSymbols)
        {
            var hasError = false;
            if (!hasErroredForType.Contains(symbol.DataType) && !symbol.DataType.IsCompatible(target.DataType))
            {
#if !TESTING
                Reporter.Report(new TypeMismatch(symbol.DataType, target.DataType).WithContext(context));
#endif
                hasErroredForType.Add(symbol.DataType);
                hasError = true;
            }

            if (!hasErroredForNullable && symbol.Nullable && !target.Nullable)
            {
#if !TESTING
                Reporter.Report(new PossibleNullAssignment(variable));
#endif
                hasErroredForNullable = true;
                hasError = true;
            }
            
            if (!hasError)
                resultingSymbols.Add(symbol);
        }
        
        // TODO: There's a lot more to be done to ensure the compiler keeps enough information such as checking for same values
        if (resultingSymbols.Count == 1)
            return resultingSymbols.Single();
        
        return new Symbol(new Value(ValueKind.Unknown, null), target.DataType, target.Nullable);
    }
}