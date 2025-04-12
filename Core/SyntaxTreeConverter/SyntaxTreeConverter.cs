using System.Linq.Expressions;
using Core.Antlr;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;

namespace Core.SyntaxTreeConverter;

public class SyntaxTreeConverter : CraterParserBaseVisitor<object?>
{
    public override object VisitProgram(CraterParser.ProgramContext context)
    {
        var block = (Block)Visit(context.block())!;
        return new Module(block);
    }

    public override object VisitBlock(CraterParser.BlockContext context)
    {
        var statements = new List<Statement>();
        
        foreach (var statement in context.statement())
        {
            statements.Add((Statement)Visit(statement)!);
        }
        
        return new Block(statements);
    }

    public override object VisitVariableDeclaration(CraterParser.VariableDeclarationContext context)
    {
        var isLocal = context.LOCAL() != null;
        var identifier = context.name.Text!;
        var dataTypeReference = (VariableReference)Visit(context.type)!;
        var isNullable = context.nullable != null;

        if (context.initializer is null)
            return new VariableDeclaration(isLocal, identifier, dataTypeReference, isNullable, null, context);
        
        var initializer = (Expression)Visit(context.initializer)!;
        
        return new VariableDeclaration(isLocal, identifier, dataTypeReference, isNullable, initializer, context);
    }

    public override object VisitFunctionDeclaration(CraterParser.FunctionDeclarationContext context)
    {
        var isLocal = context.LOCAL() != null;
        var identifier = context.name.Text!;
        var arguments = (List<ParameterDeclartion>)Visit(context.functionParameters())!;
        var returnDataTypeReference = (VariableReference)Visit(context.returnType)!;
        var returnIsNullable = context.returnNullable != null;
        var block = (Block)Visit(context.block())!;
        
        return new FunctionDeclaration(isLocal, identifier, arguments, returnDataTypeReference, returnIsNullable, block, context);
    }

    public override object VisitFunctionParameters(CraterParser.FunctionParametersContext context)
    {
        List<ParameterDeclartion> arguments = [];
        
        foreach (var argumentContext in context.functionParameter())
            arguments.Add((ParameterDeclartion)Visit(argumentContext)!);
        
        return arguments;
    }

    public override object VisitFunctionParameter(CraterParser.FunctionParameterContext context)
    {
        var name = context.name.Text!;
        var dataTypeReference = (VariableReference)Visit(context.type)!;
        var isNullable = context.nullable != null;
        
        return new ParameterDeclartion(name, dataTypeReference, isNullable, context);
    }

    public override object VisitIfStatement(CraterParser.IfStatementContext context)
    {
        var condition = (Expression)Visit(context.condition)!;
        var block = (Block)Visit(context.block())!;
        
        List<ElseIfStatement> elseIfStatements = [];
        foreach (var elseIfStatementContext in context.elseIfStatement())
            elseIfStatements.Add((ElseIfStatement)Visit(elseIfStatementContext)!);

        ElseStatement? elseStatement = null;
        if (context.elseStatement() != null)
            elseStatement = (ElseStatement)Visit(context.elseStatement())!;
        
        return new IfStatement(condition, block, elseIfStatements, elseStatement, context);
    }

    public override object VisitElseIfStatement(CraterParser.ElseIfStatementContext context)
    {
        var condition = (Expression)Visit(context.condition)!;
        var block = (Block)Visit(context.block())!;
        
        return new ElseIfStatement(condition, block, context);
    }

    public override object VisitElseStatement(CraterParser.ElseStatementContext context)
    {
        var block = (Block)Visit(context.block())!;
        return new ElseStatement(block, context);
    }

    public override object VisitFunctionCallStatement(CraterParser.FunctionCallStatementContext context)
    {
        var primaryExpression = (PrimaryExpression)Visit(context.primaryExpression())!;
        var arguments = new List<Expression>();
        if (context.functionArguments() != null)
            arguments = (List<Expression>)Visit(context.functionArguments())!;
        
        return new FunctionCallStatement(primaryExpression, arguments, context);
    }
    
    public override object VisitPrimaryExpression(CraterParser.PrimaryExpressionContext context)
    {
        var prefixExpression = (Expression)Visit(context.prefixExpression())!;
        var postfixExpressions = new List<Expression>();
        foreach (var postfixExpressionContext in context.postfixExpression())
            postfixExpressions.Add((Expression)Visit(postfixExpressionContext)!);
        
        return new PrimaryExpression(prefixExpression, postfixExpressions, context);
    }
    
    public override object VisitDotIndexing(CraterParser.DotIndexingContext context)
    {
        return new DotIndex(context.IDENTIFIER().GetText(), context);
    }

    public override object VisitBracketIndexing(CraterParser.BracketIndexingContext context)
    {
        var index = (Expression)Visit(context.expression())!;
        return new BracketIndex(index, context);
    }

    public override object VisitFunctionCall(CraterParser.FunctionCallContext context)
    {
        var arguments = new List<Expression>();
        if (context.functionArguments() != null)
            arguments = (List<Expression>)Visit(context.functionArguments())!;
        
        return new FunctionCall(arguments, context);
    }

    public override object VisitFunctionArguments(CraterParser.FunctionArgumentsContext context)
    {
        var arguments = new List<Expression>();
        foreach (var functionArgumentContext in context.expression())
            arguments.Add((Expression)Visit(functionArgumentContext)!);

        return arguments;
    }
    
    public override object VisitParenthesizedExpression(CraterParser.ParenthesizedExpressionContext context)
    {
        var expression = (Expression)Visit(context.expression())!;
        return new ParenthesizedExpression(expression, context);
    }

    public override object VisitUnaryOperation(CraterParser.UnaryOperationContext context)
    {
        var expression = (Expression)Visit(context.expression())!;
        return new UnaryOperation(expression, context.MINUS().GetText()!, context);
    }
    
    public override object VisitExponentOperation(CraterParser.ExponentOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new BinaryOperation(left, right, context.EXP().GetText(), new BinaryOperationContext(context));
    }
    
    public override object VisitMultiplicativeOperation(CraterParser.MultiplicativeOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new BinaryOperation(left, right, context.op.Text, new BinaryOperationContext(context));
    }

    public override object VisitAdditiveOperation(CraterParser.AdditiveOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new BinaryOperation(left, right, context.op.Text, new BinaryOperationContext(context));
    }

    public override object VisitConcatenationOperation(CraterParser.ConcatenationOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;
        
        return new BinaryOperation(left, right, context.CONCAT().GetText(), new BinaryOperationContext(context));
    }

    public override object VisitLogicalOperation(CraterParser.LogicalOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;
        
        return new LogicalOperation(left, right, context.op.Text, context);
    }

    public override object VisitAndOperation(CraterParser.AndOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new AndOperation(left, right, "and", context);
    }
    
    public override object VisitOrOperation(CraterParser.OrOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new OrOperation(left, right, "or", context);
    }

    public override object VisitVariableReference(CraterParser.VariableReferenceContext context)
    {
        return new VariableReference(context.IDENTIFIER().GetText()!, null, context);
    }
    
    public override object VisitLiteral(CraterParser.LiteralContext context)
    {
        if (context.number != null)
            return new NumberLiteral(context);

        if (context.STRING() != null)
            return new StringLiteral(context.STRING().GetText()!, context);
        
        if (context.BOOLEAN() != null)
            return new BooleanLiteral(context.BOOLEAN().GetText()!, context);
        
        throw new NotImplementedException($"Unknown literal type: {context}");
    }
}