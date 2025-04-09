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
    public string Code { get; private set; } = string.Empty;
    public string? Highlighted;
    public int HighlightLength = 1;

    protected static readonly string Info = "\u001b[36m[Info] \u001b[0m";
    protected static readonly string Warning = "\u001b[33m[Warning] \u001b[0m";
    protected static readonly string Error = "\u001b[31m[Error] \u001b[0m";

    public abstract string GetMessage();

    protected string GetLocation()
    {
        if (Line == 0 && Column == 0) return "";

        return $" at line \u001b[4;93m{Line}\u001b[0m, column \u001b[4;93m{Column}\u001b[0m";
    }

    protected string GetCodeLocation()
    {
        if (Code == string.Empty) return "";
        
        var code = Code.TrimStart();
        var numCharsRemoved = Code.Length - code.Length;
        
        if (Highlighted != null) code = Highlighted;
        
        var spacing = new string(' ', Column + Line.ToString().Length + 1 - numCharsRemoved);
        var highlight = new string('^', HighlightLength);
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
            if (token.Line != tokenLine) break;
            
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
        HighlightLength = context.Stop.StopIndex - context.Start.StartIndex + 1;

        var input = context.Start.InputStream;
        if (input != null)
            Code = input.ToString()?.Split('\n')[Line - 1]!;
        
        if (DiagnosticReporter.HasCommonTokenStream())
            Highlighted = GetColorFormattedTokens(context.Start);

        return this;
    }

    public Diagnostic WithContext(ITerminalNode terminal) => WithContext(terminal.Symbol);

    public Diagnostic WithContext(IToken token)
    {
        Line = token.Line;
        Column = token.Column;
        HighlightLength = token.Text.Length;

        var input = token.InputStream;
        if (input != null)
            Code = input.ToString()?.Split('\n')[Line - 1]!;
        
        if (DiagnosticReporter.HasCommonTokenStream())
            Highlighted = GetColorFormattedTokens(token);

        return this;
    }
}