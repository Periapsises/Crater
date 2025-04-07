using System.Linq.Expressions;
using System.Text;
using Antlr4.Runtime;
using Core.Antlr;
using Core.SemanticAnalyzer;
using Core.SyntaxTreeConverter;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;
using Expression = Core.SyntaxTreeConverter.Expression;

namespace Transpiler;

public class Transpiler(string input)
{
    private int _spacing = 0;
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
        
        return new TranslationResult(_builder.ToString(), semanticAnalyzer.Reporter);
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
                case FunctionDeclaration functionDeclaration:
                    TranspileFunctionDeclaration(functionDeclaration);
                    break;
                default:
                    throw new NotImplementedException($"Unsupported statement type {statement.GetType()}");
            }
        }
    }

    private void TranspileVariableDeclaration(VariableDeclaration variableDeclaration)
    {
        AppendSpacing();
        
        if (variableDeclaration.Local)
            Append("local ");

        Append(variableDeclaration.Identifier);

        if (variableDeclaration.Initializer != null)
        {
            Append(" = ");
            TranspileExpression(variableDeclaration.Initializer);
        }
        
        Append('\n');
    }

    private void TranspileFunctionDeclaration(FunctionDeclaration functionDeclaration)
    {
        AppendSpacing();
        
        if (functionDeclaration.Local)
            Append("local ");

        Append("function " + functionDeclaration.Identifier + "(");
        Append(string.Join(", ", functionDeclaration.Parameters.Select(p => p.Name)));
        Append(")\n");
        
        _spacing += 4;
        TranspileBlock(functionDeclaration.Block);
        _spacing -= 4;
        
        AppendSpacing();
        Append("end\n");
    }

    private void TranspileExpression(Expression expression)
    {
        switch (expression)
        {
            case NumberLiteral numberLiteral:
                Append(numberLiteral.StringRepresentation);
                break;
            case StringLiteral stringLiteral:
                Append('"');
                Append(stringLiteral.Value);
                Append('"');
                break;
            case BooleanLiteral booleanLiteral:
                Append(booleanLiteral.Value ? "true" : "false");
                break;
            case ParenthesizedExpression parenthesizedExpression:
                Append("( ");
                TranspileExpression(parenthesizedExpression.Expression);
                Append(" )");
                break;
            case BinaryOperation binaryOperation:
                TranspileExpression(binaryOperation.Left);
                Append($" {binaryOperation.Operator} ");
                TranspileExpression(binaryOperation.Right);
                break;
            case UnaryOperation unaryOperation:
                Append($"{unaryOperation.Operator}");
                if (unaryOperation.Operator != "-") Append(' ');
                TranspileExpression(unaryOperation.Expression);
                break;
            case LogicalOperation logicalOperation:
                TranspileExpression(logicalOperation.Left);
                Append($" {logicalOperation.Operator} ");
                TranspileExpression(logicalOperation.Right);
                break;
            case VariableReference variableReference:
                Append(variableReference.Name);
                break;
            default:
                throw new NotImplementedException($"Unsupported expression type {expression.GetType()}");
        }
    }

    private void AppendSpacing() => _builder.Append(new string(' ', _spacing));
    private void Append(string str) => _builder.Append(str);
    private void Append(char ch) => _builder.Append(ch);
}

public class TranslationResult(string translatedCode, DiagnosticReporter reporter)
{
    public readonly string TranslatedCode = translatedCode;
    public readonly DiagnosticReporter Reporter = reporter;
}
