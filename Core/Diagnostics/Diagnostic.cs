using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Core.Diagnostics;

public enum Severity
{
    Warning,
    Error,
    Fatal
}

public abstract class Diagnostic(string code, string message, Severity severity)
{
    public readonly Severity Severity = severity;
    public readonly string Code = code;
    public readonly string Message = message;

    public int Line { get; private set; } = -1;
    public int Column { get; private set; } = -1;

    public abstract string[] args { get; }

    public override string ToString()
    {
        var message = string.Format(Message, args.Cast<object?>().ToArray());
        
        if (Line == -1 || Column == -1)
            return $"[{Code}] {message} at unknown location.";
        
        return $"[{Code}] {message} at line {Line} column {Column}.";
    }

    public Diagnostic UseLocation(ParserRuleContext context) => UseLocation(context.Start);
    public Diagnostic UseLocation(ITerminalNode terminal) => UseLocation(terminal.Symbol);

    public Diagnostic UseLocation(IToken token)
    {
        Line = token.Line;
        Column = token.Column;
        return this;
    }
}

public class Diagnostic<T>(Diagnostic? diagnostic, T value)
{
    private readonly T _value = value;

    public void Report(IDiagnosticReporter reporter)
    {
        if (diagnostic != null)
            reporter.Report(diagnostic);
    }
    
    public static implicit operator T(Diagnostic<T> diagnostic) => diagnostic._value;
}
