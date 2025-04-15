using Antlr4.Runtime;
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
        {
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
    }

    private void AnalyzeVariableDeclaration(VariableDeclaration variableDeclaration)
    {
        using var _ = new ScopedContext(variableDeclaration.Context);
        
        var scope = variableDeclaration.Local ? Environment.GetLocalScope() : Environment.GetGlobalScope();
        var dataType = GetDataTypeFromExpression(variableDeclaration.DataTypeReference);

        var defaultSymbol = new Symbol(Value.NullValue, dataType, variableDeclaration.Nullable);

        if (variableDeclaration.Initializer != null)
        {
            var assignedSymbols = AnalyzeExpression(variableDeclaration.Initializer);

            defaultSymbol.Assign(ResolveSymbols(assignedSymbols, defaultSymbol, variableDeclaration.Identifier));
        }
        else if (!variableDeclaration.Nullable)
        {
            DiagnosticReporter.Report(new UninitializedNonNullable(variableDeclaration.Identifier)
                .WithContext(variableDeclaration.Context.IDENTIFIER()));
        }

        if (scope.HasSymbol(variableDeclaration.Identifier))
        {
            DiagnosticReporter.Report(new VariableShadowing(variableDeclaration.Identifier)
                .WithContext(variableDeclaration.Context.IDENTIFIER()));
        }

        scope.Declare(variableDeclaration.Identifier, defaultSymbol);
    }

    private void AnalyzeFunctionDeclaration(FunctionDeclaration functionDeclaration)
    {
        using var _ = new ScopedContext(functionDeclaration.Context);
        
        var scope = functionDeclaration.Local ? Environment.GetLocalScope() : Environment.GetGlobalScope();
        var returnType = GetDataTypeFromExpression(functionDeclaration.ReturnTypeReference);

        List<DataType> parameters = [];

        foreach (var parameter in functionDeclaration.Parameters)
        {
            var parameterType = GetDataTypeFromExpression(parameter.DataTypeReference);
            parameters.Add(parameterType);
        }

        if (scope.HasSymbol(functionDeclaration.Identifier))
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

    private void AnalyzeIfStatement(IfStatement ifStatement)
    {
        using var _ = new ScopedContext(ifStatement.Context);
        
        var symbols = AnalyzeExpression(ifStatement.Condition);
        if (symbols.AlwaysTrue())
            DiagnosticReporter.Report(new ConditionAlwaysTrue().WithContext(ifStatement.Context.condition));
        
        if (symbols.AlwaysFalse())
            DiagnosticReporter.Report(new ConditionAlwaysFalse().WithContext(ifStatement.Context.condition));
        
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
            DiagnosticReporter.Report(new ConditionAlwaysTrue().WithContext(elseIfStatement.Context.condition));
        
        if (symbols.AlwaysFalse())
            DiagnosticReporter.Report(new ConditionAlwaysFalse().WithContext(elseIfStatement.Context.condition));
        
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
        
        var symbols = AnalyzeExpression(functionCallStatement.PrimaryExpression);
        var arguments = new List<PossibleSymbols>();
        foreach (var argument in functionCallStatement.Arguments)
            arguments.Add(AnalyzeExpression(argument));
        
        foreach (var symbol in symbols)
        {
            var result = symbol.Call(arguments);
            switch (result.OperationResult)
            {
                case OperationResult.Success:
                    continue;
                case OperationResult.InvalidArgument:
                    break;
            }
        }
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
            case PrimaryExpression primaryExpression:
                return AnalyzePrimaryExpression(primaryExpression);
            default:
                throw new NotImplementedException($"Unknown expression type {expression.GetType()}");
        }
    }

    private PossibleSymbols AnalyzePrimaryExpression(PrimaryExpression primaryExpression)
    {
        using var _ = new ScopedContext(primaryExpression.Context);
        
        var symbols = AnalyzeExpression(primaryExpression.PrefixExpression);
        foreach (var postfixExpression in primaryExpression.PostfixExpressions)
        {
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
        }
        
        return symbols;
    }

    private PossibleSymbols AnalyzeFunctionCall(PossibleSymbols possibleSymbols, FunctionCall functionCall)
    {
        using var _ = new ScopedContext(functionCall.Context);
        
        var arguments = new List<PossibleSymbols>();
        foreach (var argument in functionCall.Arguments)
            arguments.Add(AnalyzeExpression(argument));
        
        var resultingSymbols = new PossibleSymbols();
        
        foreach (var symbol in possibleSymbols)
        {
            var result = symbol.Call(arguments);
            switch (result.OperationResult)
            {
                case OperationResult.Success:
                    resultingSymbols.Add(result.Symbol!);
                    break;
            }
        }
        
        return resultingSymbols;
    }

    private PossibleSymbols AnalyzeUnaryOperation(UnaryOperation unaryOperation)
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

    private PossibleSymbols AnalyzeUnaryOperation(PossibleSymbols symbols, string op, string meta, IToken token)
    {
        var resultingSymbols = new PossibleSymbols();
        var hasErroredForTypes = new HashSet<DataType>();

        foreach (var symbol in symbols)
        {
            var result = symbol.UnaryOperation(meta);
            switch (result.OperationResult)
            {
                case OperationResult.Success:
                    resultingSymbols.Add(result.Symbol!);
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
        using var _ = new ScopedContext(binaryOperation.Context);
        
        var left = AnalyzeExpression(binaryOperation.Left);
        var right = AnalyzeExpression(binaryOperation.Right);

        if (_arithmeticOperators.TryGetValue(binaryOperation.Operator, out var operatorInfo))
            return PerformArithmeticOperation(left, right, operatorInfo)
                .SendReport();
        
        throw new NotImplementedException($"Unknown binary operator {binaryOperation.Operator}");
    }

    private PossibleSymbols AnalyzeLogicalOperation(LogicalOperation logicalOperation)
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

    public PossibleSymbols AnalyzeAndOperation(AndOperation andOperation)
    {
        using var _ = new ScopedContext(andOperation.Context);
        
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
        using var _ = new ScopedContext(orOperation.Context);
        
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
        using var _ = new ScopedContext(variableReference.Context);
        
        if (!Environment.TryGetSymbol(variableReference, out var symbol))
        {
            DiagnosticReporter.Report(new VariableNotFound(variableReference)
                .WithContext(variableReference.Context.IDENTIFIER()));
            return new Symbol(Value.InvalidValue, DataType.InvalidType, false);
        }

        return symbol;
    }

    private PossibleSymbols AnalyzeDotIndex(PossibleSymbols symbols, DotIndex dotIndex)
    {
        using var _ = new ScopedContext(dotIndex.Context);
        
        var indices = new Symbol(Value.From(dotIndex.Index), DataType.StringType, false);

        return AnalyzeIndex(symbols, [indices])
            .WithContext(dotIndex.Context)
            .SendReport();
    }

    private PossibleSymbols AnalyzeBracketIndex(PossibleSymbols symbols, BracketIndex bracketIndex)
    {
        using var _ = new ScopedContext(bracketIndex.Context);
        
        var indices = AnalyzeExpression(bracketIndex.Index);
        
        return AnalyzeIndex(symbols, indices)
            .WithContext(bracketIndex.Context)
            .SendReport();
    }

    private DiagnosticReport<PossibleSymbols> AnalyzeIndex(PossibleSymbols symbols, PossibleSymbols indices)
    {
        var resultingSymbols = new PossibleSymbols();
        var reporter = new DiagnosticReport<PossibleSymbols>();
        
        foreach (var indexedSymbol in symbols)
        {
            foreach (var indexingSymbol in indices)
            {
                var result = indexedSymbol.Index(indexingSymbol);
                switch (result.OperationResult)
                {
                    case OperationResult.Success:
                        resultingSymbols.Add(result.Symbol!);
                        continue;
                }
                
                reporter.Report(new InvalidIndex(indexedSymbol.DataType, indexingSymbol.DataType));
            }
        }

        reporter.Data = resultingSymbols;
        return reporter;
    }
    
    private Symbol ResolveSymbols(PossibleSymbols possibleSymbols, Symbol target, string variable)
    {
        var resultingSymbols = new PossibleSymbols();

        var hasErroredForType = new HashSet<DataType> { DataType.InvalidType };

        var hasErroredForNullable = false;

        foreach (var symbol in possibleSymbols)
        {
            var hasError = false;
            if (!hasErroredForType.Contains(symbol.DataType) && !symbol.DataType.IsCompatible(target.DataType))
            {
                DiagnosticReporter.Report(new TypeMismatch(symbol.DataType, target.DataType));
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
                var result = leftSymbol.ArithmeticOperation(rightSymbol, opInfo.Metamethod);
                switch (result.OperationResult)
                {
                    case OperationResult.Success:
                        resultingSymbols.Add(result.Symbol!);
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
                var result = leftSymbol.LogicOperation(rightSymbol, opInfo.Metamethod);
                switch (result.OperationResult)
                {
                    case OperationResult.Success:
                        resultingSymbols.Add(result.Symbol!);
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

    private DataType GetDataTypeFromExpression(Expression expression)
    {
        var possibleSymbols = AnalyzeExpression(expression);
        if (possibleSymbols.Count != 1)
        {
            DiagnosticReporter.Report(new InvalidType(expression));
            return DataType.InvalidType;
        }

        if (possibleSymbols[0].DataType != DataType.MetaType)
        {
            DiagnosticReporter.Report(new InvalidType(expression));
            return DataType.InvalidType;
        }
        
        return possibleSymbols[0].Value.GetDataType();
    }
}