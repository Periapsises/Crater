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
    private int _highlightLength = 1;
    
    protected string Message = string.Empty;

    public abstract string GetErrorFormatted();

    protected string GetLocation()
    {
        if (Line == 0 && Column == 0) return "";

        return $" at line [underline yellow]{Line}[/], column [underline yellow]{Column}[/]";
    }

    protected string GetCodeLocation()
    {
        if (_code == string.Empty) return "";
        
        var code = _code.TrimStart();
        var numCharsRemoved = _code.Length - code.Length;
        
        var spacing = new string(' ', Column + Line.ToString().Length + 1 - numCharsRemoved);
        var highlight = new string('^', _highlightLength);
        return $"\e[90m{Line}\e[0m {code}\n{spacing}\e[91m{highlight}\e[0m";
    }

    public Diagnostic WithContext(ParserRuleContext context)
    {
        Line = context.Start.Line;
        Column = context.Start.Column;
        _highlightLength = context.Stop.StopIndex - context.Start.StartIndex + 1;

        var input = context.Start.InputStream;
        if (input != null)
            _code = input.ToString()?.Split('\n')[Line - 1]!;

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

        return this;
    }
    
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
            
            formattedArguments.Add($"[aqua]{formattedArgument}[/]");
        }
        
        return string.Format(message, formattedArguments.ToArray());
    }
}

public abstract class ErrorDiagnostic() : Diagnostic(Severity.Error)
{
    public override string GetErrorFormatted()
    {
        return $"[red][[Error]][/] {Message}{GetLocation()}";
    }
}

public abstract class WarningDiagnostic() : Diagnostic(Severity.Warning)
{
    public override string GetErrorFormatted()
    {
        return $"[yellow][[Warning]][/] {Message}{GetLocation()}";
    }
}

public abstract class InfoDiagnostic() : Diagnostic(Severity.Info)
{
    public override string GetErrorFormatted()
    {
        return $"[aqua][[Info]][/] {Message}{GetLocation()}";
    }
}
