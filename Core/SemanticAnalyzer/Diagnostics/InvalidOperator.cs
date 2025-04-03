namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidBinaryOperator(DataType left, DataType right, string op) : Diagnostic(Severity.Error)
{
    public override string GetMessage()
    {
        var message = $"{Error}Cannot apply operator '\u001b[96m{op}\u001b[0m' to operands of type '\u001b[96m{left.GetName()}\u001b[0m' and '\u001b[96m{right.GetName()}\u001b[0m'{GetLocation()}";
        
        if (Code != string.Empty)
            message += $"\n{GetCodeLocation()}";
        
        return message;
    }
}