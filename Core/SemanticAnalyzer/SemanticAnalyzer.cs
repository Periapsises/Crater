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
    public SemanticAnalyzer() { }

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

    private void AnalyzeVariableDeclaration(VariableDeclaration variableDeclaration)
    {
        var scope = variableDeclaration.Local ? Environment.GetLocalScope() : Environment.GetGlobalScope();

        var dataTypeSymbol = Environment.GetVariable(variableDeclaration.VariableReference);
        if (dataTypeSymbol == null)
        {
            DiagnosticReporter.Report(new TypeNotFound(variableDeclaration.VariableReference)
                .WithContext(variableDeclaration.Context.expression()[0]));
            return;
        }

        if (dataTypeSymbol.DataType != DataType.MetaType)
        {
            DiagnosticReporter.Report(new InvalidType(variableDeclaration.VariableReference)
                .WithContext(variableDeclaration.Context.expression()[0]));
            return;
        }

        var dataType = dataTypeSymbol.Value.GetDataType();

        var defaultSymbol = new Symbol(Value.NullValue, dataType, variableDeclaration.Nullable);

        if (variableDeclaration.Initializer != null)
        {
            var assignedSymbols = AnalyzeExpression(variableDeclaration.Initializer);

            defaultSymbol.Assign(ResolveSymbols(assignedSymbols, defaultSymbol, variableDeclaration.Identifier, variableDeclaration.Context.ASSIGN().Symbol));
        }
        else if (!variableDeclaration.Nullable)
        {
            DiagnosticReporter.Report(new UninitializedNonNullable(variableDeclaration.Identifier)
                .WithContext(variableDeclaration.Context.IDENTIFIER()));
        }

        if (scope.HasVariable(variableDeclaration.Identifier))
        {
            DiagnosticReporter.Report(new VariableShadowing(variableDeclaration.Identifier)
                .WithContext(variableDeclaration.Context.IDENTIFIER()));
        }

        scope.Declare(variableDeclaration.Identifier, defaultSymbol);
    }

    private void AnalyzeFunctionDeclaration(FunctionDeclaration functionDeclaration)
    {
        var scope = functionDeclaration.Local ? Environment.GetLocalScope() : Environment.GetGlobalScope();

        var returnTypeSymbol = Environment.GetVariable(functionDeclaration.ReturnTypeReference);
        if (returnTypeSymbol == null)
        {
            DiagnosticReporter.Report(new TypeNotFound(functionDeclaration.ReturnTypeReference)
                .WithContext(functionDeclaration.Context.expression()));
            returnTypeSymbol = Symbol.InvalidDataType;
        }

        if (returnTypeSymbol.DataType != DataType.MetaType)
        {
            DiagnosticReporter.Report(new InvalidType(functionDeclaration.ReturnTypeReference)
                .WithContext(functionDeclaration.Context.expression()));
        }

        var returnType = returnTypeSymbol.Value.GetDataType();

        List<DataType> parameters = [];

        foreach (var parameter in functionDeclaration.Parameters)
        {
            var parameterTypeSymbol = Environment.GetVariable(parameter.DataTypeReference);
            if (parameterTypeSymbol == null)
            {
                DiagnosticReporter.Report(new TypeNotFound(parameter.DataTypeReference)
                    .WithContext(parameter.Context.expression()));
                parameterTypeSymbol = Symbol.InvalidDataType;
            }

            if (parameterTypeSymbol.DataType != DataType.MetaType)
            {
                DiagnosticReporter.Report(new InvalidType(parameter.DataTypeReference)
                    .WithContext(parameter.Context.expression()));
            }

            parameters.Add(parameterTypeSymbol.Value.GetDataType());
        }

        if (scope.HasVariable(functionDeclaration.Identifier))
        {
            DiagnosticReporter.Report(new VariableShadowing(functionDeclaration.Identifier)
                .WithContext(functionDeclaration.Context.IDENTIFIER()));
        }

        var functionType = new FunctionType(parameters, [returnType]);
        var functionSymbol = new Symbol(new Value(ValueKind.Function, null), functionType, false);

        scope.Declare(functionDeclaration.Identifier, functionSymbol);

        var functionScope = Environment.CreateSubScope();

        for (var i = 0; i < functionDeclaration.Parameters.Count; i++)
        {
            var parameterSymbol = new Symbol(new Value(ValueKind.Unknown, null), parameters[i],
                functionDeclaration.Parameters[i].Nullable);
            functionScope.Declare(functionDeclaration.Parameters[i].Name, parameterSymbol);
        }

        Environment.EnterScope(functionScope);
        AnalyzeBlock(functionDeclaration.Block);
        Environment.ExitScope();
    }

    private PossibleSymbols AnalyzeExpression(Expression expression)
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
            default:
                throw new NotImplementedException($"Unknown expression type {expression.GetType()}");
        }
    }

    private PossibleSymbols AnalyzeUnaryOperation(UnaryOperation unaryOperation)
    {
        var expression = AnalyzeExpression(unaryOperation.Expression);

        switch (unaryOperation.Operator)
        {
            case "-":
                return AnalyzeUnaryOperation(expression, "-", "__umn", unaryOperation.Context.MINUS().Symbol);
            default:
                throw new NotImplementedException($"Unknown unary operator {unaryOperation.Operator}");
        }
    }

    private PossibleSymbols AnalyzeUnaryOperation(PossibleSymbols symbols, string op, string meta, IToken token)
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

            DiagnosticReporter.Report(new InvalidUnaryOperator(symbol.DataType, op)
                .WithContext(token));
            hasErroredForTypes.Add(symbol.DataType);
        }

        return resultingSymbols;
    }

    private PossibleSymbols AnalyzeBinaryOperation(BinaryOperation binaryOperation)
    {
        var left = AnalyzeExpression(binaryOperation.Left);
        var right = AnalyzeExpression(binaryOperation.Right);

        if (_arithmeticOperators.TryGetValue(binaryOperation.Operator, out var operatorInfo))
            return PerformArithmeticOperation(left, right, operatorInfo)
                .WithContext(binaryOperation.Context.Op)
                .SendReport();
        
        throw new NotImplementedException($"Unknown binary operator {binaryOperation.Operator}");
    }

    private PossibleSymbols AnalyzeLogicalOperation(LogicalOperation logicalOperation)
    {
        var left = AnalyzeExpression(logicalOperation.Left);
        var right = AnalyzeExpression(logicalOperation.Right);

        if (!_logicOperators.TryGetValue(logicalOperation.Operator, out var operatorInfo))
            throw new NotImplementedException($"Unknown logical operator {logicalOperation.Operator}");

        return PerformLogicOperation(left, right, operatorInfo)
            .WithContext(logicalOperation.Context.op)
            .SendReport();
    }

    public PossibleSymbols AnalyzeAndOperation(AndOperation andOperation)
    {
        var leftSymbols = AnalyzeExpression(andOperation.Left);
        var rightSymbols = AnalyzeExpression(andOperation.Right);
        
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
            DiagnosticReporter.Report(new AndAlwaysFalse().WithContext(andOperation.Context.expression()[0]));
            return resultingSymbols;
        }

        // If an 'and' operation's left symbols are only "truthy" then only the right symbols are passed further.
        if (!leftCanBeFalsy)
        {
            DiagnosticReporter.Report(new AndAlwaysTrue().WithContext(andOperation.Context.expression()[0]));
            return rightSymbols;
        }

        resultingSymbols.AddRange(rightSymbols);

        return resultingSymbols;
    }

    public PossibleSymbols AnalyzeOrOperation(OrOperation orOperation)
    {
        var leftSymbols = AnalyzeExpression(orOperation.Left);
        var rightSymbols = AnalyzeExpression(orOperation.Right);
        
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
            DiagnosticReporter.Report(new OrAlwaysFalse().WithContext(orOperation.Context.expression()[0]));

        if (!leftCanBeFalsy)
        {
            DiagnosticReporter.Report(new OrAlwaysTrue().WithContext(orOperation.Context.expression()[0]));
            return resultingSymbols;
        }

        resultingSymbols.AddRange(rightSymbols);

        return resultingSymbols;
    }

    private Symbol AnalyzeVariableReference(VariableReference variableReference)
    {
        var symbol = Environment.GetVariable(variableReference);
        if (symbol == null)
        {
            DiagnosticReporter.Report(new VariableNotFound(variableReference)
                .WithContext(variableReference.Context.IDENTIFIER()));
            return new Symbol(Value.InvalidValue, DataType.InvalidType, false);
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
                DiagnosticReporter.Report(new TypeMismatch(symbol.DataType, target.DataType).WithContext(context));
                hasErroredForType.Add(symbol.DataType);
                hasError = true;
            }

            if (!hasErroredForNullable && symbol.Nullable && !target.Nullable)
            {
                DiagnosticReporter.Report(new PossibleNullAssignment(variable));
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