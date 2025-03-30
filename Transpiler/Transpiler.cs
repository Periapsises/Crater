using System.Text;
using Antlr4.Runtime;
using Core.Antlr;
using Core.SemanticAnalyzer;
using Core.SyntaxTreeConverter;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;

namespace Transpiler;

public class Transpiler(string input)
{
    private readonly StringBuilder _builder = new();
    
    public TranslationResult Transpile()
    {
        var inputStream = new AntlrInputStream(input);
        var craterLexer = new CraterLexer(inputStream);
        var tokenStream = new CommonTokenStream(craterLexer);
        var craterParser = new CraterParser(tokenStream);

        var syntaxTreeConverter = new SyntaxTreeConverter();
        var module = (Module)syntaxTreeConverter.Visit(craterParser.program())!;

        var semanticAnalyzer = new SemanticAnalyzer();
        semanticAnalyzer.AnalyzeModule(module);
        
        TranspileModule(module);
        
        return new TranslationResult(_builder.ToString(), semanticAnalyzer.Diagnostics);
    }

    private void TranspileModule(Module module)
    {
        TranspileBlock(module.Block);
    }

    private void TranspileBlock(Block block)
    {
        foreach (var statement in block.Statements)
        {
            switch (statement)
            {
                case VariableDeclaration variableDeclaration:
                    TranspileVariableDeclaration(variableDeclaration);
                    break;
                default:
                    throw new NotImplementedException($"Unsupported statement type {statement.GetType()}");
            }
        }
    }

    private void TranspileVariableDeclaration(VariableDeclaration variableDeclaration)
    {
        if (variableDeclaration.Local)
            _builder.Append("local ");

        _builder.Append(variableDeclaration.Identifier);

        if (variableDeclaration.Initializer != null)
        {
            _builder.Append(" = ");
            TranspileExpression(variableDeclaration.Initializer);
        }
        
        _builder.Append('\n');
    }

    private void TranspileExpression(Expression expression)
    {
        switch (expression)
        {
            case NumberLiteral numberLiteral:
                _builder.Append(numberLiteral.Value);
                break;
            case StringLiteral stringLiteral:
                _builder.Append('"');
                _builder.Append(stringLiteral.Value);
                _builder.Append('"');
                break;
            case BooleanLiteral booleanLiteral:
                _builder.Append(booleanLiteral.Value ? "true" : "false");
                break;
            case ParenthesizedExpression parenthesizedExpression:
                _builder.Append("( ");
                TranspileExpression(parenthesizedExpression.Expression);
                _builder.Append(" )");
                break;
            default:
                throw new NotImplementedException($"Unsupported expression type {expression.GetType()}");
        }
    }
}

public class TranslationResult(string translatedCode, Diagnostics diagnostics)
{
    public readonly string TranslatedCode = translatedCode;
    public readonly Diagnostics Diagnostics = diagnostics;
}
