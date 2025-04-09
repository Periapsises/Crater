using Antlr4.Runtime;

namespace Core.SemanticAnalyzer;

public class DiagnosticReporter
{
    private static DiagnosticReporter? _instance = null;

    public readonly List<Diagnostic> InfoDiagnostics = [];
    public readonly List<Diagnostic> WarningDiagnostics = [];
    public readonly List<Diagnostic> ErrorDiagnostics = [];

    public static DiagnosticReporter CreateInstance()
    {
        _instance = new DiagnosticReporter();
        return _instance;
    }
    
    public static void Report(Diagnostic diagnostic)
    {
        if (_instance == null)
            throw new NullReferenceException();
        
        if (diagnostic.Severity == Severity.Info)
            _instance.InfoDiagnostics.Add(diagnostic);
        else if (diagnostic.Severity == Severity.Warning)
            _instance.WarningDiagnostics.Add(diagnostic);
        else if (diagnostic.Severity == Severity.Error)
            _instance.ErrorDiagnostics.Add(diagnostic);
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

    public T SendReport()
    {
        foreach (var diagnostic in _diagnostics)
            DiagnosticReporter.Report(diagnostic);

        return this.Data!;
    }
    
    public static implicit operator T(DiagnosticReport<T> diagnosticReport) => diagnosticReport.Data!;
}