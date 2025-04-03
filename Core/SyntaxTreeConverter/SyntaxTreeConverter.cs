using Core.Antlr;
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
        var dataTypeReference = (VariableReference)Visit(context.typeName())!;
        var isNullable = context.QMARK() != null;

        if (context.expression() is null)
        {
            return new VariableDeclaration(isLocal, identifier, dataTypeReference, isNullable, null, context);
        }
        
        var initializer = (Expression)Visit(context.expression())!;
        
        return new VariableDeclaration(isLocal, identifier, dataTypeReference, isNullable, initializer, context);
    }

    public override object? VisitFunctionDeclaration(CraterParser.FunctionDeclarationContext context)
    {
        var isLocal = context.LOCAL() != null;
        var identifier = context.IDENTIFIER().GetText()!;
        var arguments = (List<ParameterDeclartion>)Visit(context.functionParameters())!;
        var returnDataTypeReference = (VariableReference)Visit(context.typeName())!;
        var returnIsNullable = context.QMARK() != null;
        var block = (Block)Visit(context.block())!;
        
        return new FunctionDeclaration(isLocal, identifier, arguments, returnDataTypeReference, returnIsNullable, block, context);
    }

    public override object? VisitFunctionParameters(CraterParser.FunctionParametersContext context)
    {
        List<ParameterDeclartion> arguments = [];
        
        foreach (var argumentContext in context.functionParameter())
            arguments.Add((ParameterDeclartion)Visit(argumentContext)!);
        
        return arguments;
    }

    public override object? VisitFunctionParameter(CraterParser.FunctionParameterContext context)
    {
        var name = context.IDENTIFIER().GetText()!;
        var dataTypeReference = (VariableReference)Visit(context.typeName())!;
        var isNullable = context.QMARK() != null;
        
        return new ParameterDeclartion(name, dataTypeReference, isNullable, context);
    }
    
    public override object? VisitTypeName(CraterParser.TypeNameContext context)
    {
        return new VariableReference(context.IDENTIFIER().GetText()!, null, context);
    }

    public override object? VisitParenthesizedExpression(CraterParser.ParenthesizedExpressionContext context)
    {
        var expression = (Expression)Visit(context.expression())!;
        return new ParenthesizedExpression(expression, context);
    }
    
    public override object? VisitMultiplicativeOperation(CraterParser.MultiplicativeOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new BinaryOperation(left, right, context.op.Text, context);
    }

    public override object? VisitAdditiveOperation(CraterParser.AdditiveOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new BinaryOperation(left, right, context.op.Text, context);
    }
    
    public override object? VisitAndOperation(CraterParser.AndOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new BinaryOperation(left, right, "and", context);
    }
    
    public override object? VisitOrOperation(CraterParser.OrOperationContext context)
    {
        var left = (Expression)Visit(context.expression()[0])!;
        var right = (Expression)Visit(context.expression()[1])!;

        return new BinaryOperation(left, right, "or", context);
    }

    public override object? VisitVariableReference(CraterParser.VariableReferenceContext context)
    {
        return new VariableReference(context.IDENTIFIER().GetText()!, null, context);
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