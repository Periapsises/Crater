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