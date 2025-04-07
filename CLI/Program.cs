﻿namespace CLI;

class Program
{
    static void Main(string[] args)
    {
        var testInput = """
                        local myKnownString: string = "Hello"
                        local myOtherKnownTernaryOperation: number = ( myKnownString .. "!" == "Hello!" ) and 1 or 2
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
        
        var transpiler = new Transpiler.Transpiler(testInput);
        var output = transpiler.Transpile();

        foreach (var diagnostic in output.Reporter.ErrorDiagnostics)
            Console.WriteLine(diagnostic.GetMessage());

        foreach (var diagnostic in output.Reporter.WarningDiagnostics)
            Console.WriteLine(diagnostic.GetMessage());
        
        foreach (var diagnostic in output.Reporter.InfoDiagnostics)
            Console.WriteLine(diagnostic.GetMessage());
        
        var numErrors = output.Reporter.ErrorDiagnostics.Count;
        var numWarnings = output.Reporter.WarningDiagnostics.Count;
        var numInfos = output.Reporter.InfoDiagnostics.Count;

        var errorString = "error" + (numErrors == 1 ? "" : "s");
        var warningString = "warning" + (numWarnings == 1 ? "" : "s");
        var infosString = "info" + (numInfos == 1 ? "" : "s");

        var u = "\u001b[4m";
        var r = "\u001b[0m";
        
        Console.WriteLine();
        Console.WriteLine($"Transpilation finished with {u}{numErrors}{r} {errorString}, {u}{numWarnings}{r} {warningString} and {u}{numInfos}{r} {infosString}.");
        Console.WriteLine();

        maxLineNumberSize = output.TranslatedCode.Split('\n').Length.ToString().Length;
        
        var lines = output.TranslatedCode.Split('\n');

        for (var i = 0; i < lines.Length; i++)
        {
            var lineNumber = i.ToString().PadLeft(maxLineNumberSize);
            Console.WriteLine($"\u001b[90m{lineNumber}\u001b[0m {lines[i]}");
        }
    }
}