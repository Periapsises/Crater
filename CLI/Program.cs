namespace CLI;

class Program
{
    static void Main(string[] args)
    {
        var testInput = """
                        local myVariableDeclaration: number = 4
                        local myInvalidDeclaration: number = true
                        
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

        foreach (var diagnostic in output.Diagnostics.Errors)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[Error] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(diagnostic.GetDiagnostic());
        }

        foreach (var diagnostic in output.Diagnostics.Warnings)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[Warn] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(diagnostic.GetDiagnostic());
        }
        
        foreach (var diagnostic in output.Diagnostics.Infos)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[Info] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(diagnostic.GetDiagnostic());
        }
        
        var numErrors = output.Diagnostics.Errors.Count;
        var numWarnings = output.Diagnostics.Warnings.Count;
        var numInfos = output.Diagnostics.Infos.Count;

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