namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidUnaryOperator : Diagnostic
{
    public InvalidUnaryOperator(DataType self, string op) : base(Severity.Error)
    {
        Message = Format("Cannot apply operator '\u001b[96m" + op + "\u001b[0m' to operand of type '\u001b[96m%s\u001b[0m'", self);
    }
}