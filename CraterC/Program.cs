using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp<CraterCompiler>();
return app.Run(args);

internal sealed class CraterCompiler : Command<CraterCompiler.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to a file, a space-separated list of files, or a directory to compile.")]
        [CommandArgument(0, "<input>")]
        public required string[] inputs { get; init; }
        
        [Description("Path to the output directory.")]
        [CommandOption("-o|--output")]
        public required string? output { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (settings.inputs.Length == 0)
        {
            AnsiConsole.MarkupLine("[red]ERROR:[/] No input specified.");
            ShowUsage();
            return 1;
        }

        var invalidPaths = new List<string>();
        if (!AllPathsExist(settings.inputs, invalidPaths))
        {
            if (invalidPaths.Count == 1)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]ERROR:[/] The following path does not exist: [aqua]\"{invalidPaths[0]}\"[/]");
                return 1;
            }
            
            AnsiConsole.MarkupLine("[red]ERROR:[/] The following paths do not exist:");
            foreach (var invalidPath in invalidPaths)
                AnsiConsole.MarkupLineInterpolated($"    [aqua]\"{invalidPath}\"[/]");
            
            return 1;
        }

        if (settings.inputs.Length != 1 && !AreAllFiles(settings.inputs))
        {
            AnsiConsole.MarkupLine("[red]ERROR:[/] A directory was passed in the inputs.");
            return 1;
        }

        string[] files;
        if (settings.inputs.Length == 1 && Directory.Exists(settings.inputs[0]))
            files = Directory.GetFiles(settings.inputs[0], "*.cra");
        else
            files = settings.inputs;        

        var rand = new Random();
        
        AnsiConsole.Status()
            .SpinnerStyle(Style.Parse("blue"))
            .Start("Compiling", ctx =>
            {
                foreach (var file in files)
                {
                    ctx.Status($"Compiling {Path.GetFileName(file)}");
                    var stopwatch = Stopwatch.StartNew();
                    Thread.Sleep(rand.Next(800, 1600));
                    AnsiConsole.MarkupLineInterpolated($"Compiled [aqua]\"{Path.GetFileName(file)}\"[/] in [blue]{FormatElapsedTime(stopwatch.Elapsed)}[/]");
                }
            });
        
        return 0;
    }

    private static bool AllPathsExist(string[] inputPaths, in List<string> invalidPaths)
    {
        foreach (var inputPath in inputPaths)
        {
            if (Directory.Exists(inputPath) || File.Exists(inputPath))
                continue;
            
            invalidPaths.Add(inputPath);
        }
        
        return invalidPaths.Count == 0;
    }

    private static bool AreAllFiles(string[] inputPaths)
    {
        return inputPaths.All(inputPath => !Directory.Exists(inputPath));
    }

    private static void ShowUsage()
    {
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        
        AnsiConsole.MarkupLine("[yellow]USAGE:[/]");
        AnsiConsole.MarkupLineInterpolated($"    {assemblyName}.exe [[input]] [grey][[options]][/]");
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
