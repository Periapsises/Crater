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
    public int HighlightLength = 1;

    protected static readonly string Info = "\u001b[36m[Info] \u001b[0m";
    protected static readonly string Warning = "\u001b[33m[Warning] \u001b[0m";
    protected static readonly string Error = "\u001b[31m[Error] \u001b[0m";

    public abstract string GetMessage();

    protected string GetLocation()
    {
        if (Line == 0 && Column == 0) return "";

        return $" at line \u001b[4m{Line}\u001b[0m, column \u001b[4m{Column}\u001b[0m";
    }

    protected string GetCodeLocation()
    {
        if (Code == string.Empty) return "";

        var spacing = new string(' ', Column + Line.ToString().Length + 1);
        var highlight = new string('^', HighlightLength);
        return $"\u001b[90m{Line}\u001b[0m {Code}\n{spacing}\u001b[91m{highlight}\u001b[0m";
    }

    public Diagnostic WithContext(ParserRuleContext context)
    {
        Line = context.Start.Line;
        Column = context.Start.Column;
        HighlightLength = context.Stop.StopIndex - context.Start.StartIndex + 1;

        var input = context.Start.InputStream;
        if (input != null)
            Code = input.ToString()?.Split('\n')[Line - 1]!;

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

        return this;
    }
}