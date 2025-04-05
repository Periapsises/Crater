namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidUnaryOperator(DataType self, string op) : Diagnostic(Severity.Error)
{
    public override string GetMessage()
    {
        var message = $"{Error}Cannot apply operator '\u001b[96m{op}\u001b[0m' to operand of type '\u001b[96m{self.GetName()}\u001b[0m'{GetLocation()}";
        
        if (Code != string.Empty)
            message += $"\n{GetCodeLocation()}";
        
        return message;
    }
}