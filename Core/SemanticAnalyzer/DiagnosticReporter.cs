using Antlr4.Runtime;

namespace Core.SemanticAnalyzer;

public class DiagnosticReporter(ITokenStream? tokenStream)
{
    private static DiagnosticReporter? _instance = null;

    private readonly ITokenStream? _tokenStream = tokenStream;

    private readonly List<Diagnostic> _diagnostics = [];

    public static List<Diagnostic> GetDiagnostics()
    {
        if (_instance is null)
            throw new NullReferenceException();

        _instance._diagnostics.Sort((diagnostic1, diagnostic2) =>
        {
            var severityComparision = diagnostic2.Severity - diagnostic1.Severity;
            if (severityComparision != 0) return severityComparision;
            
            var lineComparision = diagnostic1.Line - diagnostic2.Line;
            if (lineComparision != 0) return lineComparision;
            
            return diagnostic1.Column - diagnostic2.Column;
        });
        
        return _instance._diagnostics;
    }
    
    public static DiagnosticReporter CreateInstance(ITokenStream? tokenStream = null)
    {
        _instance = new DiagnosticReporter(tokenStream);
        return _instance;
    }

    public static ITokenStream GetCommonTokenStream()
    {
        if (_instance == null)
            throw new NullReferenceException();
        
        if (_instance._tokenStream == null)
            throw new NullReferenceException();

        return _instance._tokenStream;
    }

    public static bool HasCommonTokenStream()
    {
        if (_instance == null) return false;
        if (_instance._tokenStream == null) return false;
        
        return true;
    }
    
    public static void Report(Diagnostic diagnostic)
    {
        if (_instance == null)
            throw new NullReferenceException();
        
        _instance._diagnostics.Add(diagnostic);
    }
}

public class DiagnosticReport<T>
{
    public T? Data;
    private readonly List<Diagnostic> _diagnostics = [];

    public DiagnosticReport<T> WithContext(ParserRuleContext context)
    {
        _diagnostics.ForEach(diagnostic => diagnostic.WithContext(context));
        return this;
    }
    
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

        return Data!;
    }
    
    public static implicit operator T(DiagnosticReport<T> diagnosticReport) => diagnosticReport.Data!;
}