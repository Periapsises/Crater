﻿using Core.Antlr;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;

namespace Core.SyntaxTreeConverter;

public class SyntaxTreeConverter : CraterParserBaseVisitor<object?>
{
    public override object? VisitProgram(CraterParser.ProgramContext context)
    {
        var block = (Block)Visit(context.block())!;
        return new Module(block);
    }

    public override object? VisitBlock(CraterParser.BlockContext context)
    {
        var statements = new List<Statement>();
        
        foreach (var statement in context.statement())
        {
            statements.Add((Statement)Visit(statement)!);
        }
        
        return new Block(statements);
    }

    public override object? VisitVariableDeclaration(CraterParser.VariableDeclarationContext context)
    {
        var isLocal = context.LOCAL() != null;
        var identifier = context.IDENTIFIER().GetText()!;
        var dataTypeReference = (DataTypeReference)Visit(context.typeName())!;
        var isNullable = context.QMARK() != null;

        if (context.expression() is null)
        {
            return new VariableDeclaration(isLocal, identifier, dataTypeReference, isNullable, null, context);
        }
        
        var initializer = (Expression)Visit(context.expression())!;
        
        return new VariableDeclaration(isLocal, identifier, dataTypeReference, isNullable, initializer, context);
    }

    public override object? VisitTypeName(CraterParser.TypeNameContext context)
    {
        if (context.FUNCTION() != null)
        {
            return new DataTypeReference("function");
        }
        
        return new DataTypeReference(context.IDENTIFIER().GetText()!);
    }

    public override object? VisitParenthesizedExpression(CraterParser.ParenthesizedExpressionContext context)
    {
        var expression = (Expression)Visit(context.expression())!;
        return new ParenthesizedExpression(expression, context);
    }
    
    public override object? VisitLiteral(CraterParser.LiteralContext context)
    {
        if (context.number != null)
            return new NumberLiteral(context.number.Text, context);
        
        if (context.STRING() != null)
            return new StringLiteral(context.STRING().GetText()!, context);
        
        if (context.BOOLEAN() != null)
            return new BooleanLiteral(context.BOOLEAN().GetText()!, context);
        
        throw new NotImplementedException($"Unknown literal type: {context}");
    }
}