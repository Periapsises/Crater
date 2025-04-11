namespace Core.SemanticAnalyzer.Diagnostics;

public class InvalidBinaryOperator : Diagnostic
{
    public InvalidBinaryOperator(DataType left, DataType right, string op) : base(Severity.Error)
    {
        Message = Format("Cannot apply operator '\u001b[96m" + op + "\u001b[0m' to operands of type '%s' and '%s'", left, right);
    }
}