namespace CLI;

class Program
{
    static void Main(string[] args)
    {
        var testInput = """
                        local myUndefinedNumber: number?
                        local myDefinedNumber: number = 5
                        local myDefinedFloat: number = 1.5
                        local myAddition: number = 1 + 2
                        local mySubtraction: number = 2 - 1
                        --local myMultiplication: number = 1 * 2
                        --local myDivision: number = 1 / 2
                        --local myExponent: number = 1 ^ 2
                        --local myModulo: number = 1 % 2
                        --local myNegativeNumber: number = -1
                        --local myExponentialNumber: number = 1e2
                        --local myHexadecimalNumber: number = 0xff
                        --local myBinaryNumber: number = 0b01
                        
                        local myReferencedNumber: number = myDefinedNumber
                        local myVariableAddition: number = myDefinedNumber + 1
                        local myVariableSubtraction: number = myDefinedNumber - 1
                        --local myVariableMultiplication: number = myDefinedNumber * 2
                        --local myVariableDivision: number = myDefinedNumber / 2
                        --local myVariableExponent: number = myDefinedNumber ^ 2
                        --local myVariableModulo: number = myDefinedNumber % 2
                        
                        local invalidAddition: number = 1 + "a"
                        local invalidSubtraction: number = 2 - "a"
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