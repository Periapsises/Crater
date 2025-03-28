using Core.Antlr;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;

namespace Core.SyntaxTreeConverter;

public class SyntaxTreeConverter : CraterParserBaseVisitor<object?>
{
    public override object? VisitProgram(CraterParser.ProgramContext context)
    {
        return Visit(context.block());
    }

    public override object? VisitBlock(CraterParser.BlockContext context)
    {
        var statements = new List<Statement>();
        
        foreach (var statement in context.statement())
        {
            statements.Add((Statement)Visit(statement)!);
        }
        
        return statements;
    }

    public override object? VisitVariableDeclaration(CraterParser.VariableDeclarationContext context)
    {
        var isLocal = context.LOCAL() != null;
        var identifier = context.IDENTIFIER().GetText()!;
        var dataTypeReference = (DataTypeReference)Visit(context.typeName())!;

        if (context.expression() is null)
        {
            return new VariableDeclaration(isLocal, identifier, dataTypeReference);
        }
        
        var initializer = (Expression)Visit(context.expression())!;
        
        return new VariableDeclaration(isLocal, identifier, dataTypeReference, initializer);
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
        return new ParenthesizedExpression(expression);
    }
    
    public override object? VisitLiteral(CraterParser.LiteralContext context)
    {
        if (context.number != null)
            return new NumberLiteral(context.number.Text);
        
        if (context.STRING() != null)
            return new StringLiteral(context.STRING().GetText()!);
        
        if (context.BOOLEAN() != null)
            return new BooleanLiteral(context.BOOLEAN().GetText()!);
        
        throw new NotImplementedException();
    }
}