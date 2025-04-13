﻿using Core.Antlr;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;

namespace Core.SyntaxTreeConverter;

public class SyntaxTreeConverter : CraterParserBaseVisitor<object?>
{
    public override object VisitProgram(ProgramCtx context)
    {
        var block = (Block)Visit(context.block())!;
        return new Module(block);
    }

    public override object VisitBlock(BlockCtx context)
    {
        var statements = new List<Statement>();
        
        foreach (var statement in context.statement())
        {
            statements.Add((Statement)Visit(statement)!);
        }
        
        return new Block(statements);
    }

    public override object VisitVariableDeclaration(VariableDeclarationCtx context)
    {
        var isLocal = context.LOCAL() != null;
        var identifier = context.name.Text!;
        var dataTypeReference = (Expression)Visit(context.type)!;
        var isNullable = context.nullable != null;

        if (context.initializer is null)
            return new VariableDeclaration(isLocal, identifier, dataTypeReference, isNullable, null, context);
        
        var initializer = (Expression)Visit(context.initializer)!;
        
        return new VariableDeclaration(isLocal, identifier, dataTypeReference, isNullable, initializer, context);
    }

    public override object VisitFunctionDeclaration(FunctionDeclarationCtx context)
    {
        var isLocal = context.LOCAL() != null;
        var identifier = context.name.Text!;
        var arguments = (List<ParameterDeclartion>)Visit(context.functionParameters())!;
        var returnDataTypeReference = (VariableReference)Visit(context.returnType)!;
        var returnIsNullable = context.returnNullable != null;
        var block = (Block)Visit(context.block())!;
        
        return new FunctionDeclaration(isLocal, identifier, arguments, returnDataTypeReference, returnIsNullable, block, context);
    }

    public override object VisitFunctionParameters(FunctionParametersCtx context)
    {
        List<ParameterDeclartion> arguments = [];
        
        foreach (var argumentContext in context.functionParameter())
            arguments.Add((ParameterDeclartion)Visit(argumentContext)!);
        
        return arguments;
    }

    public override object VisitFunctionParameter(FunctionParameterCtx context)
    {
        var name = context.name.Text!;
        var dataTypeReference = (VariableReference)Visit(context.type)!;
        var isNullable = context.nullable != null;
        
        return new ParameterDeclartion(name, dataTypeReference, isNullable, context);
    }

    public override object VisitIfStatement(IfStatementCtx context)
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

    public override object VisitElseIfStatement(ElseIfStatementCtx context)
    {
        var condition = (Expression)Visit(context.condition)!;
        var block = (Block)Visit(context.block())!;
        
        return new ElseIfStatement(condition, block, context);
    }

    public override object VisitElseStatement(ElseStatementCtx context)
    {
        var block = (Block)Visit(context.block())!;
        return new ElseStatement(block, context);
    }

    public override object VisitFunctionCallStatement(FunctionCallStatementCtx context)
    {
        var primaryExpression = (PrimaryExpression)Visit(context.primaryExpression())!;
        var arguments = new List<Expression>();
        if (context.functionArguments() != null)
            arguments = (List<Expression>)Visit(context.functionArguments())!;
        
        return new FunctionCallStatement(primaryExpression, arguments, context);
    }
    
    public override object VisitPrimaryExpression(PrimaryExpressionCtx context)
    {
        var prefixExpression = (Expression)Visit(context.prefixExpression())!;
        var postfixExpressions = new List<Expression>();
        foreach (var postfixExpressionContext in context.postfixExpression())
            postfixExpressions.Add((Expression)Visit(postfixExpressionContext)!);
        
        return new PrimaryExpression(prefixExpression, postfixExpressions, context);
    }
    
    public override object VisitDotIndexing(DotIndexingCtx context)
    {
        return new DotIndex(context.IDENTIFIER().GetText(), context);
    }

    public override object VisitBracketIndexing(BracketIndexingCtx context)
    {
        var index = (Expression)Visit(context.expression())!;
        return new BracketIndex(index, context);
    }

    public override object VisitFunctionCall(FunctionCallCtx context)
    {
        var arguments = new List<Expression>();
        if (context.functionArguments() != null)
            arguments = (List<Expression>)Visit(context.functionArguments())!;
        
        return new FunctionCall(arguments, context);
    }

    public override object VisitFunctionArguments(FunctionArgumentsCtx context)
    {
        var arguments = new List<Expression>();
        foreach (var functionArgumentContext in context.expression())
            arguments.Add((Expression)Visit(functionArgumentContext)!);

        return arguments;
    }
    
    public override object VisitParenthesizedExpression(ParenthesizedExpressionCtx context)
    {
        var expression = (Expression)Visit(context.expression())!;
        return new ParenthesizedExpression(expression, context);
    }

    public override object VisitUnaryOperation(UnaryOperationCtx context)
    {
        var expression = (Expression)Visit(context.expression())!;
        return new UnaryOperation(expression, context.MINUS().GetText()!, context);
    }
    
    public override object VisitExponentOperation(ExponentOperationCtx context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new BinaryOperation(left, right, context.EXP().GetText(), new BinaryOperationContext(context));
    }
    
    public override object VisitMultiplicativeOperation(MultiplicativeOperationCtx context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new BinaryOperation(left, right, context.op.Text, new BinaryOperationContext(context));
    }

    public override object VisitAdditiveOperation(AdditiveOperationCtx context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new BinaryOperation(left, right, context.op.Text, new BinaryOperationContext(context));
    }

    public override object VisitConcatenationOperation(ConcatenationOperationCtx context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;
        
        return new BinaryOperation(left, right, context.CONCAT().GetText(), new BinaryOperationContext(context));
    }

    public override object VisitLogicalOperation(LogicalOperationCtx context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;
        
        return new LogicalOperation(left, right, context.op.Text, context);
    }

    public override object VisitAndOperation(AndOperationCtx context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new AndOperation(left, right, "and", context);
    }
    
    public override object VisitOrOperation(OrOperationCtx context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new OrOperation(left, right, "or", context);
    }

    public override object VisitVariableReference(VariableReferenceCtx context)
    {
        return new VariableReference(context.IDENTIFIER().GetText()!, context);
    }
    
    public override object VisitLiteral(LiteralCtx context)
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