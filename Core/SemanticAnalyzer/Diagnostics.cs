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

    public void PushError(string message)
    {
        Errors.Add(new DiagnosticMessage(message));
    }
    
    public void PushWarning(string message)
    {
        Warnings.Add(new DiagnosticMessage(message));
    }
    
    public void PushInfo(string message)
    {
        Infos.Add(new DiagnosticMessage(message));
    }
}

public class DiagnosticMessage(string message)
{
    public readonly string Message = message;
}