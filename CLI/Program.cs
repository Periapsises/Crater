using System.Diagnostics;
using System.Text;
using Core.SemanticAnalyzer;
using Environment = System.Environment;

namespace CLI;

class Program
{
    private static void Main(string[] args)
    {
        if (args.Length < 2)
            throw new Exception("Usage: CLI.exe <input file> <output file>");
        
        if (!File.Exists(args[0]))
            throw new FileNotFoundException(args[0]);

        var sourceCode = File.ReadAllText(args[0]);
        
        var stopwatch = Stopwatch.StartNew();
        
        var transpiler = new Transpiler.Transpiler(sourceCode);
        var output = transpiler.Transpile();
        File.WriteAllText(args[1], output.TranslatedCode);
        
        stopwatch.Stop();

        foreach (var diagnostic in DiagnosticReporter.GetDiagnostics())
            Console.WriteLine(diagnostic.GetErrorFormatted());
        
        var numErrors = DiagnosticReporter.GetDiagnostics().Count(diagnostic => diagnostic.Severity == Severity.Error);
        var numWarnings = DiagnosticReporter.GetDiagnostics().Count(diagnostic => diagnostic.Severity == Severity.Warning);
        var numInfos = DiagnosticReporter.GetDiagnostics().Count(diagnostic => diagnostic.Severity == Severity.Info);

        var errorString = "error" + (numErrors == 1 ? "" : "s");
        var warningString = "warning" + (numWarnings == 1 ? "" : "s");
        var infosString = "info" + (numInfos == 1 ? "" : "s");
        
        var errorDisplay = $"\e[4m{numErrors}\e[24m {errorString}\e[0m";
        if (numErrors != 0) errorDisplay = "\e[91m" + errorDisplay;

        var warningDisplay = $"\e[4m{numWarnings}\e[24m {warningString}\e[0m";
        if (numWarnings != 0) warningDisplay = "\e[93m" + warningDisplay;
        
        var infoDisplay = $"\e[4m{numInfos}\e[24m {infosString}\e[0m";
        if (numInfos != 0) infoDisplay = "\e[96m" + infoDisplay;
        
        if (numErrors + numWarnings + numInfos != 0) Console.WriteLine();
        Console.WriteLine($"Transpilation finished with {errorDisplay}, {warningDisplay} and {infoDisplay} in {FormatElapsedTime(stopwatch.Elapsed)}.");
    }

    private static string FormatElapsedTime(TimeSpan time)
    {
        if (time.TotalSeconds >= 1)
            return $"{time.TotalSeconds:F2}s";
        if (time.TotalMilliseconds >= 1)
            return $"{time.TotalMilliseconds:F0}ms";
        
        return $"{time.TotalMicroseconds:F0}µs";
    }
}