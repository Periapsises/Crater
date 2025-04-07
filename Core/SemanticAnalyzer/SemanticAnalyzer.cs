using Antlr4.Runtime;
using Core.Antlr;
using Core.SemanticAnalyzer.DataTypes;
using Core.SemanticAnalyzer.Diagnostics;
using Core.SyntaxTreeConverter;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;
using Expression = Core.SyntaxTreeConverter.Expression;
using InvalidType = Core.SemanticAnalyzer.Diagnostics.InvalidType;

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

        _globalScope.Declare("number",
            new Symbol(new Value(ValueKind.DataType, DataType.NumberType), DataType.MetaType, false));
        _globalScope.Declare("string",
            new Symbol(new Value(ValueKind.DataType, DataType.StringType), DataType.MetaType, false));
        _globalScope.Declare("bool",
            new Symbol(new Value(ValueKind.DataType, DataType.BooleanType), DataType.MetaType, false));
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
                case FunctionDeclaration functionDeclaration:
                    AnalyzeFunctionDeclaration(functionDeclaration);
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
            defaultSymbol.Assign(
                ResolveSymbols(assignedSymbols, defaultSymbol, variableDeclaration.Identifier, context));
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

    public void AnalyzeFunctionDeclaration(FunctionDeclaration functionDeclaration)
    {
        var scope = functionDeclaration.Local ? _localScope : _globalScope;

        var returnTypeSymbol = _localScope.Find(functionDeclaration.ReturnTypeReference);
        if (returnTypeSymbol == null)
        {
            Reporter.Report(new TypeNotFound(functionDeclaration.ReturnTypeReference)
                .WithContext(((CraterParser.FunctionDeclarationContext)functionDeclaration.Context).typeName()));
            returnTypeSymbol = Symbol.InvalidDataType;
        }

        if (returnTypeSymbol.DataType != DataType.MetaType)
        {
            Reporter.Report(new InvalidType(functionDeclaration.ReturnTypeReference)
                .WithContext(((CraterParser.FunctionDeclarationContext)functionDeclaration.Context).typeName()));
        }

        var returnType = returnTypeSymbol.Value.GetDataType();

        List<DataType> parameters = [];

        foreach (var parameter in functionDeclaration.Parameters)
        {
            var parameterTypeSymbol = _localScope.Find(parameter.DataTypeReference);
            if (parameterTypeSymbol == null)
            {
                Reporter.Report(new TypeNotFound(parameter.DataTypeReference)
                    .WithContext(((CraterParser.FunctionParameterContext)parameter.Context).typeName()));
                parameterTypeSymbol = Symbol.InvalidDataType;
            }

            if (parameterTypeSymbol.DataType != DataType.MetaType)
            {
                Reporter.Report(new InvalidType(parameter.DataTypeReference)
                    .WithContext(((CraterParser.FunctionParameterContext)parameter.Context).typeName()));
            }

            parameters.Add(parameterTypeSymbol.Value.GetDataType());
        }

        if (scope.HasVariable(functionDeclaration.Identifier))
        {
            Reporter.Report(new VariableShadowing(functionDeclaration.Identifier)
                .WithContext(((CraterParser.FunctionDeclarationContext)functionDeclaration.Context).IDENTIFIER()));
        }

        var functionType = new FunctionType(parameters, [returnType]);
        var functionSymbol = new Symbol(new Value(ValueKind.Function, null), functionType, false);

        scope.Declare(functionDeclaration.Identifier, functionSymbol);

        var functionScope = new Scope(_localScope);

        for (var i = 0; i < functionDeclaration.Parameters.Count; i++)
        {
            var parameterSymbol = new Symbol(new Value(ValueKind.Unknown, null), parameters[i],
                functionDeclaration.Parameters[i].Nullable);
            functionScope.Declare(functionDeclaration.Parameters[i].Name, parameterSymbol);
        }

        var previousScope = _localScope;
        _localScope = functionScope;
        AnalyzeBlock(functionDeclaration.Block);
        _localScope = previousScope;
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
            case UnaryOperation unaryOperation:
                return AnalyzeUnaryOperation(unaryOperation);
            case LogicalOperation logicalOperation:
                return AnalyzeLogicalOperation(logicalOperation);
            case VariableReference variableReference:
                return AnalyzeVariableReference(variableReference);
            default:
                throw new NotImplementedException($"Unknown expression type {expression.GetType()}");
        }
    }

    public PossibleSymbols AnalyzeUnaryOperation(UnaryOperation unaryOperation)
    {
        var expression = AnalyzeExpression(unaryOperation.Expression);

        switch (unaryOperation.Operator)
        {
            case "-":
                return AnalyzeUnaryOperation(expression, "-", "__umn",
                    ((CraterParser.UnaryOperationContext)unaryOperation.Context).MINUS().Symbol);
            default:
                throw new NotImplementedException($"Unknown unary operator {unaryOperation.Operator}");
        }
    }

    public PossibleSymbols AnalyzeUnaryOperation(PossibleSymbols symbols, string op, string meta, IToken token)
    {
        var resultingSymbols = new PossibleSymbols();
        var hasErroredForTypes = new HashSet<DataType>();

        foreach (var symbol in symbols)
        {
            if (symbol.UnaryOperation(meta, out var result))
            {
                resultingSymbols.Add(result);
                continue;
            }

            if (hasErroredForTypes.Contains(symbol.DataType))
                continue;

            Reporter.Report(new InvalidUnaryOperator(symbol.DataType, op)
                .WithContext(token));
            hasErroredForTypes.Add(symbol.DataType);
        }

        return resultingSymbols;
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
                if (_arithmeticOperators.TryGetValue(binaryOperation.Operator, out var operatorInfo))
                    return PerformArithmeticOperation(left, right, operatorInfo)
                        .WithContext(binaryOperation.OpToken)
                        .ReportTo(Reporter);
                throw new NotImplementedException($"Unknown binary operator {binaryOperation.Operator}");
        }
    }

    public PossibleSymbols AnalyzeLogicalOperation(LogicalOperation logicalOperation)
    {
        var left = AnalyzeExpression(logicalOperation.Left);
        var right = AnalyzeExpression(logicalOperation.Right);

        if (!_logicOperators.TryGetValue(logicalOperation.Operator, out var operatorInfo))
            throw new NotImplementedException($"Unknown logical operator {logicalOperation.Operator}");

        return PerformLogicOperation(left, right, operatorInfo)
            .WithContext(logicalOperation.OpToken)
            .ReportTo(Reporter);
    }

    public PossibleSymbols AnalyzeAndOperation(PossibleSymbols leftSymbols, PossibleSymbols rightSymbols,
        BinaryOperation binaryOperation)
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
            Reporter.Report(
                new AndAlwaysFalse().WithContext(
                    ((CraterParser.AndOperationContext)binaryOperation.Context).expression()[0]));
            return resultingSymbols;
        }

        // If an 'and' operation's left symbols are only "truthy" then only the right symbols are passed further.
        if (!leftCanBeFalsy)
        {
            Reporter.Report(
                new AndAlwaysTrue().WithContext(
                    ((CraterParser.AndOperationContext)binaryOperation.Context).expression()[0]));
            return rightSymbols;
        }

        resultingSymbols.AddRange(rightSymbols);

        return resultingSymbols;
    }

    public PossibleSymbols AnalyzeOrOperation(PossibleSymbols leftSymbols, PossibleSymbols rightSymbols,
        BinaryOperation binaryOperation)
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
                leftCanBeTruthy = true;

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

        if (!leftCanBeTruthy)
        {
            Reporter.Report(
                new OrAlwaysFalse().WithContext(
                    ((CraterParser.OrOperationContext)binaryOperation.Context).expression()[0]));
        }

        if (!leftCanBeFalsy)
        {
            Reporter.Report(
                new OrAlwaysTrue().WithContext(
                    ((CraterParser.OrOperationContext)binaryOperation.Context).expression()[0]));
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
            Reporter.Report(new VariableNotFound(variableReference).WithContext(
                ((CraterParser.VariableReferenceContext)variableReference.Context).IDENTIFIER()));
            return new Symbol(new Value(ValueKind.Unknown, null), DataType.InvalidType, false);
        }

        return symbol;
    }

    private Symbol ResolveSymbols(PossibleSymbols possibleSymbols, Symbol target, string variable, IToken context)
    {
        var resultingSymbols = new PossibleSymbols();

        var hasErroredForType = new HashSet<DataType> { DataType.InvalidType };

        var hasErroredForNullable = false;

        foreach (var symbol in possibleSymbols)
        {
            var hasError = false;
            if (!hasErroredForType.Contains(symbol.DataType) && !symbol.DataType.IsCompatible(target.DataType))
            {
                Reporter.Report(new TypeMismatch(symbol.DataType, target.DataType).WithContext(context));
                hasErroredForType.Add(symbol.DataType);
                hasError = true;
            }

            if (!hasErroredForNullable && symbol.Nullable && !target.Nullable)
            {
                Reporter.Report(new PossibleNullAssignment(variable));
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

    private struct OperatorInformation(string @operator, string metamethod, bool swapOperands = false, bool negateResult = false)
    {
        public readonly string Operator = @operator;
        public readonly string Metamethod = metamethod;
        public readonly bool SwapOperands = swapOperands;
        public readonly bool NegateResult = negateResult;
    }

    private static readonly Dictionary<string, OperatorInformation> _arithmeticOperators = new()
    {
        { "+", new OperatorInformation("+", "__add") },
        { "-", new OperatorInformation("-", "__sub") },
        { "*", new OperatorInformation("*", "__mul") },
        { "/", new OperatorInformation("/", "__div") },
        { "%", new OperatorInformation("%", "__mod") },
        { "^", new OperatorInformation("^", "__pow") },
        { "..", new OperatorInformation("..", "__concat") },
    };

    private static readonly Dictionary<string, OperatorInformation> _logicOperators = new()
    {
        { "==", new OperatorInformation("==", "__eq", false, false) },
        { "~=", new OperatorInformation("~=", "__eq", false, true) },
        { "<", new OperatorInformation("<", "__lt", false, false) },
        { ">", new OperatorInformation(">", "__lt", true, false) },
        { "<=", new OperatorInformation("<=", "__le", false, false) },
        { ">=", new OperatorInformation(">=", "__le", true, false) }
    };

    private DiagnosticReport<PossibleSymbols> PerformArithmeticOperation(PossibleSymbols leftSymbols,
        PossibleSymbols rightSymbols, OperatorInformation opInfo)
    {
        var diagnostics = new DiagnosticReport<PossibleSymbols>();

        var resultingSymbols = new PossibleSymbols();
        var hasErroredForTypes = new HashSet<(DataType, DataType)>();

        foreach (var leftSymbol in leftSymbols)
        {
            foreach (var rightSymbol in rightSymbols)
            {
                if (leftSymbol.ArithmeticOperation(rightSymbol, opInfo.Metamethod, out var result))
                {
                    resultingSymbols.Add(result);
                    continue;
                }

                if (hasErroredForTypes.Contains((leftSymbol.DataType, rightSymbol.DataType)))
                    continue;

                diagnostics.Report(
                    new InvalidBinaryOperator(leftSymbol.DataType, rightSymbol.DataType, opInfo.Operator));
                hasErroredForTypes.Add((leftSymbol.DataType, rightSymbol.DataType));
            }
        }

        diagnostics.Data = resultingSymbols;
        return diagnostics;
    }
    
    private DiagnosticReport<PossibleSymbols> PerformLogicOperation(PossibleSymbols leftSymbols,
        PossibleSymbols rightSymbols, OperatorInformation opInfo)
    {
        var diagnostics = new DiagnosticReport<PossibleSymbols>();

        if (opInfo.SwapOperands)
            (leftSymbols, rightSymbols) = (rightSymbols, leftSymbols);

        var resultingSymbols = new PossibleSymbols();
        var hasErroredForTypes = new HashSet<(DataType, DataType)>();

        foreach (var leftSymbol in leftSymbols)
        {
            foreach (var rightSymbol in rightSymbols)
            {
                if (leftSymbol.LogicOperation(rightSymbol, opInfo.Metamethod, out var result))
                {
                    resultingSymbols.Add(result);
                    continue;
                }

                if (hasErroredForTypes.Contains((leftSymbol.DataType, rightSymbol.DataType)))
                    continue;

                diagnostics.Report(
                    new InvalidBinaryOperator(leftSymbol.DataType, rightSymbol.DataType, opInfo.Operator));
                hasErroredForTypes.Add((leftSymbol.DataType, rightSymbol.DataType));
            }
        }

        if (opInfo.NegateResult)
        {
            var negatedSymbols = new PossibleSymbols();

            foreach (var symbol in resultingSymbols)
            {
                if (symbol.Value.Kind == ValueKind.Boolean)
                {
                    var negatedValue = !symbol.Value.GetBoolean();
                    negatedSymbols.Add(new Symbol(new Value(ValueKind.Boolean, negatedValue), DataType.BooleanType,
                        false));
                }
                else if (symbol.Value.Kind == ValueKind.Null)
                {
                    negatedSymbols.Add(new Symbol(new Value(ValueKind.Boolean, true), DataType.BooleanType, false));
                }
                else if (symbol.DataType == DataType.BooleanType || symbol.Nullable)
                {
                    negatedSymbols.Add(new Symbol(new Value(ValueKind.Unknown, null), DataType.BooleanType, false));
                }
                else
                {
                    // Any other object in Lua that is subjected to the `not` operator will be false
                    negatedSymbols.Add(new Symbol(new Value(ValueKind.Boolean, false), DataType.BooleanType, false));
                }
            }

            resultingSymbols = negatedSymbols;
        }

        diagnostics.Data = resultingSymbols;
        return diagnostics;
    }
}