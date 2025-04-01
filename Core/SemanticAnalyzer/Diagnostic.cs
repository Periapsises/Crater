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
    
    protected static readonly string Info = "\u001b[36m[Info] \u001b[0m";
    protected static readonly string Warning = "\u001b[33m[Warning \u001b[0m";
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
        return $"\u001b[90m{Line}\u001b[0m {Code}\n{spacing}\u001b[91m^\u001b[0m";
    }
    
    public Diagnostic WithContext(ParserRuleContext context) => WithContext(context.Start);
    public Diagnostic WithContext(ITerminalNode terminal) => WithContext(terminal.Symbol);

    public Diagnostic WithContext(IToken token)
    {
        Line = token.Line;
        Column = token.Column;

        var input = token.InputStream;
        if (input != null)
            Code = input.ToString()?.Split('\n')[Line - 1]!;
        
        return this;
    }
}
