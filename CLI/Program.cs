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
        
        Console.WriteLine(output);
    }
}