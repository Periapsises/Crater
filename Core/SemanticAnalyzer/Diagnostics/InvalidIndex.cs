namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidIndex(DataType indexed) : Diagnostic(Severity.Error)
{
    public override string GetMessage()
    {
        var message = $"{Error}Key is not defined in type '\u001b[96m{indexed.GetName()}\u001b[0m'{GetLocation()}";
        
        if (Code != string.Empty)
            message += $"\n{GetCodeLocation()}";
        
        return message;
    }
}