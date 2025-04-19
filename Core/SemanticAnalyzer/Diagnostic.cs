using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Core.SyntaxTreeConverter;
using Core.SyntaxTreeConverter.Expressions;

namespace Core.SemanticAnalyzer;

public enum Severity
{
    Info,
    Warning,
    Error
}

public abstract class Diagnostic(Severity severity)
{
    public readonly Severity Severity = severity;
    public int Line { get; private set; }
    public int Column { get; private set; }
    
    private string _code = string.Empty;
    private string? _highlighted;
    private int _highlightLength = 1;
    
    protected string Message = string.Empty;

    public abstract string GetErrorFormatted();

    protected string GetLocation()
    {
        if (Line == 0 && Column == 0) return "";

        return $" at line \e[4;93m{Line}\e[0m, column \e[4;93m{Column}\e[0m";
    }

    protected string GetCodeLocation()
    {
        if (_code == string.Empty) return "";
        
        var code = _code.TrimStart();
        var numCharsRemoved = _code.Length - code.Length;
        
        if (_highlighted != null) code = _highlighted;
        
        var spacing = new string(' ', Column + Line.ToString().Length + 1 - numCharsRemoved);
        var highlight = new string('^', _highlightLength);
        return $"\e[90m{Line}\e[0m {code}\n{spacing}\e[91m{highlight}\e[0m";
    }

    private readonly HashSet<string> _keywords = ["local", "if", "elseif", "else", "then", "end"];
    
    private string GetColorFormattedTokens(IToken positionToken)
    {
        var tokenStream = DiagnosticReporter.GetCommonTokenStream();
        var tokenIndex = positionToken.TokenIndex;
        var tokenLine = positionToken.Line;

        var tokensInLine = new List<IToken>();

        for (var i = tokenIndex - 1; i >= 0; i--)
        {
            var token = tokenStream.Get(i);
            if (token.Line != tokenLine) break;
            
            tokensInLine.Insert(0, token);
        }
        
        tokensInLine.Add(positionToken);

        for (var i = tokenIndex + 1; i < tokenStream.Size; i++)
        {
            var token = tokenStream.Get(i);
            if (token.Line != tokenLine || token.Text == "<EOF>") break;
            
            tokensInLine.Add(token);
        }
        
        var result = "";
        
        foreach (var token in tokensInLine)
        {
            if (_keywords.Contains(token.Text))
                result += "\e[34m";
            else if (token.Text == "true" || token.Text == "false")
                result += "\e[35m";
            else if (token.Text.StartsWith('"') && token.Text.EndsWith('"'))
                result += "\e[32m";
            else if (double.TryParse(token.Text, out _))
                result += "\e[35m";
            else
                result += "\e[0m";
            result += token.Text;
        }
        
        return result.Trim();
    }
    
    public Diagnostic WithContext(ParserRuleContext context)
    {
        Line = context.Start.Line;
        Column = context.Start.Column;
        _highlightLength = context.Stop.StopIndex - context.Start.StartIndex + 1;

        var input = context.Start.InputStream;
        if (input != null)
            _code = input.ToString()?.Split('\n')[Line - 1]!;
        
        if (DiagnosticReporter.HasCommonTokenStream())
            _highlighted = GetColorFormattedTokens(context.Start);

        return this;
    }

    public Diagnostic WithContext(ITerminalNode terminal) => WithContext(terminal.Symbol);

    public Diagnostic WithContext(IToken token)
    {
        Line = token.Line;
        Column = token.Column;
        _highlightLength = token.Text.Length;

        var input = token.InputStream;
        if (input != null)
            _code = input.ToString()?.Split('\n')[Line - 1]!;
        
        if (DiagnosticReporter.HasCommonTokenStream())
            _highlighted = GetColorFormattedTokens(token);

        return this;
    }

    protected const string Reset = "\e[0m";
    protected const string Black = "\e[30m";
    protected const string Red = "\e[31m";
    protected const string Green = "\e[32m";
    protected const string Yellow = "\e[33m";
    protected const string Blue = "\e[34m";
    protected const string Magenta = "\e[35m";
    protected const string Cyan = "\e[36m";
    protected const string White = "\e[37m";
    protected const string Gray = "\e[90m";
    protected const string BrightRed = "\e[91m";
    protected const string BrightGreen = "\e[92m";
    protected const string BrightYellow = "\e[93m";
    protected const string BrightBlue = "\e[94m";
    protected const string BrightMagenta = "\e[95m";
    protected const string BrightCyan = "\e[96m";
    protected const string BrightWhite = "\e[97m";
    
    protected static string Format([StringSyntax("CompositeFormat"), NotNull] string message, [NotNull] params object?[] args)
    {
        var formattedArguments = new List<object>();

        foreach (var arg in args)
        {
            var formattedArgument = arg switch
            {
                DataType dataType => dataType.GetName(),
                Expression expression => expression.FullString,
                _ => arg
            };
            
            formattedArguments.Add($"{BrightCyan}{formattedArgument}{Reset}");
        }
        
        return string.Format(message, formattedArguments.ToArray());
    }
}

public abstract class ErrorDiagnostic() : Diagnostic(Severity.Error)
{
    public override string GetErrorFormatted()
    {
        return $"{BrightRed}[Error]{Reset} {Message}{GetLocation()}";
    }
}

public abstract class WarningDiagnostic() : Diagnostic(Severity.Warning)
{
    public override string GetErrorFormatted()
    {
        return $"{BrightYellow}[Warning]{Reset} {Message}{GetLocation()}";
    }
}

public abstract class InfoDiagnostic() : Diagnostic(Severity.Info)
{
    public override string GetErrorFormatted()
    {
        return $"{BrightCyan}[Info]{Reset} {Message}{GetLocation()}";
    }
}
