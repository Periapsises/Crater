namespace CLI;

class Program
{
    static void Main(string[] args)
    {
        var testInput = """
                        local myVariableDeclaration: number = 4
                        local myInvalidDeclaration: number = true
                        local myInvalidType: someType
                        local myNonNullable: number
                        
                        """;
        
        Console.WriteLine("Translating input code:");
        Console.WriteLine();
        
        var sourceLines = testInput.Split(Environment.NewLine);

        for (var i = 0; i < sourceLines.Length; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(i + 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" " + sourceLines[i]);
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
        
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine();
        Console.WriteLine($"Transpilation finished with {u}{numErrors}{r} {errorString}, {u}{numWarnings}{r} {warningString} and {u}{numInfos}{r} {infosString}.");
        Console.WriteLine();

        var lines = output.TranslatedCode.Split('\n');
        
        for (var i = 0; i < lines.Length; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(i + 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" " + lines[i]);
        }
    }
}