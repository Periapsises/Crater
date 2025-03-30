using System.Runtime.CompilerServices;

namespace Core.Utils;

// The goal of this class is to help find unfinished code such as TODOs
public static class DebugMessage
{
    public static void Write(string message, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
    {
        Console.WriteLine(message);
        Console.WriteLine($"\t{file}:{line}");
    }
}