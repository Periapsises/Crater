using System.Text;
using Antlr4.Runtime;
using Core.Antlr;
using Core.SemanticAnalyzer;
using Core.SyntaxTreeConverter;
using Core.SyntaxTreeConverter.Expressions;
using Core.SyntaxTreeConverter.Statements;
using Environment = System.Environment;
using Expression = Core.SyntaxTreeConverter.Expression;

namespace Compiler;

public class Compiler(string input)
{
    private int _spacing = 0;
    private readonly StringBuilder _builder = new();
    private readonly string _nl = Environment.NewLine;
    
    public string Compile()
    {
        var inputStream = new AntlrInputStream(input);
        var craterLexer = new CraterLexer(inputStream);
        var tokenStream = new CommonTokenStream(craterLexer);
        var craterParser = new CraterParser(tokenStream);

        var syntaxTreeConverter = new SyntaxTreeConverter();
        var module = (Module)syntaxTreeConverter.Visit(craterParser.program())!;

        DiagnosticReporter.CreateInstance(tokenStream);
        
        var semanticAnalyzer = new SemanticAnalyzer();
        semanticAnalyzer.AnalyzeModule(module);
        
        CompileModule(module);
        
        return _builder.ToString();
    }

    private void CompileModule(Module module)
    {
        CompileBlock(module.Block);
    }

    private void CompileBlock(Block block)
    {
        foreach (var statement in block.Statements)
        {
            switch (statement)
            {
                case VariableDeclaration variableDeclaration:
                    CompileVariableDeclaration(variableDeclaration);
                    break;
                case FunctionDeclaration functionDeclaration:
                    CompileFunctionDeclaration(functionDeclaration);
                    break;
                case IfStatement ifStatement:
                    CompileIfStatement(ifStatement);
                    break;
                case FunctionCallStatement functionCallStatement:
                    CompileFunctionCallStatement(functionCallStatement);
                    break;
                default:
                    throw new NotImplementedException($"Unsupported statement type {statement.GetType()}");
            }
        }
    }

    private void CompileVariableDeclaration(VariableDeclaration variableDeclaration)
    {
        AppendSpacing();
        
        if (variableDeclaration.Local)
            Append("local ");

        Append(variableDeclaration.Identifier);

        if (variableDeclaration.Initializer != null)
        {
            Append(" = ");
            CompileExpression(variableDeclaration.Initializer);
        }
        
        Append(_nl);
    }

    private void CompileFunctionDeclaration(FunctionDeclaration functionDeclaration)
    {
        AppendSpacing();
        
        if (functionDeclaration.Local)
            Append("local ");

        Append("function " + functionDeclaration.Identifier + "(");
        Append(string.Join(", ", functionDeclaration.Parameters.Select(p => p.Name)));
        Append(")" + _nl);
        
        _spacing += 4;
        CompileBlock(functionDeclaration.Block);
        _spacing -= 4;
        
        AppendSpacing();
        Append("end" + _nl);
    }

    private void CompileIfStatement(IfStatement ifStatement)
    {
        AppendSpacing();
        Append("if ");
        CompileExpression(ifStatement.Condition);
        Append(" then" + _nl);
        
        _spacing += 4;
        CompileBlock(ifStatement.Block);
        _spacing -= 4;
        
        foreach (var elseIfStatement in ifStatement.ElseIfStatements)
            CompileElseIfStatement(elseIfStatement);
        
        if (ifStatement.ElseStatement != null)
            CompileElseStatement(ifStatement.ElseStatement);
        
        Append("end" + _nl);
    }

    private void CompileElseIfStatement(ElseIfStatement ifStatement)
    {
        AppendSpacing();
        Append("elseif ");
        CompileExpression(ifStatement.Condition);
        Append(" then" + _nl);
        
        _spacing += 4;
        CompileBlock(ifStatement.Block);
        _spacing -= 4;
    }

    private void CompileElseStatement(ElseStatement elseStatement)
    {
        AppendSpacing();
        Append("else" + _nl);
        
        _spacing += 4;
        CompileBlock(elseStatement.Block);
        _spacing -= 4;
    }

    private void CompileFunctionCallStatement(FunctionCallStatement functionCallStatement)
    {
        AppendSpacing();
        CompileExpression(functionCallStatement.PrimaryExpression);
        Append('(');

        if (functionCallStatement.Arguments.Count > 0)
        {
            Append(' ');

            for (var i = 0; i < functionCallStatement.Arguments.Count; i++)
            {
                CompileExpression(functionCallStatement.Arguments[i]);
                if (i < functionCallStatement.Arguments.Count - 1)
                    Append(", ");
            }
            
            Append(' ');
        }
        
        Append(')');
    }
    
    private void CompileExpression(Expression expression)
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
                CompileExpression(parenthesizedExpression.Expression);
                Append(" )");
                break;
            case BinaryOperation binaryOperation:
                CompileExpression(binaryOperation.Left);
                Append($" {binaryOperation.Operator} ");
                CompileExpression(binaryOperation.Right);
                break;
            case LogicalOperation logicalOperation:
                CompileExpression(logicalOperation.Left);
                Append($" {logicalOperation.Operator} ");
                CompileExpression(logicalOperation.Right);
                break;
            case AndOperation andOperation:
                CompileExpression(andOperation.Left);
                Append($" {andOperation.Operator} ");
                CompileExpression(andOperation.Right);
                break;
            case OrOperation orOperation:
                CompileExpression(orOperation.Left);
                Append($" {orOperation.Operator} ");
                CompileExpression(orOperation.Right);
                break;
            case UnaryOperation unaryOperation:
                Append($"{unaryOperation.Operator}");
                if (unaryOperation.Operator != "-") Append(' ');
                CompileExpression(unaryOperation.Expression);
                break;
            case VariableReference variableReference:
                Append(variableReference.Name);
                break;
            case PrimaryExpression primaryExpression:
                CompilePrimaryExpression(primaryExpression);
                break;
            case DotIndex dotIndex:
                Append($".{dotIndex.Index}");
                break;
            case BracketIndex bracketIndex:
                Append('[');
                CompileExpression(bracketIndex.Index);
                Append(']');
                break;
            case FunctionCall functionCall:
                CompileFunctionCallExpression(functionCall);
                break;
            default:
                throw new NotImplementedException($"Unsupported expression type {expression.GetType()}");
        }
    }
    
    private void CompilePrimaryExpression(PrimaryExpression primaryExpression)
    {
        CompileExpression(primaryExpression.PrefixExpression);
        foreach (var postfixExpression in primaryExpression.PostfixExpressions)
            CompileExpression(postfixExpression);
    }

    private void CompileFunctionCallExpression(FunctionCall functionCall)
    {
        Append('(');

        if (functionCall.Arguments.Count > 0)
        {
            Append(' ');

            for (var i = 0; i < functionCall.Arguments.Count; i++)
            {
                CompileExpression(functionCall.Arguments[i]);
                if (i < functionCall.Arguments.Count - 1)
                    Append(", ");
            }
            
            Append(' ');
        }

        Append(')');
    }

    private void AppendSpacing()
    {
        if (_spacing == 0) return;
        _builder.Append(new string(' ', _spacing));
    }
    private void Append(string str) => _builder.Append(str);
    private void Append(char ch) => _builder.Append(ch);
}
