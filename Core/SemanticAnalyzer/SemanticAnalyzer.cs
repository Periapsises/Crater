using Antlr4.Runtime;
using Core.Diagnostics;
using Core.Diagnostics.TypeErrors;
using Core.SyntaxTreeConverter;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;
using Expression = Core.SyntaxTreeConverter.Expression;
using FunctionType = Core.SemanticAnalyzer.DataTypes.FunctionType;

namespace Core.SemanticAnalyzer;

public class SemanticAnalyzer(IDiagnosticReporter reporter)
{
    private static readonly Dictionary<string, OperatorInformation> _arithmeticOperators = new()
    {
        { "+", new OperatorInformation("+", "__add") },
        { "-", new OperatorInformation("-", "__sub") },
        { "*", new OperatorInformation("*", "__mul") },
        { "/", new OperatorInformation("/", "__div") },
        { "%", new OperatorInformation("%", "__mod") },
        { "^", new OperatorInformation("^", "__pow") },
        { "..", new OperatorInformation("..", "__concat") }
    };

    private static readonly Dictionary<string, OperatorInformation> _logicOperators = new()
    {
        { "==", new OperatorInformation("==", "__eq") },
        { "~=", new OperatorInformation("~=", "__eq", false, true) },
        { "<", new OperatorInformation("<", "__lt") },
        { ">", new OperatorInformation(">", "__lt", true) },
        { "<=", new OperatorInformation("<=", "__le") },
        { ">=", new OperatorInformation(">=", "__le", true) }
    };

    public void AnalyzeModule(Module module)
    {
        Environment.SetupEnvironment();
        Environment.EnterModuleScope("main");

        Environment.DeclareGlobal("number", new Symbol(Value.From(DataType.NumberType), DataType.MetaType, false));
        Environment.DeclareGlobal("string", new Symbol(Value.From(DataType.StringType), DataType.MetaType, false));
        Environment.DeclareGlobal("bool", new Symbol(Value.From(DataType.BooleanType), DataType.MetaType, false));

        AnalyzeBlock(module.Block);

        Environment.ExitModuleScope();
    }

    private void AnalyzeBlock(Block block)
    {
        using var _ = new ScopedContext(block.Context);

        foreach (var statement in block.Statements)
            switch (statement)
            {
                case VariableDeclaration variableDeclaration:
                    AnalyzeVariableDeclaration(variableDeclaration);
                    break;
                case FunctionDeclaration functionDeclaration:
                    AnalyzeFunctionDeclaration(functionDeclaration);
                    break;
                case IfStatement ifStatement:
                    AnalyzeIfStatement(ifStatement);
                    break;
                case FunctionCallStatement functionCallStatement:
                    AnalyzeFunctionCallStatement(functionCallStatement);
                    break;
                default:
                    throw new NotImplementedException($"Unknown statement type {statement.GetType()}");
            }
    }

    private void AnalyzeVariableDeclaration(VariableDeclaration variableDeclaration)
    {
        using var _ = new ScopedContext(variableDeclaration.Context);

        var scope = variableDeclaration.Local ? Environment.GetLocalScope() : Environment.GetGlobalScope();
        var dataType = GetDataTypeFromReference(variableDeclaration.DataTypeReference);

        var defaultSymbol = new Symbol(Value.NullValue, dataType, variableDeclaration.DataTypeReference.Nullable);

        if (variableDeclaration.Initializer != null)
        {
            var possibleValues = AnalyzeExpression(variableDeclaration.Initializer);
            var assignedValue = possibleValues.Resolve();
            if (assignedValue == null)
            {
                reporter.Report(new TypeResolutionFailure(variableDeclaration.Initializer)
                    .UseLocation(variableDeclaration.Context.initializer));
                assignedValue = Value.InvalidValue;
            }

            if (!assignedValue.DataType.IsCompatible(dataType))
            {
                reporter.Report(new TypeMismatch(assignedValue.DataType, dataType)
                    .UseLocation(variableDeclaration.Context.initializer));
                assignedValue = Value.InvalidValue;
            }

            defaultSymbol.Assign(assignedValue);
        }
        else if (!variableDeclaration.DataTypeReference.Nullable)
        {
            reporter.Report(new NullableTypeMismatch(dataType)
                .UseLocation(variableDeclaration.Context.type));
        }

        if (scope.HasSymbol(variableDeclaration.Identifier))
            reporter.Report(new VariableShadowing(variableDeclaration.Identifier)
                .WithContext(variableDeclaration.Context.IDENTIFIER()));

        scope.Declare(variableDeclaration.Identifier, defaultSymbol);
    }

    private void AnalyzeFunctionDeclaration(FunctionDeclaration functionDeclaration)
    {
        using var _ = new ScopedContext(functionDeclaration.Context);

        var scope = functionDeclaration.Local ? Environment.GetLocalScope() : Environment.GetGlobalScope();

        var returnTypes = new List<TypeUsage>();
        foreach (var returnDeclaration in functionDeclaration.Returns)
        {
            var returnType = GetDataTypeFromReference(returnDeclaration);
            returnTypes.Add(new TypeUsage(returnType, returnDeclaration.Nullable));
        }

        var parameters = new List<TypeUsage>();
        foreach (var parameter in functionDeclaration.Parameters)
        {
            var parameterType = GetDataTypeFromReference(parameter.DataTypeReference);
            parameters.Add(new TypeUsage(parameterType, parameter.DataTypeReference.Nullable));
        }

        if (scope.HasSymbol(functionDeclaration.Identifier))
            reporter.Report(new VariableShadowing(functionDeclaration.Identifier)
                .WithContext(functionDeclaration.Context.IDENTIFIER()));

        var functionValue = Value.From(new Function(parameters, returnTypes));
        var functionSymbol = new Symbol(functionValue, functionValue.DataType, false);

        scope.Declare(functionDeclaration.Identifier, functionSymbol);

        var functionScope = Environment.CreateSubScope();

        for (var i = 0; i < functionDeclaration.Parameters.Count; i++)
        {
            var parameterSymbol = new Symbol(Value.Unknown(parameters[i].DataType, parameters[i].Nullable),
                parameters[i].DataType, functionDeclaration.Parameters[i].DataTypeReference.Nullable);
            functionScope.Declare(functionDeclaration.Parameters[i].Name, parameterSymbol);
        }

        Environment.EnterScope(functionScope);
        AnalyzeBlock(functionDeclaration.Block);
        Environment.ExitScope();
    }

    private void AnalyzeIfStatement(IfStatement ifStatement)
    {
        using var _ = new ScopedContext(ifStatement.Context);

        var symbols = AnalyzeExpression(ifStatement.Condition);
        if (symbols.AlwaysTrue())
            reporter.Report(new ConditionAlwaysTrue().WithContext(ifStatement.Context.condition));

        if (symbols.AlwaysFalse())
            reporter.Report(new ConditionAlwaysFalse().WithContext(ifStatement.Context.condition));

        Environment.EnterScope(Environment.CreateSubScope());
        AnalyzeBlock(ifStatement.Block);
        Environment.ExitScope();

        foreach (var elseIfStatement in ifStatement.ElseIfStatements)
            AnalyzeElseIfStatement(elseIfStatement);

        if (ifStatement.ElseStatement != null)
            AnalyzeElseStatement(ifStatement.ElseStatement);
    }

    private void AnalyzeElseIfStatement(ElseIfStatement elseIfStatement)
    {
        using var _ = new ScopedContext(elseIfStatement.Context);

        var symbols = AnalyzeExpression(elseIfStatement.Condition);
        if (symbols.AlwaysTrue())
            reporter.Report(new ConditionAlwaysTrue().WithContext(elseIfStatement.Context.condition));

        if (symbols.AlwaysFalse())
            reporter.Report(new ConditionAlwaysFalse().WithContext(elseIfStatement.Context.condition));

        Environment.EnterScope(Environment.CreateSubScope());
        AnalyzeBlock(elseIfStatement.Block);
        Environment.ExitScope();
    }

    private void AnalyzeElseStatement(ElseStatement elseStatement)
    {
        using var _ = new ScopedContext(elseStatement.Context);

        Environment.EnterScope(Environment.CreateSubScope());
        AnalyzeBlock(elseStatement.Block);
        Environment.ExitScope();
    }

    private void AnalyzeFunctionCallStatement(FunctionCallStatement functionCallStatement)
    {
        using var _ = new ScopedContext(functionCallStatement.Context);

        var possibleValues = AnalyzeExpression(functionCallStatement.PrimaryExpression);
        var function = possibleValues.Resolve();
        if (function == null)
        {
            reporter.Report(new TypeResolutionFailure(functionCallStatement.PrimaryExpression)
                .UseLocation(functionCallStatement.Context.primaryExpression()));
            function = Value.InvalidValue;
        }
        else if (!function.DataType.IsCompatible(FunctionType.FunctionBase))
        {
            reporter.Report(new TypeMismatch(function.DataType, FunctionType.FunctionBase)
                .UseLocation(functionCallStatement.Context));
            function = Value.InvalidValue;
        }

        var arguments = new List<Value>();
        for (var i = 0; i < functionCallStatement.Arguments.Count; i++)
        {
            var argument = functionCallStatement.Arguments[i];

            var possibleArgumentValues = AnalyzeExpression(argument);
            var argumentValue = possibleArgumentValues.Resolve();
            if (argumentValue == null)
            {
                reporter.Report(new TypeResolutionFailure(functionCallStatement.Arguments[i])
                    .UseLocation(functionCallStatement.Context.functionArguments().expression()[i]));
                argumentValue = Value.InvalidValue;
            }

            arguments.Add(argumentValue);
        }

        var result = function.DataType.TryCall(function, arguments);
        switch (result.OperationResult)
        {
            case OperationResult.Success:
                break;
            case OperationResult.InvalidArgument:
                break;
        }
    }

    private PossibleValues AnalyzeExpression(Expression expression)
    {
        switch (expression)
        {
            case NumberLiteral numberLiteral:
                return Value.From(numberLiteral.Value);
            case StringLiteral stringLiteral:
                return Value.From(stringLiteral.Value);
            case BooleanLiteral booleanLiteral:
                return Value.From(booleanLiteral.Value);
            case ParenthesizedExpression parenthesizedExpression:
                return AnalyzeExpression(parenthesizedExpression.Expression);
            case LogicalOperation logicalOperation:
                return AnalyzeLogicalOperation(logicalOperation);
            case AndOperation andOperation:
                return AnalyzeAndOperation(andOperation);
            case OrOperation orOperation:
                return AnalyzeOrOperation(orOperation);
            case BinaryOperation binaryOperation:
                return AnalyzeBinaryOperation(binaryOperation);
            case UnaryOperation unaryOperation:
                return AnalyzeUnaryOperation(unaryOperation);
            case VariableReference variableReference:
                return AnalyzeVariableReference(variableReference);
            case PrimaryExpression primaryExpression:
                return AnalyzePrimaryExpression(primaryExpression);
            default:
                throw new NotImplementedException($"Unknown expression type {expression.GetType()}");
        }
    }

    private PossibleValues AnalyzePrimaryExpression(PrimaryExpression primaryExpression)
    {
        using var _ = new ScopedContext(primaryExpression.Context);

        var symbols = AnalyzeExpression(primaryExpression.PrefixExpression);
        foreach (var postfixExpression in primaryExpression.PostfixExpressions)
            switch (postfixExpression)
            {
                case DotIndex dotIndex:
                    symbols = AnalyzeDotIndex(symbols, dotIndex);
                    break;
                case BracketIndex bracketIndex:
                    symbols = AnalyzeBracketIndex(symbols, bracketIndex);
                    break;
                case FunctionCall functionCall:
                    symbols = AnalyzeFunctionCall(symbols, functionCall);
                    break;
                default:
                    throw new NotImplementedException($"Unknown expression type {postfixExpression.GetType()}");
            }

        return symbols;
    }

    private PossibleValues AnalyzeFunctionCall(PossibleValues possibleValues, FunctionCall functionCall)
    {
        using var _ = new ScopedContext(functionCall.Context);

        var arguments = new List<Value>();
        for (var i = 0; i < functionCall.Arguments.Count; i++)
        {
            var argument = functionCall.Arguments[i];
            var possibleArguments = AnalyzeExpression(argument);
            var argumentValue = possibleArguments.Resolve();
            if (argumentValue == null)
            {
                reporter.Report(new TypeResolutionFailure(argument)
                    .UseLocation(functionCall.Context.functionArguments().expression()[i]));
                argumentValue = Value.InvalidValue;
            }

            arguments.Add(argumentValue);
        }

        var resultingSymbols = new PossibleValues();

        foreach (var symbol in possibleValues)
        {
            var result = symbol.Call(arguments);
            if (result.OperationResult == OperationResult.Success)
            {
                resultingSymbols.Add(result.Value!);
                break;
            }
        }

        return resultingSymbols;
    }

    private PossibleValues AnalyzeUnaryOperation(UnaryOperation unaryOperation)
    {
        using var _ = new ScopedContext(unaryOperation.Context);

        var expression = AnalyzeExpression(unaryOperation.Expression);

        switch (unaryOperation.Operator)
        {
            case "-":
                return AnalyzeUnaryOperation(expression, "-", "__umn", unaryOperation.Context.MINUS().Symbol);
            default:
                throw new NotImplementedException($"Unknown unary operator {unaryOperation.Operator}");
        }
    }

    private PossibleValues AnalyzeUnaryOperation(PossibleValues values, string op, string meta, IToken token)
    {
        var resultingValues = new PossibleValues();
        var hasErroredForTypes = new HashSet<DataType>();

        foreach (var value in values)
        {
            var result = value.UnaryOperation(meta);
            if (result.OperationResult == OperationResult.InvalidArgument)
            {
                resultingValues.Add(result.Value!);
                continue;
            }

            if (hasErroredForTypes.Contains(value.DataType))
                continue;

            reporter.Report(new UnsupportedUnaryOperation(op, value.DataType)
                .UseLocation(token));
            hasErroredForTypes.Add(value.DataType);
        }

        return resultingValues;
    }

    private PossibleValues AnalyzeBinaryOperation(BinaryOperation binaryOperation)
    {
        using var _ = new ScopedContext(binaryOperation.Context);

        var left = AnalyzeExpression(binaryOperation.Left);
        var right = AnalyzeExpression(binaryOperation.Right);

        if (_arithmeticOperators.TryGetValue(binaryOperation.Operator, out var operatorInfo))
            return PerformArithmeticOperation(left, right, operatorInfo)
                .SendReport();

        throw new NotImplementedException($"Unknown binary operator {binaryOperation.Operator}");
    }

    private PossibleValues AnalyzeLogicalOperation(LogicalOperation logicalOperation)
    {
        using var _ = new ScopedContext(logicalOperation.Context);

        var left = AnalyzeExpression(logicalOperation.Left);
        var right = AnalyzeExpression(logicalOperation.Right);

        if (!_logicOperators.TryGetValue(logicalOperation.Operator, out var operatorInfo))
            throw new NotImplementedException($"Unknown logical operator {logicalOperation.Operator}");

        return PerformLogicOperation(left, right, operatorInfo)
            .WithContext(logicalOperation.Context.op)
            .SendReport();
    }

    public PossibleValues AnalyzeAndOperation(AndOperation andOperation)
    {
        using var _ = new ScopedContext(andOperation.Context);

        var leftValues = AnalyzeExpression(andOperation.Left);
        var rightValues = AnalyzeExpression(andOperation.Right);

        var leftCanBeTruthy = false;
        var leftCanBeFalsy = false;

        var resultingSymbols = new PossibleValues();

        foreach (var value in leftValues)
            // If we know the actual value of the symbol, and it is a boolean then we can determine if we need to add the symbol.
            if (value.Kind == ValueKind.Boolean)
            {
                var isTruthy = value.GetBoolean();
                leftCanBeTruthy |= isTruthy;
                leftCanBeFalsy |= !isTruthy;

                if (!isTruthy)
                    resultingSymbols.Add(value);
            }
            else if (value.Kind == ValueKind.Null)
            {
                leftCanBeFalsy = true;
                resultingSymbols.Add(value);
            }
            // If the symbol is an unknown boolean, an and operation causes only "falsy" values to pass further.
            // This means if we have a non-nil boolean, we can determine it will be added only if it is `false`.
            else if (value.DataType == DataType.BooleanType)
            {
                leftCanBeTruthy = true;
                leftCanBeFalsy = true;
                resultingSymbols.Add(Value.From(false));
            }
            else
            {
                leftCanBeTruthy = true;
            }

        // If an 'and' operation's left symbols are only "falsy" then they are the only symbols passed further.
        if (!leftCanBeTruthy)
        {
            reporter.Report(new AndAlwaysFalse().WithContext(andOperation.Context.expression()[0]));
            return resultingSymbols;
        }

        // If an 'and' operation's left symbols are only "truthy" then only the right symbols are passed further.
        if (!leftCanBeFalsy)
        {
            reporter.Report(new AndAlwaysTrue().WithContext(andOperation.Context.expression()[0]));
            return rightValues;
        }

        resultingSymbols.AddRange(rightValues);

        return resultingSymbols;
    }

    public PossibleValues AnalyzeOrOperation(OrOperation orOperation)
    {
        using var _ = new ScopedContext(orOperation.Context);

        var leftValues = AnalyzeExpression(orOperation.Left);
        var rightValues = AnalyzeExpression(orOperation.Right);

        var leftCanBeTruthy = false;
        var leftCanBeFalsy = false;

        var resultingValues = new PossibleValues();

        foreach (var value in leftValues)
            if (value.Kind == ValueKind.Boolean)
            {
                var isTruthy = value.GetBoolean();
                leftCanBeTruthy |= isTruthy;
                leftCanBeFalsy |= !isTruthy;

                if (isTruthy)
                    resultingValues.Add(value);
            }
            else if (value.Kind == ValueKind.Null)
            {
                leftCanBeFalsy = true;
                resultingValues.Add(Value.Unknown(value.DataType));
            }
            else
            {
                leftCanBeTruthy = true;

                // If the symbol is an unknown boolean, an or operation causes only "truthy" values to pass further.
                // This means if we have a non-nil boolean, we can determine it will be added only if it is `true`.
                if (value.DataType == DataType.BooleanType)
                {
                    leftCanBeFalsy = true;
                    resultingValues.Add(Value.From(true));
                }
                else
                {
                    resultingValues.Add(value);
                }
            }

        if (!leftCanBeTruthy)
            reporter.Report(new OrAlwaysFalse().WithContext(orOperation.Context.expression()[0]));

        if (!leftCanBeFalsy)
        {
            reporter.Report(new OrAlwaysTrue().WithContext(orOperation.Context.expression()[0]));
            return resultingValues;
        }

        resultingValues.AddRange(rightValues);

        return resultingValues;
    }

    private Value AnalyzeVariableReference(VariableReference variableReference)
    {
        using var _ = new ScopedContext(variableReference.Context);

        if (!Environment.TryGetSymbol(variableReference, out var symbol))
        {
            reporter.Report(new VariableNotFound(variableReference)
                .WithContext(variableReference.Context.IDENTIFIER()));
            return Value.InvalidValue;
        }

        return symbol.Value;
    }

    private PossibleValues AnalyzeDotIndex(PossibleValues values, DotIndex dotIndex)
    {
        using var _ = new ScopedContext(dotIndex.Context);

        var indices = Value.From(dotIndex.Index);

        return AnalyzeIndex(values, [indices])
            .WithContext(dotIndex.Context)
            .SendReport();
    }

    private PossibleValues AnalyzeBracketIndex(PossibleValues values, BracketIndex bracketIndex)
    {
        using var _ = new ScopedContext(bracketIndex.Context);

        var indices = AnalyzeExpression(bracketIndex.Index);

        return AnalyzeIndex(values, indices)
            .WithContext(bracketIndex.Context)
            .SendReport();
    }

    private Diagnostic<PossibleValues> AnalyzeIndex(PossibleValues values, PossibleValues indices)
    {
        var resultingValues = new PossibleValues();

        foreach (var indexedValue in values)
        foreach (var indexingValue in indices)
        {
            var result = indexedValue.Index(indexingValue);
            if (result.OperationResult == OperationResult.Success)
            {
                resultingValues.Add(result.Value!);
                continue;
            }

            return new Diagnostic<PossibleValues>(new InvalidIndex())
        }

        return new Diagnostic<PossibleValues>(null, resultingValues);
    }

    private DiagnosticReport<PossibleValues> PerformArithmeticOperation(PossibleValues leftValues,
        PossibleValues rightValues, OperatorInformation opInfo)
    {
        var diagnostics = new DiagnosticReport<PossibleValues>();

        var resultingValues = new PossibleValues();
        var hasErroredForTypes = new HashSet<(DataType, DataType)>();

        foreach (var leftValue in leftValues)
        foreach (var rightValue in rightValues)
        {
            var result = leftValue.ArithmeticOperation(rightValue, opInfo.Metamethod);
            if (result.OperationResult == OperationResult.Success)
            {
                resultingValues.Add(result.Value!);
                continue;
            }

            if (hasErroredForTypes.Contains((leftValue.DataType, rightValue.DataType)))
                continue;

            diagnostics.Report(new UnsupportedBinaryOperation(opInfo.Operator, leftValue.DataType,
                rightValue.DataType));
            hasErroredForTypes.Add((leftValue.DataType, rightValue.DataType));
        }

        diagnostics.Data = resultingValues;
        return diagnostics;
    }

    private DiagnosticReport<PossibleValues> PerformLogicOperation(PossibleValues leftValues,
        PossibleValues rightValues, OperatorInformation opInfo)
    {
        var diagnostics = new DiagnosticReport<PossibleValues>();

        if (opInfo.SwapOperands)
            (leftValues, rightValues) = (rightValues, leftValues);

        var resultingValues = new PossibleValues();
        var hasErroredForTypes = new HashSet<(DataType, DataType)>();

        foreach (var leftValue in leftValues)
        foreach (var rightValue in rightValues)
        {
            var result = leftValue.LogicOperation(rightValue, opInfo.Metamethod);
            if (result.OperationResult == OperationResult.Success)
            {
                resultingValues.Add(result.Value!);
                continue;
            }

            if (hasErroredForTypes.Contains((leftValue.DataType, rightValue.DataType)))
                continue;

            diagnostics.Report(new UnsupportedBinaryOperation(opInfo.Operator, leftValue.DataType,
                rightValue.DataType));
            hasErroredForTypes.Add((leftValue.DataType, rightValue.DataType));
        }

        if (opInfo.NegateResult)
        {
            var negatedValues = new PossibleValues();

            foreach (var value in resultingValues)
                if (value.Kind == ValueKind.Boolean)
                {
                    var negatedValue = !value.GetBoolean();
                    negatedValues.Add(Value.From(negatedValue));
                }
                else if (value.Kind == ValueKind.Null)
                {
                    negatedValues.Add(Value.From(true));
                }
                else if (value.DataType == DataType.BooleanType)
                {
                    negatedValues.Add(Value.Unknown(DataType.BooleanType));
                }
                else
                {
                    // Any other object in Lua that is subjected to the `not` operator will be false
                    negatedValues.Add(Value.From(false));
                }

            resultingValues = negatedValues;
        }

        diagnostics.Data = resultingValues;
        return diagnostics;
    }

    private DataType GetDataTypeFromReference(TypeReference typeReference)
    {
        switch (typeReference)
        {
            case ExpressionTypeReference expressionType:
                return GetDataTypeFromExpression(expressionType.Expression);
            case FunctionTypeReference functionType:
                return FunctionType.FunctionBase;
            case FuncTypeReference funcType:
                return GetDataTypeFromFuncReference(funcType);
            default:
                throw new NotImplementedException($"Unhandled type reference: {typeReference}");
        }
    }

    private DataType GetDataTypeFromExpression(Expression expression)
    {
        var possibleValues = AnalyzeExpression(expression);
        if (possibleValues.Count != 1)
        {
            reporter.Report(new InvalidType(expression));
            return DataType.InvalidType;
        }

        if (possibleValues[0].DataType != DataType.MetaType)
        {
            reporter.Report(new InvalidType(expression));
            return DataType.InvalidType;
        }

        return possibleValues[0].GetDataType();
    }

    private DataType GetDataTypeFromFuncReference(FuncTypeReference funcReference)
    {
        var parameterTypes = new List<TypeUsage>();
        foreach (var paramReference in funcReference.Parameters)
        {
            var dataType = GetDataTypeFromReference(paramReference);
            parameterTypes.Add(new TypeUsage(dataType, paramReference.Nullable));
        }

        var returnTypes = new List<TypeUsage>();
        foreach (var returnReference in funcReference.Returns)
        {
            var dataType = GetDataTypeFromReference(returnReference);
            returnTypes.Add(new TypeUsage(dataType, returnReference.Nullable));
        }

        return new FunctionType(parameterTypes, returnTypes);
    }

    private struct OperatorInformation(
        string @operator,
        string metamethod,
        bool swapOperands = false,
        bool negateResult = false)
    {
        public readonly string Operator = @operator;
        public readonly string Metamethod = metamethod;
        public readonly bool SwapOperands = swapOperands;
        public readonly bool NegateResult = negateResult;
    }
}