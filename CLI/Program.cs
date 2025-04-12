using System.Diagnostics;
using Core.SemanticAnalyzer;
using Environment = System.Environment;

namespace CLI;

class Program
{
    private static void Main(string[] args)
    {
        var testInput = """
                        local a: number = 5
                        local b: string = a.b
                        local c: number = true and 1 or 0
                        local d: string
                        local e: string
                        """;

        var maxLineNumberSize = testInput.Split('\n').Length.ToString().Length;
        
        Console.WriteLine("Translating input code:");
        Console.WriteLine();
        
        var sourceLines = testInput.Split(Environment.NewLine);

        for (var i = 0; i < sourceLines.Length; i++)
        {
            var lineNumber = i.ToString().PadLeft(maxLineNumberSize);
            Console.WriteLine($"\u001b[90m{lineNumber}\u001b[0m {sourceLines[i]}");
        }

        Console.WriteLine();
        
        var stopwatch = Stopwatch.StartNew();
        
        var transpiler = new Transpiler.Transpiler(testInput);
        var output = transpiler.Transpile();
        
        stopwatch.Stop();

        foreach (var diagnostic in DiagnosticReporter.GetDiagnostics())
            Console.WriteLine(diagnostic.GetErrorFormatted());
        
        var numErrors = DiagnosticReporter.GetDiagnostics().Count(diagnostic => diagnostic.Severity == Severity.Error);
        var numWarnings = DiagnosticReporter.GetDiagnostics().Count(diagnostic => diagnostic.Severity == Severity.Warning);
        var numInfos = DiagnosticReporter.GetDiagnostics().Count(diagnostic => diagnostic.Severity == Severity.Info);

        var errorString = "error" + (numErrors == 1 ? "" : "s");
        var warningString = "warning" + (numWarnings == 1 ? "" : "s");
        var infosString = "info" + (numInfos == 1 ? "" : "s");
        
        var errorDisplay = $"\u001b[4m{numErrors}\u001b[24m {errorString}\u001b[0m";
        if (numErrors != 0) errorDisplay = "\u001b[91m" + errorDisplay;

        var warningDisplay = $"\u001b[4m{numWarnings}\u001b[24m {warningString}\u001b[0m";
        if (numWarnings != 0) warningDisplay = "\u001b[93m" + warningDisplay;
        
        var infoDisplay = $"\u001b[4m{numInfos}\u001b[24m {infosString}\u001b[0m";
        if (numInfos != 0) infoDisplay = "\u001b[96m" + infoDisplay;
        
        Console.WriteLine();
        Console.WriteLine($"Transpilation finished with {errorDisplay}, {warningDisplay} and {infoDisplay} in {FormatElapsedTime(stopwatch.Elapsed)}.");
        Console.WriteLine();

        maxLineNumberSize = output.TranslatedCode.Split('\n').Length.ToString().Length;
        
        var lines = output.TranslatedCode.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var lineNumber = i.ToString().PadLeft(maxLineNumberSize);
            Console.WriteLine($"\u001b[90m{lineNumber}\u001b[0m {lines[i]}");
        }
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