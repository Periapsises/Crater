using Antlr4.Runtime;

namespace Core.SemanticAnalyzer;

public class DiagnosticReporter
{
    public static DiagnosticReporter? CurrentReporter;

    public readonly List<Diagnostic> InfoDiagnostics = [];
    public readonly List<Diagnostic> WarningDiagnostics = [];
    public readonly List<Diagnostic> ErrorDiagnostics = [];

    public void Report(Diagnostic diagnostic)
    {
        if (CurrentReporter == null) return;
        
        if (diagnostic.Severity == Severity.Info)
            InfoDiagnostics.Add(diagnostic);
        else if (diagnostic.Severity == Severity.Warning)
            WarningDiagnostics.Add(diagnostic);
        else if (diagnostic.Severity == Severity.Error)
            ErrorDiagnostics.Add(diagnostic);
    }
}

public class DiagnosticReport<T>
{
    public T? Data;
    private readonly List<Diagnostic> _diagnostics = [];

    public DiagnosticReport<T> WithContext(IToken context)
    {
        _diagnostics.ForEach(diagnostic => diagnostic.WithContext(context));
        return this;
    }
    
    public void Report(Diagnostic diagnostic) => _diagnostics.Add(diagnostic);

    public DiagnosticReport<T> ReportTo(DiagnosticReporter diagnosticReporter)
    {
        foreach (var diagnostic in _diagnostics)
            diagnosticReporter.Report(diagnostic);

        return this;
    }
    
    public static implicit operator T(DiagnosticReport<T> diagnosticReport) => diagnosticReport.Data!;
}