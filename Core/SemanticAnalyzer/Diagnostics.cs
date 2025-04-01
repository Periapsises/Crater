using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Core.SemanticAnalyzer;

public static class DiagnosticsReporter
{
    public static Diagnostics? CurrentDiagnostics = null;
}

public class Diagnostics
{
    public readonly List<DiagnosticMessage> Errors = [];
    public readonly List<DiagnosticMessage> Warnings = [];
    public readonly List<DiagnosticMessage> Infos = [];

    public DiagnosticMessage PushError(string message)
    {
        var diagnostic = new DiagnosticMessage(message);
        Errors.Add(diagnostic);
        return diagnostic;
    }
    
    public DiagnosticMessage PushWarning(string message)
    {
        var diagnostic = new DiagnosticMessage(message);
        Warnings.Add(diagnostic);
        return diagnostic;
    }
    
    public DiagnosticMessage PushInfo(string message)
    {
        var diagnostic = new DiagnosticMessage(message);
        Infos.Add(diagnostic);
        return diagnostic;
    }
}

public class DiagnosticMessage(string message)
{
    private int _line = -1;
    private int _column = -1;

    private string _text = String.Empty;
    
    public string GetDiagnostic()
    {
        if (_line == -1 || _column == -1)
            return message;

        if (_text == String.Empty)
            return $"{message} at line {_line}, column {_column}";
        
        var spacing = new string(' ', _column);
        
        return $"{message} at line {_line}, column {_column}\n\t{_text}\n\t{spacing}^";
    }
    
    public DiagnosticMessage WithLocation(int line, int column)
    {
        _line = line;
        _column = column;
        return this;
    }

    public DiagnosticMessage WithLocation(ParserRuleContext context)
    {
        _line = context.Start.Line;
        _column = context.Start.Column;
        
        if (context.Start == null || context.Stop == null)
            return this;

        var input = context.Start.InputStream;
        if (input == null)
            return this;
        
        var start = context.Start.StartIndex;
        var stop = context.Stop.StopIndex;
        
        _text = input.GetText(new Interval(start, stop));
        
        return this;
    }

    public DiagnosticMessage WithLocation(ITerminalNode terminal)
    {
        _line = terminal.Symbol.Line;
        _column = terminal.Symbol.Column;

        var start = terminal.Symbol.Line;
        
        var input = terminal.Symbol.InputStream;
        if (input == null)
            return this;

        _text = input.ToString().Split('\n')[start - 1];
        
        return this;
    }
    
    public DiagnosticMessage WithLocation(IToken terminal)
    {
        _line = terminal.Line;
        _column = terminal.Column;
        return this;
    }
}