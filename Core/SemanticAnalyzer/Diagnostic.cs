using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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

        return $" at line \u001b[4;93m{Line}\u001b[0m, column \u001b[4;93m{Column}\u001b[0m";
    }

    protected string GetCodeLocation()
    {
        if (_code == string.Empty) return "";
        
        var code = _code.TrimStart();
        var numCharsRemoved = _code.Length - code.Length;
        
        if (_highlighted != null) code = _highlighted;
        
        var spacing = new string(' ', Column + Line.ToString().Length + 1 - numCharsRemoved);
        var highlight = new string('^', _highlightLength);
        return $"\u001b[90m{Line}\u001b[0m {code}\n{spacing}\u001b[91m{highlight}\u001b[0m";
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
                result += "\u001b[34m";
            else if (token.Text == "true" || token.Text == "false")
                result += "\u001b[35m";
            else if (token.Text.StartsWith('"') && token.Text.EndsWith('"'))
                result += "\u001b[32m";
            else if (double.TryParse(token.Text, out _))
                result += "\u001b[35m";
            else
                result += "\u001b[0m";
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

    protected const string Reset = "\x1b[0m";
    protected const string Black = "\x1b[30m";
    protected const string Red = "\x1b[31m";
    protected const string Green = "\x1b[32m";
    protected const string Yellow = "\x1b[33m";
    protected const string Blue = "\x1b[34m";
    protected const string Magenta = "\x1b[35m";
    protected const string Cyan = "\x1b[36m";
    protected const string White = "\x1b[37m";
    protected const string Gray = "\x1b[90m";
    protected const string BrightRed = "\x1b[91m";
    protected const string BrightGreen = "\x1b[92m";
    protected const string BrightYellow = "\x1b[93m";
    protected const string BrightBlue = "\x1b[94m";
    protected const string BrightMagenta = "\x1b[95m";
    protected const string BrightCyan = "\x1b[96m";
    protected const string BrightWhite = "\x1b[97m";
    
    protected string Format(string message, params object[] args)
    {
        var formattedArguments = new List<object>();

        foreach (var arg in args)
        {
            var formattedArgument = arg switch
            {
                DataType dataType => BrightCyan + dataType.GetName() + Reset,
                _ => arg
            };
            
            formattedArguments.Add(formattedArgument);
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
