namespace CLI;

class Program
{
    static void Main(string[] args)
    {
        var testInput = """
                        local myVariableDeclaration: number = 4
                        local myInvalidDeclaration: number = true
                        """;
        
        var transpiler = new Transpiler.Transpiler(testInput);
        var output = transpiler.Transpile();

        foreach (var diagnostic in output.Diagnostics.Errors)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[Error] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(diagnostic.Message);
        }

        foreach (var diagnostic in output.Diagnostics.Warnings)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[Warn] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(diagnostic.Message);
        }
        
        foreach (var diagnostic in output.Diagnostics.Infos)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("[Info] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(diagnostic.Message);
        }
        
        var numErrors = output.Diagnostics.Errors.Count;
        var numWarnings = output.Diagnostics.Warnings.Count;
        var numInfos = output.Diagnostics.Infos.Count;

        var errorString = "error" + (numErrors == 1 ? "" : "s");
        var warningString = "warning" + (numErrors == 1 ? "" : "s");
        var infosString = "info" + (numErrors == 1 ? "" : "s");
        
        Console.WriteLine();
        Console.WriteLine($"Transpilation finished with {numErrors} {errorString}, {numWarnings} {warningString} and {numInfos} {infosString}.");
        Console.WriteLine();

        var lines = output.TranslatedCode.Split('\n');
        
        for (int i = 0; i < lines.Length; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(i + 1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" " + lines[i]);
        }
    }
}